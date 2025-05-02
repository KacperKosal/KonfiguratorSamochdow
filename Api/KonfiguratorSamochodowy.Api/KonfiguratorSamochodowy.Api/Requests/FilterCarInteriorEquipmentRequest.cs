namespace KonfiguratorSamochodowy.Api.Requests
{
    public class FilterCarInteriorEquipmentRequest
    {
        public string CarId { get; set; }
        public string CarModel { get; set; }
        public string Type { get; set; }
        public bool? IsDefault { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}