namespace KonfiguratorSamochodowy.Api.Repositories.Enums
{
    public static class InteriorEquipmentType
    {
        public const string SeatHeating = "SeatHeating";
        public const string AdjustableHeadrests = "AdjustableHeadrests";
        public const string MultifunctionSteeringWheel = "MultifunctionSteeringWheel";
        public const string RadioType = "RadioType";
        public const string CruiseControl = "CruiseControl";
        public const string ElectricMirrors = "ElectricMirrors";
        
        public static readonly string[] AllTypes = new[] 
        {
            SeatHeating, AdjustableHeadrests, MultifunctionSteeringWheel,
            RadioType, CruiseControl, ElectricMirrors
        };
    }
}