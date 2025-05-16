using KonfiguratorSamochodowy.Api.Repositories.Dto;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Models;


namespace KonfiguratorSamochodowy.Api.Repositories
{
    public interface IEngineRepository
    {
        Task<Result<IEnumerable<Engine>>> GetAllAsync();
        Task<Result<Engine>> GetByIdAsync(string id);
        Task<Result<IEnumerable<Engine>>> GetFilteredAsync(FilterEnginesRequestDto filter);
        Task<Result<Engine>> CreateAsync(Engine engine);
        Task<Result<Engine>> UpdateAsync(string id, Engine engine);
        Task<Result<bool>> DeleteAsync(string id);
    }
}