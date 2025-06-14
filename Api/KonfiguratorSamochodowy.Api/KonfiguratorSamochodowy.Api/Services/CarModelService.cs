using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Models;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Requests;
using KonfiguratorSamochodowy.Api.Validators;
using FluentValidation;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using KonfiguratorSamochodowy.Api.Repositories.Services;

namespace KonfiguratorSamochodowy.Api.Services
{
    public class CarModelService : ICarModelService
    {
        private readonly ICarModelRepository _repository;
        private readonly CreateCarModelValidator _createValidator;
        private readonly UpdateCarModelValidator _updateValidator;
        private readonly TransactionExamples _transactionExamples;
        private readonly ICarModelEngineRepository _carModelEngineRepository;
        private readonly IEngineRepository _engineRepository;
        private readonly ICarModelImageRepository _carModelImageRepository;
        
        public CarModelService(
            ICarModelRepository repository,
            CreateCarModelValidator createValidator,
            UpdateCarModelValidator updateValidator,
            TransactionExamples transactionExamples,
            ICarModelEngineRepository carModelEngineRepository,
            IEngineRepository engineRepository,
            ICarModelImageRepository carModelImageRepository)
        {
            _repository = repository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _transactionExamples = transactionExamples;
            _carModelEngineRepository = carModelEngineRepository;
            _engineRepository = engineRepository;
            _carModelImageRepository = carModelImageRepository;
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
            var filterDto = new KonfiguratorSamochodowy.Api.Repositories.Dto.FilterCarModelsRequestDto
            {
                Brand = filter.Manufacturer,           // ✅ Mapuj Manufacturer na Brand
                BodyType = filter.BodyType,
                Segment = filter.Segment,
                MinYear = filter.MinProductionYear,    // ✅ Mapuj na MinYear
                MaxYear = filter.MaxProductionYear,    // ✅ Mapuj na MaxYear
                MinPrice = filter.MinPrice,
                MaxPrice = filter.MaxPrice,
                IsActive = filter.IsActive,
                Has4x4 = filter.Has4x4,
                IsElectric = filter.IsElectric
            };
            
            var result = await _repository.GetFilteredAsync(filterDto);
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
                IsActive = request.IsActive,
                Has4x4 = request.Has4x4,
                IsElectric = request.IsElectric
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
            
            // If base price is being updated, use the transaction
            if (request.BasePrice.HasValue && request.BasePrice.Value != existingCarModel.BasePrice)
            {
                var transactionResult = await _transactionExamples.UpdateCarModelPriceWithConfigurationsAsync(
                    _repository,
                    id,
                    request.BasePrice.Value
                );

                if (!transactionResult.IsSuccess)
                    return Result<CarModelDto>.Failure(transactionResult.Error);

                // Get the updated model after transaction
                var updatedModelResult = await _repository.GetByIdAsync(id);
                if (!updatedModelResult.IsSuccess)
                    return Result<CarModelDto>.Failure(updatedModelResult.Error);

                return Result<CarModelDto>.Success(MapToDto(updatedModelResult.Value));
            }
            
            // For other updates, proceed with normal update
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
                
            if (!string.IsNullOrEmpty(request.Description))
                existingCarModel.Description = request.Description;
                
            if (!string.IsNullOrEmpty(request.ImageUrl))
                existingCarModel.ImageUrl = request.ImageUrl;
                
            if (request.IsActive.HasValue)
                existingCarModel.IsActive = request.IsActive.Value;
                
            if (request.Has4x4.HasValue)
                existingCarModel.Has4x4 = request.Has4x4.Value;
                
            if (request.IsElectric.HasValue)
                existingCarModel.IsElectric = request.IsElectric.Value;
            
            var result = await _repository.UpdateAsync(id, existingCarModel);
            if (!result.IsSuccess)
                return Result<CarModelDto>.Failure(result.Error);
                
            return Result<CarModelDto>.Success(MapToDto(result.Value));
        }

        public async Task<Result<bool>> DeleteAsync(string id)
        {
            var result = await _transactionExamples.DeleteCarModelWithRelatedDataAsync(
                _repository,
                _carModelEngineRepository,
                _engineRepository,
                _carModelImageRepository,
                id
            );
            return result;
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
                IsActive = carModel.IsActive,
                Has4x4 = carModel.Has4x4,
                IsElectric = carModel.IsElectric
            };
        }
    }
}