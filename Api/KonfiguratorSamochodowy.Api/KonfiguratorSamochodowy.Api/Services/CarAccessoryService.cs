using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using KonfiguratorSamochodowy.Api.Requests;
using KonfiguratorSamochodowy.Api.Validators;

namespace KonfiguratorSamochodowy.Api.Services
{
    public class CarAccessoryService : ICarAccessoryService
    {
        private readonly ICarAccessoryRepository _repository;
        private readonly CreateCarAccessoryValidator _createValidator;
        private readonly UpdateCarAccessoryValidator _updateValidator;

        public CarAccessoryService(
            ICarAccessoryRepository repository,
            CreateCarAccessoryValidator createValidator,
            UpdateCarAccessoryValidator updateValidator)
        {
            _repository = repository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<Result<IEnumerable<CarAccessoryDto>>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();
            if (!result.IsSuccess)
                return Result<IEnumerable<CarAccessoryDto>>.Failure(result.Error);

            return Result<IEnumerable<CarAccessoryDto>>.Success(
                result.Value.Select(MapToDto));
        }

        public async Task<Result<CarAccessoryDto>> GetByIdAsync(string id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (!result.IsSuccess)
                return Result<CarAccessoryDto>.Failure(result.Error);

            return Result<CarAccessoryDto>.Success(MapToDto(result.Value));
        }

        public async Task<Result<IEnumerable<CarAccessoryDto>>> GetByCarIdAsync(string carId)
        {
            var result = await _repository.GetByCarIdAsync(carId);
            if (!result.IsSuccess)
                return Result<IEnumerable<CarAccessoryDto>>.Failure(result.Error);

            return Result<IEnumerable<CarAccessoryDto>>.Success(
                result.Value.Select(MapToDto));
        }

        public async Task<Result<IEnumerable<CarAccessoryDto>>> GetByCarModelAsync(string carModel)
        {
            var result = await _repository.GetByCarModelAsync(carModel);
            if (!result.IsSuccess)
                return Result<IEnumerable<CarAccessoryDto>>.Failure(result.Error);

            return Result<IEnumerable<CarAccessoryDto>>.Success(
                result.Value.Select(MapToDto));
        }

        public async Task<Result<IEnumerable<CarAccessoryDto>>> GetByCategoryAsync(string category)
        {
            var result = await _repository.GetByCategoryAsync(category);
            if (!result.IsSuccess)
                return Result<IEnumerable<CarAccessoryDto>>.Failure(result.Error);

            return Result<IEnumerable<CarAccessoryDto>>.Success(
                result.Value.Select(MapToDto));
        }

        public async Task<Result<IEnumerable<CarAccessoryDto>>> GetByTypeAsync(string type)
        {
            var result = await _repository.GetByTypeAsync(type);
            if (!result.IsSuccess)
                return Result<IEnumerable<CarAccessoryDto>>.Failure(result.Error);

            return Result<IEnumerable<CarAccessoryDto>>.Success(
                result.Value.Select(MapToDto));
        }

        public async Task<Result<IEnumerable<CarAccessoryDto>>> GetFilteredAsync(FilterCarAccessoriesRequest request)
        {
            var result = await _repository.GetFilteredAsync(
                request.CarId,
                request.CarModel,
                request.Category,
                request.Type,
                request.MinPrice,
                request.MaxPrice,
                request.IsOriginalBMWPart,
                request.IsInStock,
                request.InstallationDifficulty);

            if (!result.IsSuccess)
                return Result<IEnumerable<CarAccessoryDto>>.Failure(result.Error);

            return Result<IEnumerable<CarAccessoryDto>>.Success(
                result.Value.Select(MapToDto));
        }

