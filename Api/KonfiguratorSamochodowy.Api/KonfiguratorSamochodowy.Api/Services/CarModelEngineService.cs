using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Models;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using KonfiguratorSamochodowy.Api.Requests;
using KonfiguratorSamochodowy.Api.Validators;
using FluentValidation;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;

namespace KonfiguratorSamochodowy.Api.Services
{
    public class CarModelEngineService : ICarModelEngineService
    {
        private readonly ICarModelEngineRepository _repository;
        private readonly ICarModelRepository _carModelRepository;
        private readonly IEngineRepository _engineRepository;
        private readonly AddCarModelEngineValidator _addValidator;
        private readonly UpdateCarModelEngineValidator _updateValidator;
        
        public CarModelEngineService(
            ICarModelEngineRepository repository,
            ICarModelRepository carModelRepository,
            IEngineRepository engineRepository,
            AddCarModelEngineValidator addValidator,
            UpdateCarModelEngineValidator updateValidator)
        {
            _repository = repository;
            _carModelRepository = carModelRepository;
            _engineRepository = engineRepository;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }
        
        public async Task<Result<IEnumerable<CarModelEngineDto>>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();
            if (!result.IsSuccess)
                return Result<IEnumerable<CarModelEngineDto>>.Failure(result.Error);
            
            var carModelEngines = result.Value.ToList();
            var carModelEngineDtos = new List<CarModelEngineDto>();
            
            foreach (var cme in carModelEngines)
            {
                var carModelResult = await _carModelRepository.GetByIdAsync(cme.CarModelId);
                var engineResult = await _engineRepository.GetByIdAsync(cme.EngineId);
                
                if (carModelResult.IsSuccess && engineResult.IsSuccess)
                {
                    carModelEngineDtos.Add(MapToDto(cme, carModelResult.Value, engineResult.Value));
                }
            }
            
            return Result<IEnumerable<CarModelEngineDto>>.Success(carModelEngineDtos);
        }

        public async Task<Result<CarModelEngineDto>> GetByIdAsync(string id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (!result.IsSuccess)
                return Result<CarModelEngineDto>.Failure(result.Error);
            
            var carModelResult = await _carModelRepository.GetByIdAsync(result.Value.CarModelId);
            var engineResult = await _engineRepository.GetByIdAsync(result.Value.EngineId);
            
            if (!carModelResult.IsSuccess)
                return Result<CarModelEngineDto>.Failure(carModelResult.Error);
                
            if (!engineResult.IsSuccess)
                return Result<CarModelEngineDto>.Failure(engineResult.Error);
            
            return Result<CarModelEngineDto>.Success(
                MapToDto(result.Value, carModelResult.Value, engineResult.Value));
        }

        public async Task<Result<CarModelEngineDto>> GetByCarModelAndEngineIdAsync(string carModelId, string engineId)
        {
            var result = await _repository.GetByCarModelAndEngineIdAsync(carModelId, engineId);
            if (!result.IsSuccess)
                return Result<CarModelEngineDto>.Failure(result.Error);
            
            var carModelResult = await _carModelRepository.GetByIdAsync(result.Value.CarModelId);
            var engineResult = await _engineRepository.GetByIdAsync(result.Value.EngineId);
            
            if (!carModelResult.IsSuccess)
                return Result<CarModelEngineDto>.Failure(carModelResult.Error);
                
            if (!engineResult.IsSuccess)
                return Result<CarModelEngineDto>.Failure(engineResult.Error);
            
            return Result<CarModelEngineDto>.Success(
                MapToDto(result.Value, carModelResult.Value, engineResult.Value));
        }

        public async Task<Result<IEnumerable<CarModelEngineDto>>> GetByCarModelIdAsync(string carModelId)
        {
            var result = await _repository.GetByCarModelIdAsync(carModelId);
            if (!result.IsSuccess)
                return Result<IEnumerable<CarModelEngineDto>>.Failure(result.Error);
            
            var carModelResult = await _carModelRepository.GetByIdAsync(carModelId);
            if (!carModelResult.IsSuccess)
                return Result<IEnumerable<CarModelEngineDto>>.Failure(carModelResult.Error);
            
            var carModel = carModelResult.Value;
            var carModelEngines = result.Value.ToList();
            var carModelEngineDtos = new List<CarModelEngineDto>();
            
            foreach (var cme in carModelEngines)
            {
                var engineResult = await _engineRepository.GetByIdAsync(cme.EngineId);
                if (engineResult.IsSuccess)
                {
                    carModelEngineDtos.Add(MapToDto(cme, carModel, engineResult.Value));
                }
            }
            
            return Result<IEnumerable<CarModelEngineDto>>.Success(carModelEngineDtos);
        }

