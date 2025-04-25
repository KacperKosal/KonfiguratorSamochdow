using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Exceptions;
using KonfiguratorSamochodowy.Api.Requests;
using System.Text.Json;
namespace KonfiguratorSamochodowy.Api.Endpoints;

internal static class LoginEndpoint
{
    internal static void MapEndPoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/login", async (LoginRequest request, ILoginUserService loginUserService) =>
        {
            try
            {
                await loginUserService.LoginUserAsync(request);
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
            return Results.Ok();
        }).WithTags("Autentykacja").WithName("Logowanie");
    }
}
