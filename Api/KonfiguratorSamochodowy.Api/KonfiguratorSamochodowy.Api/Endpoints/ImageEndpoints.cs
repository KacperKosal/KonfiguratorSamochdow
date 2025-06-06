using KonfiguratorSamochodowy.Api.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace KonfiguratorSamochodowy.Api.Endpoints
{
    public static class ImageEndpoints
    {
        public static void MapImageEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/images")
                .WithTags("Zdjęcia");

            // Pobieranie zdjęcia po nazwie pliku
            group.MapGet("/{fileName}", async ([FromRoute] string fileName) =>
            {
                try
                {
                    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    var filePath = Path.Combine(uploadsPath, fileName);

                    if (!File.Exists(filePath))
                    {
                        return Results.NotFound("Zdjęcie nie zostało znalezione");
                    }

                    var fileBytes = await File.ReadAllBytesAsync(filePath);
                    var contentType = GetContentType(fileName);

                    return Results.File(fileBytes, contentType);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Błąd podczas pobierania zdjęcia: {ex.Message}", statusCode: 500);
                }
            })
            .WithName("GetImage")
            .WithDisplayName("Pobierz zdjęcie")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            // Upload zdjęcia
            group.MapPost("/upload", async (HttpRequest request) =>
            {
                try
                {
                    if (!request.HasFormContentType)
                    {
                        return Results.BadRequest("Nieprawidłowy typ zawartości");
                    }

                    var form = await request.ReadFormAsync();
                    var file = form.Files.FirstOrDefault();

                    if (file == null || file.Length == 0)
                    {
                        return Results.BadRequest("Nie wybrano pliku");
                    }

                    // Sprawdź typ pliku
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                    var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return Results.BadRequest("Nieprawidłowy typ pliku. Dozwolone: jpg, jpeg, png, gif, webp");
                    }

                    // Sprawdź rozmiar pliku (max 5MB)
                    if (file.Length > 5 * 1024 * 1024)
                    {
                        return Results.BadRequest("Plik jest za duży. Maksymalny rozmiar: 5MB");
                    }

                    // Stwórz folder uploads jeśli nie istnieje
                    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    Directory.CreateDirectory(uploadsPath);

                    // Generuj unikalną nazwę pliku
                    var fileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(uploadsPath, fileName);

                    // Zapisz plik
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Results.Ok(new { fileName = fileName, originalName = file.FileName });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Błąd podczas przesyłania zdjęcia: {ex.Message}", statusCode: 500);
                }
            })
            .RequiredAuthenticatedUser()
            .WithName("UploadImage")
            .WithDisplayName("Prześlij zdjęcie")
            .Produces<object>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();

            // Usuwanie zdjęcia
            group.MapDelete("/{fileName}", async ([FromRoute] string fileName) =>
            {
                try
                {
                    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    var filePath = Path.Combine(uploadsPath, fileName);

                    if (!File.Exists(filePath))
                    {
                        return Results.NotFound("Zdjęcie nie zostało znalezione");
                    }

                    File.Delete(filePath);
                    return Results.Ok(new { message = "Zdjęcie zostało usunięte" });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Błąd podczas usuwania zdjęcia: {ex.Message}", statusCode: 500);
                }
            })
            .RequiredAuthenticatedUser()
            .WithName("DeleteImage")
            .WithDisplayName("Usuń zdjęcie")
            .Produces<object>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError);
        }

        private static string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }
    }
}