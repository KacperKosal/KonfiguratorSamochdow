using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Models;
using KonfiguratorSamochodowy.Api.Repositories;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using KonfiguratorSamochodowy.Api.Repositories.Repositories;
using KonfiguratorSamochodowy.Api.Requests;
using KonfiguratorSamochodowy.Api.Validators;
using FluentValidation;

namespace KonfiguratorSamochodowy.Api.Services
{
    public class EngineService : IEngineService
    {
        private readonly KonfiguratorSamochodowy.Api.Repositories.Repositories.IEngineRepository _repository;
        private readonly CreateEngineValidator _createValidator;
        private readonly UpdateEngineValidator _updateValidator;
        
        public EngineService(
            KonfiguratorSamochodowy.Api.Repositories.Repositories.IEngineRepository repository,
            CreateEngineValidator createValidator,
            UpdateEngineValidator updateValidator)
        {
            _repository = repository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        
        public async Task<Result<IEnumerable<EngineDto>>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();
            if (!result.IsSuccess)
                return Result<IEnumerable<EngineDto>>.Failure(result.Error);
                
            return Result<IEnumerable<EngineDto>>.Success(
                result.Value.Select(MapToDto));
        }

        public async Task<Result<EngineDto>> GetByIdAsync(string id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (!result.IsSuccess)
                return Result<EngineDto>.Failure(result.Error);
                
            return Result<EngineDto>.Success(MapToDto(result.Value));
        }

        public async Task<Result<IEnumerable<EngineDto>>> GetFilteredAsync(FilterEnginesRequest filter)
        {
            // Convert FilterEnginesRequest to FilterEnginesRequestDto
            var filterDto = new KonfiguratorSamochodowy.Api.Repositories.Dto.FilterEnginesRequestDto
            {
                Type = filter.Type,
                FuelType = filter.FuelType,
                MinCapacity = filter.MinCapacity,
                MaxCapacity = filter.MaxCapacity,
                MinPower = filter.MinPower,
                MaxPower = filter.MaxPower,
                Transmission = filter.Transmission,
                DriveType = filter.DriveType,
                IsActive = filter.IsActive
            };
            
            var result = await _repository.GetFilteredAsync(filterDto);
            if (!result.IsSuccess)
                return Result<IEnumerable<EngineDto>>.Failure(result.Error);
                
            return Result<IEnumerable<EngineDto>>.Success(
                result.Value.Select(MapToDto));
        }

        public async Task<Result<EngineDto>> CreateAsync(CreateEngineRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<EngineDto>.Failure(
                    new Error("ValidationError", errorMessage));
            }
            
            var engine = new Engine
            {
                Name = request.Name,
                Type = request.Type,
                Capacity = request.Capacity,
                Power = request.Power,
                Torque = request.Torque,
                FuelType = request.FuelType,
                Cylinders = request.Cylinders,
                Transmission = request.Transmission,
                Gears = request.Gears,
                DriveType = request.DriveType,
                FuelConsumption = request.FuelConsumption,
                CO2Emission = request.CO2Emission,
                Description = request.Description,
                IsActive = request.IsActive
            };
            
            var result = await _repository.CreateAsync(engine);
            if (!result.IsSuccess)
                return Result<EngineDto>.Failure(result.Error);
                
            return Result<EngineDto>.Success(MapToDto(result.Value));
        }

        public async Task<Result<EngineDto>> UpdateAsync(string id, UpdateEngineRequest request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<EngineDto>.Failure(
                    new Error("ValidationError", errorMessage));
            }
            
            var getResult = await _repository.GetByIdAsync(id);
            if (!getResult.IsSuccess)
                return Result<EngineDto>.Failure(getResult.Error);
                
            var existingEngine = getResult.Value;
            
            // Aktualizacja tylko tych pól, które są podane w zapytaniu
            if (!string.IsNullOrEmpty(request.Name))
                existingEngine.Name = request.Name;
                
            if (!string.IsNullOrEmpty(request.Type))
                existingEngine.Type = request.Type;
                
            if (request.Capacity.HasValue)
                existingEngine.Capacity = request.Capacity;
                
            if (request.Power.HasValue)
                existingEngine.Power = request.Power.Value;
                
            if (request.Torque.HasValue)
                existingEngine.Torque = request.Torque.Value;
                
            if (!string.IsNullOrEmpty(request.FuelType))
                existingEngine.FuelType = request.FuelType;
                
            if (request.Cylinders.HasValue)
                existingEngine.Cylinders = request.Cylinders;
                
            if (!string.IsNullOrEmpty(request.Transmission))
                existingEngine.Transmission = request.Transmission;
                
            if (request.Gears.HasValue)
                existingEngine.Gears = request.Gears.Value;
                
            if (!string.IsNullOrEmpty(request.DriveType))
                existingEngine.DriveType = request.DriveType;
                
            if (request.FuelConsumption.HasValue)
                existingEngine.FuelConsumption = request.FuelConsumption.Value;
                
            if (request.CO2Emission.HasValue)
                existingEngine.CO2Emission = request.CO2Emission.Value;
                
            if (!string.IsNullOrEmpty(request.Description))
                existingEngine.Description = request.Description;
                
            if (request.IsActive.HasValue)
                existingEngine.IsActive = request.IsActive.Value;
            
            var result = await _repository.UpdateAsync(id, existingEngine);
            if (!result.IsSuccess)
                return Result<EngineDto>.Failure(result.Error);
                
            return Result<EngineDto>.Success(MapToDto(result.Value));
        }

        public async Task<Result<bool>> DeleteAsync(string id)
        {
            return await _repository.DeleteAsync(id);
        }
        
        private EngineDto MapToDto(Engine engine)
        {
            return new EngineDto
            {
                Id = engine.Id,
                Name = engine.Name,
                Type = engine.Type,
                Capacity = engine.Capacity,
                Power = engine.Power,
                Torque = engine.Torque,
                FuelType = engine.FuelType,
                Cylinders = engine.Cylinders,
                Transmission = engine.Transmission,
                Gears = engine.Gears,
                DriveType = engine.DriveType,
                FuelConsumption = engine.FuelConsumption,
                CO2Emission = engine.CO2Emission,
                Description = engine.Description,
                IsActive = engine.IsActive
            };
        }
    }
}