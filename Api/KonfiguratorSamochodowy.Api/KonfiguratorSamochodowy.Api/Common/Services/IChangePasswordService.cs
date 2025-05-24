using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Common.Services;

internal interface IChangePasswordService
{
    /// <summary>
    /// Zmienia hasło użytkownika
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika</param>
    /// <param name="request">Żądanie zmiany hasła</param>
    /// <returns>Task</returns>
    Task ChangePasswordAsync(int userId, ChangePasswordRequest request);
}