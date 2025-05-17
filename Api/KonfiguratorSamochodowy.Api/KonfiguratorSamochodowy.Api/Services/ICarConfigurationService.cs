using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;

namespace KonfiguratorSamochodowy.Api.Services
{
    public interface ICarConfigurationService
    {
        Task<Result<CarConfigurationDto>> GetFullCarConfigurationAsync(string carModelId);
        Task<Result<CarConfigurationDto>> GetCarConfigurationWithEngineAsync(string carModelId, string engineId);
    }
}