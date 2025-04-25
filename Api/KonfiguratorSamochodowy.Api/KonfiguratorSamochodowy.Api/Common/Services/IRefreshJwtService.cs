namespace KonfiguratorSamochodowy.Api.Common.Services;

internal interface IRefreshJwtService 
{
    Task<string> RefreshJwtAsync(string refreshToken, int userId);
}
