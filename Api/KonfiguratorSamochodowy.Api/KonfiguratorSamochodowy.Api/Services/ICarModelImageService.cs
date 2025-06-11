using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Models;

namespace KonfiguratorSamochodowy.Api.Services
{
    public interface ICarModelImageService
    {
        Task<Result<List<CarModelImage>>> GetImagesByCarModelIdAsync(string carModelId);
        Task<Result<CarModelImage>> AddImageAsync(string carModelId, IFormFile imageFile, string color = "");
        Task<Result<bool>> DeleteImageAsync(string imageId);
        Task<Result<bool>> UpdateImageOrderAsync(string imageId, int newOrder);
        Task<Result<bool>> SetMainImageAsync(string carModelId, string imageId);
    }
}