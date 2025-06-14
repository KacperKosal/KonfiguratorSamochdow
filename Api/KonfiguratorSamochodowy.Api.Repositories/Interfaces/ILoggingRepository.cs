using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Enums;

namespace KonfiguratorSamochodowy.Api.Repositories.Interfaces
{
    public interface ILoggingRepository
    {
        Task<Result<bool>> LogLoginAttemptAsync(int? userId, string ipAddress, Statustyp status);
    }
} 