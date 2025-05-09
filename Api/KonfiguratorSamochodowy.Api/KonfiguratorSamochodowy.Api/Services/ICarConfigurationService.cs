using KonfiguratorSamochodowy.Api.Common;
using KonfiguratorSamochodowy.Api.Dtos;

namespace KonfiguratorSamochodowy.Api.Services
{
    public interface ICarConfigurationService
    {
        Task<Result<CarConfigurationDto>> GetFullCarConfigurationAsync(string carModelId);
        Task<Result<CarConfigurationDto>> GetCarConfigurationWithEngineAsync(string carModelId, string engineId);
    }
}