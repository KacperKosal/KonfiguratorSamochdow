using KonfiguratorSamochodowy.Api.Requests;
using KonfiguratorSamochodowy.Api.Common.Services;

namespace KonfiguratorSamochodowy.Api.Endpoints;

internal static class ModelsEndpoint
{
    internal static void MapEndPoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/models", async (IModelsService modelService, CancellationToken concellationToken, [AsParameters]GetModelsRequest request) =>
        {
            var models = await modelService.GetModelsAsync(request, concellationToken);
            return Results.Ok(models);
        }).WithTags("Modele").WithName("Pobierz modele");

        builder.MapGet("/models/image/{id}", async (IModelsService modelService, CancellationToken concellationToken, int id) =>
        {
            var image = await modelService.GetModelImageAsync(id, concellationToken);
            return Results.File(image, "image/png");
        }).WithTags("Modele").WithName("Pobierz model po ID");
    }
}