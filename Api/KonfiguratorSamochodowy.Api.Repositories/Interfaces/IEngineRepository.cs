using KonfiguratorSamochodowy.Api.Repositories.Models;

namespace KonfiguratorSamochodowy.Api.Repositories.Interfaces;

public interface IEngineRepository
{
    Task<IEnumerable<Engine>> GetAllByVechicleIdAsync(int engineId);

    Task<int> CreateAsync(Engine engineFeatures);

    Task UpdateAsync(Engine engineFeatures);

    Task DeleteAsync(int id);
}
