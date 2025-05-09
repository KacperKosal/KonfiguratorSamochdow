using KonfiguratorSamochodowy.Api.Common;
using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Services
{
    public interface ICarModelEngineService
    {
        Task<Result<IEnumerable<CarModelEngineDto>>> GetAllAsync();
        Task<Result<CarModelEngineDto>> GetByIdAsync(string id);
        Task<Result<CarModelEngineDto>> GetByCarModelAndEngineIdAsync(string carModelId, string engineId);
        Task<Result<IEnumerable<CarModelEngineDto>>> GetByCarModelIdAsync(string carModelId);
        Task<Result<IEnumerable<CarModelEngineDto>>> GetByEngineIdAsync(string engineId);
        Task<Result<CarModelEngineDto>> AddEngineToCarModelAsync(string carModelId, AddCarModelEngineRequest request);
        Task<Result<CarModelEngineDto>> UpdateEngineForCarModelAsync(string carModelId, string engineId, UpdateCarModelEngineRequest request);
        Task<Result<bool>> RemoveEngineFromCarModelAsync(string carModelId, string engineId);
    }
}