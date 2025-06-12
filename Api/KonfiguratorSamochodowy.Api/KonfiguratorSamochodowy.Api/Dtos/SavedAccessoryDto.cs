namespace KonfiguratorSamochodowy.Api.Dtos;

public class SavedAccessoryDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Type { get; set; }
}