        public async Task<Result<IEnumerable<EngineForModelDto>>> GetEnginesForCarModelAsync(string carModelId)
        {
            var result = await _repository.GetByCarModelIdAsync(carModelId);
            if (!result.IsSuccess)
                return Result<IEnumerable<EngineForModelDto>>.Failure(result.Error);
            
            var carModelEngines = result.Value.ToList();
            var engineDtos = new List<EngineForModelDto>();
            
            foreach (var cme in carModelEngines)
            {
                var engineResult = await _engineRepository.GetByIdAsync(cme.EngineId);
                if (engineResult.IsSuccess)
                {
                    var engine = engineResult.Value;
                    engineDtos.Add(new EngineForModelDto
                    {
                        EngineId = engine.Id,
                        EngineName = engine.Name,
                        AdditionalPrice = cme.AdditionalPrice,
                        Type = engine.Type,
                        Capacity = engine.Capacity.HasValue ? (decimal)engine.Capacity.Value : 0m,
                        Power = engine.Power,
                        FuelType = engine.FuelType,
                        TopSpeed = cme.TopSpeed,
                        Acceleration0To100 = cme.Acceleration0To100,
                        FuelConsumption = engine.FuelConsumption,
                        CO2Emission = (decimal)engine.CO2Emission,
                        IsDefault = cme.IsDefault,
                        IsAvailable = cme.IsAvailable
                    });
                }
            }
            
            return Result<IEnumerable<EngineForModelDto>>.Success(engineDtos);
        }

        public async Task<Result<IEnumerable<CarModelEngineDto>>> GetByEngineIdAsync(string engineId)
        {
            var result = await _repository.GetByEngineIdAsync(engineId);
            if (!result.IsSuccess)
                return Result<IEnumerable<CarModelEngineDto>>.Failure(result.Error);
            
            var engineResult = await _engineRepository.GetByIdAsync(engineId);
            if (!engineResult.IsSuccess)
                return Result<IEnumerable<CarModelEngineDto>>.Failure(engineResult.Error);
            
            var engine = engineResult.Value;
            var carModelEngines = result.Value.ToList();
            var carModelEngineDtos = new List<CarModelEngineDto>();
            
            foreach (var cme in carModelEngines)
            {
                var carModelResult = await _carModelRepository.GetByIdAsync(cme.CarModelId);
                if (carModelResult.IsSuccess)
                {
                    carModelEngineDtos.Add(MapToDto(cme, carModelResult.Value, engine));
                }
            }
            
            return Result<IEnumerable<CarModelEngineDto>>.Success(carModelEngineDtos);
        }

        public async Task<Result<CarModelEngineDto>> AddEngineToCarModelAsync(string carModelId, AddCarModelEngineRequest request)
        {
            var validationResult = await _addValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<CarModelEngineDto>.Failure(
                    new Error("ValidationError", errorMessage));
            }
            
            // Sprawdź, czy model samochodu istnieje
            var carModelResult = await _carModelRepository.GetByIdAsync(carModelId);
            if (!carModelResult.IsSuccess)
                return Result<CarModelEngineDto>.Failure(carModelResult.Error);
            
            // Sprawdź, czy silnik istnieje
            var engineResult = await _engineRepository.GetByIdAsync(request.EngineId);
            if (!engineResult.IsSuccess)
                return Result<CarModelEngineDto>.Failure(engineResult.Error);
            
            // Sprawdź, czy powiązanie już istnieje
            var existingResult = await _repository.GetByCarModelAndEngineIdAsync(carModelId, request.EngineId);
            if (existingResult.IsSuccess)
                return Result<CarModelEngineDto>.Failure(
                    new Error("AlreadyExists", "To powiązanie modelu samochodu z silnikiem już istnieje"));
            
            var carModelEngine = new CarModelEngine
            {
                CarModelId = carModelId,
                EngineId = request.EngineId,
                AdditionalPrice = request.AdditionalPrice,
                IsDefault = request.IsDefault,
                TopSpeed = request.TopSpeed,
                Acceleration0To100 = request.Acceleration0To100,
                AvailabilityDate = request.AvailabilityDate,
                IsAvailable = request.IsAvailable
            };
            
