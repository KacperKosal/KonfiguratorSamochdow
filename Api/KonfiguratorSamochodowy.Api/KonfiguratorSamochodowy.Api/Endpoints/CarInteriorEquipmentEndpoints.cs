using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Repositories.Enums;
using KonfiguratorSamochodowy.Api.Requests;
using Microsoft.AspNetCore.Mvc;

namespace KonfiguratorSamochodowy.Api.Endpoints
{
    public static class CarInteriorEquipmentEndpoints
    {
        public static void MapCarInteriorEquipmentEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/car-interior-equipment")
                .WithTags("Wyposażenie wnętrza samochodu");

            // Pobieranie wszystkich elementów wyposażenia
            group.MapGet("/", async ([FromServices] ICarInteriorEquipmentService service) =>
            {
                var result = await service.GetAllAsync();
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("GetAllCarInteriorEquipment")
            .WithDisplayName("Pobierz wszystkie elementy wyposażenia wnętrza")
            .Produces<IEnumerable<CarInteriorEquipmentDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            // Pobieranie pojedynczego elementu wyposażenia po ID
            group.MapGet("/{id}", async ([FromRoute] string id, [FromServices] ICarInteriorEquipmentService service) =>
            {
                var result = await service.GetByIdAsync(id);
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : Results.NotFound(result.Error.Message);
            })
            .WithName("GetCarInteriorEquipmentById")
            .WithDisplayName("Pobierz element wyposażenia wnętrza po ID")
            .Produces<CarInteriorEquipmentDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            // Pobieranie wyposażenia dla konkretnego samochodu
            group.MapGet("/car/{carId}", async ([FromRoute] string carId, [FromServices] ICarInteriorEquipmentService service) =>
            {
                var result = await service.GetByCarIdAsync(carId);
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("GetCarInteriorEquipmentByCarId")
            .WithDisplayName("Pobierz wyposażenie wnętrza dla danego samochodu")
            .Produces<IEnumerable<CarInteriorEquipmentDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);
            
            // Pobieranie wyposażenia dla konkretnego modelu samochodu
            group.MapGet("/model/{carModel}", async ([FromRoute] string carModel, [FromServices] ICarInteriorEquipmentService service) =>
            {
                var result = await service.GetByCarModelAsync(carModel);
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("GetCarInteriorEquipmentByCarModel")
            .WithDisplayName("Pobierz wyposażenie wnętrza dla danego modelu samochodu")
            .Produces<IEnumerable<CarInteriorEquipmentDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            // Pobieranie wyposażenia według typu
            group.MapGet("/type/{type}", async ([FromRoute] string type, [FromServices] ICarInteriorEquipmentService service) =>
            {
                if (!InteriorEquipmentType.AllTypes.Contains(type))
                {
                    return Results.BadRequest($"Nieprawidłowy typ wyposażenia: {type}");
                }
                
                var result = await service.GetByTypeAsync(type);
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("GetCarInteriorEquipmentByType")
            .WithDisplayName("Pobierz wyposażenie wnętrza według typu")
            .Produces<IEnumerable<CarInteriorEquipmentDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            // Pobieranie wyposażenia z filtrowaniem
            group.MapGet("/filter", async ([AsParameters] FilterCarInteriorEquipmentRequest request, [FromServices] ICarInteriorEquipmentService service) =>
            {
                var result = await service.GetFilteredAsync(request);
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("GetFilteredCarInteriorEquipment")
            .WithDisplayName("Pobierz wyposażenie wnętrza z filtrowaniem")
            .Produces<IEnumerable<CarInteriorEquipmentDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            // Pobieranie pełnej konfiguracji samochodu
            group.MapGet("/configuration/{carId}", async ([FromRoute] string carId, [FromServices] ICarInteriorEquipmentService service) =>
            {
                var result = await service.GetFullCarConfigurationAsync(carId);
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("GetFullCarInteriorConfiguration")
            .WithDisplayName("Pobierz pełną konfigurację wyposażenia wnętrza samochodu")
            .Produces<CarInteriorConfigurationDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            // Tworzenie nowego elementu wyposażenia
            group.MapPost("/", async ([FromBody] CreateCarInteriorEquipmentRequest request, [FromServices] ICarInteriorEquipmentService service) =>
            {
                var result = await service.CreateAsync(request);
                return result.IsSuccess 
                    ? Results.Created($"/api/car-interior-equipment/{result.Value.Id}", result.Value) 
                    : Results.BadRequest(result.Error.Message);
            })
            .WithName("CreateCarInteriorEquipment")
            .WithDisplayName("Utwórz nowy element wyposażenia wnętrza")
            .Produces<CarInteriorEquipmentDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            // Aktualizacja istniejącego elementu wyposażenia
            group.MapPut("/{id}", async ([FromRoute] string id, [FromBody] UpdateCarInteriorEquipmentRequest request, 
                [FromServices] ICarInteriorEquipmentService service) =>
            {
                var result = await service.UpdateAsync(id, request);
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : result.Error.Code == "NotFound" 
                        ? Results.NotFound(result.Error.Message) 
                        : Results.BadRequest(result.Error.Message);
            })
            .WithName("UpdateCarInteriorEquipment")
            .WithDisplayName("Zaktualizuj element wyposażenia wnętrza")
            .Produces<CarInteriorEquipmentDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

            // Usuwanie elementu wyposażenia
            group.MapDelete("/{id}", async ([FromRoute] string id, [FromServices] ICarInteriorEquipmentService service) =>
            {
                var result = await service.DeleteAsync(id);
                return result.IsSuccess 
                    ? Results.NoContent() 
                    : Results.NotFound(result.Error.Message);
            })
            .WithName("DeleteCarInteriorEquipment")
            .WithDisplayName("Usuń element wyposażenia wnętrza")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}