namespace KonfiguratorSamochodowy.Api.Common.Services;

internal interface IJwtService
{
    string GenerateToken(int userId, string role);
    string GenerateRefreshToken();
    bool ValidateToken(string token);
}
