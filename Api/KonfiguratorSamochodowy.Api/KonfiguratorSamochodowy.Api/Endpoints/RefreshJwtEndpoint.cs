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

            var userId = context.GetUserId(jwtService);

            if (userId == -1 || refreshToken == null)
            {
                return Results.Unauthorized();
            }

            var newToken = await refreshJwtService.RefreshJwtAsync(refreshToken, userId);

            return Results.Ok(newToken);
        }).WithTags("Autentykacja").WithName("Odświeżanie tokena");
    }
}
