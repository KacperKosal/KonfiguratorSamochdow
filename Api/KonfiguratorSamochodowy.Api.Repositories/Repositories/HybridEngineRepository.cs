using KonfiguratorSamochodowy.Api.Repositories.Dto;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories
{
    public class HybridEngineRepository : IEngineRepository
    {
        private readonly AppDbContext _dbContext;

        public HybridEngineRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Entity Framework implementation for GetAllAsync
        public async Task<Result<IEnumerable<Engine>>> GetAllAsync()
        {
            try
            {
                var engines = await _dbContext.Engines
                    .Where(e => e.IsActive)
                    .OrderByDescending(e => e.CreatedAt)
                    .ToListAsync();
                
                return Result<IEnumerable<Engine>>.Success(engines);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Engine>>.Failure(
                    new Error("DatabaseError", $"Error getting engines: {ex.Message}"));
            }
        }

        // Entity Framework implementation for GetByIdAsync
        public async Task<Result<Engine>> GetByIdAsync(string id)
        {
            try
            {
                var engine = await _dbContext.Engines
                    .FirstOrDefaultAsync(e => e.Id == id);

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

        // Entity Framework implementation for GetFilteredAsync
        public async Task<Result<IEnumerable<Engine>>> GetFilteredAsync(FilterEnginesRequestDto filter)
        {
            try
            {
                var query = _dbContext.Engines.AsQueryable();

                if (!string.IsNullOrEmpty(filter.Type))
                {
                    query = query.Where(e => e.Type == filter.Type);
                }

                if (!string.IsNullOrEmpty(filter.FuelType))
                {
                    query = query.Where(e => e.FuelType.Contains(filter.FuelType));
                }

                if (filter.MinCapacity.HasValue)
                {
                    query = query.Where(e => e.Capacity >= filter.MinCapacity.Value);
                }

                if (filter.MaxCapacity.HasValue)
                {
                    query = query.Where(e => e.Capacity <= filter.MaxCapacity.Value);
                }

                if (filter.MinPower.HasValue)
                {
                    query = query.Where(e => e.Power >= filter.MinPower.Value);
                }

                if (filter.MaxPower.HasValue)
                {
                    query = query.Where(e => e.Power <= filter.MaxPower.Value);
                }

                if (!string.IsNullOrEmpty(filter.Transmission))
                {
                    query = query.Where(e => e.Transmission == filter.Transmission);
                }

                if (!string.IsNullOrEmpty(filter.DriveType))
                {
                    query = query.Where(e => e.DriveType == filter.DriveType);
                }

                if (filter.IsActive.HasValue)
                {
                    query = query.Where(e => e.IsActive == filter.IsActive.Value);
                }

                var engines = await query
                    .OrderByDescending(e => e.CreatedAt)
                    .ToListAsync();
                
                return Result<IEnumerable<Engine>>.Success(engines);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Engine>>.Failure(
                    new Error("DatabaseError", $"Error filtering engines: {ex.Message}"));
            }
        }

        // Entity Framework implementation for CreateAsync
        public async Task<Result<Engine>> CreateAsync(Engine engine)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            
            try
            {
                if (string.IsNullOrEmpty(engine.Id))
                {
                    engine.Id = Guid.NewGuid().ToString();
                }
                
                engine.CreatedAt = DateTime.UtcNow;

                await _dbContext.Engines.AddAsync(engine);
                await _dbContext.SaveChangesAsync();
                
                await transaction.CommitAsync();
                
                return Result<Engine>.Success(engine);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                
                return Result<Engine>.Failure(
                    new Error("DatabaseError", $"Error creating engine: {ex.Message}"));
            }
        }

        // Direct SQL execution for UpdateAsync
        public async Task<Result<Engine>> UpdateAsync(string id, Engine engine)
        {
            try
            {
                // First check if the entity exists
                var existingEngine = await _dbContext.Engines
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (existingEngine == null)
                {
                    return Result<Engine>.Failure(
                        new Error("NotFound", $"Engine with ID {id} not found"));
                }

                // Keep the original ID and creation date
                engine.Id = id;
                engine.CreatedAt = existingEngine.CreatedAt;
                engine.UpdatedAt = DateTime.UtcNow;

                // Use raw SQL for update
                var sql = @"
                    UPDATE Engines SET
                        Name = {0},
                        Type = {1},
                        Capacity = {2},
                        Power = {3},
                        Torque = {4},
                        FuelType = {5},
                        Cylinders = {6},
                        Transmission = {7},
                        Gears = {8},
                        DriveType = {9},
                        FuelConsumption = {10},
                        CO2Emission = {11},
                        Description = {12},
                        IsActive = {13},
                        UpdatedAt = {14}
                    WHERE Id = {15}";

                await _dbContext.Database.ExecuteSqlRawAsync(sql,
                    engine.Name,
                    engine.Type,
                    engine.Capacity,
                    engine.Power,
                    engine.Torque,
                    engine.FuelType,
                    engine.Cylinders,
                    engine.Transmission,
                    engine.Gears,
                    engine.DriveType,
                    engine.FuelConsumption,
                    engine.CO2Emission,
                    engine.Description,
                    engine.IsActive,
                    engine.UpdatedAt,
                    id);

                // Get the updated entity
                var updatedEngine = await _dbContext.Engines.FirstOrDefaultAsync(e => e.Id == id);
                return Result<Engine>.Success(updatedEngine);
            }
            catch (Exception ex)
            {
                return Result<Engine>.Failure(
                    new Error("DatabaseError", $"Error updating engine with ID {id}: {ex.Message}"));
            }
        }

        // Direct SQL execution for DeleteAsync
        public async Task<Result<bool>> DeleteAsync(string id)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            
            try
            {
                // First check if the entity exists
                var existingEngine = await _dbContext.Engines
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (existingEngine == null)
                {
                    return Result<bool>.Failure(
                        new Error("NotFound", $"Engine with ID {id} not found"));
                }

                // Check if the engine is used in any car model
                var isUsed = await _dbContext.CarModelEngines
                    .AnyAsync(cme => cme.EngineId == id);

                if (isUsed)
                {
                    return Result<bool>.Failure(
                        new Error("ReferenceConstraint", 
                            $"Cannot delete engine with ID {id} because it is used by one or more car models"));
                }

                // Use raw SQL for delete
                var sql = "DELETE FROM Engines WHERE Id = {0}";
                await _dbContext.Database.ExecuteSqlRawAsync(sql, id);
                
                await transaction.CommitAsync();
                
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                
                return Result<bool>.Failure(
                    new Error("DatabaseError", $"Error deleting engine with ID {id}: {ex.Message}"));
            }
        }

        // Raw FromSqlRaw implementation for GetAllByVechicleIdAsync
        public async Task<IEnumerable<Engine>> GetAllByVechicleIdAsync(int vehicleId)
        {
            try
            {
                var sql = @"
                    SELECT e.* FROM Engines e
                    INNER JOIN VehicleEngines ve ON e.Id = ve.EngineId
                    WHERE ve.VehicleId = {0}
                    ORDER BY e.Name";

                var engines = await _dbContext.Engines
                    .FromSqlRaw(sql, vehicleId)
                    .ToListAsync();
                
                return engines;
            }
            catch
            {
                // Return empty collection on error
                return Enumerable.Empty<Engine>();
            }
        }
    }
}