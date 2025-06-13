/* ===========================================================
   ROZSZERZENIE BAZY DANYCH - LOGI, TRIGGERY, PROCEDURY I FUNKCJE
   ===========================================================*/

-- 1. TABELA LOGÓW SYSTEMOWYCH
CREATE TABLE IF NOT EXISTS system_logs (
    id SERIAL PRIMARY KEY,
    event_type VARCHAR(50) NOT NULL,
    table_name VARCHAR(100),
    record_id VARCHAR(255),
    user_id INTEGER,
    user_email VARCHAR(100),
    action VARCHAR(50) NOT NULL,
    old_values JSONB,
    new_values JSONB,
    ip_address VARCHAR(45),
    user_agent TEXT,
    description TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    severity VARCHAR(20) DEFAULT 'INFO',
    CONSTRAINT check_severity CHECK (severity IN ('DEBUG', 'INFO', 'WARNING', 'ERROR', 'CRITICAL'))
);

-- Indeksy dla lepszej wydajności
CREATE INDEX idx_system_logs_event_type ON system_logs(event_type);
CREATE INDEX idx_system_logs_user_id ON system_logs(user_id);
CREATE INDEX idx_system_logs_created_at ON system_logs(created_at);
CREATE INDEX idx_system_logs_table_name ON system_logs(table_name);
CREATE INDEX idx_system_logs_severity ON system_logs(severity);

/* ===========================================================
   2. FUNKCJE
   ===========================================================*/

-- FUNKCJA 1: Obliczanie całkowitej ceny konfiguracji
CREATE OR REPLACE FUNCTION calculate_configuration_total_price(
    p_car_model_id VARCHAR(255),
    p_engine_id VARCHAR(255),
    p_color VARCHAR(50),
    p_accessories TEXT,
    p_interior_equipment TEXT
) RETURNS DECIMAL(12, 2) AS $$
DECLARE
    v_base_price DECIMAL(12, 2) := 0;
    v_engine_price DECIMAL(12, 2) := 0;
    v_color_price DECIMAL(12, 2) := 0;
    v_accessories_price DECIMAL(12, 2) := 0;
    v_interior_price DECIMAL(12, 2) := 0;
    v_total DECIMAL(12, 2) := 0;
    v_accessory_id VARCHAR(255);
    v_interior_id VARCHAR(255);
BEGIN
    -- Cena bazowa modelu
    SELECT COALESCE(Cena, 0) INTO v_base_price
    FROM Pojazd
    WHERE ID::VARCHAR = p_car_model_id;
    
    -- Cena silnika
    SELECT COALESCE(ms.CenaDodatkowa, 0) INTO v_engine_price
    FROM ModelSilnik ms
    INNER JOIN "Silnik" s ON s."ID" = ms.SilnikID
    WHERE ms.ModelID::VARCHAR = p_car_model_id 
    AND s."ID"::VARCHAR = p_engine_id;
    
    -- Cena koloru
    SELECT COALESCE("Price", 0) INTO v_color_price
    FROM car_model_colors
    WHERE "CarModelId" = p_car_model_id 
    AND "ColorName" = p_color;
    
    -- Suma cen akcesoriów
    IF p_accessories IS NOT NULL AND p_accessories != '' THEN
        FOR v_accessory_id IN
            SELECT unnest(string_to_array(p_accessories, ','))
        LOOP
            v_accessories_price := v_accessories_price + (
                SELECT COALESCE(price, 0)
                FROM car_accessories
                WHERE id = v_accessory_id
            );
        END LOOP;
    END IF;
    
    -- Suma cen wyposażenia wnętrza
    IF p_interior_equipment IS NOT NULL AND p_interior_equipment != '' THEN
        FOR v_interior_id IN
            SELECT unnest(string_to_array(p_interior_equipment, ','))
        LOOP
            v_interior_price := v_interior_price + (
                SELECT COALESCE(additional_price, 0)
                FROM car_interior_equipment
                WHERE id = v_interior_id
            );
        END LOOP;
    END IF;
    
    v_total := v_base_price + v_engine_price + v_color_price + v_accessories_price + v_interior_price;
    
    RETURN v_total;
