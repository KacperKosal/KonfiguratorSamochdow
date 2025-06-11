-- CarModelQueries.sql
-- SQL queries for CarModel repository operations

-- GetAllAsync: Get all car models
SELECT
    CAST(p.ID AS VARCHAR) AS Id,
    p.Model AS Name,
    EXTRACT(YEAR FROM NOW()) AS ProductionYear,
    COALESCE(p.TypNadwozia, 
        CASE
            WHEN p.Model ILIKE '%suv%' THEN 'SUV'
            WHEN p.Model ILIKE '%sedan%' THEN 'Sedan'
            WHEN p.Model ILIKE '%coupe%' THEN 'Coupe'
            WHEN p.Model ILIKE '%kombi%' THEN 'Kombi'
            ELSE 'Sedan'
        END
    ) AS BodyType,
    CASE
        WHEN p.Model ILIKE '%bmw%' THEN 'BMW'
        WHEN p.Model ILIKE '%toyota%' THEN 'Toyota'
        WHEN p.Model ILIKE '%tesla%' THEN 'Tesla'
        ELSE SPLIT_PART(p.Model, ' ', 1)
    END AS Manufacturer,
    CASE
        WHEN p.Cena > 200000 THEN 'Premium'
        WHEN p.Cena > 150000 THEN 'E'
        WHEN p.Cena > 100000 THEN 'D'
        WHEN p.Cena > 70000 THEN 'C'
        ELSE 'B'
    END AS Segment,
    p.Cena AS BasePrice,
    p.Opis AS Description,
    COALESCE(p.ImageUrl,
        CASE
            WHEN p.Model ILIKE '%bmw%' THEN '/images/models/bmw.jpg'
            WHEN p.Model ILIKE '%toyota%' THEN '/images/models/toyota.jpg'
            WHEN p.Model ILIKE '%tesla%' THEN '/images/models/tesla.jpg'
            ELSE '/images/models/default.jpg'
        END
    ) AS ImageUrl,
    p.Ma4x4 AS Has4x4,
    p.JestElektryczny AS IsElectric,
    TRUE AS IsActive,
    NOW() - INTERVAL '30 days' * RANDOM() AS CreatedAt,
    NULL AS UpdatedAt
FROM pojazd p
ORDER BY p.Cena DESC;

-- GetByIdAsync: Get car model by ID
SELECT
    CAST(p.ID AS VARCHAR) AS Id,
    p.Model AS Name,
    EXTRACT(YEAR FROM NOW()) AS ProductionYear,
    CASE
        WHEN p.Model ILIKE '%suv%' THEN 'SUV'
        WHEN p.Model ILIKE '%sedan%' THEN 'Sedan'
        WHEN p.Model ILIKE '%coupe%' THEN 'Coupe'
        WHEN p.Model ILIKE '%kombi%' THEN 'Kombi'
        ELSE 'Sedan'
    END AS BodyType,
    CASE
        WHEN p.Model ILIKE '%bmw%' THEN 'BMW'
        WHEN p.Model ILIKE '%toyota%' THEN 'Toyota'
        WHEN p.Model ILIKE '%tesla%' THEN 'Tesla'
        ELSE SPLIT_PART(p.Model, ' ', 1)
    END AS Manufacturer,
    CASE
        WHEN p.Cena > 200000 THEN 'Premium'
        WHEN p.Cena > 150000 THEN 'E'
        WHEN p.Cena > 100000 THEN 'D'
        WHEN p.Cena > 70000 THEN 'C'
        ELSE 'B'
    END AS Segment,
    p.Cena AS BasePrice,
    p.Opis AS Description,
    CASE
        WHEN p.Model ILIKE '%bmw%' THEN '/images/models/bmw.jpg'
        WHEN p.Model ILIKE '%toyota%' THEN '/images/models/toyota.jpg'
        WHEN p.Model ILIKE '%tesla%' THEN '/images/models/tesla.jpg'
        ELSE '/images/models/default.jpg'
    END AS ImageUrl,
    TRUE AS IsActive,
    NOW() - INTERVAL '30 days' * RANDOM() AS CreatedAt,
    NULL AS UpdatedAt
