namespace KonfiguratorSamochodowy.Api.Dtos;

    public class CarInteriorEquipmentDto
    {
        public string Id { get; set; }
        public string CarId { get; set; }
        public string CarModel { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public decimal AdditionalPrice { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        
        // Dodatkowe pola w zależności od typu
        public string ColorCode { get; set; }
        public int? Intensity { get; set; }
        public bool? HasNavigation { get; set; }
        public bool? HasPremiumSound { get; set; }
        public string ControlType { get; set; }
    }
