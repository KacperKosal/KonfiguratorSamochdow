using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;

namespace KonfiguratorSamochodowy.Api.Services
{
    public class CarModelColorService : ICarModelColorService
    {
        private readonly ICarModelColorRepository _carModelColorRepository;

        public CarModelColorService(ICarModelColorRepository carModelColorRepository)
        {
            _carModelColorRepository = carModelColorRepository;
        }

        public async Task<Result<List<CarModelColor>>> GetColorsByCarModelIdAsync(string carModelId)
        {
            if (string.IsNullOrEmpty(carModelId))
            {
                return Result<List<CarModelColor>>.Failure(
                    new Error("VALIDATION_ERROR", "Car model ID cannot be empty")
                );
            }

            return await _carModelColorRepository.GetColorsByCarModelIdAsync(carModelId);
        }

        public async Task<Result<CarModelColor>> SetColorPriceAsync(string carModelId, string colorName, int price)
        {
            // Validate input
            if (string.IsNullOrEmpty(carModelId))
            {
                return Result<CarModelColor>.Failure(
                    new Error("VALIDATION_ERROR", "Car model ID cannot be empty")
                );
            }

            if (string.IsNullOrEmpty(colorName))
            {
                return Result<CarModelColor>.Failure(
                    new Error("VALIDATION_ERROR", "Nazwa koloru jest wymagana.")
                );
            }

            if (price < 0 || price > 60000)
            {
                return Result<CarModelColor>.Failure(
                    new Error("VALIDATION_ERROR", "Cena musi być liczbą całkowitą od 0 do 60 000.")
                );
            }

            var color = new CarModelColor
            {
                CarModelId = carModelId,
                ColorName = colorName,
                Price = price
            };

            return await _carModelColorRepository.CreateOrUpdateColorAsync(color);
        }

        public async Task<Result<CarModelColor>> GetColorPriceAsync(string carModelId, string colorName)
        {
            if (string.IsNullOrEmpty(carModelId))
            {
                return Result<CarModelColor>.Failure(
                    new Error("VALIDATION_ERROR", "Car model ID cannot be empty")
                );
            }

            if (string.IsNullOrEmpty(colorName))
            {
                return Result<CarModelColor>.Failure(
                    new Error("VALIDATION_ERROR", "Color name cannot be empty")
                );
            }

            return await _carModelColorRepository.GetColorByCarModelIdAndNameAsync(carModelId, colorName);
        }

        public async Task<Result<Dictionary<string, int>>> GetColorPricesForModelAsync(string carModelId)
        {
            if (string.IsNullOrEmpty(carModelId))
            {
                return Result<Dictionary<string, int>>.Failure(
                    new Error("VALIDATION_ERROR", "Car model ID cannot be empty")
                );
            }

            var colorsResult = await _carModelColorRepository.GetColorsByCarModelIdAsync(carModelId);
            
            if (!colorsResult.IsSuccess)
            {
                return Result<Dictionary<string, int>>.Failure(colorsResult.Error);
            }

            var colorPrices = colorsResult.Value.ToDictionary(c => c.ColorName, c => c.Price);
            return Result<Dictionary<string, int>>.Success(colorPrices);
        }
    }
}