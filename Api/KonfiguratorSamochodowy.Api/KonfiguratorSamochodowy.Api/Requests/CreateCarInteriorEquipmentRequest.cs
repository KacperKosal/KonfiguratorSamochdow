using System.ComponentModel.DataAnnotations;

namespace KonfiguratorSamochodowy.Api.Requests
{
    public class CreateCarInteriorEquipmentRequest
    {
        public string CarId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public decimal AdditionalPrice { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        
        // Dodatkowe pola w zależności od typu
        public string ColorCode { get; set; }
        public string Intensity { get; set; }
        public bool? HasNavigation { get; set; }
        public bool? HasPremiumSound { get; set; }
        public string ControlType { get; set; }
    }
}