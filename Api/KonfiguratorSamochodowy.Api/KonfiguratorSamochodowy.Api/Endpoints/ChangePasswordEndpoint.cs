using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Exceptions;
using KonfiguratorSamochodowy.Api.Extensions;
using KonfiguratorSamochodowy.Api.Requests;
using System.Text.Json;

namespace KonfiguratorSamochodowy.Api.Endpoints;

internal static class ChangePasswordEndpoint
{
    internal static void MapEndPoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/change-password", async (HttpContext context, ChangePasswordRequest request, 
            IChangePasswordService changePasswordService, IJwtService jwtService) =>
        {
            try
            {
                // Sprawdzenie czy użytkownik jest zalogowany
                if (!context.IsAuthenticated(jwtService))
                {
                    return Results.Unauthorized();
                }
                
                // Pobranie ID użytkownika z tokenu
                var userId = context.GetUserIdFromToken(jwtService);
                if (userId == null)
                {
                    return Results.Unauthorized();
                }
                
                await changePasswordService.ChangePasswordAsync(userId.Value, request);
                
                return Results.Ok(new { message = "Hasło zostało pomyślnie zmienione." });
            }
            catch (ChangePasswordInvalidCurrentPassword e)
            {
                return Results.BadRequest(JsonSerializer.Deserialize<object>(e.Message));
            }
            catch (ChangePasswordInvalidNewPassword e)
            {
                return Results.BadRequest(JsonSerializer.Deserialize<object>(e.Message));
            }
            catch (Exception)
            {
                return Results.StatusCode(500);
            }
        }).WithTags("Autentykacja").WithName("ZmianaHasla").RequierdAuthenticatedUser();
    }
}