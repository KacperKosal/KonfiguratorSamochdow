using KonfiguratorSamochodowy.Api.Services;
using KonfiguratorSamochodowy.Api.Extensions;

namespace KonfiguratorSamochodowy.Api.Endpoints
{
    public static class CarModelColorEndpoints
    {
        public static void MapCarModelColorEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/api/car-model-colors")
                .WithTags("Car Model Colors");

            group.MapGet("/{carModelId}", GetColorsByCarModelId)
                .WithName("GetColorsByCarModelId")
                .WithOpenApi();

            group.MapPost("/{carModelId}/{colorName}/price", SetColorPrice)
                .WithName("SetColorPrice")
                .WithOpenApi()
                .RequiredAuthenticatedUser();

            group.MapGet("/{carModelId}/{colorName}/price", GetColorPrice)
                .WithName("GetColorPrice")
                .WithOpenApi();

            group.MapGet("/{carModelId}/prices", GetColorPricesForModel)
                .WithName("GetColorPricesForModel")
                .WithOpenApi();
        }

        private static async Task<IResult> GetColorsByCarModelId(
            string carModelId,
            ICarModelColorService carModelColorService)
        {
            Console.WriteLine($"GetColorsByCarModelId endpoint called with carModelId: {carModelId}");
            
            var result = await carModelColorService.GetColorsByCarModelIdAsync(carModelId);

            if (!result.IsSuccess)
            {
                Console.WriteLine($"GetColorsByCarModelId failed: {result.Error.Message}");
                return Results.BadRequest(new { error = result.Error.Message });
            }

            Console.WriteLine($"GetColorsByCarModelId successful, returning {result.Value.Count} colors");
            return Results.Ok(result.Value);
        }

        private static async Task<IResult> SetColorPrice(
            string carModelId,
            string colorName,
            SetColorPriceRequest request,
            ICarModelColorService carModelColorService)
        {
            Console.WriteLine($"SetColorPrice endpoint called. CarModelId: {carModelId}, ColorName: {colorName}, Price: {request.Price}");
            
            // Additional server-side validation
            if (request.Price < 0 || request.Price > 60000)
            {
                return Results.BadRequest(new { error = "Cena musi być liczbą całkowitą od 0 do 60 000." });
            }
            
            var result = await carModelColorService.SetColorPriceAsync(carModelId, colorName, request.Price);

            if (!result.IsSuccess)
            {
                Console.WriteLine($"SetColorPrice failed: {result.Error.Message}");
                return Results.BadRequest(new { error = result.Error.Message });
            }

            Console.WriteLine($"SetColorPrice successful. Color ID: {result.Value.Id}");
            return Results.Ok(result.Value);
        }

        private static async Task<IResult> GetColorPrice(
            string carModelId,
            string colorName,
            ICarModelColorService carModelColorService)
        {
            var result = await carModelColorService.GetColorPriceAsync(carModelId, colorName);

            if (!result.IsSuccess)
            {
                if (result.Error.Code == "NOT_FOUND")
                {
                    return Results.Ok(new { price = 0 }); // Default price if not set
                }
                return Results.BadRequest(new { error = result.Error.Message });
            }

            return Results.Ok(new { price = result.Value.Price });
        }

        private static async Task<IResult> GetColorPricesForModel(
            string carModelId,
            ICarModelColorService carModelColorService)
        {
            var result = await carModelColorService.GetColorPricesForModelAsync(carModelId);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(new { error = result.Error.Message });
            }

            return Results.Ok(result.Value);
        }
    }

    public record SetColorPriceRequest(int Price);
}