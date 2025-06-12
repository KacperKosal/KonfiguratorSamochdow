using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using KonfiguratorSamochodowy.Api.Models;
using KonfiguratorSamochodowy.Api.Repositories.Enums;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories
{
    public class SqlCarModelEngineRepository : ICarModelEngineRepository
    {
        private readonly IDbConnection _connection;

        public SqlCarModelEngineRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<Result<IEnumerable<CarModelEngine>>> GetAllAsync()
        {
            try
            {
                // Check if ""Silnik"" table exists first
                var tableExistsQuery = @"
                    SELECT EXISTS (
                        SELECT FROM information_schema.tables 
                        WHERE table_name = 'Silnik'
                    )";
                
                var silnikTableExists = await _connection.QuerySingleAsync<bool>(tableExistsQuery);
                var modelsilnikTableExists = await _connection.QuerySingleAsync<bool>(@"
                    SELECT EXISTS (
                        SELECT FROM information_schema.tables 
                        WHERE table_name = 'modelsilnik'
                    )");

                if (!silnikTableExists || !modelsilnikTableExists)
                {
                    // Return empty list if tables don't exist yet
                    return Result<IEnumerable<CarModelEngine>>.Success(new List<CarModelEngine>());
                }

                var query = @"
                    SELECT
                        CAST(ms.id AS VARCHAR) as Id,
                        CAST(ms.modelid AS VARCHAR) as CarModelId,
                        CAST(ms.silnikid AS VARCHAR) as EngineId,
                        ms.cenadodatkowa as AdditionalPrice,
                        TRUE as IsDefault,
                        CAST(CASE 
                            WHEN s.""Typ"" = 'benzyna' THEN 200 + CAST(s.""Moc"" AS INTEGER) / 2
                            WHEN s.""Typ"" = 'diesel' THEN 180 + CAST(s.""Moc"" AS INTEGER) / 3
                            WHEN s.""Typ"" = 'elektryczny' THEN 220 + CAST(s.""Moc"" AS INTEGER) / 4
                            ELSE 160 + CAST(s.""Moc"" AS INTEGER) / 5
                        END AS INT) as TopSpeed,
                        CAST(CASE
                            WHEN s.""Typ"" = 'elektryczny' THEN 5.0
                            ELSE (1000.0 - CAST(s.""Moc"" AS INTEGER)) / 100.0
                        END AS DECIMAL(4,1)) as Acceleration0To100,
                        NOW() as AvailabilityDate,
                        TRUE as IsAvailable, 
                        NOW() - INTERVAL '30 days' * RANDOM() as CreatedAt,
                        NULL as UpdatedAt
                    FROM 
                        modelsilnik ms
                    JOIN 
                        pojazd p ON ms.modelid = p.id
                    JOIN 
                        ""Silnik"" s ON ms.silnikid = s.""ID""
                    ORDER BY 
                        p.model, CAST(s.""Moc"" AS INTEGER) DESC";

                var carModelEngines = await _connection.QueryAsync<CarModelEngine>(query);
                
                return Result<IEnumerable<CarModelEngine>>.Success(carModelEngines);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<CarModelEngine>>.Failure(new Error("DatabaseError", $"Failed to get car model engines: {ex.Message}"));
            }
        }

        public async Task<Result<CarModelEngine>> GetByIdAsync(string id)
        {
            try
            {
                var query = @"
                    SELECT
                        CAST(ms.id AS VARCHAR) as Id,
                        CAST(ms.modelid AS VARCHAR) as CarModelId,
                        CAST(ms.silnikid AS VARCHAR) as EngineId,
                        ms.cenadodatkowa as AdditionalPrice,
                        TRUE as IsDefault,
                        CASE 
                            WHEN s.""Typ"" = 'benzyna' THEN 200 + CAST(s.""Moc"" AS INTEGER) / 2
                            WHEN s.""Typ"" = 'diesel' THEN 180 + CAST(s.""Moc"" AS INTEGER) / 3
                            WHEN s.""Typ"" = 'elektryczny' THEN 220 + CAST(s.""Moc"" AS INTEGER) / 4
                            ELSE 160 + CAST(s.""Moc"" AS INTEGER) / 5
                        END as TopSpeed,
                        CASE
                            WHEN s.""Typ"" = 'elektryczny' THEN 5.0
                            ELSE (1000.0 - CAST(s.""Moc"" AS INTEGER)) / 100.0
                        END as Acceleration0To100,
                        NOW() as AvailabilityDate,
                        TRUE as IsAvailable, 
                        NOW() - INTERVAL '30 days' * RANDOM() as CreatedAt,
                        NULL as UpdatedAt
                    FROM 
                        modelsilnik ms
                    JOIN 
                        pojazd p ON ms.modelid = p.id
                    JOIN 
                        ""Silnik"" s ON ms.silnikid = s.""ID""
                    WHERE 
                        ms.id = CAST(@Id AS INTEGER)";

                var carModelEngine = await _connection.QueryFirstOrDefaultAsync<CarModelEngine>(query, new { Id = id });

                if (carModelEngine == null)
                {
                    return Result<CarModelEngine>.Failure(new Error("NotFound", $"Car model engine with id {id} not found"));
                }

                return Result<CarModelEngine>.Success(carModelEngine);
            }
            catch (Exception ex)
            {
                return Result<CarModelEngine>.Failure(new Error("DatabaseError", $"Failed to get car model engine with id {id}: {ex.Message}"));
            }
        }

        public async Task<Result<CarModelEngine>> GetByCarModelAndEngineIdAsync(string carModelId, string engineId)
        {
            try
            {
                var query = @"
                    SELECT
                        CAST(ms.id AS VARCHAR) as Id,
                        CAST(ms.modelid AS VARCHAR) as CarModelId,
                        CAST(ms.silnikid AS VARCHAR) as EngineId,
                        ms.cenadodatkowa as AdditionalPrice,
                        TRUE as IsDefault,
                        CASE 
                            WHEN s.""Typ"" = 'benzyna' THEN 200 + CAST(s.""Moc"" AS INTEGER) / 2
                            WHEN s.""Typ"" = 'diesel' THEN 180 + CAST(s.""Moc"" AS INTEGER) / 3
                            WHEN s.""Typ"" = 'elektryczny' THEN 220 + CAST(s.""Moc"" AS INTEGER) / 4
                            ELSE 160 + CAST(s.""Moc"" AS INTEGER) / 5
                        END as TopSpeed,
                        CASE
                            WHEN s.""Typ"" = 'elektryczny' THEN 5.0
                            ELSE (1000.0 - CAST(s.""Moc"" AS INTEGER)) / 100.0
                        END as Acceleration0To100,
                        NOW() as AvailabilityDate,
                        TRUE as IsAvailable, 
                        NOW() - INTERVAL '30 days' * RANDOM() as CreatedAt,
                        NULL as UpdatedAt
                    FROM 
                        modelsilnik ms
                    JOIN 
                        pojazd p ON ms.modelid = p.id
                    JOIN 
                        ""Silnik"" s ON ms.silnikid = s.""ID""
                    WHERE 
                        ms.modelid = CAST(@CarModelId AS INTEGER)
                        AND ms.silnikid = CAST(@EngineId AS INTEGER)";

                var carModelEngine = await _connection.QueryFirstOrDefaultAsync<CarModelEngine>(query, new { CarModelId = carModelId, EngineId = engineId });

                if (carModelEngine == null)
                {
                    return Result<CarModelEngine>.Failure(new Error("NotFound", $"Car model engine with carModelId {carModelId} and engineId {engineId} not found"));
                }

                return Result<CarModelEngine>.Success(carModelEngine);
            }
            catch (Exception ex)
            {
                return Result<CarModelEngine>.Failure(new Error("DatabaseError", $"Failed to get car model engine with carModelId {carModelId} and engineId {engineId}: {ex.Message}"));
            }
        }

        public async Task<Result<IEnumerable<CarModelEngine>>> GetByCarModelIdAsync(string carModelId)
        {
            try
            {
                var query = @"
                    SELECT
                        CAST(ms.id AS VARCHAR) as Id,
                        CAST(ms.modelid AS VARCHAR) as CarModelId,
                        CAST(ms.silnikid AS VARCHAR) as EngineId,
                        ms.cenadodatkowa as AdditionalPrice,
                        TRUE as IsDefault,
                        CASE 
                            WHEN s.""Typ"" = 'benzyna' THEN 200 + CAST(s.""Moc"" AS INTEGER) / 2
                            WHEN s.""Typ"" = 'diesel' THEN 180 + CAST(s.""Moc"" AS INTEGER) / 3
                            WHEN s.""Typ"" = 'elektryczny' THEN 220 + CAST(s.""Moc"" AS INTEGER) / 4
                            ELSE 160 + CAST(s.""Moc"" AS INTEGER) / 5
                        END as TopSpeed,
                        CASE
                            WHEN s.""Typ"" = 'elektryczny' THEN 5.0
                            ELSE (1000.0 - CAST(s.""Moc"" AS INTEGER)) / 100.0
                        END as Acceleration0To100,
                        NOW() as AvailabilityDate,
                        TRUE as IsAvailable, 
                        NOW() - INTERVAL '30 days' * RANDOM() as CreatedAt,
                        NULL as UpdatedAt
                    FROM 
                        modelsilnik ms
                    JOIN 
                        pojazd p ON ms.modelid = p.id
                    JOIN 
                        ""Silnik"" s ON ms.silnikid = s.""ID""
                    WHERE 
                        ms.modelid = CAST(@CarModelId AS INTEGER)
                    ORDER BY 
                        CAST(s.""Moc"" AS INTEGER) DESC";

                var carModelEngines = await _connection.QueryAsync<CarModelEngine>(query, new { CarModelId = carModelId });
                
                return Result<IEnumerable<CarModelEngine>>.Success(carModelEngines);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<CarModelEngine>>.Failure(new Error("DatabaseError", $"Failed to get car model engines by car model id {carModelId}: {ex.Message}"));
            }
        }

        public async Task<Result<IEnumerable<CarModelEngine>>> GetByEngineIdAsync(string engineId)
        {
            try
            {
                var query = @"
                    SELECT
                        CAST(ms.id AS VARCHAR) as Id,
                        CAST(ms.modelid AS VARCHAR) as CarModelId,
                        CAST(ms.silnikid AS VARCHAR) as EngineId,
                        ms.cenadodatkowa as AdditionalPrice,
                        TRUE as IsDefault,
                        CASE 
                            WHEN s.""Typ"" = 'benzyna' THEN 200 + s.""Moc"" / 2
                            WHEN s.""Typ"" = 'diesel' THEN 180 + s.""Moc"" / 3
                            WHEN s.""Typ"" = 'elektryczny' THEN 220 + s.""Moc"" / 4
                            ELSE 160 + s.""Moc"" / 5
                        END as TopSpeed,
                        CASE
                            WHEN s.""Typ"" = 'elektryczny' THEN 5.0
                            ELSE (1000.0 - s.""Moc"") / 100.0
                        END as Acceleration0To100,
                        NOW() as AvailabilityDate,
                        TRUE as IsAvailable, 
                        NOW() - INTERVAL '30 days' * RANDOM() as CreatedAt,
                        NULL as UpdatedAt
                    FROM 
                        modelsilnik ms
                    JOIN 
                        pojazd p ON ms.modelid = p.id
                    JOIN 
                        ""Silnik"" s ON ms.silnikid = s.""ID""
                    WHERE 
                        ms.silnikid = CAST(@EngineId AS INTEGER)
                    ORDER BY 
                        p.marka, p.model";

                var carModelEngines = await _connection.QueryAsync<CarModelEngine>(query, new { EngineId = engineId });
                
                return Result<IEnumerable<CarModelEngine>>.Success(carModelEngines);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<CarModelEngine>>.Failure(new Error("DatabaseError", $"Failed to get car model engines by engine id {engineId}: {ex.Message}"));
            }
        }

        public async Task<Result<CarModelEngine>> CreateAsync(CarModelEngine carModelEngine)
        {
            try
            {
                // Check if the combination already exists
                var checkQuery = @"
                    SELECT COUNT(*) 
                    FROM modelsilnik 
                    WHERE modelid = CAST(@CarModelId AS INTEGER) AND silnikid = CAST(@EngineId AS INTEGER)";

                var existingCount = await _connection.ExecuteScalarAsync<int>(checkQuery, new 
                { 
                    CarModelId = carModelEngine.CarModelId, 
                    EngineId = carModelEngine.EngineId 
                });

                if (existingCount > 0)
                {
                    return Result<CarModelEngine>.Failure(new Error("Conflict", "This car model engine combination already exists"));
                }

                var query = @"
                    INSERT INTO modelsilnik (
                        modelid,
                        silnikid,
                        cenadodatkowa
                    )
                    VALUES (
                        CAST(@CarModelId AS INTEGER),
                        CAST(@EngineId AS INTEGER),
                        @AdditionalPrice
                    ) RETURNING id";

                var parameters = new
                {
                    CarModelId = carModelEngine.CarModelId,
                    EngineId = carModelEngine.EngineId,
                    AdditionalPrice = carModelEngine.AdditionalPrice
                };

                var insertedId = await _connection.QueryFirstOrDefaultAsync<int>(query, parameters);
                
                // Now get the created record
                var selectQuery = @"
                    SELECT
                        CAST(ms.id AS VARCHAR) as Id,
                        CAST(ms.modelid AS VARCHAR) as CarModelId,
                        CAST(ms.silnikid AS VARCHAR) as EngineId,
                        ms.cenadodatkowa as AdditionalPrice,
                        TRUE as IsDefault,
                        CASE 
                            WHEN s.""Typ"" = 'benzyna' THEN 200 + s.""Moc"" / 2
                            WHEN s.""Typ"" = 'diesel' THEN 180 + s.""Moc"" / 3
                            WHEN s.""Typ"" = 'elektryczny' THEN 220 + s.""Moc"" / 4
                            ELSE 160 + s.""Moc"" / 5
                        END as TopSpeed,
                        CASE
                            WHEN s.""Typ"" = 'elektryczny' THEN 5.0
                            ELSE (1000.0 - s.""Moc"") / 100.0
                        END as Acceleration0To100,
                        NOW() as AvailabilityDate,
                        TRUE as IsAvailable, 
                        NOW() as CreatedAt,
                        NULL as UpdatedAt
                    FROM 
                        modelsilnik ms
                    JOIN 
                        pojazd p ON ms.modelid = p.id
                    JOIN 
                        ""Silnik"" s ON ms.silnikid = s.""ID""
                    WHERE 
                        ms.id = @InsertedId";
                        
                var result = await _connection.QueryFirstOrDefaultAsync<CarModelEngine>(selectQuery, new { InsertedId = insertedId });

                if (result == null)
                {
                    return Result<CarModelEngine>.Failure(new Error("DatabaseError", "Failed to create car model engine"));
                }

                return Result<CarModelEngine>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<CarModelEngine>.Failure(new Error("DatabaseError", $"Failed to create car model engine: {ex.Message}"));
            }
        }

        public async Task<Result<CarModelEngine>> UpdateAsync(string id, CarModelEngine carModelEngine)
        {
            try
            {
                // Check if the record exists
                var checkQuery = "SELECT COUNT(*) FROM modelsilnik WHERE id = CAST(@Id AS INTEGER)";
                var existingCount = await _connection.ExecuteScalarAsync<int>(checkQuery, new { Id = id });

                if (existingCount == 0)
                {
                    return Result<CarModelEngine>.Failure(new Error("NotFound", $"Car model engine with id {id} not found"));
                }

                // Check if the updated combination would conflict with an existing one
                var conflictCheckQuery = @"
                    SELECT COUNT(*) 
                    FROM modelsilnik 
                    WHERE modelid = CAST(@CarModelId AS INTEGER) 
                      AND silnikid = CAST(@EngineId AS INTEGER) 
                      AND id != CAST(@Id AS INTEGER)";

                var conflictCount = await _connection.ExecuteScalarAsync<int>(conflictCheckQuery, new 
                { 
                    Id = id,
                    CarModelId = carModelEngine.CarModelId, 
                    EngineId = carModelEngine.EngineId 
                });

                if (conflictCount > 0)
                {
                    return Result<CarModelEngine>.Failure(new Error("Conflict", "This car model engine combination already exists with a different ID"));
                }

                var query = @"
                    UPDATE modelsilnik SET
                        modelid = CAST(@CarModelId AS INTEGER),
                        silnikid = CAST(@EngineId AS INTEGER),
                        cenadodatkowa = @AdditionalPrice
                    WHERE 
                        id = CAST(@Id AS INTEGER);
                        
                    SELECT
                        CAST(ms.id AS VARCHAR) as Id,
                        CAST(ms.modelid AS VARCHAR) as CarModelId,
                        CAST(ms.silnikid AS VARCHAR) as EngineId,
                        ms.cenadodatkowa as AdditionalPrice,
                        TRUE as IsDefault,
                        CASE 
                            WHEN s.""Typ"" = 'benzyna' THEN 200 + s.""Moc"" / 2
                            WHEN s.""Typ"" = 'diesel' THEN 180 + s.""Moc"" / 3
                            WHEN s.""Typ"" = 'elektryczny' THEN 220 + s.""Moc"" / 4
                            ELSE 160 + s.""Moc"" / 5
                        END as TopSpeed,
                        CASE
                            WHEN s.""Typ"" = 'elektryczny' THEN 5.0
                            ELSE (1000.0 - s.""Moc"") / 100.0
                        END as Acceleration0To100,
                        NOW() as AvailabilityDate,
                        TRUE as IsAvailable, 
                        NOW() - INTERVAL '30 days' * RANDOM() as CreatedAt,
                        NOW() as UpdatedAt
                    FROM 
                        modelsilnik ms
                    JOIN 
                        pojazd p ON ms.modelid = p.id
                    JOIN 
                        ""Silnik"" s ON ms.silnikid = s.""ID""
                    WHERE 
                        ms.id = CAST(@Id AS INTEGER)";

                var parameters = new
                {
                    Id = id,
                    CarModelId = carModelEngine.CarModelId,
                    EngineId = carModelEngine.EngineId,
                    AdditionalPrice = carModelEngine.AdditionalPrice
                };

                var result = await _connection.QueryFirstOrDefaultAsync<CarModelEngine>(query, parameters);

                if (result == null)
                {
                    return Result<CarModelEngine>.Failure(new Error("NotFound", $"Car model engine with id {id} not found after update"));
                }

                return Result<CarModelEngine>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<CarModelEngine>.Failure(new Error("DatabaseError", $"Failed to update car model engine with id {id}: {ex.Message}"));
            }
        }

        public async Task<Result<bool>> DeleteAsync(string id)
        {
            try
            {
                // Check if there are related records in other tables
                var checkRelatedQuery = @"
                    SELECT COUNT(*) 
                    FROM konfiguracja k 
                    WHERE k.modelsilnikid = CAST(@Id AS INTEGER)";

                var relatedCount = await _connection.ExecuteScalarAsync<int>(checkRelatedQuery, new { Id = id });

                if (relatedCount > 0)
                {
                    return Result<bool>.Failure(new Error("ReferenceConstraint", $"Cannot delete car model engine with id {id} because it has related records in konfiguracja table"));
                }

                var query = "DELETE FROM modelsilnik WHERE id = CAST(@Id AS INTEGER)";
                var rowsAffected = await _connection.ExecuteAsync(query, new { Id = id });

                if (rowsAffected == 0)
                {
                    return Result<bool>.Failure(new Error("NotFound", $"Car model engine with id {id} not found"));
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(new Error("DatabaseError", $"Failed to delete car model engine with id {id}: {ex.Message}"));
            }
        }

        public async Task<Result<bool>> DeleteByCarModelAndEngineIdAsync(string carModelId, string engineId)
        {
            try
            {
                // Check if there are related records in other tables
                var checkRelatedQuery = @"
                    SELECT COUNT(*) 
                    FROM konfiguracja k 
                    JOIN modelsilnik ms ON k.modelsilnikid = ms.id
                    WHERE ms.modelid = CAST(@CarModelId AS INTEGER) AND ms.silnikid = CAST(@EngineId AS INTEGER)";

                var relatedCount = await _connection.ExecuteScalarAsync<int>(checkRelatedQuery, new { CarModelId = carModelId, EngineId = engineId });

                if (relatedCount > 0)
                {
                    return Result<bool>.Failure(new Error("ReferenceConstraint", $"Cannot delete car model engine with carModelId {carModelId} and engineId {engineId} because it has related records in konfiguracja table"));
                }

                var query = "DELETE FROM modelsilnik WHERE modelid = CAST(@CarModelId AS INTEGER) AND silnikid = CAST(@EngineId AS INTEGER)";
                var rowsAffected = await _connection.ExecuteAsync(query, new { CarModelId = carModelId, EngineId = engineId });

                if (rowsAffected == 0)
                {
                    return Result<bool>.Failure(new Error("NotFound", $"Car model engine with carModelId {carModelId} and engineId {engineId} not found"));
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(new Error("DatabaseError", $"Failed to delete car model engine with carModelId {carModelId} and engineId {engineId}: {ex.Message}"));
            }
        }
    }
}