using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Exceptions;
using KonfiguratorSamochodowy.Api.Requests;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace KonfiguratorSamochodowy.Api.Endpoints;

internal static class RegisterEndpoint
{
    internal static void MapEndPoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/register", async (RegisterRequest request, IUserCreateService userCreateService) =>
        {
            try
            {
                await userCreateService.CreateUserAsync(request);
            }
            catch (RegisterRequestInvalidFirstName e) {
                return Results.BadRequest(JsonSerializer.Deserialize<object>(e.Message));
            }
            catch (RegisterRequestInvalidLastName e) {
                return Results.BadRequest(JsonSerializer.Deserialize<object>(e.Message));
            }
            catch (RegisterRequestInvalidEmail e) {
                return Results.BadRequest(JsonSerializer.Deserialize<object>(e.Message));
            }
            catch (RegisterRequestEmailAlreadyExists e) {
                return Results.BadRequest(JsonSerializer.Deserialize<object>(e.Message));
            }
            catch (RegisterRequestInvalidPassword e) {
                return Results.BadRequest(JsonSerializer.Deserialize<object>(e.Message));
            }
            catch (RegisterRequestInvalidConfirmPassword e) {
                return Results.BadRequest(JsonSerializer.Deserialize<object>(e.Message));
            }
            
            return Results.Ok();
        }).WithTags("Autentykacja").WithName("Rejestracja");
    }
}