FROM Pojazd p
WHERE p.ID = @Id;

-- GetFilteredAsync: Get filtered car models (Dynamic SQL)
/*
This will be constructed dynamically in code based on provided filters.
Base query pattern:

SELECT
    CAST(p.ID AS VARCHAR) AS Id,
    p.Model AS Name,
    EXTRACT(YEAR FROM NOW()) AS ProductionYear,
    CASE
        WHEN p.Model ILIKE '%suv%' THEN 'SUV'
        WHEN p.Model ILIKE '%sedan%' THEN 'Sedan'
        WHEN p.Model ILIKE '%coupe%' THEN 'Coupe'
        WHEN p.Model ILIKE '%kombi%' THEN 'Kombi'
        ELSE 'Sedan'
    END AS BodyType,
    CASE
        WHEN p.Model ILIKE '%bmw%' THEN 'BMW'
        WHEN p.Model ILIKE '%toyota%' THEN 'Toyota'
        WHEN p.Model ILIKE '%tesla%' THEN 'Tesla'
        ELSE SPLIT_PART(p.Model, ' ', 1)
    END AS Manufacturer,
    CASE
        WHEN p.Cena > 200000 THEN 'Premium'
        WHEN p.Cena > 150000 THEN 'E'
        WHEN p.Cena > 100000 THEN 'D'
        WHEN p.Cena > 70000 THEN 'C'
        ELSE 'B'
    END AS Segment,
    p.Cena AS BasePrice,
    p.Opis AS Description,
    CASE
        WHEN p.Model ILIKE '%bmw%' THEN '/images/models/bmw.jpg'
        WHEN p.Model ILIKE '%toyota%' THEN '/images/models/toyota.jpg'
        WHEN p.Model ILIKE '%tesla%' THEN '/images/models/tesla.jpg'
        ELSE '/images/models/default.jpg'
    END AS ImageUrl,
    TRUE AS IsActive,
    NOW() - INTERVAL '30 days' * RANDOM() AS CreatedAt,
    NULL AS UpdatedAt
FROM Pojazd p
WHERE 1=1
-- Conditional filters added here dynamically
-- Manufacturer filter: AND CASE WHEN p.Model ILIKE '%bmw%' THEN 'BMW' WHEN p.Model ILIKE '%toyota%' THEN 'Toyota' WHEN p.Model ILIKE '%tesla%' THEN 'Tesla' ELSE SPLIT_PART(p.Model, ' ', 1) END = @Manufacturer
-- BodyType filter: AND CASE WHEN p.Model ILIKE '%suv%' THEN 'SUV' WHEN p.Model ILIKE '%sedan%' THEN 'Sedan' WHEN p.Model ILIKE '%coupe%' THEN 'Coupe' WHEN p.Model ILIKE '%kombi%' THEN 'Kombi' ELSE 'Sedan' END = @BodyType
-- Segment filter: AND CASE WHEN p.Cena > 200000 THEN 'Premium' WHEN p.Cena > 150000 THEN 'E' WHEN p.Cena > 100000 THEN 'D' WHEN p.Cena > 70000 THEN 'C' ELSE 'B' END = @Segment
-- MinProductionYear filter: AND EXTRACT(YEAR FROM NOW()) >= @MinProductionYear
-- MaxProductionYear filter: AND EXTRACT(YEAR FROM NOW()) <= @MaxProductionYear
-- MinPrice filter: AND p.Cena >= @MinPrice
-- MaxPrice filter: AND p.Cena <= @MaxPrice
-- IsActive filter: AND TRUE = @IsActive
ORDER BY p.Cena DESC;
*/

