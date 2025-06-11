using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;

namespace KonfiguratorSamochodowy.Api.Services
{
    public class CarModelImageService : ICarModelImageService
    {
        private readonly ICarModelImageRepository _carModelImageRepository;
        private readonly IWebHostEnvironment _environment;
        private const int MaxImagesPerModel = 8;
        private readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        public CarModelImageService(
            ICarModelImageRepository carModelImageRepository,
            IWebHostEnvironment environment)
        {
            _carModelImageRepository = carModelImageRepository;
            _environment = environment;
            Console.WriteLine($"CarModelImageService initialized. WebRootPath: {environment.WebRootPath}");
        }

        public async Task<Result<List<CarModelImage>>> GetImagesByCarModelIdAsync(string carModelId)
        {
            Console.WriteLine($"GetImagesByCarModelIdAsync called with carModelId: '{carModelId}'");
            
            if (string.IsNullOrEmpty(carModelId))
            {
                Console.WriteLine("Car model ID is null or empty");
                return Result<List<CarModelImage>>.Failure(
                    new Error("VALIDATION_ERROR", "Car model ID cannot be empty")
                );
            }

            var result = await _carModelImageRepository.GetImagesByCarModelIdAsync(carModelId);
            Console.WriteLine($"Repository returned success: {result.IsSuccess}");
            if (result.IsSuccess)
            {
                Console.WriteLine($"Found {result.Value.Count} images for car model {carModelId}");
            }
            else
            {
                Console.WriteLine($"Repository error: {result.Error.Message}");
            }
            return result;
        }

        public async Task<Result<CarModelImage>> AddImageAsync(string carModelId, IFormFile imageFile, string color = "")
        {
            // Validate car model ID
            if (string.IsNullOrEmpty(carModelId))
            {
                return Result<CarModelImage>.Failure(
                    new Error("VALIDATION_ERROR", "Car model ID cannot be empty")
                );
            }

            // Validate color
            if (string.IsNullOrEmpty(color))
            {
                return Result<CarModelImage>.Failure(
                    new Error("VALIDATION_ERROR", "Wybierz kolor, do którego chcesz dodać zdjęcia.")
                );
            }

            // Validate file
            var fileValidation = ValidateImageFile(imageFile);
            if (!fileValidation.IsSuccess)
            {
                return Result<CarModelImage>.Failure(fileValidation.Error);
            }

            // Check if we've reached the maximum number of images for this color
            if (!string.IsNullOrEmpty(color))
            {
                var colorCountResult = await _carModelImageRepository.GetImageCountByCarModelIdAndColorAsync(carModelId, color);
                if (!colorCountResult.IsSuccess)
                {
                    Console.WriteLine($"Error getting image count for car model {carModelId} and color {color}: {colorCountResult.Error.Message}");
                    return Result<CarModelImage>.Failure(colorCountResult.Error);
                }

                if (colorCountResult.Value >= 8) // Max 8 images per color
                {
                    return Result<CarModelImage>.Failure(
                        new Error("VALIDATION_ERROR", "Można dodać maksymalnie 8 zdjęć dla jednego koloru.")
                    );
                }
            }

            // Check total images for model (fallback)
            var countResult = await _carModelImageRepository.GetImageCountByCarModelIdAsync(carModelId);
            if (!countResult.IsSuccess)
            {
                Console.WriteLine($"Error getting total image count for car model {carModelId}: {countResult.Error.Message}");
                return Result<CarModelImage>.Failure(countResult.Error);
            }

            // Save the image file
            var saveResult = await SaveImageFileAsync(imageFile);
            if (!saveResult.IsSuccess)
            {
                return Result<CarModelImage>.Failure(saveResult.Error);
            }

            // Get the next display order
            var existingImagesResult = await _carModelImageRepository.GetImagesByCarModelIdAsync(carModelId);
            var nextOrder = existingImagesResult.IsSuccess ? existingImagesResult.Value.Count + 1 : 1;

            // Create the image record
            var carModelImage = new CarModelImage
            {
                Id = Guid.NewGuid().ToString(),
                CarModelId = carModelId,
                ImageUrl = saveResult.Value,
                Color = color ?? "",
                DisplayOrder = nextOrder,
                IsMainImage = countResult.Value == 0, // First image is main by default
                CreatedAt = DateTime.UtcNow
            };

            Console.WriteLine($"Adding image for car model {carModelId}, image ID: {carModelImage.Id}");
            var result = await _carModelImageRepository.AddImageAsync(carModelImage);
            if (!result.IsSuccess)
            {
                Console.WriteLine($"Error adding image to repository: {result.Error.Message}");
            }
            return result;
        }

