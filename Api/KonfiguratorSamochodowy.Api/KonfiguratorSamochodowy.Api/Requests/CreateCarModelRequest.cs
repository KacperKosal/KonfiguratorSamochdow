namespace KonfiguratorSamochodowy.Api.Requests
{
    public class CreateCarModelRequest
    {
        public string Name { get; set; }
        public int ProductionYear { get; set; }
        public string BodyType { get; set; }
        public string Manufacturer { get; set; }
        public string? Segment { get; set; }
        public decimal BasePrice { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public bool Has4x4 { get; set; } = false;
        public bool IsElectric { get; set; } = false;
    }
}