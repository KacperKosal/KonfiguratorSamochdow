using System.Data;
using Dapper;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories;

internal class EngineRepository(IDbConnection connection) : IEngineRepository
{
    public Task<int> CreateAsync(Engine engineFeatures)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Engine>> GetAllByVechicleIdAsync(int engineId)
    {
        const string sql = "SELECT * FROM Silnik WHERE PojazdID = @EngineId";
        return connection.QueryAsync<Engine>(sql, new { EngineId = engineId });
    }

    public Task UpdateAsync(Engine engineFeatures)
    {
        throw new NotImplementedException();
    }
}
