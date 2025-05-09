namespace KonfiguratorSamochodowy.Api.Enums
{
    public static class EngineType
    {
        public const string Petrol = "Petrol";
        public const string Diesel = "Diesel";
        public const string Electric = "Electric";
        public const string Hybrid = "Hybrid";
        public const string PlugInHybrid = "PlugInHybrid";
        
        public static readonly string[] AllTypes = new[]
        {
            Petrol, Diesel, Electric, Hybrid, PlugInHybrid
        };
    }
}