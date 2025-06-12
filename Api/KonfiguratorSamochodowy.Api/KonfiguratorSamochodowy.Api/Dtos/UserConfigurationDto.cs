namespace KonfiguratorSamochodowy.Api.Dtos;

public class UserConfigurationDto
{
    public int Id { get; set; }
    public string? ConfigurationName { get; set; }
    public string? CarModelId { get; set; }
    public string? CarModelName { get; set; }
    public string? EngineId { get; set; }
    public string? EngineName { get; set; }
    public string? ExteriorColor { get; set; }
    public string? ExteriorColorName { get; set; }
    public List<SavedAccessoryDto>? SelectedAccessories { get; set; }
    public List<SavedInteriorEquipmentDto>? SelectedInteriorEquipment { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}