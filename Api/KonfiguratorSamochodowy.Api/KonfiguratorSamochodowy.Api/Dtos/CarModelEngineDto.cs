namespace KonfiguratorSamochodowy.Api.Dtos
{
    public class CarModelEngineDto
    {
        public string Id { get; set; }
        public string CarModelId { get; set; }
        public string EngineId { get; set; }
        public decimal AdditionalPrice { get; set; }
        public bool IsDefault { get; set; }
        public int TopSpeed { get; set; }
        public decimal Acceleration0To100 { get; set; }
        public DateTime? AvailabilityDate { get; set; }
        public bool IsAvailable { get; set; }
        
        // Zagnieżdżone obiekty
        public CarModelDto CarModel { get; set; }
        public EngineDto Engine { get; set; }
    }
}