            // Jeśli ten silnik ma być domyślny, zaktualizuj istniejące powiązania
            if (request.IsDefault)
            {
                await UpdateDefaultEngine(carModelId, request.EngineId);
            }
            
            var result = await _repository.CreateAsync(carModelEngine);
            if (!result.IsSuccess)
                return Result<CarModelEngineDto>.Failure(result.Error);
                
            return Result<CarModelEngineDto>.Success(
                MapToDto(result.Value, carModelResult.Value, engineResult.Value));
        }

        public async Task<Result<CarModelEngineDto>> UpdateEngineForCarModelAsync(string carModelId, string engineId, UpdateCarModelEngineRequest request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<CarModelEngineDto>.Failure(
                    new Error("ValidationError", errorMessage));
            }
            
            // Sprawdź, czy powiązanie istnieje
            var existingResult = await _repository.GetByCarModelAndEngineIdAsync(carModelId, engineId);
            if (!existingResult.IsSuccess)
                return Result<CarModelEngineDto>.Failure(existingResult.Error);
            
            var existingCarModelEngine = existingResult.Value;
            
            // Aktualizacja tylko tych pól, które są podane w zapytaniu
            if (request.AdditionalPrice.HasValue)
                existingCarModelEngine.AdditionalPrice = request.AdditionalPrice.Value;
                
            if (request.IsDefault.HasValue && request.IsDefault.Value && !existingCarModelEngine.IsDefault)
            {
                await UpdateDefaultEngine(carModelId, engineId);
                existingCarModelEngine.IsDefault = true;
            }
            else if (request.IsDefault.HasValue)
            {
                existingCarModelEngine.IsDefault = request.IsDefault.Value;
            }
                
            if (request.TopSpeed.HasValue)
                existingCarModelEngine.TopSpeed = request.TopSpeed.Value;
                
            if (request.Acceleration0To100.HasValue)
                existingCarModelEngine.Acceleration0To100 = request.Acceleration0To100.Value;
                
            if (request.AvailabilityDate.HasValue)
                existingCarModelEngine.AvailabilityDate = request.AvailabilityDate;
                
            if (request.IsAvailable.HasValue)
                existingCarModelEngine.IsAvailable = request.IsAvailable.Value;
            
            var result = await _repository.UpdateAsync(existingCarModelEngine.Id, existingCarModelEngine);
            if (!result.IsSuccess)
                return Result<CarModelEngineDto>.Failure(result.Error);
            
            var carModelResult = await _carModelRepository.GetByIdAsync(carModelId);
            var engineResult = await _engineRepository.GetByIdAsync(engineId);
            
            return Result<CarModelEngineDto>.Success(
                MapToDto(result.Value, carModelResult.Value, engineResult.Value));
        }

        public async Task<Result<bool>> RemoveEngineFromCarModelAsync(string carModelId, string engineId)
        {
            return await _repository.DeleteByCarModelAndEngineIdAsync(carModelId, engineId);
        }
        
        private async Task UpdateDefaultEngine(string carModelId, string newDefaultEngineId)
        {
            var carModelEnginesResult = await _repository.GetByCarModelIdAsync(carModelId);
            if (!carModelEnginesResult.IsSuccess)
                return;
                
            foreach (var cme in carModelEnginesResult.Value)
            {
                if (cme.EngineId != newDefaultEngineId && cme.IsDefault)
                {
                    cme.IsDefault = false;
                    await _repository.UpdateAsync(cme.Id, cme);
                }
            }
        }
        
        private CarModelEngineDto MapToDto(CarModelEngine carModelEngine, CarModel carModel, Engine engine)
        {
            return new CarModelEngineDto
            {
                Id = carModelEngine.Id,
                CarModelId = carModelEngine.CarModelId,
                EngineId = carModelEngine.EngineId,
                AdditionalPrice = carModelEngine.AdditionalPrice,
                IsDefault = carModelEngine.IsDefault,
                TopSpeed = carModelEngine.TopSpeed,
                Acceleration0To100 = carModelEngine.Acceleration0To100,
                AvailabilityDate = carModelEngine.AvailabilityDate,
                IsAvailable = carModelEngine.IsAvailable,
                CarModel = new CarModelDto
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
                },
                Engine = new EngineDto
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
                }
            };
        }
    }
}