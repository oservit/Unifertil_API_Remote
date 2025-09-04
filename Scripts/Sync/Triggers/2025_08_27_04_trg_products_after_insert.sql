CREATE OR REPLACE TRIGGER trg_products_after_insert
AFTER INSERT ON products
FOR EACH ROW
DECLARE
    v_hash_db   VARCHAR2(256);
BEGIN

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

    -- Só dispara se não existir hash
    IF v_hash_db IS NULL THEN
        sync_pkg.send_product(
            p_id              => :NEW.id,
            p_name            => :NEW.name,
            p_description     => :NEW.description,
            p_category        => :NEW.category,
            p_unit_price      => :NEW.unit_price,
            p_stock_qty       => :NEW.stock_quantity,
            p_unit_of_measure => :NEW.unit_of_measure,
            p_manufacturer    => :NEW.manufacturer,
            p_operation_id    => 1
        );
    END IF;
END;
/