        public async Task<Result<bool>> DeleteImageAsync(string imageId)
        {
            if (string.IsNullOrEmpty(imageId))
            {
                return Result<bool>.Failure(
                    new Error("VALIDATION_ERROR", "Image ID cannot be empty")
                );
            }

            try
            {
                // First get the image to retrieve the file path for deletion
                var imageResult = await _carModelImageRepository.GetImageByIdAsync(imageId);
                if (imageResult.IsSuccess)
                {
                    var imageToDelete = imageResult.Value;
                    
                    // Delete from database first
                    var deleteResult = await _carModelImageRepository.DeleteImageAsync(imageId);
                    if (deleteResult.IsSuccess && deleteResult.Value)
                    {
                        // Delete the physical file
                        DeleteImageFile(imageToDelete.ImageUrl);
                    }
                    return deleteResult;
                }
                else
                {
                    // Image not found, try to delete anyway
                    return await _carModelImageRepository.DeleteImageAsync(imageId);
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(
                    new Error("UNEXPECTED_ERROR", $"Error deleting image: {ex.Message}")
                );
            }
        }

        public async Task<Result<bool>> UpdateImageOrderAsync(string imageId, int newOrder)
        {
            if (string.IsNullOrEmpty(imageId))
            {
                return Result<bool>.Failure(
                    new Error("VALIDATION_ERROR", "Image ID cannot be empty")
                );
            }

            if (newOrder < 1)
            {
                return Result<bool>.Failure(
                    new Error("VALIDATION_ERROR", "Display order must be greater than 0")
                );
            }

            return await _carModelImageRepository.UpdateImageOrderAsync(imageId, newOrder);
        }

        public async Task<Result<bool>> SetMainImageAsync(string carModelId, string imageId)
        {
            if (string.IsNullOrEmpty(carModelId))
            {
                return Result<bool>.Failure(
                    new Error("VALIDATION_ERROR", "Car model ID cannot be empty")
                );
            }

            if (string.IsNullOrEmpty(imageId))
            {
                return Result<bool>.Failure(
                    new Error("VALIDATION_ERROR", "Image ID cannot be empty")
                );
            }

            return await _carModelImageRepository.SetMainImageAsync(carModelId, imageId);
        }

        private Result<bool> ValidateImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Result<bool>.Failure(
                    new Error("VALIDATION_ERROR", "Wybierz plik zdjęcia.")
                );
            }

            if (file.Length > MaxFileSize)
            {
                return Result<bool>.Failure(
                    new Error("VALIDATION_ERROR", "Maksymalny rozmiar zdjęcia to 5 MB.")
                );
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                return Result<bool>.Failure(
                    new Error("VALIDATION_ERROR", "Nieprawidłowy format pliku. Dozwolone: JPG, PNG.")
                );
            }

            return Result<bool>.Success(true);
        }

        private async Task<Result<string>> SaveImageFileAsync(IFormFile file)
        {
            try
            {
                var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
                
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Result<string>.Success($"/uploads/{fileName}");
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(
                    new Error("UNEXPECTED_ERROR", $"Error saving file: {ex.Message}")
                );
            }
        }

        private void DeleteImageFile(string imageUrl)
        {
            try
            {
                if (!string.IsNullOrEmpty(imageUrl) && imageUrl.StartsWith("/uploads/"))
                {
                    var fileName = imageUrl.Replace("/uploads/", "");
                    var filePath = Path.Combine(_environment.WebRootPath, "uploads", fileName);
                    
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }
            catch
            {
                // Log error but don't fail the operation
            }
        }
    }
}