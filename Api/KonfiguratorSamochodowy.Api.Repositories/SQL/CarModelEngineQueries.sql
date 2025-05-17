-- CarModelEngineQueries.sql
-- SQL queries for CarModelEngine repository operations

/*
Note: CarModelEngine is a bridge entity between CarModel and Engine,
but there's no direct corresponding table in the database schema. 
We need to create a temporary solution using the existing tables 
until a proper CarModelEngine table is created.

For the purposes of these queries, we'll use the Silnik (Engine) table
and its relationship with Pojazd (CarModel), but in a real scenario,
you'd want a proper many-to-many relationship table.
*/

-- Create the CarModelEngines table if it doesn't exist
CREATE TABLE IF NOT EXISTS CarModelEngines (
    Id VARCHAR(255) PRIMARY KEY,
    CarModelId VARCHAR(255) NOT NULL,
    EngineId VARCHAR(255) NOT NULL,
    AdditionalPrice DECIMAL(10, 2) NOT NULL DEFAULT 0,
    IsDefault BOOLEAN NOT NULL DEFAULT false,
    TopSpeed INT NOT NULL DEFAULT 180,
    Acceleration0To100 DECIMAL(4, 1) NOT NULL DEFAULT 10.0,
    AvailabilityDate TIMESTAMP,
    IsAvailable BOOLEAN NOT NULL DEFAULT true,
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMP,
    UNIQUE(CarModelId, EngineId)
);

-- GetAllAsync: Get all car model engine mappings
SELECT
    cme.Id,
    cme.CarModelId,
    cme.EngineId,
    cme.AdditionalPrice,
    cme.IsDefault,
    cme.TopSpeed,
    cme.Acceleration0To100,
    cme.AvailabilityDate,
    cme.IsAvailable,
    cme.CreatedAt,
    cme.UpdatedAt
FROM CarModelEngines cme
ORDER BY cme.IsDefault DESC, cme.AdditionalPrice ASC;

-- GetByIdAsync: Get car model engine mapping by ID
SELECT
    cme.Id,
    cme.CarModelId,
    cme.EngineId,
    cme.AdditionalPrice,
    cme.IsDefault,
    cme.TopSpeed,
    cme.Acceleration0To100,
    cme.AvailabilityDate,
    cme.IsAvailable,
    cme.CreatedAt,
    cme.UpdatedAt
FROM CarModelEngines cme
WHERE cme.Id = @Id;

-- GetByCarModelAndEngineIdAsync: Get car model engine mapping by car model ID and engine ID
SELECT
    cme.Id,
    cme.CarModelId,
    cme.EngineId,
    cme.AdditionalPrice,
    cme.IsDefault,
    cme.TopSpeed,
    cme.Acceleration0To100,
    cme.AvailabilityDate,
    cme.IsAvailable,
    cme.CreatedAt,
    cme.UpdatedAt
FROM CarModelEngines cme
WHERE cme.CarModelId = @CarModelId AND cme.EngineId = @EngineId;

-- GetByCarModelIdAsync: Get all car model engine mappings for a specific car model
SELECT
    cme.Id,
    cme.CarModelId,
    cme.EngineId,
    cme.AdditionalPrice,
    cme.IsDefault,
    cme.TopSpeed,
    cme.Acceleration0To100,
    cme.AvailabilityDate,
    cme.IsAvailable,
    cme.CreatedAt,
    cme.UpdatedAt
FROM CarModelEngines cme
WHERE cme.CarModelId = @CarModelId
ORDER BY cme.IsDefault DESC, cme.AdditionalPrice ASC;

-- GetByEngineIdAsync: Get all car model engine mappings for a specific engine
SELECT
    cme.Id,
    cme.CarModelId,
    cme.EngineId,
    cme.AdditionalPrice,
    cme.IsDefault,
    cme.TopSpeed,
    cme.Acceleration0To100,
    cme.AvailabilityDate,
    cme.IsAvailable,
    cme.CreatedAt,
    cme.UpdatedAt
