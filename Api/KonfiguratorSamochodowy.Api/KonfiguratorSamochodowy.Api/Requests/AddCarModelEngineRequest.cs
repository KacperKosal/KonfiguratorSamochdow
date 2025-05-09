namespace KonfiguratorSamochodowy.Api.Requests
{
    public class AddCarModelEngineRequest
    {
        public string EngineId { get; set; }
        public decimal AdditionalPrice { get; set; }
        public bool IsDefault { get; set; }
        public int TopSpeed { get; set; }
        public decimal Acceleration0To100 { get; set; }
        public DateTime? AvailabilityDate { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}