        public async Task<Result<CarAccessoryConfigurationDto>> GetFullCarConfigurationAsync(string carId)
        {
            var result = await _repository.GetByCarIdAsync(carId);
            if (!result.IsSuccess)
                return Result<CarAccessoryConfigurationDto>.Failure(result.Error);

            var accessories = result.Value;
            var carModel = accessories.FirstOrDefault()?.CarModel ?? "Unknown";

            var configuration = new CarAccessoryConfigurationDto
            {
                CarId = carId,
                CarModel = carModel,
                AccessoriesByCategory = accessories
                    .GroupBy(a => a.Category)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(MapToDto).ToList())
            };

            return Result<CarAccessoryConfigurationDto>.Success(configuration);
        }

        public async Task<Result<CarAccessoryDto>> CreateAsync(CreateCarAccessoryRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<CarAccessoryDto>.Failure(
                    new Error("ValidationError", errorMessage));
            }

            // Pobierz model samochodu na podstawie CarId
            var carResult = await _repository.GetByCarIdAsync(request.CarId);
            var carModel = "Unknown";

            if (carResult.IsSuccess && carResult.Value.Any())
            {
                carModel = carResult.Value.First().CarModel;
            }

            var accessory = new CarAccessory
            {
                CarId = request.CarId,
                CarModel = carModel,
                Category = request.Category,
                Type = request.Type,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Manufacturer = request.Manufacturer,
                PartNumber = request.PartNumber,
                IsOriginalBMWPart = request.IsOriginalBMWPart,
                IsInStock = request.IsInStock,
                StockQuantity = request.StockQuantity,
                ImageUrl = request.ImageUrl,
                Size = request.Size,
                Pattern = request.Pattern,
                Color = request.Color,
                Material = request.Material,
                Capacity = request.Capacity ?? 0,
                Compatibility = request.Compatibility,
                AgeGroup = request.AgeGroup,
                MaxLoad = request.MaxLoad ?? 0,
                IsUniversal = request.IsUniversal,
                InstallationDifficulty = request.InstallationDifficulty,
                Warranty = request.Warranty
            };

            var result = await _repository.CreateAsync(accessory);
            if (!result.IsSuccess)
                return Result<CarAccessoryDto>.Failure(result.Error);

