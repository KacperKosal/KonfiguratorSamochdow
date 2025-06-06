using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Extensions;

namespace KonfiguratorSamochodowy.Api.Endpoints;

internal static class RefreshJwtEndpoint
{
    internal static void MapEndPoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/refresh-jwt", async (HttpContext context, IJwtService jwtService, IRefreshJwtService refreshJwtService, IConfiguration configuration) =>
        {
            var refreshToken = context.GetRefreshToken();

            // Użyj nowej metody, która działa z wygasłymi tokenami
            var userId = context.GetUserIdFromExpiredToken();

            if (userId == null || refreshToken == null)
            {
                return Results.Unauthorized();
            }

            var newToken = await refreshJwtService.RefreshJwtAsync(refreshToken, userId.Value);

            return Results.Ok(newToken);
        }).WithTags("Autentykacja").WithName("Odświeżanie tokena");
    }
}
