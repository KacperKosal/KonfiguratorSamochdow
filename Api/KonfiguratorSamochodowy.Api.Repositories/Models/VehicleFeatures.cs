namespace KonfiguratorSamochodowy.Api.Repositories.Models;

public class VehicleFeatures
{
    /// <summary>
    /// Unikalny identyfikator Cech pojazdu
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID pojazdy
    /// </summary>
    public int IDPojazdu { get; set; }

    /// <summary>
    /// Vehicle ID (English property)
    /// </summary>
    public int VehicleId { get; set; }

    /// <summary>
    /// Cecha
    /// </summary>
    public string? Cecha { get; set; }

    /// <summary>
    /// Feature (English property)
    /// </summary>
    public string? Feature { get; set; }
}
