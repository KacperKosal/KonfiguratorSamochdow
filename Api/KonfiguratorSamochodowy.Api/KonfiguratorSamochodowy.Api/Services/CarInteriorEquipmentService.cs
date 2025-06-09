using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using KonfiguratorSamochodowy.Api.Requests;
using KonfiguratorSamochodowy.Api.Validators;

namespace KonfiguratorSamochodowy.Api.Services;

    public class CarInteriorEquipmentService : ICarInteriorEquipmentService
    {
        private readonly ICarInteriorEquipmentRepository _repository;
        private readonly CreateCarInteriorEquipmentValidator _createValidator;
        private readonly UpdateCarInteriorEquipmentValidator _updateValidator;
        
        public CarInteriorEquipmentService(
            ICarInteriorEquipmentRepository repository, 
            CreateCarInteriorEquipmentValidator createValidator,
            UpdateCarInteriorEquipmentValidator updateValidator)
        {
            _repository = repository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        
        public async Task<Result<IEnumerable<CarInteriorEquipmentDto>>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();
            if (!result.IsSuccess)
                return Result<IEnumerable<CarInteriorEquipmentDto>>.Failure(result.Error);
                
            return Result<IEnumerable<CarInteriorEquipmentDto>>.Success(
                result.Value.Select(MapToDto));
        }

        public async Task<Result<CarInteriorEquipmentDto>> GetByIdAsync(string id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (!result.IsSuccess)
                return Result<CarInteriorEquipmentDto>.Failure(result.Error);
                
            return Result<CarInteriorEquipmentDto>.Success(MapToDto(result.Value));
        }

        public async Task<Result<IEnumerable<CarInteriorEquipmentDto>>> GetByCarIdAsync(string carId)
        {
            var result = await _repository.GetByCarIdAsync(carId);
            if (!result.IsSuccess)
                return Result<IEnumerable<CarInteriorEquipmentDto>>.Failure(result.Error);
                
            return Result<IEnumerable<CarInteriorEquipmentDto>>.Success(
                result.Value.Select(MapToDto));
        }
        
        public async Task<Result<IEnumerable<CarInteriorEquipmentDto>>> GetByCarModelAsync(string carModel)
        {
            var result = await _repository.GetByCarModelAsync(carModel);
            if (!result.IsSuccess)
                return Result<IEnumerable<CarInteriorEquipmentDto>>.Failure(result.Error);
                
            return Result<IEnumerable<CarInteriorEquipmentDto>>.Success(
                result.Value.Select(MapToDto));
        }

        public async Task<Result<IEnumerable<CarInteriorEquipmentDto>>> GetByTypeAsync(string type)
        {
            var result = await _repository.GetByTypeAsync(type);
            if (!result.IsSuccess)
                return Result<IEnumerable<CarInteriorEquipmentDto>>.Failure(result.Error);
                
            return Result<IEnumerable<CarInteriorEquipmentDto>>.Success(
                result.Value.Select(MapToDto));
        }

        public async Task<Result<IEnumerable<CarInteriorEquipmentDto>>> GetFilteredAsync(FilterCarInteriorEquipmentRequest request)
        {
            var result = await _repository.GetFilteredAsync(
                request.CarId, 
                request.CarModel, 
                request.Type, 
                request.IsDefault, 
                request.MaxPrice);
                
            if (!result.IsSuccess)
                return Result<IEnumerable<CarInteriorEquipmentDto>>.Failure(result.Error);
                
            return Result<IEnumerable<CarInteriorEquipmentDto>>.Success(
                result.Value.Select(MapToDto));
        }
        
        public async Task<Result<CarInteriorConfigurationDto>> GetFullCarConfigurationAsync(string carId)
        {
            var result = await _repository.GetByCarIdAsync(carId);
            if (!result.IsSuccess)
                return Result<CarInteriorConfigurationDto>.Failure(result.Error);
                
            var equipment = result.Value;
            
            // Sprawdź czy mamy wszystkie typy wyposażenia
            var carModel = equipment.FirstOrDefault()?.CarModel ?? "Unknown";
            
            var configuration = new CarInteriorConfigurationDto
            {
                CarId = carId,
                CarModel = carModel,
                Equipment = equipment.Select(MapToDto)
            };
            
            return Result<CarInteriorConfigurationDto>.Success(configuration);
        }

        public async Task<Result<CarInteriorEquipmentDto>> CreateAsync(CreateCarInteriorEquipmentRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<CarInteriorEquipmentDto>.Failure(
                    new Error("ValidationError", errorMessage));
            }
            
            var equipment = new CarInteriorEquipment
            {
                CarId = request.CarId,
                Type = request.Type,
                Value = request.Value,
                AdditionalPrice = request.AdditionalPrice,
                Description = request.Description,
                IsDefault = request.IsDefault,
                ColorCode = request.ColorCode,
                Intensity = string.IsNullOrEmpty(request.Intensity) ? null : int.TryParse(request.Intensity, out var intensity) ? intensity : null,
                HasNavigation = request.HasNavigation,
                HasPremiumSound = request.HasPremiumSound,
                ControlType = request.ControlType
            };
            
            // Pobierz model samochodu na podstawie CarId
            var carResult = await _repository.GetByCarIdAsync(request.CarId);
            if (carResult.IsSuccess && carResult.Value.Any())
            {
                equipment.CarModel = carResult.Value.First().CarModel;
            }
            else
            {
                return Result<CarInteriorEquipmentDto>.Failure(
                    new Error("ValidationError", $"Nie znaleziono samochodu o ID: {request.CarId}"));
            }
            
            var result = await _repository.CreateAsync(equipment);
            if (!result.IsSuccess)
                return Result<CarInteriorEquipmentDto>.Failure(result.Error);
                
            return Result<CarInteriorEquipmentDto>.Success(MapToDto(result.Value));
        }

        public async Task<Result<CarInteriorEquipmentDto>> UpdateAsync(string id, UpdateCarInteriorEquipmentRequest request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<CarInteriorEquipmentDto>.Failure(
                    new Error("ValidationError", errorMessage));
            }
            
            var getResult = await _repository.GetByIdAsync(id);
            if (!getResult.IsSuccess)
                return Result<CarInteriorEquipmentDto>.Failure(getResult.Error);
                
            var existingEquipment = getResult.Value;
            
            // Aktualizacja tylko tych pól, które są podane w zapytaniu
            if (!string.IsNullOrEmpty(request.Value))
                existingEquipment.Value = request.Value;
                
            if (request.AdditionalPrice.HasValue)
                existingEquipment.AdditionalPrice = request.AdditionalPrice.Value;
                
            if (!string.IsNullOrEmpty(request.Description))
                existingEquipment.Description = request.Description;
                
            if (request.IsDefault.HasValue)
                existingEquipment.IsDefault = request.IsDefault.Value;
                
            if (!string.IsNullOrEmpty(request.ColorCode))
                existingEquipment.ColorCode = request.ColorCode;
                
            if (!string.IsNullOrEmpty(request.Intensity))
                existingEquipment.Intensity = int.TryParse(request.Intensity, out var intensity) ? intensity : null;
                
            if (request.HasNavigation.HasValue)
                existingEquipment.HasNavigation = request.HasNavigation;
                
            if (request.HasPremiumSound.HasValue)
                existingEquipment.HasPremiumSound = request.HasPremiumSound;
                
            if (!string.IsNullOrEmpty(request.ControlType))
                existingEquipment.ControlType = request.ControlType;
            
            var result = await _repository.UpdateAsync(id, existingEquipment);
            if (!result.IsSuccess)
                return Result<CarInteriorEquipmentDto>.Failure(result.Error);
                
            return Result<CarInteriorEquipmentDto>.Success(MapToDto(result.Value));
        }

        public async Task<Result<bool>> DeleteAsync(string id)
        {
            return await _repository.DeleteAsync(id);
        }
        
        // Pomocnicza metoda do mapowania modelu na DTO
        private CarInteriorEquipmentDto MapToDto(CarInteriorEquipment equipment)
        {
            return new CarInteriorEquipmentDto
            {
                Id = equipment.Id,
                CarId = equipment.CarId,
                CarModel = equipment.CarModel,
                Type = equipment.Type,
                Value = equipment.Value,
                AdditionalPrice = equipment.AdditionalPrice,
                Description = equipment.Description,
                IsDefault = equipment.IsDefault,
                ColorCode = equipment.ColorCode,
                Intensity = equipment.Intensity,
                HasNavigation = equipment.HasNavigation,
                HasPremiumSound = equipment.HasPremiumSound,
                ControlType = equipment.ControlType
            };
        }
    }
