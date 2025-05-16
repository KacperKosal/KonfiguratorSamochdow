
namespace KonfiguratorSamochodowy.Api.Repositories.Dto
{
    public class FilterCarModelsRequestDto
    {
        public string Manufacturer { get; set; }
        public string BodyType { get; set; }
        public string Segment { get; set; }
        public int? MinProductionYear { get; set; }
        public int? MaxProductionYear { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsActive { get; set; }
    }
}
