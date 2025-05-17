-- EngineQueries.sql
-- SQL queries for Engine repository operations

-- GetAllAsync: Get all active engines
SELECT
    ID,
    CAST(ID AS VARCHAR) AS Id,
    COALESCE(Pojemnosc, '') AS Name,
    Typ AS Type,
    CAST(Pojemnosc AS INT) AS Capacity,
    Moc AS Power,
    CAST(Moc * 1.5 AS INT) AS Torque,
    CASE
        WHEN Typ = 'benzyna' THEN 'Benzyna'
        WHEN Typ = 'diesel' THEN 'Diesel'
        WHEN Typ = 'elektryczny' THEN 'Elektryczny'
        ELSE Typ
    END AS FuelType,
    CASE
        WHEN Typ = 'elektryczny' THEN NULL
        ELSE 
            CASE
                WHEN Pojemnosc LIKE '%L%' THEN 
                    CAST(SUBSTRING(TRIM(Pojemnosc), 1, POSITION('L' IN Pojemnosc) - 1) AS FLOAT) * 1000
                ELSE CAST(TRIM(Pojemnosc) AS FLOAT)
            END / 500
    END AS Cylinders,
    'Automatyczna' AS Transmission,
    CASE
        WHEN Typ = 'elektryczny' THEN 1
        ELSE 6
    END AS Gears,
    CASE
        WHEN EXISTS (SELECT 1 FROM CechyPojazdu cp JOIN Pojazd p ON cp.IDPojazdu = p.ID WHERE p.ID = PojazdID AND cp.Cecha = 'Napęd 4×4')
        THEN 'AWD'
        ELSE 'RWD'
    END AS DriveType,
    CASE
        WHEN Typ = 'elektryczny' THEN 0
        WHEN Typ = 'diesel' THEN 5.5
        ELSE 7.2
    END AS FuelConsumption,
    CASE
        WHEN Typ = 'elektryczny' THEN 0
        WHEN Typ = 'diesel' THEN 145
        ELSE 165
    END AS CO2Emission,
    CASE
        WHEN Typ = 'benzyna' THEN 'Silnik benzynowy o dobrej dynamice i wysokiej kulturze pracy'
        WHEN Typ = 'diesel' THEN 'Ekonomiczny silnik wysokoprężny z dużym momentem obrotowym'
        WHEN Typ = 'elektryczny' THEN 'Napęd elektryczny o zerowej emisji i błyskawicznej dynamice'
        ELSE 'Nowoczesny napęd o dobrych osiągach'
    END AS Description,
    TRUE AS IsActive,
    NOW() - INTERVAL '60 days' * RANDOM() AS CreatedAt,
    NULL AS UpdatedAt
FROM Silnik
WHERE 1=1
ORDER BY Moc DESC;

-- GetByIdAsync: Get engine by ID
SELECT
    ID,
    CAST(ID AS VARCHAR) AS Id,
    COALESCE(Pojemnosc, '') AS Name,
    Typ AS Type,
    CAST(Pojemnosc AS INT) AS Capacity,
    Moc AS Power,
    CAST(Moc * 1.5 AS INT) AS Torque,
    CASE
        WHEN Typ = 'benzyna' THEN 'Benzyna'
        WHEN Typ = 'diesel' THEN 'Diesel'
        WHEN Typ = 'elektryczny' THEN 'Elektryczny'
        ELSE Typ
    END AS FuelType,
    CASE
        WHEN Typ = 'elektryczny' THEN NULL
        ELSE 
            CASE
                WHEN Pojemnosc LIKE '%L%' THEN 
                    CAST(SUBSTRING(TRIM(Pojemnosc), 1, POSITION('L' IN Pojemnosc) - 1) AS FLOAT) * 1000
                ELSE CAST(TRIM(Pojemnosc) AS FLOAT)
            END / 500
    END AS Cylinders,
    'Automatyczna' AS Transmission,
    CASE
        WHEN Typ = 'elektryczny' THEN 1
        ELSE 6
    END AS Gears,
    CASE
        WHEN EXISTS (SELECT 1 FROM CechyPojazdu cp JOIN Pojazd p ON cp.IDPojazdu = p.ID WHERE p.ID = PojazdID AND cp.Cecha = 'Napęd 4×4')
        THEN 'AWD'
        ELSE 'RWD'
    END AS DriveType,
    CASE
        WHEN Typ = 'elektryczny' THEN 0
        WHEN Typ = 'diesel' THEN 5.5
        ELSE 7.2
    END AS FuelConsumption,
    CASE
        WHEN Typ = 'elektryczny' THEN 0
        WHEN Typ = 'diesel' THEN 145
        ELSE 165
    END AS CO2Emission,
    CASE
        WHEN Typ = 'benzyna' THEN 'Silnik benzynowy o dobrej dynamice i wysokiej kulturze pracy'
        WHEN Typ = 'diesel' THEN 'Ekonomiczny silnik wysokoprężny z dużym momentem obrotowym'
        WHEN Typ = 'elektryczny' THEN 'Napęd elektryczny o zerowej emisji i błyskawicznej dynamice'
        ELSE 'Nowoczesny napęd o dobrych osiągach'
    END AS Description,
    TRUE AS IsActive,
    NOW() - INTERVAL '60 days' * RANDOM() AS CreatedAt,
    NULL AS UpdatedAt
