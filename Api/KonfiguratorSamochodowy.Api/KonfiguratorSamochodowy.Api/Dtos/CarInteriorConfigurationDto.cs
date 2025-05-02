namespace KonfiguratorSamochodowy.Api.Dtos;

public class CarInteriorConfigurationDto
{
    public string CarId { get; set; }
    public string CarModel { get; set; }
    public IEnumerable<CarInteriorEquipmentDto> Equipment { get; set; }
    public decimal TotalAdditionalPrice => Equipment.Sum(e => e.AdditionalPrice);
}