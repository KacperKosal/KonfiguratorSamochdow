using KonfiguratorSamochodowy.Api.Common;
using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Services
{
    public interface IEngineService
    {
        Task<Result<IEnumerable<EngineDto>>> GetAllAsync();
        Task<Result<EngineDto>> GetByIdAsync(string id);
        Task<Result<IEnumerable<EngineDto>>> GetFilteredAsync(FilterEnginesRequest filter);
        Task<Result<EngineDto>> CreateAsync(CreateEngineRequest request);
        Task<Result<EngineDto>> UpdateAsync(string id, UpdateEngineRequest request);
        Task<Result<bool>> DeleteAsync(string id);
    }
}