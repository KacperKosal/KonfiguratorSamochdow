namespace KonfiguratorSamochodowy.Api.Requests
{
    public class UpdateCarAccessoryRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public string Manufacturer { get; set; }
        public string PartNumber { get; set; }
        public bool? IsOriginalBMWPart { get; set; }
        public bool? IsInStock { get; set; }
        public int? StockQuantity { get; set; }
        public string ImageUrl { get; set; }

        // Specific properties
        public string Size { get; set; }
        public string Pattern { get; set; }
        public string Color { get; set; }
        public string Material { get; set; }
        public int? Capacity { get; set; }
        public string Compatibility { get; set; }
        public string AgeGroup { get; set; }
        public int? MaxLoad { get; set; }
        public bool? IsUniversal { get; set; }
        public string InstallationDifficulty { get; set; }
        public string Warranty { get; set; }
    }
}