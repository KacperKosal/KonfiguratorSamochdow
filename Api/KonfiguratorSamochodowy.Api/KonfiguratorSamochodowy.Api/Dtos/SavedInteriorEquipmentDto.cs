namespace KonfiguratorSamochodowy.Api.Dtos;

public class SavedInteriorEquipmentDto
{
    public string Id { get; set; } = string.Empty;
    public string? Type { get; set; }
    public string Value { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal AdditionalPrice { get; set; }
}