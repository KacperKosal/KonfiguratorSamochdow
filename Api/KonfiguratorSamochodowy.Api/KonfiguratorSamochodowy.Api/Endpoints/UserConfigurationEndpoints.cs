using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Requests;
using KonfiguratorSamochodowy.Api.Services;
using KonfiguratorSamochodowy.Api.Extensions;
using KonfiguratorSamochodowy.Api.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace KonfiguratorSamochodowy.Api.Endpoints;

public static class UserConfigurationEndpoints
{
    public static void MapEndPoint(WebApplication app)
    {
        var group = app.MapGroup("/api/user-configurations")
            .WithTags("User Configurations");

        // Zapisz konfigurację użytkownika
        group.MapPost("/", async (
            HttpContext context,
            SaveUserConfigurationRequest request,
            IUserConfigurationService service,
            IJwtService jwtService) =>
        {
            var userId = context.GetUserIdFromToken(jwtService);
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var result = await service.SaveUserConfigurationAsync(userId.Value, request);
            
            if (result.IsSuccess)
            {
                return Results.Ok(new { Id = result.Value, Message = "Konfiguracja została zapisana pomyślnie" });
            }
            
            // Sprawdź czy to błąd walidacji (np. limit konfiguracji)
            if (result.Error.Code == "Configuration.LimitExceeded")
            {
                return Results.BadRequest(new { error = result.Error.Message });
            }
            
            return Results.Problem(result.Error.Message, statusCode: 500);
        })
        .WithName("SaveUserConfiguration")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Zapisz konfigurację użytkownika",
            Description = "Zapisuje konfigurację samochodu w koncie użytkownika"
        })
        .RequiredAuthenticatedUser();

        // Pobierz wszystkie konfiguracje użytkownika
        group.MapGet("/", async (
            HttpContext context,
            IUserConfigurationService service,
            IJwtService jwtService) =>
        {
            var userId = context.GetUserIdFromToken(jwtService);
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var result = await service.GetUserConfigurationsAsync(userId.Value);
            
            return result.IsSuccess 
                ? Results.Ok(result.Value)
                : Results.Problem(result.Error.Message, statusCode: 500);
        })
        .WithName("GetUserConfigurations")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Pobierz konfiguracje użytkownika",
            Description = "Pobiera wszystkie zapisane konfiguracje zalogowanego użytkownika"
        })
        .Produces<List<UserConfigurationDto>>(StatusCodes.Status200OK)
        .RequiredAuthenticatedUser();

        // Pobierz konkretną konfigurację użytkownika
        group.MapGet("/{configurationId:int}", async (
            HttpContext context,
            [FromRoute] int configurationId,
            IUserConfigurationService service,
            IJwtService jwtService) =>
        {
            var userId = context.GetUserIdFromToken(jwtService);
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var result = await service.GetUserConfigurationByIdAsync(configurationId, userId.Value);
            
            return result.IsSuccess 
                ? Results.Ok(result.Value)
                : result.Error.Code == "Configuration.NotFound"
                    ? Results.NotFound(result.Error.Message)
                    : Results.Problem(result.Error.Message, statusCode: 500);
        })
        .WithName("GetUserConfigurationById")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Pobierz konfigurację po ID",
            Description = "Pobiera konkretną konfigurację użytkownika po jej identyfikatorze"
        })
        .Produces<UserConfigurationDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequiredAuthenticatedUser();

        // Usuń konfigurację użytkownika
        group.MapDelete("/{configurationId:int}", async (
            HttpContext context,
            [FromRoute] int configurationId,
            IUserConfigurationService service,
            IJwtService jwtService) =>
        {
            var userId = context.GetUserIdFromToken(jwtService);
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var result = await service.DeleteUserConfigurationAsync(configurationId, userId.Value);
            
            return result.IsSuccess && result.Value
                ? Results.Ok(new { Message = "Konfiguracja została usunięta" })
                : Results.NotFound(new { Message = "Konfiguracja nie została znaleziona" });
        })
        .WithName("DeleteUserConfiguration")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Usuń konfigurację użytkownika",
            Description = "Usuwa konfigurację z konta użytkownika"
        })
        .RequiredAuthenticatedUser();
    }
}