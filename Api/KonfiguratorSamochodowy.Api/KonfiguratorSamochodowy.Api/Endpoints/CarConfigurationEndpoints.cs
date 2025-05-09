using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace KonfiguratorSamochodowy.Api.Endpoints
{
    public static class CarConfigurationEndpoints
    {
        public static void MapCarConfigurationEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/car-configurations")
                .WithTags("Konfiguracje samochodów");

            // Pobieranie pełnej konfiguracji dla modelu samochodu
            group.MapGet("/{carModelId}", async ([FromRoute] string carModelId, [FromServices] ICarConfigurationService service) =>
            {
                var result = await service.GetFullCarConfigurationAsync(carModelId);
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : result.Error.Code == "NotFound" 
                        ? Results.NotFound(result.Error.Message) 
                        : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("GetFullCarConfiguration")
            .WithDisplayName("Pobierz pełną konfigurację samochodu")
            .Produces<CarConfigurationDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            // Pobieranie konfiguracji dla modelu samochodu z konkretnym silnikiem
            group.MapGet("/{carModelId}/engines/{engineId}", async ([FromRoute] string carModelId, [FromRoute] string engineId, 
                [FromServices] ICarConfigurationService service) =>
            {
                var result = await service.GetCarConfigurationWithEngineAsync(carModelId, engineId);
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : result.Error.Code == "NotFound" 
                        ? Results.NotFound(result.Error.Message) 
                        : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("GetCarConfigurationWithEngine")
            .WithDisplayName("Pobierz konfigurację samochodu z konkretnym silnikiem")
            .Produces<CarConfigurationDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}