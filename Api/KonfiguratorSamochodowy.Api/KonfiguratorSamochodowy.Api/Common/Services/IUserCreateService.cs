using KonfiguratorSamochodowy.Api.Requests;
namespace KonfiguratorSamochodowy.Api.Common.Services;

internal interface IUserCreateService
{
    /// <summary>
    /// Tworzy nowego użytkownika
    /// </summary>
    /// <param name="request">Żądanie rejestracji użytkownika</param>
    /// <returns>Identyfikator nowego użytkownika</returns>
    Task CreateUserAsync(RegisterRequest request);
    
}
