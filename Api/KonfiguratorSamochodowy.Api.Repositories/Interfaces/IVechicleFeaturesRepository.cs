using KonfiguratorSamochodowy.Api.Repositories.Models;

namespace KonfiguratorSamochodowy.Api.Repositories.Interfaces;

public interface IVechicleFeaturesRepository
{
    Task<IEnumerable<VehicleFeatures>> GetAllByVechicleIdAsync(int vechicleId);

    Task<int> CreateAsync(VehicleFeatures vehicleFeatures);

    Task UpdateAsync(VehicleFeatures vehicleFeatures);
    
    Task DeleteAsync(int id);
}
