using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Repositories.Enums;
using KonfiguratorSamochodowy.Api.Requests;
using Microsoft.AspNetCore.Mvc;

namespace KonfiguratorSamochodowy.Api.Endpoints
{
    public static class CarAccessoryEndpoints
    {
        public static void MapCarAccessoryEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/car-accessories")
                .WithTags("Akcesoria samochodowe");

            // Pobieranie wszystkich akcesoriów
            group.MapGet("/", async ([FromServices] ICarAccessoryService service) =>
            {
                var result = await service.GetAllAsync();
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("GetAllCarAccessories")
            .WithDisplayName("Pobierz wszystkie akcesoria")
            .Produces<IEnumerable<CarAccessoryDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            // Pobieranie pojedynczego akcesorium po ID
            group.MapGet("/{id}", async ([FromRoute] string id, [FromServices] ICarAccessoryService service) =>
            {
                var result = await service.GetByIdAsync(id);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.NotFound(result.Error.Message);
            })
            .WithName("GetCarAccessoryById")
            .WithDisplayName("Pobierz akcesorium po ID")
            .Produces<CarAccessoryDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            // Pobieranie akcesoriów dla konkretnego samochodu
            group.MapGet("/car/{carId}", async ([FromRoute] string carId, [FromServices] ICarAccessoryService service) =>
            {
                var result = await service.GetByCarIdAsync(carId);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("GetCarAccessoriesByCarId")
            .WithDisplayName("Pobierz akcesoria dla danego samochodu")
            .Produces<IEnumerable<CarAccessoryDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            // Pobieranie akcesoriów dla konkretnego modelu samochodu
            group.MapGet("/model/{carModel}", async ([FromRoute] string carModel, [FromServices] ICarAccessoryService service) =>
            {
                var result = await service.GetByCarModelAsync(carModel);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("GetCarAccessoriesByCarModel")
            .WithDisplayName("Pobierz akcesoria dla danego modelu samochodu")
            .Produces<IEnumerable<CarAccessoryDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            // Pobieranie akcesoriów według kategorii
            group.MapGet("/category/{category}", async ([FromRoute] string category, [FromServices] ICarAccessoryService service) =>
            {
                if (!AccessoryCategory.AllCategories.Contains(category))
                {
                    return Results.BadRequest($"Nieprawidłowa kategoria: {category}");
                }

                var result = await service.GetByCategoryAsync(category);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("GetCarAccessoriesByCategory")
            .WithDisplayName("Pobierz akcesoria według kategorii")
            .Produces<IEnumerable<CarAccessoryDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            // Pobieranie akcesoriów według typu
            group.MapGet("/type/{type}", async ([FromRoute] string type, [FromServices] ICarAccessoryService service) =>
            {
                if (!AccessoryType.AllTypes.Contains(type))
                {
                    return Results.BadRequest($"Nieprawidłowy typ akcesorium: {type}");
                }

                var result = await service.GetByTypeAsync(type);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("GetCarAccessoriesByType")
            .WithDisplayName("Pobierz akcesoria według typu")
            .Produces<IEnumerable<CarAccessoryDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            // Pobieranie akcesoriów z filtrowaniem
            group.MapGet("/filter", async ([AsParameters] FilterCarAccessoriesRequest request, [FromServices] ICarAccessoryService service) =>
            {
                var result = await service.GetFilteredAsync(request);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("GetFilteredCarAccessories")
            .WithDisplayName("Pobierz akcesoria z filtrowaniem")
            .Produces<IEnumerable<CarAccessoryDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            // Pobieranie pełnej konfiguracji akcesoriów samochodu
            group.MapGet("/configuration/{carId}", async ([FromRoute] string carId, [FromServices] ICarAccessoryService service) =>
            {
                var result = await service.GetFullCarConfigurationAsync(carId);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("GetFullCarAccessoryConfiguration")
            .WithDisplayName("Pobierz pełną konfigurację akcesoriów samochodu")
            .Produces<CarAccessoryConfigurationDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            // Tworzenie nowego akcesorium
            group.MapPost("/", async ([FromBody] CreateCarAccessoryRequest request, [FromServices] ICarAccessoryService service) =>
            {
                var result = await service.CreateAsync(request);
                return result.IsSuccess
                    ? Results.Created($"/api/car-accessories/{result.Value.Id}", result.Value)
                    : Results.BadRequest(result.Error.Message);
            })
            .WithName("CreateCarAccessory")
            .WithDisplayName("Utwórz nowe akcesorium")
            .Produces<CarAccessoryDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            // Aktualizacja istniejącego akcesorium
            group.MapPut("/{id}", async ([FromRoute] string id, [FromBody] UpdateCarAccessoryRequest request,
                [FromServices] ICarAccessoryService service) =>
            {
                var result = await service.UpdateAsync(id, request);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : result.Error.Code == "NotFound"
                        ? Results.NotFound(result.Error.Message)
                        : Results.BadRequest(result.Error.Message);
            })
            .WithName("UpdateCarAccessory")
            .WithDisplayName("Zaktualizuj akcesorium")
            .Produces<CarAccessoryDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

            // Usuwanie akcesorium
            group.MapDelete("/{id}", async ([FromRoute] string id, [FromServices] ICarAccessoryService service) =>
            {
                var result = await service.DeleteAsync(id);
                return result.IsSuccess
                    ? Results.NoContent()
                    : Results.NotFound(result.Error.Message);
            })
            .WithName("DeleteCarAccessory")
            .WithDisplayName("Usuń akcesorium")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}