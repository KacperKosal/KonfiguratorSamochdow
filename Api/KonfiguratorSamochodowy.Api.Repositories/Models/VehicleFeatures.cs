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
    /// Cecha
    /// </summary>
    public string? Cecha { get; set; }
}
