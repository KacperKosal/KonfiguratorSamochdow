using KonfiguratorSamochodowy.Api.Services;
using KonfiguratorSamochodowy.Api.Extensions;

namespace KonfiguratorSamochodowy.Api.Endpoints
{
    public static class CarModelImageEndpoints
    {
        public static void MapCarModelImageEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/api/car-model-images")
                .WithTags("Car Model Images");

            group.MapGet("/{carModelId}", GetCarModelImages)
                .WithName("GetCarModelImages")
                .WithOpenApi();

            group.MapPost("/{carModelId}", UploadCarModelImage)
                .WithName("UploadCarModelImage")
                .WithOpenApi()
                .RequiredAuthenticatedUser()
                .DisableAntiforgery();

            group.MapDelete("/{imageId}", DeleteCarModelImage)
                .WithName("DeleteCarModelImage")
                .WithOpenApi()
                .RequiredAuthenticatedUser();

            group.MapPut("/{imageId}/order", UpdateImageOrder)
                .WithName("UpdateImageOrder")
                .WithOpenApi()
                .RequiredAuthenticatedUser();

            group.MapPut("/{carModelId}/main-image/{imageId}", SetMainImage)
                .WithName("SetMainImage")
                .WithOpenApi()
                .RequiredAuthenticatedUser();

            group.MapGet("/{carModelId}/available-colors", GetAvailableColors)
                .WithName("GetAvailableColors")
                .WithOpenApi();
        }

        private static async Task<IResult> GetCarModelImages(
            string carModelId,
            ICarModelImageService carModelImageService)
        {
            Console.WriteLine($"GetCarModelImages endpoint called with carModelId: {carModelId}");
            
            var result = await carModelImageService.GetImagesByCarModelIdAsync(carModelId);

            if (!result.IsSuccess)
            {
                Console.WriteLine($"GetCarModelImages failed: {result.Error.Message}");
                return Results.BadRequest(new { error = result.Error.Message });
            }

            Console.WriteLine($"GetCarModelImages successful, returning {result.Value.Count} images");
            return Results.Ok(result.Value);
        }

        private static async Task<IResult> UploadCarModelImage(
            string carModelId,
            HttpRequest request,
            ICarModelImageService carModelImageService)
        {
            var form = await request.ReadFormAsync();
            var file = form.Files.GetFile("file");
            var color = form["color"].ToString();
            
            Console.WriteLine($"UploadCarModelImage endpoint called. CarModelId: {carModelId}, File: {file?.FileName}, Color: {color}");
            
            if (file == null)
            {
                Console.WriteLine("No file provided in upload request");
                return Results.BadRequest(new { error = "No file provided" });
            }

            var result = await carModelImageService.AddImageAsync(carModelId, file, color);

            if (!result.IsSuccess)
            {
                Console.WriteLine($"Upload failed: {result.Error.Message}");
                return Results.BadRequest(new { error = result.Error.Message });
            }

            Console.WriteLine($"Upload successful. Image ID: {result.Value.Id}");
            return Results.Created($"/api/car-model-images/{result.Value.Id}", result.Value);
        }

        private static async Task<IResult> DeleteCarModelImage(
            string imageId,
            ICarModelImageService carModelImageService)
        {
            var result = await carModelImageService.DeleteImageAsync(imageId);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(new { error = result.Error.Message });
            }

            if (!result.Value)
            {
                return Results.NotFound(new { error = "Image not found" });
            }

            return Results.NoContent();
        }

        private static async Task<IResult> UpdateImageOrder(
            string imageId,
            UpdateImageOrderRequest request,
            ICarModelImageService carModelImageService)
        {
            var result = await carModelImageService.UpdateImageOrderAsync(imageId, request.DisplayOrder);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(new { error = result.Error.Message });
            }

            if (!result.Value)
            {
                return Results.NotFound(new { error = "Image not found" });
            }

            return Results.Ok();
        }

        private static async Task<IResult> SetMainImage(
            string carModelId,
            string imageId,
            ICarModelImageService carModelImageService)
        {
            var result = await carModelImageService.SetMainImageAsync(carModelId, imageId);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(new { error = result.Error.Message });
            }

            if (!result.Value)
            {
                return Results.NotFound(new { error = "Image not found" });
            }

            return Results.Ok();
        }

        private static async Task<IResult> GetAvailableColors(
            string carModelId,
            ICarModelImageService carModelImageService)
        {
            var result = await carModelImageService.GetAvailableColorsForModelAsync(carModelId);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(new { error = result.Error.Message });
            }

            return Results.Ok(result.Value);
        }
    }

    public record UpdateImageOrderRequest(int DisplayOrder);
}