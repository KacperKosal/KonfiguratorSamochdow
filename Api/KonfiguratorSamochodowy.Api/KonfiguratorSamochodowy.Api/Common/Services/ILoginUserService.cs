using KonfiguratorSamochodowy.Api.Requests;
namespace KonfiguratorSamochodowy.Api.Common.Services;

internal interface ILoginUserService
{
    /// <summary>
    /// Loguje użytkownika
    /// </summary>
    /// <param name="request">Żądanie logowania użytkownika</param>
    /// <returns>Identyfikator zalogowanego użytkownika</returns>
    Task LoginUserAsync(LoginRequest request);
}
