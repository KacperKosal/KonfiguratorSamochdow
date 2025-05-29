namespace KonfiguratorSamochodowy.Api.Repositories.Models
{
    public class CarModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int ProductionYear { get; set; }
        public string BodyType { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string Segment { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool Has4x4 { get; set; }
        public bool IsElectric { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}