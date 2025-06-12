using System.Text.Json;
using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Services;

public class UserConfigurationService : IUserConfigurationService
{
    private readonly IUserConfigurationRepository _repository;
    private readonly ICarModelRepository _carModelRepository;
    private readonly IEngineRepository _engineRepository;
    private readonly ICarAccessoryRepository _accessoryRepository;
    private readonly ICarInteriorEquipmentRepository _interiorRepository;

    public UserConfigurationService(
        IUserConfigurationRepository repository,
        ICarModelRepository carModelRepository,
        IEngineRepository engineRepository,
        ICarAccessoryRepository accessoryRepository,
        ICarInteriorEquipmentRepository interiorRepository)
    {
        _repository = repository;
        _carModelRepository = carModelRepository;
        _engineRepository = engineRepository;
        _accessoryRepository = accessoryRepository;
        _interiorRepository = interiorRepository;
    }

    public async Task<Result<int>> SaveUserConfigurationAsync(int userId, SaveUserConfigurationRequest request)
    {
        try
        {
            // Sprawdź limit konfiguracji użytkownika (maksymalnie 9)
            var existingConfigurationsResult = await _repository.GetUserConfigurationsAsync(userId);
            if (existingConfigurationsResult.IsSuccess && existingConfigurationsResult.Value.Count >= 9)
            {
                return Repositories.Helpers.Result.Failure<int>(Error.Validation("Configuration.LimitExceeded", 
                    "Osiągnięto maksymalny limit 9 konfiguracji na użytkownika. Usuń starsze konfiguracje, aby dodać nową."));
            }

            // Pobierz szczegóły modelu samochodu
            var carModelResult = await _carModelRepository.GetByIdAsync(request.CarModelId);
            var carModelName = carModelResult.IsSuccess ? carModelResult.Value.Name : "Unknown Model";

            // Pobierz szczegóły silnika
            string? engineName = null;
            if (!string.IsNullOrEmpty(request.EngineId))
            {
                var engineResult = await _engineRepository.GetByIdAsync(request.EngineId);
                engineName = engineResult.IsSuccess ? engineResult.Value.Name : null;
            }

            // Pobierz szczegóły akcesoriów
            var accessoriesJson = "[]";
            if (request.AccessoryIds != null && request.AccessoryIds.Any())
            {
                var accessories = new List<object>();
                foreach (var accessoryId in request.AccessoryIds)
                {
                    var accessoryResult = await _accessoryRepository.GetByIdAsync(accessoryId);
                    if (accessoryResult.IsSuccess)
                    {
                        accessories.Add(new
                        {
                            Id = accessoryResult.Value.Id,
                            Name = accessoryResult.Value.Name,
                            Price = accessoryResult.Value.Price,
                            Type = accessoryResult.Value.Type?.ToString()
                        });
                    }
                }
                accessoriesJson = JsonSerializer.Serialize(accessories);
            }

            // Pobierz szczegóły wyposażenia wnętrza
            var interiorEquipmentJson = "[]";
            if (request.InteriorEquipmentIds != null && request.InteriorEquipmentIds.Any())
            {
                var interiorEquipment = new List<object>();
                foreach (var equipmentId in request.InteriorEquipmentIds)
                {
                    var equipmentResult = await _interiorRepository.GetByIdAsync(equipmentId);
                    if (equipmentResult.IsSuccess)
                    {
                        interiorEquipment.Add(new
                        {
                            Id = equipmentResult.Value.Id,
                            Type = equipmentResult.Value.Type?.ToString(),
                            Value = equipmentResult.Value.Value,
                            Description = equipmentResult.Value.Description,
                            AdditionalPrice = equipmentResult.Value.AdditionalPrice
                        });
                    }
                }
                interiorEquipmentJson = JsonSerializer.Serialize(interiorEquipment);
            }

            var configuration = new UserConfiguration
            {
                UserId = userId,
                ConfigurationName = request.ConfigurationName,
                CarModelId = request.CarModelId,
                CarModelName = carModelName,
                EngineId = request.EngineId,
                EngineName = engineName,
                ExteriorColor = request.ExteriorColor,
                ExteriorColorName = request.ExteriorColorName,
                SelectedAccessories = accessoriesJson,
                SelectedInteriorEquipment = interiorEquipmentJson,
                TotalPrice = request.TotalPrice,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            return await _repository.SaveUserConfigurationAsync(configuration);
        }
        catch (Exception ex)
        {
            return Repositories.Helpers.Result.Failure<int>(Repositories.Helpers.Error.Failure("Service.SaveConfiguration", ex.Message));
        }
    }

    public async Task<Result<List<UserConfigurationDto>>> GetUserConfigurationsAsync(int userId)
    {
        var result = await _repository.GetUserConfigurationsAsync(userId);
        if (!result.IsSuccess)
            return Repositories.Helpers.Result.Failure<List<UserConfigurationDto>>(result.Error);

        var dtos = result.Value.Select(config => new UserConfigurationDto
        {
            Id = config.Id,
            ConfigurationName = config.ConfigurationName,
            CarModelId = config.CarModelId,
            CarModelName = config.CarModelName,
            EngineId = config.EngineId,
            EngineName = config.EngineName,
            ExteriorColor = config.ExteriorColor,
            ExteriorColorName = config.ExteriorColorName,
            SelectedAccessories = ParseJsonToAccessories(config.SelectedAccessories),
            SelectedInteriorEquipment = ParseJsonToInteriorEquipment(config.SelectedInteriorEquipment),
            TotalPrice = config.TotalPrice,
            CreatedAt = config.CreatedAt,
            UpdatedAt = config.UpdatedAt
        }).ToList();

        return Repositories.Helpers.Result.Success(dtos);
    }

    public async Task<Result<UserConfigurationDto>> GetUserConfigurationByIdAsync(int configurationId, int userId)
    {
        var result = await _repository.GetUserConfigurationByIdAsync(configurationId, userId);
        if (!result.IsSuccess)
            return Repositories.Helpers.Result.Failure<UserConfigurationDto>(result.Error);

        var config = result.Value;
        var dto = new UserConfigurationDto
        {
            Id = config.Id,
            ConfigurationName = config.ConfigurationName,
            CarModelId = config.CarModelId,
            CarModelName = config.CarModelName,
            EngineId = config.EngineId,
            EngineName = config.EngineName,
            ExteriorColor = config.ExteriorColor,
            ExteriorColorName = config.ExteriorColorName,
            SelectedAccessories = ParseJsonToAccessories(config.SelectedAccessories),
            SelectedInteriorEquipment = ParseJsonToInteriorEquipment(config.SelectedInteriorEquipment),
            TotalPrice = config.TotalPrice,
            CreatedAt = config.CreatedAt,
            UpdatedAt = config.UpdatedAt
        };

        return Repositories.Helpers.Result.Success(dto);
    }

    public async Task<Result<bool>> DeleteUserConfigurationAsync(int configurationId, int userId)
    {
        return await _repository.DeleteUserConfigurationAsync(configurationId, userId);
    }

    private List<SavedAccessoryDto>? ParseJsonToAccessories(string? json)
    {
        if (string.IsNullOrEmpty(json))
            return new List<SavedAccessoryDto>();

        try
        {
            return JsonSerializer.Deserialize<List<SavedAccessoryDto>>(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing accessories JSON: {ex.Message}");
            return new List<SavedAccessoryDto>();
        }
    }

    private List<SavedInteriorEquipmentDto>? ParseJsonToInteriorEquipment(string? json)
    {
        if (string.IsNullOrEmpty(json))
            return new List<SavedInteriorEquipmentDto>();

        try
        {
            return JsonSerializer.Deserialize<List<SavedInteriorEquipmentDto>>(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing interior equipment JSON: {ex.Message}");
            return new List<SavedInteriorEquipmentDto>();
        }
    }
}