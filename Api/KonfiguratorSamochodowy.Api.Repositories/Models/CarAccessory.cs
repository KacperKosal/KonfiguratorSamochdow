namespace KonfiguratorSamochodowy.Api.Repositories.Models
{
    public class CarAccessory
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

        // Specific properties for different accessory types
        public string Size { get; set; } // For wheels, mats
        public string Pattern { get; set; } // For wheels
        public string Color { get; set; } // Multiple use
        public string Material { get; set; } // For mats, covers
        public int Capacity { get; set; } // For roof boxes, powerbanks
        public string Compatibility { get; set; } // For electronics
        public string AgeGroup { get; set; } // For child seats
        public int MaxLoad { get; set; } // For carriers, racks
        public bool IsUniversal { get; set; }
        public string InstallationDifficulty { get; set; } // Easy, Medium, Professional
        public string Warranty { get; set; }
    }
}
