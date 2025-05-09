namespace KonfiguratorSamochodowy.Api.Dtos
{
    public class CarConfigurationDto
    {
        public CarModelDto CarModel { get; set; }
        public List<CarModelEngineDto> AvailableEngines { get; set; }
        public List<CarInteriorEquipmentDto> AvailableInteriorEquipment { get; set; }
        public List<CarAccessoryDto> AvailableAccessories { get; set; }
        public decimal BasePrice => CarModel?.BasePrice ?? 0;
        
        // Metody pomocnicze do obliczania ceny z konkretnym silnikiem
        public decimal CalculatePriceWithEngine(string engineId)
        {
            var engine = AvailableEngines?.FirstOrDefault(e => e.EngineId == engineId);
            return BasePrice + (engine?.AdditionalPrice ?? 0);
        }
    }
}