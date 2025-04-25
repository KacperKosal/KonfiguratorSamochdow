namespace KonfiguratorSamochodowy.Api.Dtos;

internal class ModelDto
{
    public int VechicleID { get; set; }


    public string? Description { get; set; } = string.Empty;

    public string? ImageUrl { get; set; } = string.Empty;

    public string? Model { get; set; } = string.Empty;
    public IEnumerable<string?>? VehicleFeatures { get; set; } = [];

    public IEnumerable<string?>? Engines { get; set; } = [];

    public int Price { get; set; } 
}
