using System.Data;
using Dapper;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories;

internal class VechicleFeaturesRepository(IDbConnection connection) : IVechicleFeaturesRepository
{
    public Task<int> CreateAsync(VehicleFeatures vehicleFeatures)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<VehicleFeatures>> GetAllByVechicleIdAsync(int vechicleId)
    {
        const string sql = "SELECT * FROM CechyPojazdu WHERE IDPojazdu = @VechicleId";
        return connection.QueryAsync<VehicleFeatures>(sql, new { VechicleId = vechicleId });   

    }

    public Task UpdateAsync(VehicleFeatures vehicleFeatures)
    {
        throw new NotImplementedException();
    }
}
