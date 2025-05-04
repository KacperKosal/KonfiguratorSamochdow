namespace KonfiguratorSamochodowy.Api.Repositories.Enums
{
    public static class AccessoryCategory
    {
        public const string Wheels = "Wheels";
        public const string Exterior = "Exterior";
        public const string Interior = "Interior";
        public const string Electronics = "Electronics";
        public const string Transport = "Transport";
        public const string Safety = "Safety";
        public const string Seasonal = "Seasonal";
        public const string Connectivity = "Connectivity";
        public const string Family = "Family";

        public static readonly string[] AllCategories = new[]
        {
            Wheels, Exterior, Interior, Electronics, Transport,
            Safety, Seasonal, Connectivity, Family
        };
    }
}
