using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Models;

namespace KonfiguratorSamochodowy.Api.Repositories.Interfaces
{
    public interface ICarModelImageRepository
    {
        Task<Result<List<CarModelImage>>> GetImagesByCarModelIdAsync(string carModelId);
        Task<Result<CarModelImage>> GetImageByIdAsync(string imageId);
        Task<Result<CarModelImage>> AddImageAsync(CarModelImage image);
        Task<Result<bool>> DeleteImageAsync(string imageId);
        Task<Result<bool>> UpdateImageOrderAsync(string imageId, int newOrder);
        Task<Result<bool>> SetMainImageAsync(string carModelId, string imageId);
        Task<Result<int>> GetImageCountByCarModelIdAsync(string carModelId);
    }
}