            return Result<CarAccessoryDto>.Success(MapToDto(result.Value));
        }

        public async Task<Result<CarAccessoryDto>> UpdateAsync(string id, UpdateCarAccessoryRequest request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<CarAccessoryDto>.Failure(
                    new Error("ValidationError", errorMessage));
            }

            var getResult = await _repository.GetByIdAsync(id);
            if (!getResult.IsSuccess)
                return Result<CarAccessoryDto>.Failure(getResult.Error);

            var existingAccessory = getResult.Value;

            // Sprawdź wymagane pola dla Update
            if (!string.IsNullOrEmpty(request.Manufacturer) && string.IsNullOrWhiteSpace(request.Manufacturer))
                return Result<CarAccessoryDto>.Failure(
                    new Error("ValidationError", "Pole producent jest wymagane."));
                    
            if (!string.IsNullOrEmpty(request.Compatibility) && string.IsNullOrWhiteSpace(request.Compatibility))
                return Result<CarAccessoryDto>.Failure(
                    new Error("ValidationError", "Pole kompatybilność jest wymagane."));
                    
            if (!string.IsNullOrEmpty(request.PartNumber) && string.IsNullOrWhiteSpace(request.PartNumber))
                return Result<CarAccessoryDto>.Failure(
                    new Error("ValidationError", "Numer części jest wymagany."));

            // Aktualizacja tylko tych pól, które są podane w zapytaniu
            if (!string.IsNullOrEmpty(request.Name))
                existingAccessory.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Description))
                existingAccessory.Description = request.Description;

            if (request.Price.HasValue)
                existingAccessory.Price = request.Price.Value;

            if (!string.IsNullOrEmpty(request.Manufacturer))
                existingAccessory.Manufacturer = request.Manufacturer;

            if (!string.IsNullOrEmpty(request.PartNumber))
            {
                // Sprawdź unikalność numeru części
                var uniqueResult = await _repository.IsPartNumberUniqueAsync(request.PartNumber, id);
                if (!uniqueResult.IsSuccess)
                    return Result<CarAccessoryDto>.Failure(uniqueResult.Error);
                
                if (!uniqueResult.Value)
                    return Result<CarAccessoryDto>.Failure(
                        new Error("ValidationError", "Numer części musi być unikalny."));
                
                existingAccessory.PartNumber = request.PartNumber;
            }

            if (request.IsOriginalBMWPart.HasValue)
                existingAccessory.IsOriginalBMWPart = request.IsOriginalBMWPart.Value;

            if (request.IsInStock.HasValue)
                existingAccessory.IsInStock = request.IsInStock.Value;

            if (request.StockQuantity.HasValue)
                existingAccessory.StockQuantity = request.StockQuantity.Value;

            if (!string.IsNullOrEmpty(request.ImageUrl))
                existingAccessory.ImageUrl = request.ImageUrl;

            if (!string.IsNullOrEmpty(request.Size))
                existingAccessory.Size = request.Size;

            if (!string.IsNullOrEmpty(request.Pattern))
                existingAccessory.Pattern = request.Pattern;

            if (!string.IsNullOrEmpty(request.Color))
                existingAccessory.Color = request.Color;

            if (!string.IsNullOrEmpty(request.Material))
                existingAccessory.Material = request.Material;

            if (request.Capacity.HasValue)
                existingAccessory.Capacity = request.Capacity.Value;

            if (!string.IsNullOrEmpty(request.Compatibility))
                existingAccessory.Compatibility = request.Compatibility;

            if (!string.IsNullOrEmpty(request.AgeGroup))
                existingAccessory.AgeGroup = request.AgeGroup;

            if (request.MaxLoad.HasValue)
                existingAccessory.MaxLoad = request.MaxLoad.Value;

            if (request.IsUniversal.HasValue)
                existingAccessory.IsUniversal = request.IsUniversal.Value;

            if (!string.IsNullOrEmpty(request.InstallationDifficulty))
                existingAccessory.InstallationDifficulty = request.InstallationDifficulty;

            if (!string.IsNullOrEmpty(request.Warranty))
                existingAccessory.Warranty = request.Warranty;

            var result = await _repository.UpdateAsync(id, existingAccessory);
            if (!result.IsSuccess)
                return Result<CarAccessoryDto>.Failure(result.Error);

            return Result<CarAccessoryDto>.Success(MapToDto(result.Value));
        }

        public async Task<Result<bool>> DeleteAsync(string id)
        {
            return await _repository.DeleteAsync(id);
        }

        private CarAccessoryDto MapToDto(CarAccessory accessory)
        {
            return new CarAccessoryDto
            {
                Id = accessory.Id,
                CarId = accessory.CarId,
                CarModel = accessory.CarModel,
                Category = accessory.Category,
                Type = accessory.Type,
                Name = accessory.Name,
                Description = accessory.Description,
                Price = accessory.Price,
                Manufacturer = accessory.Manufacturer,
                PartNumber = accessory.PartNumber,
                IsOriginalBMWPart = accessory.IsOriginalBMWPart,
                IsInStock = accessory.IsInStock,
                StockQuantity = accessory.StockQuantity,
                ImageUrl = accessory.ImageUrl,
                Size = accessory.Size,
                Pattern = accessory.Pattern,
                Color = accessory.Color,
                Material = accessory.Material,
                Capacity = accessory.Capacity > 0 ? accessory.Capacity : null,
                Compatibility = accessory.Compatibility,
                AgeGroup = accessory.AgeGroup,
                MaxLoad = accessory.MaxLoad > 0 ? accessory.MaxLoad : null,
                IsUniversal = accessory.IsUniversal,
                InstallationDifficulty = accessory.InstallationDifficulty,
                Warranty = accessory.Warranty
            };
        }
    }
}