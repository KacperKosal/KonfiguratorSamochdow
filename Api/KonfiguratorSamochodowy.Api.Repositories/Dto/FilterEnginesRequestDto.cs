
namespace KonfiguratorSamochodowy.Api.Repositories.Dto
{
    public class FilterEnginesRequestDto
    {
        public string Type { get; set; }
        public string FuelType { get; set; }
        public int? MinCapacity { get; set; }
        public int? MaxCapacity { get; set; }
        public int? MinPower { get; set; }
        public int? MaxPower { get; set; }
        public string Transmission { get; set; }
        public string DriveType { get; set; }
        public bool? IsActive { get; set; }
    }
}
