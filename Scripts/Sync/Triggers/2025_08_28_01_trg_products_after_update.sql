CREATE OR REPLACE TRIGGER trg_products_after_update
AFTER UPDATE ON products
FOR EACH ROW
BEGIN
    sync_pkg.send_product(
        p_id              => :NEW.id,
        p_name            => :NEW.name,
        p_description     => :NEW.description,
        p_category        => :NEW.category,
        p_unit_price      => :NEW.unit_price,
        p_stock_qty       => :NEW.stock_quantity,
        p_unit_of_measure => :NEW.unit_of_measure,
        p_manufacturer    => :NEW.manufacturer,
        p_operation_id    => 2
    );
END;
/
