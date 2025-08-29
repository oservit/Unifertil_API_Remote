CREATE OR REPLACE PACKAGE BODY sync_pkg AS

    -- Procedure que retorna token de autenticação
    PROCEDURE get_auth_token(
        po_token OUT VARCHAR2
    ) IS
        req          UTL_HTTP.req;
        resp         UTL_HTTP.resp;
        buffer       VARCHAR2(32767);
        payload      CLOB;
        payload_raw  RAW(32767);
        v_success    VARCHAR2(10);
        v_message    VARCHAR2(4000);
    BEGIN
        payload := '{"username":"admin","password":"admin123"}';
        payload_raw := UTL_RAW.cast_to_raw(payload);
    
        req := UTL_HTTP.begin_request('http://127.0.0.1:50020/api/Auth/GetToken', 'POST', 'HTTP/1.1');
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
    
        -- Verifica se veio algo
        IF buffer IS NULL THEN
            RAISE_APPLICATION_ERROR(-20001, 'Resposta da API vazia.');
        END IF;
    
        -- Verifica se success = true
        SELECT JSON_VALUE(buffer, '$.success'),
               JSON_VALUE(buffer, '$.message')
          INTO v_success, v_message
          FROM dual;
    
        IF v_success <> 'true' THEN
            RAISE_APPLICATION_ERROR(-20002, 'Erro de autenticação: ' || v_message);
        END IF;
    
        -- Extrai token
        SELECT JSON_VALUE(buffer, '$.data')
          INTO po_token
          FROM dual;
    
    EXCEPTION
        WHEN OTHERS THEN
            RAISE; -- qualquer outro erro vai subir para send_to_api e ser logado
    END get_auth_token;


    -- Procedure genérica que envia JSON para qualquer entidade
    PROCEDURE send_to_api(
        pi_entity_id    IN NUMBER,
        pi_record_id    IN NUMBER,
        pi_operation_id IN NUMBER,
        pi_payload_json IN CLOB,
        pi_sender_id    IN NUMBER DEFAULT 1,
        pi_receiver_id  IN NUMBER DEFAULT 2,
        pi_hash         IN VARCHAR2,
        pi_url          IN VARCHAR2,
        pi_use_auth     IN BOOLEAN DEFAULT TRUE
    ) IS
        req           UTL_HTTP.req;
        resp          UTL_HTTP.resp;
        buffer        VARCHAR2(32767);
        log_msg       VARCHAR2(4000);
        payload_bytes RAW(32767);
        v_token       VARCHAR2(4000);
    BEGIN
        -- Converte CLOB para RAW UTF-8
        payload_bytes := UTL_RAW.cast_to_raw(pi_payload_json);

        req := UTL_HTTP.begin_request(pi_url, 'POST', 'HTTP/1.1');
        UTL_HTTP.set_header(req, 'Content-Type', 'application/json; charset=UTF-8');
        UTL_HTTP.set_header(req, 'Content-Length', TO_CHAR(UTL_RAW.length(payload_bytes)));

        -- Se precisar de autenticação, obtem o token e adiciona no header
        IF pi_use_auth THEN
            get_auth_token(v_token);
            UTL_HTTP.set_header(req, 'Authorization', 'Bearer ' || v_token);
        END IF;

        -- Envia JSON em bytes UTF-8
        UTL_HTTP.write_raw(req, payload_bytes);
        resp := UTL_HTTP.get_response(req);

        -- Lê resposta
        BEGIN
            UTL_HTTP.read_text(resp, buffer, 32767);
        EXCEPTION
            WHEN UTL_HTTP.end_of_body THEN NULL;
        END;

        UTL_HTTP.end_response(resp);

        -- Grava log de sucesso
        INSERT INTO sync_log(
            entity_id, record_id, status_id, operation_id, message, log_datetime, payload, hash_value
        ) VALUES (
            pi_entity_id, pi_record_id, 1, pi_operation_id, NULL, SYSTIMESTAMP, pi_payload_json, pi_hash
        );

    EXCEPTION
        WHEN OTHERS THEN
            log_msg := SUBSTR(SQLERRM,1,4000);
            INSERT INTO sync_log(
                entity_id, record_id, status_id, operation_id, message, log_datetime, payload, hash_value
            ) VALUES (
                pi_entity_id, pi_record_id, 2, pi_operation_id, log_msg, SYSTIMESTAMP, pi_payload_json, pi_hash
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


    -- Procedure que monta JSON seguro usando JSON_OBJECT_T
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
        v_payload_json CLOB;
        v_payload_obj  JSON_OBJECT_T;
        v_inner_obj    JSON_OBJECT_T;
        v_hash         VARCHAR2(128);
        v_url          VARCHAR2(4000);
    BEGIN
        -- Calcula hash e URL específicos do Product
        v_hash := hash_product(p_id, p_name);
        v_url  := 'http://127.0.0.1:50020/api/Product/Sync/Send';

        -- Monta o JSON interno
        v_inner_obj := JSON_OBJECT_T();
        v_inner_obj.put('id', p_id);
        v_inner_obj.put('name', NVL(p_name,''));
        v_inner_obj.put('description', NVL(p_description,''));
        v_inner_obj.put('category', NVL(p_category,''));
        v_inner_obj.put('unitPrice', NVL(p_unit_price,0));
        v_inner_obj.put('stockQuantity', NVL(p_stock_qty,0));
        v_inner_obj.put('unitOfMeasure', NVL(p_unit_of_measure,''));
        v_inner_obj.put('manufacturer', NVL(p_manufacturer,''));

        -- JSON principal
        v_payload_obj := JSON_OBJECT_T();
        v_payload_obj.put('senderId', p_sender_id);
        v_payload_obj.put('receiverId', p_receiver_id);
        v_payload_obj.put('payload', v_inner_obj);

        -- Converte para CLOB
        v_payload_json := v_payload_obj.to_clob;

        -- Chama a procedure genérica passando hash e URL
        send_to_api(1, p_id, p_operation_id, v_payload_json, p_sender_id, p_receiver_id, v_hash, v_url);
    END send_product;

END sync_pkg;
/
COMMIT;
