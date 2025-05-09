using KonfiguratorSamochodowy.Api.Common;
using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Models;
using KonfiguratorSamochodowy.Api.Repositories;
using KonfiguratorSamochodowy.Api.Requests;
using KonfiguratorSamochodowy.Api.Validators;
using FluentValidation;

namespace KonfiguratorSamochodowy.Api.Services
{
    public class CarModelService : ICarModelService
    {
        private readonly ICarModelRepository _repository;
        private readonly CreateCarModelValidator _createValidator;
        private readonly UpdateCarModelValidator _updateValidator;
        
        public CarModelService(
            ICarModelRepository repository,
            CreateCarModelValidator createValidator,
            UpdateCarModelValidator updateValidator)
        {
            _repository = repository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        
        public async Task<Result<IEnumerable<CarModelDto>>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();
            if (!result.IsSuccess)
                return Result<IEnumerable<CarModelDto>>.Failure(result.Error);
                
            return Result<IEnumerable<CarModelDto>>.Success(
                result.Value.Select(MapToDto));
        }

        public async Task<Result<CarModelDto>> GetByIdAsync(string id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (!result.IsSuccess)
                return Result<CarModelDto>.Failure(result.Error);
                
            return Result<CarModelDto>.Success(MapToDto(result.Value));
        }

        public async Task<Result<IEnumerable<CarModelDto>>> GetFilteredAsync(FilterCarModelsRequest filter)
        {
            var result = await _repository.GetFilteredAsync(filter);
            if (!result.IsSuccess)
                return Result<IEnumerable<CarModelDto>>.Failure(result.Error);
                
            return Result<IEnumerable<CarModelDto>>.Success(
                result.Value.Select(MapToDto));
        }

        public async Task<Result<CarModelDto>> CreateAsync(CreateCarModelRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<CarModelDto>.Failure(
                    new Error("ValidationError", errorMessage));
            }
            
            var carModel = new CarModel
            {
                Name = request.Name,
                ProductionYear = request.ProductionYear,
                BodyType = request.BodyType,
                Manufacturer = request.Manufacturer,
                Segment = request.Segment,
                BasePrice = request.BasePrice,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                IsActive = request.IsActive
            };
            
            var result = await _repository.CreateAsync(carModel);
            if (!result.IsSuccess)
                return Result<CarModelDto>.Failure(result.Error);
                
            return Result<CarModelDto>.Success(MapToDto(result.Value));
        }

        public async Task<Result<CarModelDto>> UpdateAsync(string id, UpdateCarModelRequest request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<CarModelDto>.Failure(
                    new Error("ValidationError", errorMessage));
            }
            
            var getResult = await _repository.GetByIdAsync(id);
            if (!getResult.IsSuccess)
                return Result<CarModelDto>.Failure(getResult.Error);
                
            var existingCarModel = getResult.Value;
            
            // Aktualizacja tylko tych pól, które są podane w zapytaniu
            if (!string.IsNullOrEmpty(request.Name))
                existingCarModel.Name = request.Name;
                
            if (request.ProductionYear.HasValue)
                existingCarModel.ProductionYear = request.ProductionYear.Value;
                
            if (!string.IsNullOrEmpty(request.BodyType))
                existingCarModel.BodyType = request.BodyType;
                
            if (!string.IsNullOrEmpty(request.Manufacturer))
                existingCarModel.Manufacturer = request.Manufacturer;
                
            if (!string.IsNullOrEmpty(request.Segment))
                existingCarModel.Segment = request.Segment;
                
            if (request.BasePrice.HasValue)
                existingCarModel.BasePrice = request.BasePrice.Value;
                
            if (!string.IsNullOrEmpty(request.Description))
                existingCarModel.Description = request.Description;
                
            if (!string.IsNullOrEmpty(request.ImageUrl))
                existingCarModel.ImageUrl = request.ImageUrl;
                
            if (request.IsActive.HasValue)
                existingCarModel.IsActive = request.IsActive.Value;
            
            var result = await _repository.UpdateAsync(id, existingCarModel);
            if (!result.IsSuccess)
                return Result<CarModelDto>.Failure(result.Error);
                
            return Result<CarModelDto>.Success(MapToDto(result.Value));
        }

        public async Task<Result<bool>> DeleteAsync(string id)
        {
            return await _repository.DeleteAsync(id);
        }
        
        private CarModelDto MapToDto(CarModel carModel)
        {
            return new CarModelDto
            {
                Id = carModel.Id,
                Name = carModel.Name,
                ProductionYear = carModel.ProductionYear,
                BodyType = carModel.BodyType,
                Manufacturer = carModel.Manufacturer,
                Segment = carModel.Segment,
                BasePrice = carModel.BasePrice,
                Description = carModel.Description,
                ImageUrl = carModel.ImageUrl,
                IsActive = carModel.IsActive
            };
        }
    }
}