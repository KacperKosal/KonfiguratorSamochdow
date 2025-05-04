namespace KonfiguratorSamochodowy.Api.Requests
{
    public class FilterCarAccessoriesRequest
    {
        public string CarId { get; set; }
        public string CarModel { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsOriginalBMWPart { get; set; }
        public bool? IsInStock { get; set; }
        public string InstallationDifficulty { get; set; }
    }
}
