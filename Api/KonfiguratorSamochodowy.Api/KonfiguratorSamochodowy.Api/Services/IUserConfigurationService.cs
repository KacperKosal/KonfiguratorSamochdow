using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Services;

public interface IUserConfigurationService
{
    Task<Result<int>> SaveUserConfigurationAsync(int userId, SaveUserConfigurationRequest request);
    Task<Result<List<UserConfigurationDto>>> GetUserConfigurationsAsync(int userId);
    Task<Result<UserConfigurationDto>> GetUserConfigurationByIdAsync(int configurationId, int userId);
    Task<Result<bool>> DeleteUserConfigurationAsync(int configurationId, int userId);
}