FROM Silnik
WHERE ID = @Id;

-- GetFilteredAsync: Get filtered engines (Dynamic SQL)
/* 
This will be constructed dynamically in code based on provided filters.
Base query pattern:

SELECT
    ID,
    CAST(ID AS VARCHAR) AS Id,
    COALESCE(Pojemnosc, '') AS Name,
    Typ AS Type,
    CAST(CASE
        WHEN Pojemnosc LIKE '%L%' THEN 
            CAST(SUBSTRING(TRIM(Pojemnosc), 1, POSITION('L' IN Pojemnosc) - 1) AS FLOAT) * 1000
        WHEN Pojemnosc = '—' THEN NULL
        ELSE CAST(TRIM(Pojemnosc) AS FLOAT)
    END AS INT) AS Capacity,
    Moc AS Power,
    CAST(Moc * 1.5 AS INT) AS Torque,
    CASE
        WHEN Typ = 'benzyna' THEN 'Benzyna'
        WHEN Typ = 'diesel' THEN 'Diesel'
        WHEN Typ = 'elektryczny' THEN 'Elektryczny'
        ELSE Typ
    END AS FuelType,
    CASE
        WHEN Typ = 'elektryczny' THEN NULL
        ELSE 
            CASE
                WHEN Pojemnosc LIKE '%L%' THEN 
                    CAST(SUBSTRING(TRIM(Pojemnosc), 1, POSITION('L' IN Pojemnosc) - 1) AS FLOAT) * 1000
                ELSE CAST(TRIM(Pojemnosc) AS FLOAT)
            END / 500
    END AS Cylinders,
    'Automatyczna' AS Transmission,
    CASE
        WHEN Typ = 'elektryczny' THEN 1
        ELSE 6
    END AS Gears,
    CASE
        WHEN EXISTS (SELECT 1 FROM CechyPojazdu cp JOIN Pojazd p ON cp.IDPojazdu = p.ID WHERE p.ID = PojazdID AND cp.Cecha = 'Napęd 4×4')
        THEN 'AWD'
        ELSE 'RWD'
    END AS DriveType,
    CASE
        WHEN Typ = 'elektryczny' THEN 0
        WHEN Typ = 'diesel' THEN 5.5
        ELSE 7.2
    END AS FuelConsumption,
    CASE
        WHEN Typ = 'elektryczny' THEN 0
        WHEN Typ = 'diesel' THEN 145
        ELSE 165
    END AS CO2Emission,
    CASE
        WHEN Typ = 'benzyna' THEN 'Silnik benzynowy o dobrej dynamice i wysokiej kulturze pracy'
        WHEN Typ = 'diesel' THEN 'Ekonomiczny silnik wysokoprężny z dużym momentem obrotowym'
        WHEN Typ = 'elektryczny' THEN 'Napęd elektryczny o zerowej emisji i błyskawicznej dynamice'
        ELSE 'Nowoczesny napęd o dobrych osiągach'
    END AS Description,
    TRUE AS IsActive,
    NOW() - INTERVAL '60 days' * RANDOM() AS CreatedAt,
    NULL AS UpdatedAt
FROM Silnik
WHERE 1=1
-- Conditional filters added here dynamically
-- Type filter: AND Typ = @Type
-- FuelType filter: AND CASE WHEN Typ = 'benzyna' THEN 'Benzyna' WHEN Typ = 'diesel' THEN 'Diesel' WHEN Typ = 'elektryczny' THEN 'Elektryczny' ELSE Typ END LIKE '%' || @FuelType || '%'
-- MinCapacity filter: AND CAST(CASE WHEN Pojemnosc LIKE '%L%' THEN CAST(SUBSTRING(TRIM(Pojemnosc), 1, POSITION('L' IN Pojemnosc) - 1) AS FLOAT) * 1000 WHEN Pojemnosc = '—' THEN NULL ELSE CAST(TRIM(Pojemnosc) AS FLOAT) END AS INT) >= @MinCapacity
-- MaxCapacity filter: AND CAST(CASE WHEN Pojemnosc LIKE '%L%' THEN CAST(SUBSTRING(TRIM(Pojemnosc), 1, POSITION('L' IN Pojemnosc) - 1) AS FLOAT) * 1000 WHEN Pojemnosc = '—' THEN NULL ELSE CAST(TRIM(Pojemnosc) AS FLOAT) END AS INT) <= @MaxCapacity
-- MinPower filter: AND Moc >= @MinPower
-- MaxPower filter: AND Moc <= @MaxPower
-- Transmission filter: AND 'Automatyczna' = @Transmission
-- DriveType filter: AND CASE WHEN EXISTS (SELECT 1 FROM CechyPojazdu cp JOIN Pojazd p ON cp.IDPojazdu = p.ID WHERE p.ID = PojazdID AND cp.Cecha = 'Napęd 4×4') THEN 'AWD' ELSE 'RWD' END = @DriveType
-- IsActive filter: AND TRUE = @IsActive
ORDER BY Moc DESC;
*/

