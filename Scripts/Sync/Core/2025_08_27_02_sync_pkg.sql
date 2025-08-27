CREATE OR REPLACE PACKAGE sync_pkg AS

    -- Procedure genérica que envia JSON para qualquer entidade
    PROCEDURE send_to_api(
        pi_entity_name  IN VARCHAR2,
        pi_record_id    IN NUMBER,
        pi_payload_json IN CLOB,
        pi_sender_id    IN NUMBER DEFAULT 1,
        pi_receiver_id  IN NUMBER DEFAULT 2
    );

    -- Procedure específica para Product que monta o JSON e chama send_to_api
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
    );

END sync_pkg;
/
