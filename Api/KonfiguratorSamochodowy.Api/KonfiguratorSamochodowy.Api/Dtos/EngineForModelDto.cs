namespace KonfiguratorSamochodowy.Api.Dtos
{
    public class EngineForModelDto
    {
        public string EngineId { get; set; }
        public string EngineName { get; set; }
        public decimal AdditionalPrice { get; set; }
        public string Type { get; set; }
        public decimal Capacity { get; set; }
        public int Power { get; set; }
        public string FuelType { get; set; }
        public int TopSpeed { get; set; }
        public decimal Acceleration0To100 { get; set; }
        public decimal FuelConsumption { get; set; }
        public decimal CO2Emission { get; set; }
        public bool IsDefault { get; set; }
        public bool IsAvailable { get; set; }
    }
}