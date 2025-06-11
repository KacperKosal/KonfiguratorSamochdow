using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Models;

namespace KonfiguratorSamochodowy.Api.Services
{
    public interface ICarModelColorService
    {
        Task<Result<List<CarModelColor>>> GetColorsByCarModelIdAsync(string carModelId);
        Task<Result<CarModelColor>> SetColorPriceAsync(string carModelId, string colorName, int price);
        Task<Result<CarModelColor>> GetColorPriceAsync(string carModelId, string colorName);
        Task<Result<Dictionary<string, int>>> GetColorPricesForModelAsync(string carModelId);
    }
}