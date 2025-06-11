using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Models;

namespace KonfiguratorSamochodowy.Api.Repositories.Interfaces
{
    public interface ICarModelColorRepository
    {
        Task<Result<List<CarModelColor>>> GetColorsByCarModelIdAsync(string carModelId);
        Task<Result<CarModelColor>> GetColorByCarModelIdAndNameAsync(string carModelId, string colorName);
        Task<Result<CarModelColor>> CreateOrUpdateColorAsync(CarModelColor color);
        Task<Result<bool>> DeleteColorAsync(string id);
        Task<Result<List<string>>> GetAvailableColorsWithPricesForModelAsync(string carModelId);
    }
}