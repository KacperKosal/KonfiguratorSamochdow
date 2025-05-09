using KonfiguratorSamochodowy.Api.Common;
using KonfiguratorSamochodowy.Api.Models;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Repositories
{
    public interface IEngineRepository
    {
        Task<Result<IEnumerable<Engine>>> GetAllAsync();
        Task<Result<Engine>> GetByIdAsync(string id);
        Task<Result<IEnumerable<Engine>>> GetFilteredAsync(FilterEnginesRequest filter);
        Task<Result<Engine>> CreateAsync(Engine engine);
        Task<Result<Engine>> UpdateAsync(string id, Engine engine);
        Task<Result<bool>> DeleteAsync(string id);
    }
}