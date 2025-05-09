namespace KonfiguratorSamochodowy.Api.Requests
{
    public class UpdateCarModelEngineRequest
    {
        public decimal? AdditionalPrice { get; set; }
        public bool? IsDefault { get; set; }
        public int? TopSpeed { get; set; }
        public decimal? Acceleration0To100 { get; set; }
        public DateTime? AvailabilityDate { get; set; }
        public bool? IsAvailable { get; set; }
    }
}