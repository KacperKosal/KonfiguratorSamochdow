using KonfiguratorSamochodowy.Api.Models;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;

namespace KonfiguratorSamochodowy.Api.Repositories.Interfaces
{
    public interface ICarModelEngineRepository
    {
        Task<Result<IEnumerable<CarModelEngine>>> GetAllAsync();
        Task<Result<CarModelEngine>> GetByIdAsync(string id);
        Task<Result<CarModelEngine>> GetByCarModelAndEngineIdAsync(string carModelId, string engineId);
        Task<Result<IEnumerable<CarModelEngine>>> GetByCarModelIdAsync(string carModelId);
        Task<Result<IEnumerable<CarModelEngine>>> GetByEngineIdAsync(string engineId);
        Task<Result<CarModelEngine>> CreateAsync(CarModelEngine carModelEngine);
        Task<Result<CarModelEngine>> UpdateAsync(string id, CarModelEngine carModelEngine);
        Task<Result<bool>> DeleteAsync(string id);
        Task<Result<bool>> DeleteByCarModelAndEngineIdAsync(string carModelId, string engineId);
    }
}