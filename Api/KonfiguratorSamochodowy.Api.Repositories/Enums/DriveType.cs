namespace KonfiguratorSamochodowy.Api.Enums
{
    public static class DriveType
    {
        public const string FWD = "FWD";
        public const string RWD = "RWD";
        public const string AWD = "AWD";
        
        public static readonly string[] AllTypes = new[]
        {
            FWD, RWD, AWD
        };
    }
}