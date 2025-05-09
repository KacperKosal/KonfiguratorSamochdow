using KonfiguratorSamochodowy.Api.Common;
using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Services;

namespace KonfiguratorSamochodowy.Api.Services
{
    public class CarConfigurationService : ICarConfigurationService
    {
        private readonly ICarModelService _carModelService;
        private readonly ICarModelEngineService _carModelEngineService;
        private readonly ICarInteriorEquipmentService _carInteriorEquipmentService;
        private readonly ICarAccessoryService _carAccessoryService;
        
        public CarConfigurationService(
            ICarModelService carModelService,
            ICarModelEngineService carModelEngineService,
            ICarInteriorEquipmentService carInteriorEquipmentService,
            ICarAccessoryService carAccessoryService)
        {
            _carModelService = carModelService;
            _carModelEngineService = carModelEngineService;
            _carInteriorEquipmentService = carInteriorEquipmentService;
            _carAccessoryService = carAccessoryService;
        }
        
        public async Task<Result<CarConfigurationDto>> GetFullCarConfigurationAsync(string carModelId)
        {
            // Pobierz model samochodu
            var carModelResult = await _carModelService.GetByIdAsync(carModelId);
            if (!carModelResult.IsSuccess)
                return Result<CarConfigurationDto>.Failure(carModelResult.Error);
            
            // Pobierz dostępne silniki dla modelu
            var enginesResult = await _carModelEngineService.GetByCarModelIdAsync(carModelId);
            if (!enginesResult.IsSuccess)
                return Result<CarConfigurationDto>.Failure(enginesResult.Error);
            
            // Pobierz dostępne wyposażenie wnętrza
            var interiorEquipmentResult = await _carInteriorEquipmentService.GetByCarIdAsync(carModelId);
            
            // Pobierz dostępne akcesoria
            var accessoriesResult = await _carAccessoryService.GetByCarIdAsync(carModelId);
            
            var configuration = new CarConfigurationDto
            {
                CarModel = carModelResult.Value,
                AvailableEngines = enginesResult.Value.ToList(),
                AvailableInteriorEquipment = interiorEquipmentResult.IsSuccess ? interiorEquipmentResult.Value.ToList() : new List<CarInteriorEquipmentDto>(),
                AvailableAccessories = accessoriesResult.IsSuccess ? accessoriesResult.Value.ToList() : new List<CarAccessoryDto>()
            };
            
            return Result<CarConfigurationDto>.Success(configuration);
        }

        public async Task<Result<CarConfigurationDto>> GetCarConfigurationWithEngineAsync(string carModelId, string engineId)
        {
            // Pobierz pełną konfigurację
            var configurationResult = await GetFullCarConfigurationAsync(carModelId);
            if (!configurationResult.IsSuccess)
                return configurationResult;
            
            // Sprawdź, czy silnik jest dostępny dla tego modelu
            var carModelEngineResult = await _carModelEngineService.GetByCarModelAndEngineIdAsync(carModelId, engineId);
            if (!carModelEngineResult.IsSuccess)
                return Result<CarConfigurationDto>.Failure(
                    new Error("NotFound", $"Silnik o ID {engineId} nie jest dostępny dla modelu o ID {carModelId}"));
            
            // Zwróć pełną konfigurację
            return configurationResult;
        }
    }
}