-- CreateAsync: Insert new engine
INSERT INTO Silnik (PojazdID, Pojemnosc, Typ, Moc)
VALUES (1, @Capacity, @Type, @Power)
RETURNING 
    ID,
    CAST(ID AS VARCHAR) AS Id,
    COALESCE(Pojemnosc, '') AS Name,
    Typ AS Type,
    CAST(Pojemnosc AS INT) AS Capacity,
    Moc AS Power,
    CAST(Moc * 1.5 AS INT) AS Torque,
    CASE
        WHEN Typ = 'benzyna' THEN 'Benzyna'
        WHEN Typ = 'diesel' THEN 'Diesel'
        WHEN Typ = 'elektryczny' THEN 'Elektryczny'
        ELSE Typ
    END AS FuelType,
    CASE
        WHEN Typ = 'elektryczny' THEN NULL
        ELSE 
            CASE
                WHEN Pojemnosc LIKE '%L%' THEN 
                    CAST(SUBSTRING(TRIM(Pojemnosc), 1, POSITION('L' IN Pojemnosc) - 1) AS FLOAT) * 1000
                ELSE CAST(TRIM(Pojemnosc) AS FLOAT)
            END / 500
    END AS Cylinders,
    'Automatyczna' AS Transmission,
    CASE
        WHEN Typ = 'elektryczny' THEN 1
        ELSE 6
    END AS Gears,
    'RWD' AS DriveType,
    CASE
        WHEN Typ = 'elektryczny' THEN 0
        WHEN Typ = 'diesel' THEN 5.5
        ELSE 7.2
    END AS FuelConsumption,
    CASE
        WHEN Typ = 'elektryczny' THEN 0
        WHEN Typ = 'diesel' THEN 145
        ELSE 165
    END AS CO2Emission,
    CASE
        WHEN Typ = 'benzyna' THEN 'Silnik benzynowy o dobrej dynamice i wysokiej kulturze pracy'
        WHEN Typ = 'diesel' THEN 'Ekonomiczny silnik wysokoprężny z dużym momentem obrotowym'
        WHEN Typ = 'elektryczny' THEN 'Napęd elektryczny o zerowej emisji i błyskawicznej dynamice'
        ELSE 'Nowoczesny napęd o dobrych osiągach'
    END AS Description,
    TRUE AS IsActive,
    NOW() AS CreatedAt,
    NULL AS UpdatedAt;

-- UpdateAsync: Update engine
UPDATE Silnik
SET
    Pojemnosc = @Capacity,
    Typ = @Type,
    Moc = @Power
