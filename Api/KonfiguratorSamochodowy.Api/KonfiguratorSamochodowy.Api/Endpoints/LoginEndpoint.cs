using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Exceptions;
using KonfiguratorSamochodowy.Api.Requests;
using System.Text.Json;
namespace KonfiguratorSamochodowy.Api.Endpoints;

internal static class LoginEndpoint
{
    internal static void MapEndPoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/login", async (HttpContext context, LoginRequest request, ILoginUserService loginUserService, IConfiguration configuration) =>
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            try
            {
                var result = await loginUserService.LoginUserAsync(request, ipAddress);

                int refreshTokenExpiration = configuration.GetValue<int>("JwtInformations:RefreshTokenExpirationSeconds");

                context.Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddSeconds(refreshTokenExpiration)
                });

                return Results.Ok(result.Token); 
            }
            catch (LoginRequestEmailEmpty e)
            {
                return Results.BadRequest(JsonSerializer.Deserialize<object>(e.Message));
            }
            catch (LoginRequestInvalidEmail e)
            {
                return Results.BadRequest(JsonSerializer.Deserialize<object>(e.Message));
            }
            catch (LoginRequestInvalidPassword e)
            {
                return Results.BadRequest(JsonSerializer.Deserialize<object>(e.Message));
            }
        }).WithTags("Autentykacja").WithName("Logowanie");
    }
}
