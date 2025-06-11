-- EngineQueries.sql
-- SQL queries for Engine repository operations

-- GetAllAsync: Get all active engines
SELECT
    id,
    CAST(id AS VARCHAR) AS Id,
    COALESCE(CAST(pojemnosc AS VARCHAR), '') AS Name,
    typ AS type,
    CASE
        WHEN pojemnosc LIKE '%L%' THEN 
            CAST(REPLACE(SUBSTRING(pojemnosc, 1, POSITION('L' IN pojemnosc) - 1), ',', '.') AS FLOAT)::INT * 1000
        WHEN pojemnosc = '—' THEN 0
        WHEN pojemnosc ~ '^[0-9]+$' THEN CAST(pojemnosc AS INT)
        ELSE 0
    END AS Capacity,
    moc AS Power,
    CAST(moc * 1.5 AS INT) AS Torque,
    CASE
        WHEN typ = 'benzyna' THEN 'Benzyna'
        WHEN typ = 'diesel' THEN 'Diesel'
        WHEN typ = 'elektryczny' THEN 'Elektryczny'
        ELSE typ
    END AS Fueltype,
    CASE
        WHEN typ = 'elektryczny' THEN NULL
        ELSE 
            CASE
                WHEN pojemnosc LIKE '%L%' THEN 
                    CAST(SUBSTRING(TRIM(pojemnosc), 1, POSITION('L' IN pojemnosc) - 1) AS FLOAT) * 1000
                ELSE CAST(TRIM(pojemnosc) AS FLOAT)
            END / 500
    END AS Cylinders,
    'Automatyczna' AS Transmission,
    CASE
        WHEN typ = 'elektryczny' THEN 1
        ELSE 6
    END AS Gears,
    CASE
        WHEN EXISTS (SELECT 1 FROM CechyPojazdu cp JOIN Pojazd p ON cp.idPojazdu = p.id WHERE p.id = pojazdid AND cp.Cecha = 'Napęd 4×4')
        THEN 'AWD'
        ELSE 'RWD'
    END AS Drivetype,
    CASE
        WHEN typ = 'elektryczny' THEN 0
        WHEN typ = 'diesel' THEN 5.5
        ELSE 7.2
    END AS FuelConsumption,
    CASE
        WHEN typ = 'elektryczny' THEN 0
        WHEN typ = 'diesel' THEN 145
        ELSE 165
    END AS CO2Emission,
    CASE
        WHEN typ = 'benzyna' THEN 'Silnik benzynowy o dobrej dynamice i wysokiej kulturze pracy'
        WHEN typ = 'diesel' THEN 'Ekonomiczny silnik wysokoprężny z dużym momentem obrotowym'
        WHEN typ = 'elektryczny' THEN 'Napęd elektryczny o zerowej emisji i błyskawicznej dynamice'
        ELSE 'Nowoczesny napęd o dobrych osiągach'
    END AS Description,
    TRUE AS IsActive,
    NOW() - INTERVAL '60 days' * RANDOM() AS CreatedAt,
    NULL AS UpdatedAt
FROM silnik
WHERE 1=1
ORDER BY moc DESC;

-- GetByIdAsync: Get engine by id
SELECT
    id,
    CAST(id AS VARCHAR) AS Id,
    COALESCE(pojemnosc, '') AS Name,
    typ AS type,
    CAST(pojemnosc AS INT) AS Capacity,
    moc AS Power,
    CAST(moc * 1.5 AS INT) AS Torque,
    CASE
        WHEN typ = 'benzyna' THEN 'Benzyna'
        WHEN typ = 'diesel' THEN 'Diesel'
        WHEN typ = 'elektryczny' THEN 'Elektryczny'
        ELSE typ
    END AS Fueltype,
    CASE
        WHEN typ = 'elektryczny' THEN NULL
        ELSE 
            CASE
                WHEN pojemnosc LIKE '%L%' THEN 
                    CAST(SUBSTRING(TRIM(pojemnosc), 1, POSITION('L' IN pojemnosc) - 1) AS FLOAT) * 1000
                ELSE CAST(TRIM(pojemnosc) AS FLOAT)
            END / 500
    END AS Cylinders,
    'Automatyczna' AS Transmission,
    CASE
        WHEN typ = 'elektryczny' THEN 1
        ELSE 6
    END AS Gears,
    CASE
        WHEN EXISTS (SELECT 1 FROM CechyPojazdu cp JOIN Pojazd p ON cp.idPojazdu = p.id WHERE p.id = pojazdid AND cp.Cecha = 'Napęd 4×4')
        THEN 'AWD'
        ELSE 'RWD'
    END AS Drivetype,
    CASE
        WHEN typ = 'elektryczny' THEN 0
        WHEN typ = 'diesel' THEN 5.5
        ELSE 7.2
    END AS FuelConsumption,
    CASE
        WHEN typ = 'elektryczny' THEN 0
        WHEN typ = 'diesel' THEN 145
        ELSE 165
    END AS CO2Emission,
    CASE
        WHEN typ = 'benzyna' THEN 'Silnik benzynowy o dobrej dynamice i wysokiej kulturze pracy'
        WHEN typ = 'diesel' THEN 'Ekonomiczny silnik wysokoprężny z dużym momentem obrotowym'
        WHEN typ = 'elektryczny' THEN 'Napęd elektryczny o zerowej emisji i błyskawicznej dynamice'
        ELSE 'Nowoczesny napęd o dobrych osiągach'
    END AS Description,
    TRUE AS IsActive,
    NOW() - INTERVAL '60 days' * RANDOM() AS CreatedAt,
    NULL AS UpdatedAt
