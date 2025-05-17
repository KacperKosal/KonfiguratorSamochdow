using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Services
{
    public interface ICarModelService
    {
        Task<Result<IEnumerable<CarModelDto>>> GetAllAsync();
        Task<Result<CarModelDto>> GetByIdAsync(string id);
        Task<Result<IEnumerable<CarModelDto>>> GetFilteredAsync(FilterCarModelsRequest filter);
        Task<Result<CarModelDto>> CreateAsync(CreateCarModelRequest request);
        Task<Result<CarModelDto>> UpdateAsync(string id, UpdateCarModelRequest request);
        Task<Result<bool>> DeleteAsync(string id);
    }
}