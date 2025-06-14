using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Services;

public interface IUserConfigurationService
{
    Task<Repositories.Helpers.Result<int>> SaveUserConfigurationAsync(int userId, SaveUserConfigurationRequest request);
    Task<Repositories.Helpers.Result<List<UserConfigurationDto>>> GetUserConfigurationsAsync(int userId);
    Task<Repositories.Helpers.Result<UserConfigurationDto>> GetUserConfigurationByIdAsync(int configurationId, int userId);
    Task<Repositories.Helpers.Result<bool>> DeleteUserConfigurationAsync(int configurationId, int userId);
    Task<Repositories.Helpers.Result<bool>> UpdateUserConfigurationAsync(UserConfigurationDto configurationDto, int userId);
}