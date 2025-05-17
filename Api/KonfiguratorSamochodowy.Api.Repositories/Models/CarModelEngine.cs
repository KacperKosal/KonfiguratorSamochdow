namespace KonfiguratorSamochodowy.Api.Models
{
    public class CarModelEngine
    {
        public string Id { get; set; } = string.Empty;
        public string CarModelId { get; set; } = string.Empty;
        public string EngineId { get; set; } = string.Empty;
        public decimal AdditionalPrice { get; set; }
        public bool IsDefault { get; set; }
        public int TopSpeed { get; set; } // km/h
        public decimal Acceleration0To100 { get; set; } // sekundy
        public DateTime? AvailabilityDate { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}