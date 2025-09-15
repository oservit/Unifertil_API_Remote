CREATE OR REPLACE TRIGGER trg_products_after_update
AFTER UPDATE ON products
FOR EACH ROW
DECLARE
    v_hash_new  VARCHAR2(256);
    v_hash_db   VARCHAR2(256);
BEGIN
    -- Calcula hash dos dados novos
    v_hash_new := sync_pkg.hash_product(
        p_id              => :NEW.id,
        p_name            => :NEW.name
    );

    BEGIN
        -- Busca hash atual na tabela de sincronização
        SELECT hash_value
          INTO v_hash_db
          FROM sync_hashes
         WHERE entity_id = 1
           AND record_id = :NEW.id
         FETCH FIRST 1 ROWS ONLY;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            v_hash_db := NULL;
    END;

    -- Só dispara se for novo ou alterado
    IF v_hash_db IS NULL OR v_hash_db <> v_hash_new THEN
        sync_pkg.send_product(
            p_id              => :NEW.id,
            p_name            => :NEW.name,
            p_description     => :NEW.description,
            p_category        => :NEW.category,
            p_unit_price      => :NEW.unit_price,
            p_stock_qty       => :NEW.stock_quantity,
            p_unit_of_measure => :NEW.unit_of_measure,
            p_manufacturer    => :NEW.manufacturer,
            p_operation_id    => 2,
            p_sender_id       => 2, -- varia conforme o local
            p_receiver_id     => 1 -- varia conforme o local
        );
    END IF;
END;
/
