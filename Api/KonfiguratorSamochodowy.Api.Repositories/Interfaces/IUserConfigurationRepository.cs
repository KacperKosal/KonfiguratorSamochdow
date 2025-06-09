using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Models;

namespace KonfiguratorSamochodowy.Api.Repositories.Interfaces;

public interface IUserConfigurationRepository
{
    Task<Result<int>> SaveUserConfigurationAsync(UserConfiguration configuration);
    Task<Result<List<UserConfiguration>>> GetUserConfigurationsAsync(int userId);
    Task<Result<UserConfiguration>> GetUserConfigurationByIdAsync(int configurationId, int userId);
    Task<Result<bool>> DeleteUserConfigurationAsync(int configurationId, int userId);
    Task<Result<bool>> UpdateUserConfigurationAsync(UserConfiguration configuration);
}