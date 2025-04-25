using KonfiguratorSamochodowy.Api.Common.Services;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KonfiguratorSamochodowy.Api.Extensions;

internal static class AuthExtensions
{
    internal static bool IsAuthenticated(this HttpContext context, IJwtService jwtService)
    {
        if (!context.Request.Headers.TryGetValue("Authorizatoin", out StringValues token) || string.IsNullOrEmpty(token)) return false;

        if (!token.ToString().StartsWith("Bearer")) return false;

        var tokenValue = token.ToString()["Bearer ".Length..].Trim();

        return jwtService.ValidateToken(tokenValue);
    }
    internal static int GetUserId(this HttpContext context, IJwtService jwtService)
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out StringValues token) || string.IsNullOrEmpty(token)) return -1;

        if (!token.ToString().StartsWith("Bearer")) return -1;

        var tokenValue = token.ToString()["Bearer ".Length..].Trim();

        if (!jwtService.ValidateToken(tokenValue)) return -1;

        var handler = new JwtSecurityTokenHandler();

        var jwtToken = handler.ReadJwtToken(tokenValue);

        var nameIdentifierClaim = jwtToken.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier);

        return nameIdentifierClaim != null ? int.Parse(nameIdentifierClaim.Value) : -1;
    }
    internal static string? GetRefreshToken(this HttpContext context)
    {
        if (!context.Request.Cookies.TryGetValue("refreshToken", out string refreshToken) || string.IsNullOrEmpty(refreshToken)) return null;
        return refreshToken;
    }
}