namespace KonfiguratorSamochodowy.Api.Repositories.Models;

public class Engine
{
    /// <summary>
    /// Unikalny identyfikator pojazdu
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// Pojemnosc
    /// </summary>
    public string? Pojemnosc { get; set; }

    /// <summary>
    /// Typ
    /// </summary>
    public string? Typ { get; set; }

    /// <summary>
    /// Moc
    /// </summary>
    public short Moc { get; set; }
}
