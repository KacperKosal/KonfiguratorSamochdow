using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Services
{
    public interface ICarModelEngineService
    {
        Task<KonfiguratorSamochodowy.Api.Repositories.Helpers.Result<IEnumerable<CarModelEngineDto>>> GetAllAsync();
        Task<KonfiguratorSamochodowy.Api.Repositories.Helpers.Result<CarModelEngineDto>> GetByIdAsync(string id);
        Task<KonfiguratorSamochodowy.Api.Repositories.Helpers.Result<CarModelEngineDto>> GetByCarModelAndEngineIdAsync(string carModelId, string engineId);
        Task<KonfiguratorSamochodowy.Api.Repositories.Helpers.Result<IEnumerable<CarModelEngineDto>>> GetByCarModelIdAsync(string carModelId);
        Task<KonfiguratorSamochodowy.Api.Repositories.Helpers.Result<IEnumerable<EngineForModelDto>>> GetEnginesForCarModelAsync(string carModelId);
        Task<KonfiguratorSamochodowy.Api.Repositories.Helpers.Result<IEnumerable<CarModelEngineDto>>> GetByEngineIdAsync(string engineId);
        Task<KonfiguratorSamochodowy.Api.Repositories.Helpers.Result<CarModelEngineDto>> AddEngineToCarModelAsync(string carModelId, AddCarModelEngineRequest request);
        Task<KonfiguratorSamochodowy.Api.Repositories.Helpers.Result<CarModelEngineDto>> UpdateEngineForCarModelAsync(string carModelId, string engineId, UpdateCarModelEngineRequest request);
        Task<KonfiguratorSamochodowy.Api.Repositories.Helpers.Result<bool>> RemoveEngineFromCarModelAsync(string carModelId, string engineId);
    }
}