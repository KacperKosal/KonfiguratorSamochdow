using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Services;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KonfiguratorSamochodowy.Api.Extensions;

internal static class AuthExtensions
{
    internal static bool IsAuthenticated(this HttpContext context, IJwtService jwtService)
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out StringValues token) || string.IsNullOrEmpty(token)) return false;

        if (!token.ToString().StartsWith("Bearer")) return false;

        var tokenValue = token.ToString()["Bearer ".Length..].Trim();

        return jwtService.ValidateToken(tokenValue);
    }

    internal static string? GetRefreshToken(this HttpContext context)
    {
        return context.Request.Cookies["refreshToken"];
    }

    internal static int? GetUserIdFromToken(this HttpContext context, IJwtService jwtService)
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out StringValues token) || string.IsNullOrEmpty(token))
            return null;

        if (!token.ToString().StartsWith("Bearer"))
            return null;

        var tokenValue = token.ToString()["Bearer ".Length..].Trim();

        if (!jwtService.ValidateToken(tokenValue))
            return null;

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(tokenValue);
        
        var userIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
        {
            return userId;
        }

        return null;
    }

    internal static int? GetUserIdFromExpiredToken(this HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out StringValues token) || string.IsNullOrEmpty(token))
            return null;

        if (!token.ToString().StartsWith("Bearer"))
            return null;

        var tokenValue = token.ToString()["Bearer ".Length..].Trim();

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(tokenValue);
            
            var userIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
        }
        catch
        {
            // Token jest niepoprawny strukturalnie
            return null;
        }

        return null;
    }

    internal static RouteHandlerBuilder RequierdAuthenticatedUser(this RouteHandlerBuilder builder)
    {
        builder.AddEndpointFilter(async (context, next) =>
        {
            var jwtService = context.HttpContext.RequestServices.GetRequiredService<IJwtService>();

            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues token) || string.IsNullOrEmpty(token))
                return Results.Json(new{Error = "BRAK TOKENU W NAGŁÓWKU AUTORYZACYJNYM !"}, contentType: "application/json", statusCode: StatusCodes.Status401Unauthorized);

            if (!token.ToString().StartsWith("Bearer"))
                return Results.Json(new { Error = "NIEPRAWIDŁOWY FORMAT TOKENU" }, contentType: "application/json", statusCode: StatusCodes.Status401Unauthorized);

            var tokenValue = token.ToString()["Bearer ".Length..].Trim();

            if (!jwtService.ValidateToken(tokenValue))
                return Results.Json(new { Error = "NIEPRAWIDŁOWY LUB WYGASŁY TOKEN" }, contentType: "application/json", statusCode: StatusCodes.Status401Unauthorized);

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(tokenValue);

            context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(jsonToken.Claims.Select(c => new Claim(c.Type, c.Value))));

            return await next(context);
        });

        return builder;
    }
}