FROM CarModelEngines cme
WHERE cme.EngineId = @EngineId
ORDER BY cme.CreatedAt DESC;

-- CreateAsync: Insert new car model engine mapping
-- First, if IsDefault is true, reset all existing default mappings for the car model
WITH updated_defaults AS (
    UPDATE CarModelEngines
    SET IsDefault = false,
        UpdatedAt = NOW()
    WHERE CarModelId = @CarModelId AND IsDefault = true AND @IsDefault = true
    RETURNING *
)
INSERT INTO CarModelEngines (
    Id,
    CarModelId,
    EngineId,
    AdditionalPrice,
    IsDefault,
    TopSpeed,
    Acceleration0To100,
    AvailabilityDate,
    IsAvailable,
    CreatedAt,
    UpdatedAt
) VALUES (
    @Id,
    @CarModelId,
    @EngineId,
    @AdditionalPrice,
    @IsDefault,
    @TopSpeed,
    @Acceleration0To100,
    @AvailabilityDate,
    @IsAvailable,
    NOW(),
    NULL
)
RETURNING
    Id,
    CarModelId,
    EngineId,
    AdditionalPrice,
    IsDefault,
    TopSpeed,
    Acceleration0To100,
    AvailabilityDate,
    IsAvailable,
    CreatedAt,
    UpdatedAt;

-- UpdateAsync: Update car model engine mapping
-- First, if IsDefault is being set to true, reset all existing default mappings for the car model
WITH updated_defaults AS (
    UPDATE CarModelEngines
    SET IsDefault = false,
        UpdatedAt = NOW()
    WHERE CarModelId = @CarModelId AND IsDefault = true AND Id != @Id AND @IsDefault = true
    RETURNING *
)
UPDATE CarModelEngines
SET
    AdditionalPrice = @AdditionalPrice,
    IsDefault = @IsDefault,
    TopSpeed = @TopSpeed,
    Acceleration0To100 = @Acceleration0To100,
    AvailabilityDate = @AvailabilityDate,
    IsAvailable = @IsAvailable,
    UpdatedAt = NOW()
WHERE Id = @Id
RETURNING
    Id,
    CarModelId,
    EngineId,
    AdditionalPrice,
    IsDefault,
    TopSpeed,
    Acceleration0To100,
    AvailabilityDate,
    IsAvailable,
    CreatedAt,
    UpdatedAt;

-- DeleteAsync: Delete car model engine mapping
DELETE FROM CarModelEngines
WHERE Id = @Id;

-- DeleteByCarModelAndEngineIdAsync: Delete car model engine mapping by car model ID and engine ID
DELETE FROM CarModelEngines
WHERE CarModelId = @CarModelId AND EngineId = @EngineId;

-- ---- Alternative Queries if CarModelEngines table doesn't exist ----

-- Legacy system queries based on Silnik and Pojazd tables
-- These are fallback queries for systems without a dedicated CarModelEngines table

-- Legacy GetAllAsync: Get all engines and their vehicles as a mapping
SELECT
    CONCAT(s.ID, '-', p.ID) AS Id,
    CAST(p.ID AS VARCHAR) AS CarModelId,
    CAST(s.ID AS VARCHAR) AS EngineId,
    0 AS AdditionalPrice,
    CASE WHEN ROW_NUMBER() OVER (PARTITION BY p.ID ORDER BY s.Moc DESC) = 1 THEN TRUE ELSE FALSE END AS IsDefault,
    CASE
        WHEN s.Moc > 200 THEN 240
        WHEN s.Moc > 150 THEN 220
        WHEN s.Moc > 100 THEN 200
        ELSE 180
    END AS TopSpeed,
    CASE
        WHEN s.Moc > 200 THEN 6.0
        WHEN s.Moc > 150 THEN 8.0
        WHEN s.Moc > 100 THEN 10.0
        ELSE 12.0
    END AS Acceleration0To100,
    NOW() AS AvailabilityDate,
    TRUE AS IsAvailable,
    NOW() - INTERVAL '60 days' * RANDOM() AS CreatedAt,
    NULL AS UpdatedAt
