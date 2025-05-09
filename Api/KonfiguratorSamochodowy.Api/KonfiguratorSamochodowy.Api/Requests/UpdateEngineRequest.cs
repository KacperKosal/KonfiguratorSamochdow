namespace KonfiguratorSamochodowy.Api.Requests
{
    public class UpdateEngineRequest
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int? Capacity { get; set; }
        public int? Power { get; set; }
        public int? Torque { get; set; }
        public string FuelType { get; set; }
        public int? Cylinders { get; set; }
        public string Transmission { get; set; }
        public int? Gears { get; set; }
        public string DriveType { get; set; }
        public decimal? FuelConsumption { get; set; }
        public int? CO2Emission { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
    }
}