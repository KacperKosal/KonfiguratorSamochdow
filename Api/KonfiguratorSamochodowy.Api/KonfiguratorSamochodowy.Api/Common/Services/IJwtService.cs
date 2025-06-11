namespace KonfiguratorSamochodowy.Api.Common.Services;

internal interface IJwtService
{
    string GenerateToken(int userId, string role, string? name = null);
    string GenerateRefreshToken();
    bool ValidateToken(string token);
}
