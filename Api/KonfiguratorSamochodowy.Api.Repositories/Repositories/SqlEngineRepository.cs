using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using KonfiguratorSamochodowy.Api.Repositories.Dto;
using KonfiguratorSamochodowy.Api.Repositories.Enums;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using KonfiguratorSamochodowy.Api.Repositories.Repositories;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories
{
    public class SqlEngineRepository : IEngineRepository
    {
        private readonly IDbConnection _connection;

        public SqlEngineRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<Result<IEnumerable<Engine>>> GetAllAsync()
        {
            try
            {
                const string sql = @"
                    SELECT
                        id,
                        id::TEXT AS Id,
                        COALESCE(pojemnosc, '') AS Name,
                        typ AS Type,
                        CASE
                            WHEN pojemnosc LIKE '%L%' THEN 
                                CAST(SUBSTRING(TRIM(pojemnosc), 1, POSITION('L' IN pojemnosc) - 1) AS FLOAT)::INT * 1000
                            WHEN pojemnosc = '—' THEN 0
                            WHEN pojemnosc ~ E'^\\d+$' THEN CAST(pojemnosc AS INT)
                            ELSE 0
                        END AS Capacity,
                        moc AS Power,
                        CAST(moc * 1.5 AS INT) AS Torque,
                        CASE
                            WHEN typ = 'benzyna' THEN 'Benzyna'
                            WHEN typ = 'diesel' THEN 'Diesel'
                            WHEN typ = 'elektryczny' THEN 'Elektryczny'
                            ELSE typ
                        END AS FuelType,
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
                            WHEN EXISTS (SELECT 1 FROM cechypojazdu cp JOIN pojazd p ON cp.idpojazdu = p.id WHERE p.id = pojazdid AND cp.cecha = 'Napęd 4×4')
                            THEN 'AWD'
                            ELSE 'RWD'
                        END AS DriveType,
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
                    ORDER BY moc DESC";

                var engines = await _connection.QueryAsync<Engine>(sql);
                return Result<IEnumerable<Engine>>.Success(engines);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Engine>>.Failure(
                    new Error("DatabaseError", $"Error getting engines: {ex.Message}"));
            }
        }

        public async Task<Result<Engine>> GetByIdAsync(string id)
        {
            try
            {
                const string sql = @"
                    SELECT
                        id,
                        id::TEXT AS Id,
                        COALESCE(pojemnosc, '') AS Name,
                        typ AS Type,
                        CASE
                            WHEN pojemnosc LIKE '%L%' THEN 
                                CAST(SUBSTRING(TRIM(pojemnosc), 1, POSITION('L' IN pojemnosc) - 1) AS FLOAT)::INT * 1000
                            WHEN pojemnosc = '—' THEN 0
                            WHEN pojemnosc ~ E'^\\d+$' THEN CAST(pojemnosc AS INT)
                            ELSE 0
                        END AS Capacity,
                        moc AS Power,
                        CAST(moc * 1.5 AS INT) AS Torque,
                        CASE
                            WHEN typ = 'benzyna' THEN 'Benzyna'
                            WHEN typ = 'diesel' THEN 'Diesel'
                            WHEN typ = 'elektryczny' THEN 'Elektryczny'
                            ELSE typ
                        END AS FuelType,
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
                            WHEN EXISTS (SELECT 1 FROM cechypojazdu cp JOIN pojazd p ON cp.idpojazdu = p.id WHERE p.id = pojazdid AND cp.cecha = 'Napęd 4×4')
                            THEN 'AWD'
                            ELSE 'RWD'
                        END AS DriveType,
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
                    WHERE id = @Id";

                var engine = await _connection.QueryFirstOrDefaultAsync<Engine>(sql, new { Id = id });

                if (engine == null)
                {
                    return Result<Engine>.Failure(
                        new Error("NotFound", $"Engine with ID {id} not found"));
                }

                return Result<Engine>.Success(engine);
            }
            catch (Exception ex)
            {
                return Result<Engine>.Failure(
                    new Error("DatabaseError", $"Error getting engine with ID {id}: {ex.Message}"));
            }
        }

        public async Task<Result<IEnumerable<Engine>>> GetFilteredAsync(FilterEnginesRequestDto filter)
        {
            try
            {
                var conditions = new List<string>();
                var parameters = new DynamicParameters();

                // Base SQL query
                var sqlBuilder = new StringBuilder(@"
                    SELECT
                        id,
                        id::TEXT AS Id,
                        COALESCE(pojemnosc, '') AS Name,
                        typ AS Type,
                        CASE
                            WHEN pojemnosc LIKE '%L%' THEN 
                                CAST(SUBSTRING(TRIM(pojemnosc), 1, POSITION('L' IN pojemnosc) - 1) AS FLOAT)::INT * 1000
                            WHEN pojemnosc = '—' THEN 0
                            WHEN pojemnosc ~ E'^\\d+$' THEN CAST(pojemnosc AS INT)
                            ELSE 0
                        END AS Capacity,
                        moc AS Power,
                        CAST(moc * 1.5 AS INT) AS Torque,
                        CASE
                            WHEN typ = 'benzyna' THEN 'Benzyna'
                            WHEN typ = 'diesel' THEN 'Diesel'
                            WHEN typ = 'elektryczny' THEN 'Elektryczny'
                            ELSE typ
                        END AS FuelType,
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
                            WHEN EXISTS (SELECT 1 FROM cechypojazdu cp JOIN pojazd p ON cp.idpojazdu = p.id WHERE p.id = pojazdid AND cp.cecha = 'Napęd 4×4')
                            THEN 'AWD'
                            ELSE 'RWD'
                        END AS DriveType,
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
                    WHERE 1=1");

                // Add filter conditions
                if (!string.IsNullOrEmpty(filter.Type))
                {
                    conditions.Add("typ = @Type");
                    parameters.Add("@Type", filter.Type);
                }

                if (!string.IsNullOrEmpty(filter.FuelType))
                {
                    conditions.Add(@"CASE
                                        WHEN typ = 'benzyna' THEN 'Benzyna'
                                        WHEN typ = 'diesel' THEN 'Diesel'
                                        WHEN typ = 'elektryczny' THEN 'Elektryczny'
                                        ELSE typ
                                    END LIKE @FuelType");
                    parameters.Add("@FuelType", $"%{filter.FuelType}%");
                }

                if (filter.MinCapacity.HasValue)
                {
                    conditions.Add(@"CASE
                                        WHEN pojemnosc LIKE '%L%' THEN 
                                            CAST(SUBSTRING(TRIM(pojemnosc), 1, POSITION('L' IN pojemnosc) - 1) AS FLOAT)::INT * 1000
                                        WHEN pojemnosc = '—' THEN 0
                                        WHEN pojemnosc ~ E'^\\d+$' THEN CAST(pojemnosc AS INT)
                                        ELSE 0
                                    END >= @MinCapacity");
                    parameters.Add("@MinCapacity", filter.MinCapacity.Value);
                }

                if (filter.MaxCapacity.HasValue)
                {
                    conditions.Add(@"CASE
                                        WHEN pojemnosc LIKE '%L%' THEN 
                                            CAST(SUBSTRING(TRIM(pojemnosc), 1, POSITION('L' IN pojemnosc) - 1) AS FLOAT)::INT * 1000
                                        WHEN pojemnosc = '—' THEN 0
                                        WHEN pojemnosc ~ E'^\\d+$' THEN CAST(pojemnosc AS INT)
                                        ELSE 0
                                    END <= @MaxCapacity");
                    parameters.Add("@MaxCapacity", filter.MaxCapacity.Value);
                }

                if (filter.MinPower.HasValue)
                {
                    conditions.Add("moc >= @MinPower");
                    parameters.Add("@MinPower", filter.MinPower.Value);
                }

                if (filter.MaxPower.HasValue)
                {
                    conditions.Add("moc <= @MaxPower");
                    parameters.Add("@MaxPower", filter.MaxPower.Value);
                }

                if (!string.IsNullOrEmpty(filter.Transmission))
                {
                    conditions.Add("'Automatyczna' = @Transmission");
                    parameters.Add("@Transmission", filter.Transmission);
                }

                if (!string.IsNullOrEmpty(filter.DriveType))
                {
                    conditions.Add(@"CASE
                                        WHEN EXISTS (SELECT 1 FROM cechypojazdu cp JOIN pojazd p ON cp.idpojazdu = p.id WHERE p.id = pojazdid AND cp.cecha = 'Napęd 4×4')
                                        THEN 'AWD'
                                        ELSE 'RWD'
                                    END = @DriveType");
                    parameters.Add("@DriveType", filter.DriveType);
                }

                if (filter.IsActive.HasValue)
                {
                    conditions.Add("TRUE = @IsActive");
                    parameters.Add("@IsActive", filter.IsActive.Value);
                }

                // Add conditions to query
                if (conditions.Count > 0)
                {
                    sqlBuilder.AppendLine(" AND " + string.Join(" AND ", conditions));
                }

                // Add ordering
                sqlBuilder.AppendLine(" ORDER BY moc DESC");

                var sql = sqlBuilder.ToString();
                var engines = await _connection.QueryAsync<Engine>(sql, parameters);
                
                return Result<IEnumerable<Engine>>.Success(engines);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Engine>>.Failure(
                    new Error("DatabaseError", $"Error filtering engines: {ex.Message}"));
            }
        }

        public async Task<Result<Engine>> CreateAsync(Engine engine)
        {
            try
            {
                const string sql = @"
                    INSERT INTO silnik (pojazdid, pojemnosc, typ, moc)
                    VALUES (1, @Capacity, @Type, @Power)
                    RETURNING 
                        id,
                        id::TEXT AS Id,
                        COALESCE(pojemnosc, '') AS Name,
                        typ AS Type,
                        CASE
                            WHEN pojemnosc LIKE '%L%' THEN 
                                CAST(SUBSTRING(TRIM(pojemnosc), 1, POSITION('L' IN pojemnosc) - 1) AS FLOAT)::INT * 1000
                            WHEN pojemnosc = '—' THEN 0
                            WHEN pojemnosc ~ E'^\\d+$' THEN CAST(pojemnosc AS INT)
                            ELSE 0
                        END AS Capacity,
                        moc AS Power,
                        CAST(moc * 1.5 AS INT) AS Torque,
                        CASE
                            WHEN typ = 'benzyna' THEN 'Benzyna'
                            WHEN typ = 'diesel' THEN 'Diesel'
                            WHEN typ = 'elektryczny' THEN 'Elektryczny'
                            ELSE typ
                        END AS FuelType,
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
                        'RWD' AS DriveType,
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
                        NULL AS UpdatedAt";

                var parameters = new DynamicParameters();
                parameters.Add("@Capacity", engine.Capacity.ToString());
                parameters.Add("@Type", engine.Type?.ToLower() switch
                {
                    "benzyna" or "petrol" => "benzyna",
                    "diesel" => "diesel",
                    "elektryczny" or "electric" => "elektryczny",
                    _ => "benzyna"
                });
                parameters.Add("@Power", engine.Power);

                var createdEngine = await _connection.QueryFirstOrDefaultAsync<Engine>(sql, parameters);
                
                return Result<Engine>.Success(createdEngine);
            }
            catch (Exception ex)
            {
                return Result<Engine>.Failure(
                    new Error("DatabaseError", $"Error creating engine: {ex.Message}"));
            }
        }

        public async Task<Result<Engine>> UpdateAsync(string id, Engine engine)
        {
            try
            {
                // First check if the entity exists
                var existingResult = await GetByIdAsync(id);
                if (!existingResult.IsSuccess)
                {
                    return existingResult;
                }

                const string sql = @"
                    UPDATE silnik
                    SET
                        pojemnosc = @Capacity,
                        typ = @Type,
                        moc = @Power
                    WHERE id = @Id
                    RETURNING 
                        id,
                        id::TEXT AS Id,
                        COALESCE(pojemnosc, '') AS Name,
                        typ AS Type,
                        CASE
                            WHEN pojemnosc LIKE '%L%' THEN 
                                CAST(SUBSTRING(TRIM(pojemnosc), 1, POSITION('L' IN pojemnosc) - 1) AS FLOAT)::INT * 1000
                            WHEN pojemnosc = '—' THEN 0
                            WHEN pojemnosc ~ E'^\\d+$' THEN CAST(pojemnosc AS INT)
                            ELSE 0
                        END AS Capacity,
                        moc AS Power,
                        CAST(moc * 1.5 AS INT) AS Torque,
                        CASE
                            WHEN typ = 'benzyna' THEN 'Benzyna'
                            WHEN typ = 'diesel' THEN 'Diesel'
                            WHEN typ = 'elektryczny' THEN 'Elektryczny'
                            ELSE typ
                        END AS FuelType,
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
                            WHEN EXISTS (SELECT 1 FROM cechypojazdu cp JOIN pojazd p ON cp.idpojazdu = p.id WHERE p.id = pojazdid AND cp.cecha = 'Napęd 4×4')
                            THEN 'AWD'
                            ELSE 'RWD'
                        END AS DriveType,
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
                        NOW() AS UpdatedAt";

                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                parameters.Add("@Capacity", engine.Capacity.ToString());
                parameters.Add("@Type", engine.Type?.ToLower() switch
                {
                    "benzyna" or "petrol" => "benzyna",
                    "diesel" => "diesel",
                    "elektryczny" or "electric" => "elektryczny",
                    _ => "benzyna"
                });
                parameters.Add("@Power", engine.Power);

                var updatedEngine = await _connection.QueryFirstOrDefaultAsync<Engine>(sql, parameters);
                
                return Result<Engine>.Success(updatedEngine);
            }
            catch (Exception ex)
            {
                return Result<Engine>.Failure(
                    new Error("DatabaseError", $"Error updating engine with ID {id}: {ex.Message}"));
            }
        }

        public async Task<Result<bool>> DeleteAsync(string id)
        {
            try
            {
                // First check if the entity exists
                var existingResult = await GetByIdAsync(id);
                if (!existingResult.IsSuccess)
                {
                    return Result<bool>.Failure(existingResult.Error);
                }

                // Check if engine is used by any vehicles
                const string checkSql = @"
                    SELECT COUNT(*) AS Count
                    FROM pojazd
                    WHERE id IN (SELECT pojazdid FROM silnik WHERE id = @Id)";

                var count = await _connection.ExecuteScalarAsync<int>(checkSql, new { Id = id });
                if (count > 0)
                {
                    return Result<bool>.Failure(
                        new Error("ReferenceConstraint", 
                            $"Cannot delete engine with ID {id} because it is used by vehicles"));
                }

                const string sql = @"
                    DELETE FROM silnik
                    WHERE id = @Id";

                await _connection.ExecuteAsync(sql, new { Id = id });
                
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(
                    new Error("DatabaseError", $"Error deleting engine with ID {id}: {ex.Message}"));
            }
        }

        public async Task<IEnumerable<Engine>> GetAllByVechicleIdAsync(int vehicleId)
        {
            try
            {
                const string sql = @"
                    SELECT
                        s.id,
                        s.id::TEXT AS Id,
                        COALESCE(s.pojemnosc, '') AS Name,
                        s.typ AS Type,
                        CASE
                            WHEN s.pojemnosc LIKE '%L%' THEN 
                                CAST(SUBSTRING(TRIM(s.pojemnosc), 1, POSITION('L' IN s.pojemnosc) - 1) AS FLOAT)::INT * 1000
                            WHEN s.pojemnosc = '—' THEN 0
                            WHEN s.pojemnosc ~ E'^\\d+$' THEN CAST(s.pojemnosc AS INT)
                            ELSE 0
                        END AS Capacity,
                        s.moc AS Power,
                        CAST(s.moc * 1.5 AS INT) AS Torque,
                        CASE
                            WHEN s.typ = 'benzyna' THEN 'Benzyna'
                            WHEN s.typ = 'diesel' THEN 'Diesel'
                            WHEN s.typ = 'elektryczny' THEN 'Elektryczny'
                            ELSE s.typ
                        END AS FuelType,
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
                            WHEN EXISTS (SELECT 1 FROM cechypojazdu cp JOIN pojazd p ON cp.idpojazdu = p.id WHERE p.id = s.pojazdid AND cp.cecha = 'Napęd 4×4')
                            THEN 'AWD'
                            ELSE 'RWD'
                        END AS DriveType,
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
                    JOIN pojazd p ON s.pojazdid = p.id
                    WHERE p.id = @VehicleId
                    ORDER BY s.moc DESC";

                return await _connection.QueryAsync<Engine>(sql, new { VehicleId = vehicleId });
            }
            catch
            {
                // Return empty collection on error
                return Enumerable.Empty<Engine>();
            }
        }
    }
}