FROM silnik
WHERE id = @Id;

-- GetFilteredAsync: Get filtered engines (Dynamic SQL)
/* 
This will be constructed dynamically in code based on provided filters.
Base query pattern:

SELECT
    id,
    CAST(id AS VARCHAR) AS Id,
    COALESCE(pojemnosc, '') AS Name,
    typ AS type,
    CAST(CASE
        WHEN pojemnosc LIKE '%L%' THEN 
            CAST(SUBSTRING(TRIM(pojemnosc), 1, POSITION('L' IN pojemnosc) - 1) AS FLOAT) * 1000
        WHEN pojemnosc = '—' THEN NULL
        ELSE CAST(TRIM(pojemnosc) AS FLOAT)
    END AS INT) AS Capacity,
    moc AS Power,
    CAST(moc * 1.5 AS INT) AS Torque,
    CASE
        WHEN typ = 'benzyna' THEN 'Benzyna'
        WHEN typ = 'diesel' THEN 'Diesel'
        WHEN typ = 'elektryczny' THEN 'Elektryczny'
        ELSE typ
    END AS Fueltype,
    CASE
        WHEN typ = 'elektryczny' THEN NULL
        ELSE 
            CASE
                WHEN pojemnosc LIKE '%L%' THEN 
                    CAST(SUBSTRING(TRIM(pojemnosc), 1, POSITION('L' IN pojemnosc) - 1) AS FLOAT) * 1000
                ELSE CAST(TRIM(pojemnosc) AS FLOAT)
            END / 500
    END AS Cylinders,
    'Automatyczna' AS Transmission,
    CASE
        WHEN typ = 'elektryczny' THEN 1
        ELSE 6
    END AS Gears,
    CASE
        WHEN EXISTS (SELECT 1 FROM CechyPojazdu cp JOIN Pojazd p ON cp.idPojazdu = p.id WHERE p.id = pojazdid AND cp.Cecha = 'Napęd 4×4')
        THEN 'AWD'
        ELSE 'RWD'
    END AS Drivetype,
    CASE
        WHEN typ = 'elektryczny' THEN 0
        WHEN typ = 'diesel' THEN 5.5
        ELSE 7.2
    END AS FuelConsumption,
    CASE
        WHEN typ = 'elektryczny' THEN 0
        WHEN typ = 'diesel' THEN 145
        ELSE 165
    END AS CO2Emission,
    CASE
        WHEN typ = 'benzyna' THEN 'Silnik benzynowy o dobrej dynamice i wysokiej kulturze pracy'
        WHEN typ = 'diesel' THEN 'Ekonomiczny silnik wysokoprężny z dużym momentem obrotowym'
        WHEN typ = 'elektryczny' THEN 'Napęd elektryczny o zerowej emisji i błyskawicznej dynamice'
        ELSE 'Nowoczesny napęd o dobrych osiągach'
    END AS Description,
    TRUE AS IsActive,
    NOW() - INTERVAL '60 days' * RANDOM() AS CreatedAt,
    NULL AS UpdatedAt
FROM silnik
WHERE 1=1
-- Conditional filters added here dynamically
-- type filter: AND typ = @type
-- Fueltype filter: AND CASE WHEN typ = 'benzyna' THEN 'Benzyna' WHEN typ = 'diesel' THEN 'Diesel' WHEN typ = 'elektryczny' THEN 'Elektryczny' ELSE typ END LIKE '%' || @Fueltype || '%'
-- MinCapacity filter: AND CAST(CASE WHEN pojemnosc LIKE '%L%' THEN CAST(SUBSTRING(TRIM(pojemnosc), 1, POSITION('L' IN pojemnosc) - 1) AS FLOAT) * 1000 WHEN pojemnosc = '—' THEN NULL ELSE CAST(TRIM(pojemnosc) AS FLOAT) END AS INT) >= @MinCapacity
-- MaxCapacity filter: AND CAST(CASE WHEN pojemnosc LIKE '%L%' THEN CAST(SUBSTRING(TRIM(pojemnosc), 1, POSITION('L' IN pojemnosc) - 1) AS FLOAT) * 1000 WHEN pojemnosc = '—' THEN NULL ELSE CAST(TRIM(pojemnosc) AS FLOAT) END AS INT) <= @MaxCapacity
-- MinPower filter: AND moc >= @MinPower
-- MaxPower filter: AND moc <= @MaxPower
-- Transmission filter: AND 'Automatyczna' = @Transmission
-- Drivetype filter: AND CASE WHEN EXISTS (SELECT 1 FROM CechyPojazdu cp JOIN Pojazd p ON cp.idPojazdu = p.id WHERE p.id = pojazdid AND cp.Cecha = 'Napęd 4×4') THEN 'AWD' ELSE 'RWD' END = @Drivetype
-- IsActive filter: AND TRUE = @IsActive
ORDER BY moc DESC;
*/

