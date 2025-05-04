namespace KonfiguratorSamochodowy.Api.Dtos
{
    public class CarAccessoryDto
    {
        public string Id { get; set; }
        public string CarId { get; set; }
        public string CarModel { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Manufacturer { get; set; }
        public string PartNumber { get; set; }
        public bool IsOriginalBMWPart { get; set; }
        public bool IsInStock { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }

        // Additional properties
        public string Size { get; set; }
        public string Pattern { get; set; }
        public string Color { get; set; }
        public string Material { get; set; }
        public int? Capacity { get; set; }
        public string Compatibility { get; set; }
        public string AgeGroup { get; set; }
        public int? MaxLoad { get; set; }
        public bool IsUniversal { get; set; }
        public string InstallationDifficulty { get; set; }
        public string Warranty { get; set; }
    }

    public class CarAccessoryConfigurationDto
    {
        public string CarId { get; set; }
        public string CarModel { get; set; }
        public Dictionary<string, List<CarAccessoryDto>> AccessoriesByCategory { get; set; }
        public decimal TotalPrice => AccessoriesByCategory
            .SelectMany(x => x.Value)
            .Sum(a => a.Price);
    }
}