-- CreateAsync: Insert new car model
INSERT INTO pojazd (model, kolornadwozia, typnadwozia, wyposazeniewnetrza, cena, opis, ma4x4, jestelektryczny, akcesoria, imageurl)
VALUES (
    @Name, 
    'Biały',
    @BodyType, 
    'Standardowe', 
    @BasePrice, 
    @Description, 
    @Has4x4, 
    @IsElectric, 
    'Brak',
    @ImageUrl
)
RETURNING
    CAST(id AS VARCHAR) AS Id,
    model AS Name,
    EXTRACT(YEAR FROM NOW()) AS ProductionYear,
    COALESCE(typnadwozia, 
        CASE
            WHEN model ILIKE '%suv%' THEN 'SUV'
            WHEN model ILIKE '%sedan%' THEN 'Sedan'
            WHEN model ILIKE '%coupe%' THEN 'Coupe'
            WHEN model ILIKE '%kombi%' THEN 'Kombi'
            ELSE 'Sedan'
        END
    ) AS BodyType,
    CASE
        WHEN model ILIKE '%bmw%' THEN 'BMW'
        WHEN model ILIKE '%toyota%' THEN 'Toyota'
        WHEN model ILIKE '%tesla%' THEN 'Tesla'
        ELSE SPLIT_PART(model, ' ', 1)
    END AS Manufacturer,
    CASE
        WHEN cena > 200000 THEN 'Premium'
        WHEN cena > 150000 THEN 'E'
        WHEN cena > 100000 THEN 'D'
        WHEN cena > 70000 THEN 'C'
        ELSE 'B'
    END AS Segment,
    cena AS BasePrice,
    opis AS Description,
    COALESCE(imageurl, '/images/models/default.jpg') AS ImageUrl,
    ma4x4 AS Has4x4,
    jestelektryczny AS IsElectric,
    TRUE AS IsActive,
    NOW() AS CreatedAt,
    NULL AS UpdatedAt;

-- UpdateAsync: Update car model
UPDATE pojazd
SET
    model = @Name,
    typnadwozia = @BodyType,
    cena = @BasePrice,
    opis = @Description,
    ma4x4 = @Has4x4,
    jestelektryczny = @IsElectric,
    imageurl = @ImageUrl
WHERE id = @Id::integer
RETURNING
    CAST(id AS VARCHAR) AS Id,
    model AS Name,
    EXTRACT(YEAR FROM NOW()) AS ProductionYear,
    COALESCE(typnadwozia, 
        CASE
            WHEN model ILIKE '%suv%' THEN 'SUV'
            WHEN model ILIKE '%sedan%' THEN 'Sedan'
            WHEN model ILIKE '%coupe%' THEN 'Coupe'
            WHEN model ILIKE '%kombi%' THEN 'Kombi'
            ELSE 'Sedan'
        END
    ) AS BodyType,
    CASE
        WHEN model ILIKE '%bmw%' THEN 'BMW'
        WHEN model ILIKE '%toyota%' THEN 'Toyota'
        WHEN model ILIKE '%tesla%' THEN 'Tesla'
        ELSE SPLIT_PART(model, ' ', 1)
    END AS Manufacturer,
    CASE
        WHEN cena > 200000 THEN 'Premium'
        WHEN cena > 150000 THEN 'E'
        WHEN cena > 100000 THEN 'D'
        WHEN cena > 70000 THEN 'C'
        ELSE 'B'
    END AS Segment,
    cena AS BasePrice,
    opis AS Description,
    COALESCE(imageurl, '/images/models/default.jpg') AS ImageUrl,
    ma4x4 AS Has4x4,
    jestelektryczny AS IsElectric,
    TRUE AS IsActive,
    NOW() - INTERVAL '30 days' * RANDOM() AS CreatedAt,
    NOW() AS UpdatedAt;

-- DeleteAsync: Delete car model
-- First check if car model is referenced in other tables
SELECT COUNT(*) AS ConfigurationCount FROM konfiguracja WHERE idpojazdu = @Id::integer;
SELECT COUNT(*) AS EngineCount FROM silnik WHERE pojazdid = @Id::integer;
SELECT COUNT(*) AS ModelEngineCount FROM modelsilnik WHERE modelid = @Id::integer;

-- If no references exist, delete the car model
DELETE FROM pojazd
WHERE id = @Id::integer;