-- CreateAsync: Insert new engine
INSERT INTO silnik (pojazdid, pojemnosc, typ, moc)
VALUES (1, @Capacity, @type, @Power)
RETURNING 
    id,
    CAST(id AS VARCHAR) AS Id,
    COALESCE(pojemnosc, '') AS Name,
    typ AS type,
    CAST(pojemnosc AS INT) AS Capacity,
    moc AS Power,
    CAST(moc * 1.5 AS INT) AS Torque,
    CASE
        WHEN typ = 'benzyna' THEN 'Benzyna'
        WHEN typ = 'diesel' THEN 'Diesel'
        WHEN typ = 'elektryczny' THEN 'Elektryczny'
        ELSE typ
    END AS Fueltype,
    CASE
        WHEN typ = 'elektryczny' THEN NULL
        ELSE 
            CASE
                WHEN pojemnosc LIKE '%L%' THEN 
                    CAST(SUBSTRING(TRIM(pojemnosc), 1, POSITION('L' IN pojemnosc) - 1) AS FLOAT) * 1000
                ELSE CAST(TRIM(pojemnosc) AS FLOAT)
            END / 500
    END AS Cylinders,
    'Automatyczna' AS Transmission,
    CASE
        WHEN typ = 'elektryczny' THEN 1
        ELSE 6
    END AS Gears,
    'RWD' AS Drivetype,
    CASE
        WHEN typ = 'elektryczny' THEN 0
        WHEN typ = 'diesel' THEN 5.5
        ELSE 7.2
    END AS FuelConsumption,
    CASE
        WHEN typ = 'elektryczny' THEN 0
        WHEN typ = 'diesel' THEN 145
        ELSE 165
    END AS CO2Emission,
    CASE
        WHEN typ = 'benzyna' THEN 'Silnik benzynowy o dobrej dynamice i wysokiej kulturze pracy'
        WHEN typ = 'diesel' THEN 'Ekonomiczny silnik wysokoprężny z dużym momentem obrotowym'
        WHEN typ = 'elektryczny' THEN 'Napęd elektryczny o zerowej emisji i błyskawicznej dynamice'
        ELSE 'Nowoczesny napęd o dobrych osiągach'
    END AS Description,
    TRUE AS IsActive,
    NOW() AS CreatedAt,
    NULL AS UpdatedAt;

-- UpdateAsync: Update engine
UPDATE silnik
SET
    pojemnosc = @Capacity,
    typ = @type,
    moc = @Power