END;
$$ LANGUAGE plpgsql;

-- FUNKCJA 2: Sprawdzanie dostępności akcesoriów
CREATE OR REPLACE FUNCTION check_accessories_availability(
    p_accessory_ids TEXT
) RETURNS TABLE (
    accessory_id VARCHAR(255),
    name VARCHAR(255),
    is_available BOOLEAN,
    stock_quantity INT,
    message TEXT
) AS $$
DECLARE
    v_accessory_id VARCHAR(255);
BEGIN
    FOR v_accessory_id IN
        SELECT unnest(string_to_array(p_accessory_ids, ','))
    LOOP
        RETURN QUERY
        SELECT 
            ca.id,
            ca.name,
            ca.is_in_stock AND ca.stock_quantity > 0,
            ca.stock_quantity,
            CASE 
                WHEN NOT ca.is_in_stock THEN 'Produkt niedostępny'
                WHEN ca.stock_quantity = 0 THEN 'Brak w magazynie'
                WHEN ca.stock_quantity < 5 THEN 'Ostatnie sztuki (' || ca.stock_quantity || ')'
                ELSE 'Dostępny'
            END
        FROM car_accessories ca
        WHERE ca.id = v_accessory_id;
    END LOOP;
END;
$$ LANGUAGE plpgsql;

