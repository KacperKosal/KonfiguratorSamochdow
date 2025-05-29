
using KonfiguratorSamochodowy.Api.Repositories.Enums;
namespace KonfiguratorSamochodowy.Api.Repositories.Dto
{
    public class FilterCarModelsRequestDto
    {
        public string? Manufacturer { get; set; }
        public string? BodyType { get; set; }
        public string? Segment { get; set; }
        public int? MinProductionYear { get; set; }
        public int? MaxProductionYear { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsActive { get; set; }
        public string? ModelName { get; set; }
        public string? Brand { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
        public bool? Has4x4 { get; set; }
        public bool? IsElectric { get; set; }
        public Enums.SortingOption SortingOption { get; set; } = Enums.SortingOption.NameAscending;
    }
}
