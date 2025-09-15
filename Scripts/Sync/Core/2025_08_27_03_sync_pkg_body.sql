CREATE OR REPLACE NONEDITIONABLE PACKAGE BODY sync_pkg AS

    -- Função que retorna API info cacheada
    FUNCTION get_api_info_cached(
        p_sender_id   IN NUMBER,
        p_receiver_id IN NUMBER
    ) RETURN t_api_info IS
        v_api_info t_api_info;
    BEGIN
        -- Seleciona dados do banco para sender/receiver
        SELECT 
            snt.url,
            u.username,
            u.password
          INTO v_api_info.url_base, v_api_info.username, v_api_info.password
          FROM sync_routes r
          INNER JOIN sync_nodes snt ON r.target_node_id = snt.id
          INNER JOIN api_users u  ON r.user_id = u.id
         WHERE r.source_node_id = p_sender_id
           AND r.target_node_id = p_receiver_id
           AND ROWNUM = 1;
                          
        RETURN v_api_info;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RAISE_APPLICATION_ERROR(-20010, 'API info não encontrada para receiver_id ' || p_receiver_id);
    END get_api_info_cached;

    -- Procedure que retorna token de autenticação
    PROCEDURE get_auth_token(
        po_token     OUT VARCHAR2,
        pi_api_info  IN t_api_info
    ) IS
        req         UTL_HTTP.req;
        resp        UTL_HTTP.resp;
        buffer      VARCHAR2(32767);
        payload     CLOB;
        payload_raw RAW(32767);
        v_success   VARCHAR2(10);
        v_message   VARCHAR2(4000);
    BEGIN
        payload := '{"username":"' || pi_api_info.username || '","password":"' || pi_api_info.password || '"}';
        payload_raw := UTL_RAW.cast_to_raw(payload);
    
        req := UTL_HTTP.begin_request(pi_api_info.url_base || '/Auth/GetToken', 'POST', 'HTTP/1.1');
        UTL_HTTP.set_header(req, 'Content-Type', 'application/json; charset=UTF-8');
        UTL_HTTP.set_header(req, 'Content-Length', TO_CHAR(UTL_RAW.length(payload_raw)));
    
        UTL_HTTP.write_raw(req, payload_raw);
        resp := UTL_HTTP.get_response(req);
    
        BEGIN
            UTL_HTTP.read_text(resp, buffer, 32767);
        EXCEPTION
            WHEN UTL_HTTP.end_of_body THEN NULL;
        END;
    
        UTL_HTTP.end_response(resp);
    
        IF buffer IS NULL THEN
            RAISE_APPLICATION_ERROR(-20001, 'Resposta da API vazia ao requisitar token');
        END IF;
    
        SELECT JSON_VALUE(buffer, '$.success'),
               JSON_VALUE(buffer, '$.message')
          INTO v_success, v_message
          FROM dual;
    
        IF v_success <> 'true' THEN
            RAISE_APPLICATION_ERROR(-20002, 'Erro de autenticação: ' || v_message);
        END IF;
    
        SELECT JSON_VALUE(buffer, '$.data')
          INTO po_token
          FROM dual;
    
    EXCEPTION
        WHEN OTHERS THEN
            RAISE;
    END get_auth_token;

    -- Procedure genérica que envia JSON para qualquer entidade
    PROCEDURE send_to_api(
        pi_entity_id    IN NUMBER,
        pi_record_id    IN NUMBER,
        pi_operation_id IN NUMBER,
        pi_inner_json   IN JSON_OBJECT_T,
        pi_sender_id    IN NUMBER DEFAULT 1,
        pi_receiver_id  IN NUMBER DEFAULT 2,
        pi_hash         IN VARCHAR2,
        pi_endpoint     IN VARCHAR2,
        pi_api_info     IN t_api_info,
        pi_use_auth     IN BOOLEAN DEFAULT TRUE
    ) IS
        req            UTL_HTTP.req;
        resp           UTL_HTTP.resp;
        buffer         VARCHAR2(32767);
        log_msg        VARCHAR2(4000);
        payload_bytes  RAW(32767);
        v_token        VARCHAR2(4000);
        v_info_obj     JSON_OBJECT_T;
        v_payload_obj  JSON_OBJECT_T;
        v_payload_json CLOB;
        v_inner_json   JSON_OBJECT_T := pi_inner_json;
        v_success      VARCHAR2(5);
        v_message      VARCHAR2(4000);
    BEGIN
        IF v_inner_json IS NULL THEN
            v_inner_json := JSON_OBJECT_T();
        END IF;

        v_info_obj := JSON_OBJECT_T();
        v_info_obj.put('senderId',    pi_sender_id);
        v_info_obj.put('receiverId',  pi_receiver_id);
        v_info_obj.put('operationId', pi_operation_id);
        v_info_obj.put('entityId',    pi_entity_id);
        v_info_obj.put('hash',        pi_hash);

        v_payload_obj := JSON_OBJECT_T();
        v_payload_obj.put('info',    v_info_obj);
        v_payload_obj.put('payload', v_inner_json);

        v_payload_json := v_payload_obj.to_clob;
        payload_bytes := UTL_RAW.cast_to_raw(v_payload_json);

        req := UTL_HTTP.begin_request(pi_api_info.url_base || pi_endpoint, 'POST', 'HTTP/1.1');
        UTL_HTTP.set_header(req, 'Content-Type', 'application/json; charset=UTF-8');
        UTL_HTTP.set_header(req, 'Content-Length', TO_CHAR(UTL_RAW.length(payload_bytes)));

        IF pi_use_auth THEN
            BEGIN
                get_auth_token(v_token, pi_api_info);
                UTL_HTTP.set_header(req, 'Authorization', 'Bearer ' || v_token);
            EXCEPTION
                WHEN OTHERS THEN
                    log_msg := SUBSTR(SQLERRM,1,4000);
                    INSERT INTO sync_logs(
                        entity_id, record_id, status_id, operation_id, api_url, api_username, message, log_datetime, payload, hash_value
                    ) VALUES (
                        pi_entity_id, pi_record_id, 2, pi_operation_id, pi_api_info.url_base || '/Auth/GetToken', pi_api_info.username, log_msg, SYSTIMESTAMP, NULL, pi_hash
                    );
                    RETURN;
            END;
        END IF;

        UTL_HTTP.write_raw(req, payload_bytes);
        resp := UTL_HTTP.get_response(req);

        BEGIN
            UTL_HTTP.read_text(resp, buffer, 32767);
        EXCEPTION
            WHEN UTL_HTTP.end_of_body THEN NULL;
        END;

        UTL_HTTP.end_response(resp);

        BEGIN
            v_success := JSON_VALUE(buffer, '$.success');
            v_message := JSON_VALUE(buffer, '$.message');
        EXCEPTION
            WHEN OTHERS THEN
                v_success := NULL;
                v_message := NULL;
        END;

        IF v_success = 'false' THEN
            INSERT INTO sync_logs(
                entity_id, record_id, status_id, operation_id, api_url, api_username, message, log_datetime, payload, hash_value
            ) VALUES (
                pi_entity_id, pi_record_id, 2, pi_operation_id, pi_api_info.url_base || pi_endpoint, pi_api_info.username, SUBSTR(v_message,1,4000), SYSTIMESTAMP, v_payload_json, pi_hash
            );
        ELSE

            MERGE INTO sync_hashes sh
            USING (SELECT pi_entity_id AS entity_id,
                          pi_record_id AS record_id
                   FROM dual) src
            ON (sh.entity_id = src.entity_id AND sh.record_id = src.record_id)
            WHEN MATCHED THEN
                UPDATE SET
                    sh.hash_value     = pi_hash,
                    sh.operation_date = SYSTIMESTAMP,
                    sh.operation_id   = pi_operation_id
            WHEN NOT MATCHED THEN
                INSERT (entity_id, record_id, operation_id, hash_value, operation_date)
                VALUES (pi_entity_id, pi_record_id, pi_operation_id, pi_hash, SYSTIMESTAMP);

            INSERT INTO sync_logs(
                entity_id, record_id, status_id, operation_id, api_url, api_username, message, log_datetime, payload, hash_value
            ) VALUES (
                pi_entity_id, pi_record_id, 1, pi_operation_id, pi_api_info.url_base || pi_endpoint, pi_api_info.username, NULL, SYSTIMESTAMP, v_payload_json, pi_hash
            );
        END IF;

    EXCEPTION
        WHEN OTHERS THEN
            log_msg := SUBSTR(SQLERRM,1,4000);
            INSERT INTO sync_logs(
                entity_id, record_id, status_id, operation_id, api_url, api_username, message, log_datetime, payload, hash_value
            ) VALUES (
                pi_entity_id, pi_record_id, 2, pi_operation_id, pi_api_info.url_base || pi_endpoint, pi_api_info.username, log_msg, SYSTIMESTAMP, v_payload_json, pi_hash
            );
    END send_to_api;

    -- Função que gera hash de produto
    FUNCTION hash_product(
        p_id   IN NUMBER,
        p_name IN VARCHAR2
    ) RETURN VARCHAR2 IS
        v_concat VARCHAR2(4000);
        v_hash   VARCHAR2(64);
    BEGIN
        v_concat := p_id || '-' || NVL(p_name, '');
        SELECT RAWTOHEX(STANDARD_HASH(v_concat, 'SHA256'))
          INTO v_hash
          FROM dual;
        RETURN v_hash;
    END hash_product;

    -- Procedure que monta JSON seguro do produto
    PROCEDURE send_product(
        p_id              IN NUMBER,
        p_name            IN VARCHAR2,
        p_description     IN VARCHAR2,
        p_category        IN VARCHAR2,
        p_unit_price      IN NUMBER,
        p_stock_qty       IN NUMBER,
        p_unit_of_measure IN VARCHAR2,
        p_manufacturer    IN VARCHAR2,
        p_operation_id    IN NUMBER,
        p_sender_id       IN NUMBER DEFAULT 1,
        p_receiver_id     IN NUMBER DEFAULT 2
    ) IS
        v_inner_obj JSON_OBJECT_T;
        v_hash      VARCHAR2(128);
        v_endpoint  VARCHAR2(4000);
        v_api_info  t_api_info;
    BEGIN
        v_hash := hash_product(p_id, p_name);
        v_api_info := get_api_info_cached(p_sender_id, p_receiver_id);
        v_endpoint := '/Product/Sync/Receive';

        v_inner_obj := JSON_OBJECT_T();
        v_inner_obj.put('id', p_id);
        v_inner_obj.put('name', NVL(p_name,''));
        v_inner_obj.put('description', NVL(p_description,''));
        v_inner_obj.put('category', NVL(p_category,''));
        v_inner_obj.put('unitPrice', NVL(p_unit_price,0));
        v_inner_obj.put('stockQuantity', NVL(p_stock_qty,0));
        v_inner_obj.put('unitOfMeasure', NVL(p_unit_of_measure,''));
        v_inner_obj.put('manufacturer', NVL(p_manufacturer,''));

        send_to_api(1, p_id, p_operation_id, v_inner_obj, p_sender_id, p_receiver_id, v_hash, v_endpoint, v_api_info);
    END send_product;

END sync_pkg;
