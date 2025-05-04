using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Common.Services
{
    public interface ICarAccessoryService
    {
        Task<Result<IEnumerable<CarAccessoryDto>>> GetAllAsync();
        Task<Result<CarAccessoryDto>> GetByIdAsync(string id);
        Task<Result<IEnumerable<CarAccessoryDto>>> GetByCarIdAsync(string carId);
        Task<Result<IEnumerable<CarAccessoryDto>>> GetByCarModelAsync(string carModel);
        Task<Result<IEnumerable<CarAccessoryDto>>> GetByCategoryAsync(string category);
        Task<Result<IEnumerable<CarAccessoryDto>>> GetByTypeAsync(string type);
        Task<Result<IEnumerable<CarAccessoryDto>>> GetFilteredAsync(FilterCarAccessoriesRequest request);
        Task<Result<CarAccessoryConfigurationDto>> GetFullCarConfigurationAsync(string carId);
        Task<Result<CarAccessoryDto>> CreateAsync(CreateCarAccessoryRequest request);
        Task<Result<CarAccessoryDto>> UpdateAsync(string id, UpdateCarAccessoryRequest request);
        Task<Result<bool>> DeleteAsync(string id);
    }
}
