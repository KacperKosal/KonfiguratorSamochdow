namespace KonfiguratorSamochodowy.Api.Requests;

public record SaveUserConfigurationRequest(
    string ConfigurationName,
    string CarModelId,
    string? EngineId,
    string ExteriorColor,
    string? ExteriorColorName,
    string[]? AccessoryIds,
    string[]? InteriorEquipmentIds,
    decimal TotalPrice
);