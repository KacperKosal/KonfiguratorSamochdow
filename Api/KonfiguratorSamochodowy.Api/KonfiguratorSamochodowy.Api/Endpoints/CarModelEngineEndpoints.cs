using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Requests;
using KonfiguratorSamochodowy.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace KonfiguratorSamochodowy.Api.Endpoints
{
    public static class CarModelEngineEndpoints
    {
        public static void MapCarModelEngineEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/car-models")
                .WithTags("Powiązania model-silnik");

            // Pobieranie silników dla danego modelu
            group.MapGet("/{carModelId}/engines", async ([FromRoute] string carModelId, [FromServices] ICarModelEngineService service) =>
            {
                var result = await service.GetEnginesForCarModelAsync(carModelId);
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : result.Error.Code == "NotFound" 
                        ? Results.NotFound(result.Error.Message) 
                        : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("GetEnginesByCarModel")
            .WithDisplayName("Pobierz silniki dla modelu samochodu")
            .Produces<IEnumerable<EngineForModelDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            // Pobieranie konkretnego powiązania model-silnik
            group.MapGet("/{carModelId}/engines/{engineId}", async ([FromRoute] string carModelId, [FromRoute] string engineId, 
                [FromServices] ICarModelEngineService service) =>
            {
                var result = await service.GetByCarModelAndEngineIdAsync(carModelId, engineId);
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : Results.NotFound(result.Error.Message);
            })
            .WithName("GetCarModelEngineByIds")
            .WithDisplayName("Pobierz powiązanie model-silnik")
            .Produces<CarModelEngineDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            // Dodawanie silnika do modelu
            group.MapPost("/{carModelId}/engines", async ([FromRoute] string carModelId, [FromBody] AddCarModelEngineRequest request, 
                [FromServices] ICarModelEngineService service) =>
            {
                var result = await service.AddEngineToCarModelAsync(carModelId, request);
                return result.IsSuccess 
                    ? Results.Created($"/api/car-models/{carModelId}/engines/{request.EngineId}", result.Value) 
                    : result.Error.Code == "NotFound" 
                        ? Results.NotFound(result.Error.Message) 
                        : Results.BadRequest(result.Error.Message);
            })
            .WithName("AddEngineToCarModel")
            .WithDisplayName("Dodaj silnik do modelu samochodu")
            .Produces<CarModelEngineDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

            // Aktualizacja powiązania model-silnik
            group.MapPut("/{carModelId}/engines/{engineId}", async ([FromRoute] string carModelId, [FromRoute] string engineId, 
                [FromBody] UpdateCarModelEngineRequest request, [FromServices] ICarModelEngineService service) =>
            {
                var result = await service.UpdateEngineForCarModelAsync(carModelId, engineId, request);
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : result.Error.Code == "NotFound" 
                        ? Results.NotFound(result.Error.Message) 
                        : Results.BadRequest(result.Error.Message);
            })
            .WithName("UpdateCarModelEngine")
            .WithDisplayName("Zaktualizuj powiązanie model-silnik")
            .Produces<CarModelEngineDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

            // Usuwanie silnika z modelu
            group.MapDelete("/{carModelId}/engines/{engineId}", async ([FromRoute] string carModelId, [FromRoute] string engineId, 
                [FromServices] ICarModelEngineService service) =>
            {
                var result = await service.RemoveEngineFromCarModelAsync(carModelId, engineId);
                return result.IsSuccess 
                    ? Results.NoContent() 
                    : Results.NotFound(result.Error.Message);
            })
            .WithName("RemoveEngineFromCarModel")
            .WithDisplayName("Usuń silnik z modelu samochodu")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

            // Endpointy dla silników
            var enginesGroup = app.MapGroup("/api/engines")
                .WithTags("Powiązania model-silnik");

            // Pobieranie modeli dla danego silnika
            enginesGroup.MapGet("/{engineId}/car-models", async ([FromRoute] string engineId, [FromServices] ICarModelEngineService service) =>
            {
                var result = await service.GetByEngineIdAsync(engineId);
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : result.Error.Code == "NotFound" 
                        ? Results.NotFound(result.Error.Message) 
                        : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("GetCarModelsByEngine")
            .WithDisplayName("Pobierz modele samochodów dla silnika")
            .Produces<IEnumerable<CarModelEngineDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}