WHERE ID = @Id
RETURNING 
    ID,
    CAST(ID AS VARCHAR) AS Id,
    COALESCE(Pojemnosc, '') AS Name,
    Typ AS Type,
    CAST(Pojemnosc AS INT) AS Capacity,
    Moc AS Power,
    CAST(Moc * 1.5 AS INT) AS Torque,
    CASE
        WHEN Typ = 'benzyna' THEN 'Benzyna'
        WHEN Typ = 'diesel' THEN 'Diesel'
        WHEN Typ = 'elektryczny' THEN 'Elektryczny'
        ELSE Typ
    END AS FuelType,
    CASE
        WHEN Typ = 'elektryczny' THEN NULL
        ELSE 
            CASE
                WHEN Pojemnosc LIKE '%L%' THEN 
                    CAST(SUBSTRING(TRIM(Pojemnosc), 1, POSITION('L' IN Pojemnosc) - 1) AS FLOAT) * 1000
                ELSE CAST(TRIM(Pojemnosc) AS FLOAT)
            END / 500
    END AS Cylinders,
    'Automatyczna' AS Transmission,
    CASE
        WHEN Typ = 'elektryczny' THEN 1
        ELSE 6
    END AS Gears,
    CASE
        WHEN EXISTS (SELECT 1 FROM CechyPojazdu cp JOIN Pojazd p ON cp.IDPojazdu = p.ID WHERE p.ID = PojazdID AND cp.Cecha = 'Napęd 4×4')
        THEN 'AWD'
        ELSE 'RWD'
    END AS DriveType,
    CASE
        WHEN Typ = 'elektryczny' THEN 0
        WHEN Typ = 'diesel' THEN 5.5
        ELSE 7.2
    END AS FuelConsumption,
    CASE
        WHEN Typ = 'elektryczny' THEN 0
        WHEN Typ = 'diesel' THEN 145
        ELSE 165
    END AS CO2Emission,
    CASE
        WHEN Typ = 'benzyna' THEN 'Silnik benzynowy o dobrej dynamice i wysokiej kulturze pracy'
        WHEN Typ = 'diesel' THEN 'Ekonomiczny silnik wysokoprężny z dużym momentem obrotowym'
        WHEN Typ = 'elektryczny' THEN 'Napęd elektryczny o zerowej emisji i błyskawicznej dynamice'
        ELSE 'Nowoczesny napęd o dobrych osiągach'
    END AS Description,
    TRUE AS IsActive,
    NOW() - INTERVAL '60 days' * RANDOM() AS CreatedAt,
    NOW() AS UpdatedAt;

-- DeleteAsync: Delete engine
DELETE FROM Silnik
WHERE ID = @Id;

-- Check if engine is used by any vehicles
SELECT COUNT(*) AS Count
FROM Pojazd
WHERE ID IN (SELECT PojazdID FROM Silnik WHERE ID = @Id);

-- GetAllByVechicleIdAsync: Get all engines by vehicle ID
SELECT
    s.ID,
    CAST(s.ID AS VARCHAR) AS Id,
    COALESCE(s.Pojemnosc, '') AS Name,
    s.Typ AS Type,
    CAST(s.Pojemnosc AS INT) AS Capacity,
    s.Moc AS Power,
    CAST(s.Moc * 1.5 AS INT) AS Torque,
    CASE
        WHEN s.Typ = 'benzyna' THEN 'Benzyna'
        WHEN s.Typ = 'diesel' THEN 'Diesel'
        WHEN s.Typ = 'elektryczny' THEN 'Elektryczny'
        ELSE s.Typ
    END AS FuelType,
    CASE
        WHEN s.Typ = 'elektryczny' THEN NULL
        ELSE 
            CASE
                WHEN s.Pojemnosc LIKE '%L%' THEN 
                    CAST(SUBSTRING(TRIM(s.Pojemnosc), 1, POSITION('L' IN s.Pojemnosc) - 1) AS FLOAT) * 1000
                ELSE CAST(TRIM(s.Pojemnosc) AS FLOAT)
            END / 500
    END AS Cylinders,
    'Automatyczna' AS Transmission,
    CASE
        WHEN s.Typ = 'elektryczny' THEN 1
        ELSE 6
    END AS Gears,
    CASE
        WHEN EXISTS (SELECT 1 FROM CechyPojazdu cp JOIN Pojazd p ON cp.IDPojazdu = p.ID WHERE p.ID = s.PojazdID AND cp.Cecha = 'Napęd 4×4')
        THEN 'AWD'
        ELSE 'RWD'
    END AS DriveType,
    CASE
        WHEN s.Typ = 'elektryczny' THEN 0
        WHEN s.Typ = 'diesel' THEN 5.5
        ELSE 7.2
    END AS FuelConsumption,
    CASE
        WHEN s.Typ = 'elektryczny' THEN 0
        WHEN s.Typ = 'diesel' THEN 145
        ELSE 165
    END AS CO2Emission,
    CASE
        WHEN s.Typ = 'benzyna' THEN 'Silnik benzynowy o dobrej dynamice i wysokiej kulturze pracy'
        WHEN s.Typ = 'diesel' THEN 'Ekonomiczny silnik wysokoprężny z dużym momentem obrotowym'
        WHEN s.Typ = 'elektryczny' THEN 'Napęd elektryczny o zerowej emisji i błyskawicznej dynamice'
        ELSE 'Nowoczesny napęd o dobrych osiągach'
    END AS Description,
    TRUE AS IsActive,
    NOW() - INTERVAL '60 days' * RANDOM() AS CreatedAt,
    NULL AS UpdatedAt
FROM Silnik s
JOIN Pojazd p ON s.PojazdID = p.ID
WHERE p.ID = @VehicleId
ORDER BY s.Moc DESC;