FROM Silnik s
JOIN Pojazd p ON s.PojazdID = p.ID
ORDER BY p.ID, s.Moc DESC;

-- Legacy GetByIdAsync: Get engine and vehicle mapping by ID (constructed ID)
SELECT
    CONCAT(s.ID, '-', p.ID) AS Id,
    CAST(p.ID AS VARCHAR) AS CarModelId,
    CAST(s.ID AS VARCHAR) AS EngineId,
    0 AS AdditionalPrice,
    CASE WHEN ROW_NUMBER() OVER (PARTITION BY p.ID ORDER BY s.Moc DESC) = 1 THEN TRUE ELSE FALSE END AS IsDefault,
    CASE
        WHEN s.Moc > 200 THEN 240
        WHEN s.Moc > 150 THEN 220
        WHEN s.Moc > 100 THEN 200
        ELSE 180
    END AS TopSpeed,
    CASE
        WHEN s.Moc > 200 THEN 6.0
        WHEN s.Moc > 150 THEN 8.0
        WHEN s.Moc > 100 THEN 10.0
        ELSE 12.0
    END AS Acceleration0To100,
    NOW() AS AvailabilityDate,
    TRUE AS IsAvailable,
    NOW() - INTERVAL '60 days' * RANDOM() AS CreatedAt,
    NULL AS UpdatedAt
FROM Silnik s
JOIN Pojazd p ON s.PojazdID = p.ID
WHERE CONCAT(s.ID, '-', p.ID) = @Id;

-- Legacy GetByCarModelIdAsync: Get all engines for a car model
SELECT
    CONCAT(s.ID, '-', p.ID) AS Id,
    CAST(p.ID AS VARCHAR) AS CarModelId,
    CAST(s.ID AS VARCHAR) AS EngineId,
    0 AS AdditionalPrice,
    CASE WHEN ROW_NUMBER() OVER (PARTITION BY p.ID ORDER BY s.Moc DESC) = 1 THEN TRUE ELSE FALSE END AS IsDefault,
    CASE
        WHEN s.Moc > 200 THEN 240
        WHEN s.Moc > 150 THEN 220
        WHEN s.Moc > 100 THEN 200
        ELSE 180
    END AS TopSpeed,
    CASE
        WHEN s.Moc > 200 THEN 6.0
        WHEN s.Moc > 150 THEN 8.0
        WHEN s.Moc > 100 THEN 10.0
        ELSE 12.0
    END AS Acceleration0To100,
    NOW() AS AvailabilityDate,
    TRUE AS IsAvailable,
    NOW() - INTERVAL '60 days' * RANDOM() AS CreatedAt,
    NULL AS UpdatedAt
FROM Silnik s
JOIN Pojazd p ON s.PojazdID = p.ID
WHERE p.ID = @CarModelId
ORDER BY s.Moc DESC;

-- Legacy CreateAsync: Insert a new engine for a car model
INSERT INTO Silnik (PojazdID, Pojemnosc, Typ, Moc)
VALUES (@CarModelId::INTEGER, '2.0L', 'benzyna', 150)
RETURNING
    CONCAT(ID, '-', PojazdID) AS Id,
    CAST(PojazdID AS VARCHAR) AS CarModelId,
    CAST(ID AS VARCHAR) AS EngineId,
    0 AS AdditionalPrice,
    TRUE AS IsDefault,
    220 AS TopSpeed,
    8.0 AS Acceleration0To100,
    NOW() AS AvailabilityDate,
    TRUE AS IsAvailable,
    NOW() AS CreatedAt,
    NULL AS UpdatedAt;