-- FUNKCJA 3: Generowanie statystyk użytkownika
CREATE OR REPLACE FUNCTION get_user_statistics(
    p_user_id INTEGER
) RETURNS TABLE (
    total_configurations INT,
    active_configurations INT,
    total_spent DECIMAL(12, 2),
    favorite_brand VARCHAR(255),
    last_configuration_date TIMESTAMP WITH TIME ZONE,
    average_configuration_price DECIMAL(12, 2)
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        COUNT(*)::INT AS total_configurations,
        COUNT(*) FILTER (WHERE is_active = true)::INT AS active_configurations,
        COALESCE(SUM(total_price), 0) AS total_spent,
        (
            SELECT car_model_name 
            FROM user_configurations uc2 
            WHERE uc2.user_id = p_user_id 
            GROUP BY car_model_name 
            ORDER BY COUNT(*) DESC 
            LIMIT 1
        ) AS favorite_brand,
        MAX(created_at) AS last_configuration_date,
        COALESCE(AVG(total_price), 0) AS average_configuration_price
    FROM user_configurations
    WHERE user_id = p_user_id;
END;
$$ LANGUAGE plpgsql;

/* ===========================================================
   3. TRIGGERY
   ===========================================================*/

-- TRIGGER 1: Logowanie zmian w konfiguracjach użytkownika
CREATE OR REPLACE FUNCTION log_user_configuration_changes()
RETURNS TRIGGER AS $$
BEGIN
    IF TG_OP = 'INSERT' THEN
        INSERT INTO system_logs (
            event_type, table_name, record_id, user_id, action,
            new_values, description, severity
        ) VALUES (
            'CONFIGURATION_CREATED', 'user_configurations', NEW.id::VARCHAR, NEW.user_id, 'INSERT',
            row_to_json(NEW)::JSONB,
            'Utworzono nową konfigurację: ' || NEW.configuration_name,
            'INFO'
        );
    ELSIF TG_OP = 'UPDATE' THEN
        IF OLD.is_active = true AND NEW.is_active = false THEN
            INSERT INTO system_logs (
                event_type, table_name, record_id, user_id, action,
                old_values, new_values, description, severity
            ) VALUES (
                'CONFIGURATION_DELETED', 'user_configurations', NEW.id::VARCHAR, NEW.user_id, 'UPDATE',
                row_to_json(OLD)::JSONB, row_to_json(NEW)::JSONB,
                'Usunięto konfigurację: ' || OLD.configuration_name,
                'WARNING'
            );
        ELSE
            INSERT INTO system_logs (
                event_type, table_name, record_id, user_id, action,
                old_values, new_values, description, severity
            ) VALUES (
                'CONFIGURATION_UPDATED', 'user_configurations', NEW.id::VARCHAR, NEW.user_id, 'UPDATE',
                row_to_json(OLD)::JSONB, row_to_json(NEW)::JSONB,
                'Zaktualizowano konfigurację: ' || NEW.configuration_name,
                'INFO'
            );
        END IF;
    ELSIF TG_OP = 'DELETE' THEN
        INSERT INTO system_logs (
            event_type, table_name, record_id, user_id, action,
            old_values, description, severity
        ) VALUES (
            'CONFIGURATION_DELETED', 'user_configurations', OLD.id::VARCHAR, OLD.user_id, 'DELETE',
            row_to_json(OLD)::JSONB,
            'Trwale usunięto konfigurację: ' || OLD.configuration_name,
            'WARNING'
        );
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_log_user_configuration_changes
AFTER INSERT OR UPDATE OR DELETE ON user_configurations
FOR EACH ROW EXECUTE FUNCTION log_user_configuration_changes();

-- TRIGGER 2: Automatyczna aktualizacja stanu magazynowego akcesoriów
CREATE OR REPLACE FUNCTION update_accessory_stock()
RETURNS TRIGGER AS $$
DECLARE
    v_accessory_id VARCHAR(255);
    v_accessory_ids TEXT[];
BEGIN
    -- Rozdziel listę akcesoriów na tablicę
    IF NEW.selected_accessories IS NOT NULL AND NEW.selected_accessories != '' THEN
        v_accessory_ids := string_to_array(NEW.selected_accessories, ',');
        
        -- Dla każdego akcesorium zmniejsz stan magazynowy
        FOREACH v_accessory_id IN ARRAY v_accessory_ids
        LOOP
            UPDATE car_accessories
            SET stock_quantity = stock_quantity - 1
            WHERE id = v_accessory_id 
            AND stock_quantity > 0;
            
            -- Loguj zmianę stanu magazynowego
            INSERT INTO system_logs (
                event_type, table_name, record_id, user_id, action,
                description, severity
            ) VALUES (
                'STOCK_UPDATE', 'car_accessories', v_accessory_id, NEW.user_id, 'UPDATE',
                'Zmniejszono stan magazynowy akcesorium o 1 sztukę',
                'INFO'
            );
            
            -- Sprawdź czy stan jest niski
            IF (SELECT stock_quantity FROM car_accessories WHERE id = v_accessory_id) < 5 THEN
                INSERT INTO system_logs (
                    event_type, table_name, record_id, user_id, action,
                    description, severity
                ) VALUES (
                    'LOW_STOCK_WARNING', 'car_accessories', v_accessory_id, NEW.user_id, 'WARNING',
                    'Niski stan magazynowy akcesorium (poniżej 5 sztuk)',
                    'WARNING'
                );
            END IF;
        END LOOP;
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_update_accessory_stock
AFTER INSERT ON user_configurations
FOR EACH ROW EXECUTE FUNCTION update_accessory_stock();

-- TRIGGER 3: Logowanie prób logowania
CREATE OR REPLACE FUNCTION log_login_attempts()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO system_logs (
        event_type, table_name, record_id, user_email, action,
        ip_address, description, severity
    ) VALUES (
        CASE 
            WHEN NEW.Status = 'Sukces' THEN 'LOGIN_SUCCESS'
            ELSE 'LOGIN_FAILED'
        END,
        'Logowanie',
        NEW.ID::VARCHAR,
        (SELECT Email FROM Uzytkownik WHERE ID = NEW.IDUzytkownika),
        'LOGIN',
        NEW.AdresIP,
        CASE 
            WHEN NEW.Status = 'Sukces' THEN 'Pomyślne logowanie użytkownika'
            ELSE 'Nieudana próba logowania'
        END,
        CASE 
            WHEN NEW.Status = 'Sukces' THEN 'INFO'
            ELSE 'WARNING'
        END
    );
    
    -- Sprawdź liczbę nieudanych prób logowania w ostatnich 30 minutach
    IF NEW.Status = 'Niepowodzenie' THEN
        IF (
            SELECT COUNT(*) 
            FROM Logowanie 
            WHERE IDUzytkownika = NEW.IDUzytkownika 
            AND Status = 'Niepowodzenie'
            AND DataLogowania > NOW() - INTERVAL '30 minutes'
        ) >= 5 THEN
            INSERT INTO system_logs (
                event_type, table_name, user_id, action,
                description, severity
            ) VALUES (
                'SECURITY_ALERT', 'Logowanie', NEW.IDUzytkownika, 'ALERT',
                'Wykryto 5 lub więcej nieudanych prób logowania w ciągu 30 minut',
                'CRITICAL'
            );
        END IF;
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_log_login_attempts
AFTER INSERT ON Logowanie
FOR EACH ROW EXECUTE FUNCTION log_login_attempts();

/* ===========================================================
   4. PROCEDURY
   ===========================================================*/

-- PROCEDURA 1: Czyszczenie starych logów
CREATE OR REPLACE PROCEDURE clean_old_logs(
    p_days_to_keep INTEGER DEFAULT 90,
    p_severity_to_keep VARCHAR(20) DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_deleted_count INTEGER;
BEGIN
    -- Usuń stare logi
    DELETE FROM system_logs
    WHERE created_at < CURRENT_TIMESTAMP - (p_days_to_keep || ' days')::INTERVAL
    AND (p_severity_to_keep IS NULL OR severity != p_severity_to_keep);
    
    GET DIAGNOSTICS v_deleted_count = ROW_COUNT;
    
    -- Zaloguj operację czyszczenia
    INSERT INTO system_logs (
        event_type, action, description, severity
    ) VALUES (
        'MAINTENANCE', 'CLEANUP',
        'Usunięto ' || v_deleted_count || ' starych wpisów logów',
        'INFO'
    );
    
    -- Analizuj tabelę po czyszczeniu
    ANALYZE system_logs;
    
    RAISE NOTICE 'Usunięto % starych wpisów logów', v_deleted_count;
END;
$$;

-- PROCEDURA 2: Generowanie raportu konfiguracji
CREATE OR REPLACE PROCEDURE generate_configuration_report(
    p_start_date DATE,
    p_end_date DATE,
    OUT p_total_configurations INTEGER,
    OUT p_total_revenue DECIMAL(12, 2),
    OUT p_most_popular_model VARCHAR(255),
    OUT p_average_price DECIMAL(12, 2)
)
LANGUAGE plpgsql
AS $$
BEGIN
    -- Liczba konfiguracji
    SELECT COUNT(*) INTO p_total_configurations
    FROM user_configurations
    WHERE created_at::DATE BETWEEN p_start_date AND p_end_date
    AND is_active = true;
    
    -- Całkowity przychód
    SELECT COALESCE(SUM(total_price), 0) INTO p_total_revenue
    FROM user_configurations
    WHERE created_at::DATE BETWEEN p_start_date AND p_end_date
    AND is_active = true;
    
    -- Najpopularniejszy model
    SELECT car_model_name INTO p_most_popular_model
    FROM user_configurations
    WHERE created_at::DATE BETWEEN p_start_date AND p_end_date
    AND is_active = true
    GROUP BY car_model_name
    ORDER BY COUNT(*) DESC
    LIMIT 1;
    
    -- Średnia cena
    SELECT COALESCE(AVG(total_price), 0) INTO p_average_price
    FROM user_configurations
    WHERE created_at::DATE BETWEEN p_start_date AND p_end_date
    AND is_active = true;
    
    -- Zaloguj generowanie raportu
    INSERT INTO system_logs (
        event_type, action, description, severity
    ) VALUES (
        'REPORT_GENERATED', 'REPORT',
        'Wygenerowano raport konfiguracji za okres ' || p_start_date || ' - ' || p_end_date,
        'INFO'
    );
END;
$$;

-- PROCEDURA 3: Archiwizacja nieaktywnych konfiguracji
CREATE OR REPLACE PROCEDURE archive_inactive_configurations(
    p_days_inactive INTEGER DEFAULT 365
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_archived_count INTEGER;
    v_config RECORD;
BEGIN
    -- Utwórz tabelę archiwum jeśli nie istnieje
    CREATE TABLE IF NOT EXISTS archived_user_configurations (
        LIKE user_configurations INCLUDING ALL,
        archived_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
    );
    
    -- Przenieś stare konfiguracje do archiwum
    FOR v_config IN 
        SELECT * FROM user_configurations
        WHERE is_active = false
        AND updated_at < CURRENT_TIMESTAMP - (p_days_inactive || ' days')::INTERVAL
    LOOP
        -- Wstaw do archiwum
        INSERT INTO archived_user_configurations
        SELECT v_config.*, CURRENT_TIMESTAMP;
        
        -- Usuń z głównej tabeli
        DELETE FROM user_configurations WHERE id = v_config.id;
        
        -- Zaloguj archiwizację
        INSERT INTO system_logs (
            event_type, table_name, record_id, user_id, action,
            description, severity
        ) VALUES (
            'CONFIGURATION_ARCHIVED', 'user_configurations', 
            v_config.id::VARCHAR, v_config.user_id, 'ARCHIVE',
            'Zarchiwizowano nieaktywną konfigurację: ' || v_config.configuration_name,
            'INFO'
        );
    END LOOP;
    
    GET DIAGNOSTICS v_archived_count = ROW_COUNT;
    
    -- Podsumowanie
    INSERT INTO system_logs (
        event_type, action, description, severity
    ) VALUES (
        'MAINTENANCE', 'ARCHIVE',
        'Zarchiwizowano ' || v_archived_count || ' nieaktywnych konfiguracji',
        'INFO'
    );
    
    RAISE NOTICE 'Zarchiwizowano % nieaktywnych konfiguracji', v_archived_count;
END;
$$;

/* ===========================================================
   5. UPRAWNIENIA
   ===========================================================*/

-- Nadaj odpowiednie uprawnienia
GRANT SELECT, INSERT ON system_logs TO PUBLIC;
GRANT EXECUTE ON FUNCTION calculate_configuration_total_price TO PUBLIC;
GRANT EXECUTE ON FUNCTION check_accessories_availability TO PUBLIC;
GRANT EXECUTE ON FUNCTION get_user_statistics TO PUBLIC;

/* ===========================================================
   6. PRZYKŁADY UŻYCIA
   ===========================================================*/

-- Przykład wywołania funkcji obliczania ceny
-- SELECT calculate_configuration_total_price('model-123', 'engine-456', 'Niebieski', 'acc1,acc2', 'int1,int2');

-- Przykład sprawdzania dostępności akcesoriów
-- SELECT * FROM check_accessories_availability('acc1,acc2,acc3');

-- Przykład pobierania statystyk użytkownika
-- SELECT * FROM get_user_statistics(1);

-- Przykład wywołania procedury czyszczenia logów
-- CALL clean_old_logs(90, 'CRITICAL');

-- Przykład generowania raportu
-- CALL generate_configuration_report('2024-01-01', '2024-12-31', NULL, NULL, NULL, NULL);

-- Przykład archiwizacji
-- CALL archive_inactive_configurations(365);