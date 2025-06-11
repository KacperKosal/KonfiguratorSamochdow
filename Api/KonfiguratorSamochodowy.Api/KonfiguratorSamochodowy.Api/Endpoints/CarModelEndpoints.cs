using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Extensions;
using KonfiguratorSamochodowy.Api.Requests;
using KonfiguratorSamochodowy.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace KonfiguratorSamochodowy.Api.Endpoints
{
    public static class CarModelEndpoints
    {
        public static void MapCarModelEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/car-models")
                .WithTags("Modele samochodów");

            // Pobieranie wszystkich modeli
            group.MapGet("/", async ([FromServices] ICarModelService service, HttpContext context, [FromServices] IJwtService jwtService) =>
            {
                bool isAdmin = false;

                if (context.Request.Headers.TryGetValue("Authorization", out StringValues authorizationValue)) 
                    {
                        var token = authorizationValue.ToString().Replace("Bearer ", string.Empty);
                        jwtService.ValidateToken(authorizationValue.ToString().Replace("Bearer ", string.Empty));
                        
                        if (jwtService.ValidateToken(authorizationValue.ToString().Replace("Bearer ", string.Empty)))
                            {
                                var handler = new JwtSecurityTokenHandler();
                                var jsonToken = handler.ReadJwtToken(token);
                                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(jsonToken.Claims.Select(c => new Claim(c.Type, c.Value))));
                                var role = claimsPrincipal.FindFirst(c => c.Type == ClaimTypes.Role)?.Value;
                                isAdmin = role == "Administrator";
                                }
                    }
                var result = await service.GetAllAsync();
                return result.IsSuccess 
                    ? Results.Ok(result.Value.Where(e => isAdmin? true : e.IsActive)) 
                    : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("GetAllCarModels")
            .WithDisplayName("Pobierz wszystkie modele samochodów")
            .Produces<IEnumerable<CarModelDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            // Pobieranie modelu po ID
            group.MapGet("/{id}", async ([FromRoute] int id, [FromServices] ICarModelService service) =>
            {
                var result = await service.GetByIdAsync(id.ToString());
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : Results.NotFound(result.Error.Message);
            })
            .WithName("GetCarModelById")
            .WithDisplayName("Pobierz model samochodu po ID")
            .Produces<CarModelDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            // Filtrowanie modeli
            group.MapGet("/filter", async ([AsParameters] FilterCarModelsRequest request, [FromServices] ICarModelService service) =>
            {
                var result = await service.GetFilteredAsync(request);
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : Results.Problem(result.Error.Message, statusCode: 500);
            })
            .WithName("FilterCarModels")
            .WithDisplayName("Filtruj modele samochodów")
            .Produces<IEnumerable<CarModelDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            // Tworzenie nowego modelu
            group.MapPost("/", async ([FromBody] CreateCarModelRequest request, [FromServices] ICarModelService service) =>
            {
                var result = await service.CreateAsync(request);
                return result.IsSuccess 
                    ? Results.Created($"/api/car-models/{result.Value.Id}", result.Value) 
                    : Results.BadRequest(result.Error.Message);
            })
            .RequiredAuthenticatedUser()
            .WithName("CreateCarModel")
            .WithDisplayName("Utwórz nowy model samochodu")
            .Produces<CarModelDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError);

            // Aktualizacja modelu
            group.MapPut("/{id}", async ([FromRoute] int id, [FromBody] UpdateCarModelRequest request, 
                [FromServices] ICarModelService service) =>
            {
                var result = await service.UpdateAsync(id.ToString(), request);
                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : result.Error.Code == "NotFound" 
                        ? Results.NotFound(result.Error.Message) 
                        : Results.BadRequest(result.Error.Message);
            })
            .RequiredAuthenticatedUser()
            .WithName("UpdateCarModel")
            .WithDisplayName("Zaktualizuj model samochodu")
            .Produces<CarModelDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound);

            // Usuwanie modelu
            group.MapDelete("/{id}", async ([FromRoute] int id, [FromServices] ICarModelService service) =>
            {
                var result = await service.DeleteAsync(id.ToString());
                return result.IsSuccess 
                    ? Results.NoContent() 
                    : Results.NotFound(result.Error.Message);
            })
            .RequiredAuthenticatedUser()
            .WithName("DeleteCarModel")
            .WithDisplayName("Usuń model samochodu")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}