WHERE id = @Id
RETURNING 
    id,
    CAST(id AS VARCHAR) AS Id,
    COALESCE(pojemnosc, '') AS Name,
    typ AS type,
    CAST(pojemnosc AS INT) AS Capacity,
    moc AS Power,
    CAST(moc * 1.5 AS INT) AS Torque,
    CASE
        WHEN typ = 'benzyna' THEN 'Benzyna'
        WHEN typ = 'diesel' THEN 'Diesel'
        WHEN typ = 'elektryczny' THEN 'Elektryczny'
        ELSE typ
    END AS Fueltype,
    CASE
        WHEN typ = 'elektryczny' THEN NULL
        ELSE 
            CASE
                WHEN pojemnosc LIKE '%L%' THEN 
                    CAST(SUBSTRING(TRIM(pojemnosc), 1, POSITION('L' IN pojemnosc) - 1) AS FLOAT) * 1000
                ELSE CAST(TRIM(pojemnosc) AS FLOAT)
            END / 500
    END AS Cylinders,
    'Automatyczna' AS Transmission,
    CASE
        WHEN typ = 'elektryczny' THEN 1
        ELSE 6
    END AS Gears,
    CASE
        WHEN EXISTS (SELECT 1 FROM CechyPojazdu cp JOIN Pojazd p ON cp.idPojazdu = p.id WHERE p.id = pojazdid AND cp.Cecha = 'Napęd 4×4')
        THEN 'AWD'
        ELSE 'RWD'
    END AS Drivetype,
    CASE
        WHEN typ = 'elektryczny' THEN 0
        WHEN typ = 'diesel' THEN 5.5
        ELSE 7.2
    END AS FuelConsumption,
    CASE
        WHEN typ = 'elektryczny' THEN 0
        WHEN typ = 'diesel' THEN 145
        ELSE 165
    END AS CO2Emission,
    CASE
        WHEN typ = 'benzyna' THEN 'Silnik benzynowy o dobrej dynamice i wysokiej kulturze pracy'
        WHEN typ = 'diesel' THEN 'Ekonomiczny silnik wysokoprężny z dużym momentem obrotowym'
        WHEN typ = 'elektryczny' THEN 'Napęd elektryczny o zerowej emisji i błyskawicznej dynamice'
        ELSE 'Nowoczesny napęd o dobrych osiągach'
    END AS Description,
    TRUE AS IsActive,
    NOW() - INTERVAL '60 days' * RANDOM() AS CreatedAt,
    NOW() AS UpdatedAt;

-- DeleteAsync: Delete engine
DELETE FROM silnik
WHERE id = @Id;

-- Check if engine is used by any vehicles
SELECT COUNT(*) AS Count
FROM Pojazd
WHERE id IN (SELECT pojazdid FROM silnik WHERE id = @Id);

-- GetAllByVechicleIdAsync: Get all engines by vehicle id
SELECT
    s.id,
    CAST(s.id AS VARCHAR) AS Id,
    COALESCE(s.pojemnosc, '') AS Name,
    s.typ AS type,
    CAST(s.pojemnosc AS INT) AS Capacity,
    s.moc AS Power,
    CAST(s.moc * 1.5 AS INT) AS Torque,
    CASE
        WHEN s.typ = 'benzyna' THEN 'Benzyna'
        WHEN s.typ = 'diesel' THEN 'Diesel'
        WHEN s.typ = 'elektryczny' THEN 'Elektryczny'
        ELSE s.typ
    END AS Fueltype,
    CASE
        WHEN s.typ = 'elektryczny' THEN NULL
        ELSE 
            CASE
                WHEN s.pojemnosc LIKE '%L%' THEN 
                    CAST(SUBSTRING(TRIM(s.pojemnosc), 1, POSITION('L' IN s.pojemnosc) - 1) AS FLOAT) * 1000
                ELSE CAST(TRIM(s.pojemnosc) AS FLOAT)
            END / 500
    END AS Cylinders,
    'Automatyczna' AS Transmission,
    CASE
        WHEN s.typ = 'elektryczny' THEN 1
        ELSE 6
    END AS Gears,
    CASE
        WHEN EXISTS (SELECT 1 FROM CechyPojazdu cp JOIN Pojazd p ON cp.idPojazdu = p.id WHERE p.id = s.pojazdid AND cp.Cecha = 'Napęd 4×4')
        THEN 'AWD'
        ELSE 'RWD'
    END AS Drivetype,
    CASE
        WHEN s.typ = 'elektryczny' THEN 0
        WHEN s.typ = 'diesel' THEN 5.5
        ELSE 7.2
    END AS FuelConsumption,
    CASE
        WHEN s.typ = 'elektryczny' THEN 0
        WHEN s.typ = 'diesel' THEN 145
        ELSE 165
    END AS CO2Emission,
    CASE
        WHEN s.typ = 'benzyna' THEN 'Silnik benzynowy o dobrej dynamice i wysokiej kulturze pracy'
        WHEN s.typ = 'diesel' THEN 'Ekonomiczny silnik wysokoprężny z dużym momentem obrotowym'
        WHEN s.typ = 'elektryczny' THEN 'Napęd elektryczny o zerowej emisji i błyskawicznej dynamice'
        ELSE 'Nowoczesny napęd o dobrych osiągach'
    END AS Description,
    TRUE AS IsActive,
    NOW() - INTERVAL '60 days' * RANDOM() AS CreatedAt,
    NULL AS UpdatedAt
FROM silnik s
JOIN Pojazd p ON s.pojazdid = p.id
WHERE p.id = @VehicleId
ORDER BY s.moc DESC;