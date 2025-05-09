namespace KonfiguratorSamochodowy.Api.Enums
{
    public static class BodyType
    {
        public const string Sedan = "Sedan";
        public const string Combi = "Combi";
        public const string Hatchback = "Hatchback";
        public const string SUV = "SUV";
        public const string Coupe = "Coupe";
        public const string Convertible = "Convertible";
        public const string Limousine = "Limousine";
        
        public static readonly string[] AllTypes = new[]
        {
            Sedan, Combi, Hatchback, SUV, Coupe, Convertible, Limousine
        };
    }
}