CREATE OR REPLACE NONEDITIONABLE PACKAGE sync_pkg AS

    -- Tipo para armazenar informa��es da API
    TYPE t_api_info IS RECORD (
        url_base VARCHAR2(4000),
        username VARCHAR2(100),
        password VARCHAR2(100)
    );

    -- Fun��o que retorna API info cacheada
    FUNCTION get_api_info_cached(
        p_sender_id   IN NUMBER,
        p_receiver_id IN NUMBER
    ) RETURN t_api_info;

    -- Procedure que retorna token de autentica��o
    PROCEDURE get_auth_token(
        po_token     OUT VARCHAR2,
        pi_api_info  IN t_api_info
    );

    -- Procedure gen�rica que envia JSON para qualquer entidade
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
    );

    -- Fun��o que gera hash de produto
    FUNCTION hash_product(
        p_id   IN NUMBER,
        p_name IN VARCHAR2
    ) RETURN VARCHAR2;

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
    );

END sync_pkg;
