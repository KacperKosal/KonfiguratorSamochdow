using KonfiguratorSamochodowy.Api.Common.Services;

namespace KonfiguratorSamochodowy.Api.Endpoints;


internal static class ValidateJwtEndpoint
{
    internal static void MapEndPoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/validate-jwt", async (HttpContext context, IJwtService jwtService) =>
        {
            var validationResult = context.Request.Headers.TryGetValue("Authorization", out var token);
            if (!validationResult || string.IsNullOrEmpty(token) || !token.Contains("Bearer"))
            {
                return Results.Unauthorized();
            }
            token = token.ToString().Replace("Bearer ", string.Empty);
            var isValid = jwtService.ValidateToken(token);
            if (!isValid)
            {
                return Results.Unauthorized();
            }
            return Results.Ok(new { message = "Token is valid" });

        }).WithTags("Autentykacja").WithName("Weryfikacja tokena");
    }
}
