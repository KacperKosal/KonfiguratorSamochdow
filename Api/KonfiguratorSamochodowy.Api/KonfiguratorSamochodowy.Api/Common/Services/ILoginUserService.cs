using KonfiguratorSamochodowy.Api.Requests;
using KonfiguratorSamochodowy.Api.Result;
namespace KonfiguratorSamochodowy.Api.Common.Services;

internal interface ILoginUserService
{
    /// <summary>
    /// Loguje użytkownika
    /// </summary>
    /// <param name="request">Żądanie logowania użytkownika</param>
    /// <param name="ipAddress">Adres IP użytkownika</param>
    /// <returns>Identyfikator zalogowanego użytkownika</returns>
    Task<LoginResult> LoginUserAsync(LoginRequest request, string ipAddress);
}
