using KonfiguratorSamochodowy.Api.Models;
using KonfiguratorSamochodowy.Api.Repositories.Dto;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories
{
    public class EfCarModelRepository : ICarModelRepository
    {
        private readonly AppDbContext _dbContext;

        public EfCarModelRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Entity Framework implementation for GetAllAsync
        public async Task<Result<IEnumerable<CarModel>>> GetAllAsync()
        {
            try
            {
                var carModels = await _dbContext.CarModels
                    .ToListAsync();
                
                return Result<IEnumerable<CarModel>>.Success(carModels);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<CarModel>>.Failure(
                    new Error("DatabaseError", $"Error getting car models: {ex.Message}"));
            }
        }

        // Entity Framework implementation for GetByIdAsync
        public async Task<Result<CarModel>> GetByIdAsync(string id)
        {
            try
            {
                var carModel = await _dbContext.CarModels
                    .FirstOrDefaultAsync(cm => cm.Id == id);

                if (carModel == null)
                {
                    return Result<CarModel>.Failure(
                        new Error("NotFound", $"Car model with ID {id} not found"));
                }

                return Result<CarModel>.Success(carModel);
            }
            catch (Exception ex)
            {
                return Result<CarModel>.Failure(
                    new Error("DatabaseError", $"Error getting car model with ID {id}: {ex.Message}"));
            }
        }

        // Entity Framework implementation for GetFilteredAsync
        public async Task<Result<IEnumerable<CarModel>>> GetFilteredAsync(FilterCarModelsRequestDto filter)
        {
            try
            {
                var query = _dbContext.CarModels.AsQueryable();

                if (!string.IsNullOrEmpty(filter.Manufacturer))
                {
                    query = query.Where(c => c.Manufacturer.Contains(filter.Manufacturer));
                }

                if (!string.IsNullOrEmpty(filter.BodyType))
                {
                    query = query.Where(c => c.BodyType == filter.BodyType);
                }

                if (!string.IsNullOrEmpty(filter.Segment))
                {
                    query = query.Where(c => c.Segment == filter.Segment);
                }

                if (filter.MinProductionYear.HasValue)
                {
                    query = query.Where(c => c.ProductionYear >= filter.MinProductionYear.Value);
                }

                if (filter.MaxProductionYear.HasValue)
                {
                    query = query.Where(c => c.ProductionYear <= filter.MaxProductionYear.Value);
                }

                if (filter.MinPrice.HasValue)
                {
                    query = query.Where(c => c.BasePrice >= filter.MinPrice.Value);
                }

                if (filter.MaxPrice.HasValue)
                {
                    query = query.Where(c => c.BasePrice <= filter.MaxPrice.Value);
                }

                if (filter.IsActive.HasValue)
                {
                    query = query.Where(c => c.IsActive == filter.IsActive.Value);
                }

                var filteredCarModels = await query.ToListAsync();
                return Result<IEnumerable<CarModel>>.Success(filteredCarModels);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<CarModel>>.Failure(
                    new Error("DatabaseError", $"Error filtering car models: {ex.Message}"));
            }
        }

        // Entity Framework implementation for CreateAsync
        public async Task<Result<CarModel>> CreateAsync(CarModel carModel)
        {
            try
            {
                if (string.IsNullOrEmpty(carModel.Id))
                {
                    carModel.Id = Guid.NewGuid().ToString();
                }
                
                carModel.CreatedAt = DateTime.UtcNow;

                await _dbContext.CarModels.AddAsync(carModel);
                await _dbContext.SaveChangesAsync();

                return Result<CarModel>.Success(carModel);
            }
            catch (Exception ex)
            {
                return Result<CarModel>.Failure(
                    new Error("DatabaseError", $"Error creating car model: {ex.Message}"));
            }
        }

        // Entity Framework implementation for UpdateAsync
        public async Task<Result<CarModel>> UpdateAsync(string id, CarModel carModel)
        {
            try
            {
                var existingCarModel = await _dbContext.CarModels
                    .FirstOrDefaultAsync(cm => cm.Id == id);

                if (existingCarModel == null)
                {
                    return Result<CarModel>.Failure(
                        new Error("NotFound", $"Car model with ID {id} not found"));
                }

                // Keep the original creation date
                carModel.Id = id;
                carModel.CreatedAt = existingCarModel.CreatedAt;
                carModel.UpdatedAt = DateTime.UtcNow;

                _dbContext.Entry(existingCarModel).State = EntityState.Detached;
                _dbContext.CarModels.Update(carModel);
                await _dbContext.SaveChangesAsync();

                return Result<CarModel>.Success(carModel);
            }
            catch (Exception ex)
            {
                return Result<CarModel>.Failure(
                    new Error("DatabaseError", $"Error updating car model with ID {id}: {ex.Message}"));
            }
        }

        // Entity Framework implementation for DeleteAsync
        public async Task<Result<bool>> DeleteAsync(string id)
        {
            try
            {
                var carModel = await _dbContext.CarModels
                    .FirstOrDefaultAsync(cm => cm.Id == id);

                if (carModel == null)
                {
                    return Result<bool>.Failure(
                        new Error("NotFound", $"Car model with ID {id} not found"));
                }

                _dbContext.CarModels.Remove(carModel);
                await _dbContext.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(
                    new Error("DatabaseError", $"Error deleting car model with ID {id}: {ex.Message}"));
            }
        }
    }
}