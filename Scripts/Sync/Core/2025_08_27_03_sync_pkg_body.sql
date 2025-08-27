CREATE OR REPLACE PACKAGE BODY sync_pkg AS

    -- Procedure genérica que envia JSON para qualquer entidade
    PROCEDURE send_to_api(
        pi_entity_name  IN VARCHAR2,
        pi_record_id    IN NUMBER,
        pi_payload_json IN CLOB,
        pi_sender_id    IN NUMBER DEFAULT 1,
        pi_receiver_id  IN NUMBER DEFAULT 2
    ) IS
        req       UTL_HTTP.req;
        resp      UTL_HTTP.resp;
        buffer    VARCHAR2(32767);
        log_msg   VARCHAR2(4000);
        url       VARCHAR2(4000);
        payload_bytes RAW(32767);
    BEGIN
        url := 'http://127.0.0.1:50020/api/' || pi_entity_name || '/Sync/Send';

        -- Converte CLOB para RAW UTF-8
        payload_bytes := UTL_RAW.cast_to_raw(pi_payload_json);

        req := UTL_HTTP.begin_request(url, 'POST', 'HTTP/1.1');
        UTL_HTTP.set_header(req, 'Content-Type', 'application/json; charset=UTF-8');
        UTL_HTTP.set_header(req, 'Content-Length', TO_CHAR(UTL_RAW.length(payload_bytes)));

        -- Log antes de enviar
        log_msg := SUBSTR(pi_payload_json,1,4000);
        INSERT INTO sync_log(entity_name, record_id, status, message, log_datetime, payload)
        VALUES (pi_entity_name, pi_record_id, 'SENDING', log_msg, SYSTIMESTAMP, pi_payload_json);

        -- Envia JSON em bytes UTF-8
        UTL_HTTP.write_raw(req, payload_bytes);
        resp := UTL_HTTP.get_response(req);

        BEGIN
            -- Lê resposta
            UTL_HTTP.read_text(resp, buffer, 32767);
            log_msg := SUBSTR(buffer,1,4000);

            INSERT INTO sync_log(entity_name, record_id, status, message, log_datetime, payload)
            VALUES (pi_entity_name, pi_record_id, 'SUCCESS', log_msg, SYSTIMESTAMP, pi_payload_json);

        EXCEPTION
            WHEN UTL_HTTP.end_of_body THEN NULL;
        END;

        UTL_HTTP.end_response(resp);

    EXCEPTION
        WHEN OTHERS THEN
            log_msg := SUBSTR(SQLERRM,1,4000);
            INSERT INTO sync_log(entity_name, record_id, status, message, log_datetime, payload)
            VALUES (pi_entity_name, pi_record_id, 'ERROR', log_msg, SYSTIMESTAMP, pi_payload_json);
    END send_to_api;


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
        p_sender_id       IN NUMBER DEFAULT 1,
        p_receiver_id     IN NUMBER DEFAULT 2
    ) IS
        v_payload_json CLOB;
        v_payload_obj  JSON_OBJECT_T;
        v_inner_obj    JSON_OBJECT_T;
    BEGIN
        -- Monta o JSON interno
        v_inner_obj := JSON_OBJECT_T();
        v_inner_obj.put('id', p_id);
        v_inner_obj.put('name', NVL(p_name,''));
        v_inner_obj.put('description', NVL(p_description,'')); -- garante string vazia, não NULL
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

        -- Chama a procedure genérica
        send_to_api('Product', p_id, v_payload_json, p_sender_id, p_receiver_id);
    END send_product;

END sync_pkg;
/
COMMIT;
