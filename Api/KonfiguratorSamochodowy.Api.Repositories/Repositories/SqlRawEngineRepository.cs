using System.Data;
using Dapper;
using KonfiguratorSamochodowy.Api.Repositories.Dto;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories
{
    public class SqlRawEngineRepository : IEngineRepository
    {
        private readonly IDbConnection _connection;

        public SqlRawEngineRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        
        // Raw SQL implementation for GetAllAsync
        public async Task<Result<IEnumerable<Engine>>> GetAllAsync()
        {
            try
            {
                const string sql = @"
                    SELECT * FROM Engines
                    WHERE IsActive = true
                    ORDER BY CreatedAt DESC";

                var engines = await _connection.QueryAsync<Engine>(sql);
                return Result<IEnumerable<Engine>>.Success(engines);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Engine>>.Failure(
                    new Error("DatabaseError", $"Error getting engines: {ex.Message}"));
            }
        }

        // Raw SQL implementation for GetByIdAsync
        public async Task<Result<Engine>> GetByIdAsync(string id)
        {
            try
            {
                const string sql = @"
                    SELECT * FROM Engines
                    WHERE Id = @Id";

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

        // Raw SQL implementation for GetFilteredAsync with dynamic SQL
        public async Task<Result<IEnumerable<Engine>>> GetFilteredAsync(FilterEnginesRequestDto filter)
        {
            try
            {
                var conditions = new List<string>();
                var parameters = new DynamicParameters();

                if (!string.IsNullOrEmpty(filter.Type))
                {
                    conditions.Add("Type = @Type");
                    parameters.Add("@Type", filter.Type);
                }

                if (!string.IsNullOrEmpty(filter.FuelType))
                {
                    conditions.Add("FuelType LIKE @FuelType");
                    parameters.Add("@FuelType", $"%{filter.FuelType}%");
                }

                if (filter.MinCapacity.HasValue)
                {
                    conditions.Add("Capacity >= @MinCapacity");
                    parameters.Add("@MinCapacity", filter.MinCapacity.Value);
                }

                if (filter.MaxCapacity.HasValue)
                {
                    conditions.Add("Capacity <= @MaxCapacity");
                    parameters.Add("@MaxCapacity", filter.MaxCapacity.Value);
                }

                if (filter.MinPower.HasValue)
                {
                    conditions.Add("Power >= @MinPower");
                    parameters.Add("@MinPower", filter.MinPower.Value);
                }

                if (filter.MaxPower.HasValue)
                {
                    conditions.Add("Power <= @MaxPower");
                    parameters.Add("@MaxPower", filter.MaxPower.Value);
                }

                if (!string.IsNullOrEmpty(filter.Transmission))
                {
                    conditions.Add("Transmission = @Transmission");
                    parameters.Add("@Transmission", filter.Transmission);
                }

                if (!string.IsNullOrEmpty(filter.DriveType))
                {
                    conditions.Add("DriveType = @DriveType");
                    parameters.Add("@DriveType", filter.DriveType);
                }

                if (filter.IsActive.HasValue)
                {
                    conditions.Add("IsActive = @IsActive");
                    parameters.Add("@IsActive", filter.IsActive.Value);
                }

                var whereClause = conditions.Count > 0
                    ? "WHERE " + string.Join(" AND ", conditions)
                    : string.Empty;

                var sql = $@"
                    SELECT * FROM Engines
                    {whereClause}
                    ORDER BY CreatedAt DESC";

                var engines = await _connection.QueryAsync<Engine>(sql, parameters);
                
                return Result<IEnumerable<Engine>>.Success(engines);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Engine>>.Failure(
                    new Error("DatabaseError", $"Error filtering engines: {ex.Message}"));
            }
        }

        // Raw SQL implementation for CreateAsync
        public async Task<Result<Engine>> CreateAsync(Engine engine)
        {
            try
            {
                if (string.IsNullOrEmpty(engine.Id))
                {
                    engine.Id = Guid.NewGuid().ToString();
                }
                
                engine.CreatedAt = DateTime.UtcNow;

                const string sql = @"
                    INSERT INTO Engines (
                        Id, Name, Type, Capacity, Power, Torque, FuelType, 
                        Cylinders, Transmission, Gears, DriveType, 
                        FuelConsumption, CO2Emission, Description, IsActive, 
                        CreatedAt, UpdatedAt
                    ) VALUES (
                        @Id, @Name, @Type, @Capacity, @Power, @Torque, @FuelType, 
                        @Cylinders, @Transmission, @Gears, @DriveType, 
                        @FuelConsumption, @CO2Emission, @Description, @IsActive, 
                        @CreatedAt, @UpdatedAt
                    )
                    RETURNING *";

                var createdEngine = await _connection.QueryFirstOrDefaultAsync<Engine>(sql, engine);
                
                return Result<Engine>.Success(createdEngine);
            }
            catch (Exception ex)
            {
                return Result<Engine>.Failure(
                    new Error("DatabaseError", $"Error creating engine: {ex.Message}"));
            }
        }

        // Raw SQL implementation for UpdateAsync
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

                // Keep the original ID and creation date
                engine.Id = id;
                engine.CreatedAt = existingResult.Value.CreatedAt;
                engine.UpdatedAt = DateTime.UtcNow;

                const string sql = @"
                    UPDATE Engines SET
                        Name = @Name,
                        Type = @Type,
                        Capacity = @Capacity,
                        Power = @Power,
                        Torque = @Torque,
                        FuelType = @FuelType,
                        Cylinders = @Cylinders,
                        Transmission = @Transmission,
                        Gears = @Gears,
                        DriveType = @DriveType,
                        FuelConsumption = @FuelConsumption,
                        CO2Emission = @CO2Emission,
                        Description = @Description,
                        IsActive = @IsActive,
                        UpdatedAt = @UpdatedAt
                    WHERE Id = @Id
                    RETURNING *";

                var updatedEngine = await _connection.QueryFirstOrDefaultAsync<Engine>(sql, engine);
                
                return Result<Engine>.Success(updatedEngine);
            }
            catch (Exception ex)
            {
                return Result<Engine>.Failure(
                    new Error("DatabaseError", $"Error updating engine with ID {id}: {ex.Message}"));
            }
        }

        // Raw SQL implementation for DeleteAsync
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

                // Check if the engine is used in any car model
                const string checkSql = @"
                    SELECT COUNT(*) FROM CarModelEngines
                    WHERE EngineId = @Id";

                var count = await _connection.ExecuteScalarAsync<int>(checkSql, new { Id = id });
                if (count > 0)
                {
                    return Result<bool>.Failure(
                        new Error("ReferenceConstraint", 
                            $"Cannot delete engine with ID {id} because it is used by {count} car models"));
                }

                const string sql = @"
                    DELETE FROM Engines
                    WHERE Id = @Id";

                await _connection.ExecuteAsync(sql, new { Id = id });
                
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(
                    new Error("DatabaseError", $"Error deleting engine with ID {id}: {ex.Message}"));
            }
        }

        // Raw SQL implementation for GetAllByVechicleIdAsync
        public async Task<IEnumerable<Engine>> GetAllByVechicleIdAsync(int vehicleId)
        {
            try
            {
                const string sql = @"
                    SELECT e.* FROM Engines e
                    INNER JOIN VehicleEngines ve ON e.Id = ve.EngineId
                    WHERE ve.VehicleId = @VehicleId
                    ORDER BY e.Name";

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