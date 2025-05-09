using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Requests;
using KonfiguratorSamochodowy.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace KonfiguratorSamochodowy.Api.Endpoints
{
    public static class EngineEndpoints
    {
        public static void MapEngineEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/engines")
                .WithTags("Silniki");

            // Pobieranie wszystkich silników
            group.MapGet("/", async ([FromServices] IEngineService service) =>
            {
                var result = await service.GetAllAsync();
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("GetAllEngines")
            .WithDisplayName("Pobierz wszystkie silniki")
            .Produces<IEnumerable<EngineDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            // Pobieranie silnika po ID
            group.MapGet("/{id}", async ([FromRoute] string id, [FromServices] IEngineService service) =>
            {
                var result = await service.GetByIdAsync(id);
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : Results.NotFound(result.Error.Message);
            })
            .WithName("GetEngineById")
            .WithDisplayName("Pobierz silnik po ID")
            .Produces<EngineDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            // Filtrowanie silników
            group.MapGet("/filter", async ([AsParameters] FilterEnginesRequest request, [FromServices] IEngineService service) =>
            {
                var result = await service.GetFilteredAsync(request);
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("FilterEngines")
            .WithDisplayName("Filtruj silniki")
            .Produces<IEnumerable<EngineDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            // Tworzenie nowego silnika
            group.MapPost("/", async ([FromBody] CreateEngineRequest request, [FromServices] IEngineService service) =>
            {
                var result = await service.CreateAsync(request);
                return result.IsSuccess 
                    ? Results.Created($"/api/engines/{result.Value.Id}", result.Value) 
                    : Results.BadRequest(result.Error.Message);
            })
            .WithName("CreateEngine")
            .WithDisplayName("Utwórz nowy silnik")
            .Produces<EngineDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            // Aktualizacja silnika
            group.MapPut("/{id}", async ([FromRoute] string id, [FromBody] UpdateEngineRequest request, 
                [FromServices] IEngineService service) =>
            {
                var result = await service.UpdateAsync(id, request);
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : result.Error.Code == "NotFound" 
                        ? Results.NotFound(result.Error.Message) 
                        : Results.BadRequest(result.Error.Message);
            })
            .WithName("UpdateEngine")
            .WithDisplayName("Zaktualizuj silnik")
            .Produces<EngineDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

            // Usuwanie silnika
            group.MapDelete("/{id}", async ([FromRoute] string id, [FromServices] IEngineService service) =>
            {
                var result = await service.DeleteAsync(id);
                return result.IsSuccess 
                    ? Results.NoContent() 
                    : Results.NotFound(result.Error.Message);
            })
            .WithName("DeleteEngine")
            .WithDisplayName("Usuń silnik")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}