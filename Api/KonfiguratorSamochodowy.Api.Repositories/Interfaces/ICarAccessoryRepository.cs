using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Models;

namespace KonfiguratorSamochodowy.Api.Repositories.Interfaces
{
    public interface ICarAccessoryRepository
    {
        Task<Result<IEnumerable<CarAccessory>>> GetAllAsync();
        Task<Result<CarAccessory>> GetByIdAsync(string id);
        Task<Result<IEnumerable<CarAccessory>>> GetByCarIdAsync(string carId);
        Task<Result<IEnumerable<CarAccessory>>> GetByCarModelAsync(string carModel);
        Task<Result<IEnumerable<CarAccessory>>> GetByCategoryAsync(string category);
        Task<Result<IEnumerable<CarAccessory>>> GetByTypeAsync(string type);
        Task<Result<IEnumerable<CarAccessory>>> GetFilteredAsync(
            string? carId = null,
            string? carModel = null,
            string? category = null,
            string? type = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            bool? isOriginalBMWPart = null,
            bool? isInStock = null,
            string? installationDifficulty = null);
        Task<Result<CarAccessory>> CreateAsync(CarAccessory accessory);
        Task<Result<CarAccessory>> UpdateAsync(string id, CarAccessory accessory);
        Task<Result<bool>> DeleteAsync(string id);
        Task<Result<bool>> IsPartNumberUniqueAsync(string partNumber, string? excludeId = null);
    }
}
