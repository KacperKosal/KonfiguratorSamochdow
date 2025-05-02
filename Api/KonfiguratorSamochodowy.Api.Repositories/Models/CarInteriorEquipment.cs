namespace KonfiguratorSamochodowy.Api.Repositories.Models;

    public class CarInteriorEquipment
    {
        public string Id { get; set; }
        public string CarId { get; set; }
        public string CarModel { get; set; } // Model BMW
        public string Type { get; set; }
        public string Value { get; set; }
        public decimal AdditionalPrice { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        
        // Dodatkowe pola specyficzne dla typu
        public string ColorCode { get; set; }
        public int? Intensity { get; set; }
        public bool? HasNavigation { get; set; }
        public bool? HasPremiumSound { get; set; }
        public string ControlType { get; set; }
    }
