namespace KonfiguratorSamochodowy.Api.Repositories.Models;

/// <summary>
/// Klasa reprezentująca pojazd w systemie
/// </summary>
public class Vehicle
{
    /// <summary>
    /// Unikalny identyfikator pojazdu
    /// </summary>
    public int Id { get; set; }


    /// <summary>
    /// Model pojazdu
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// Kolor nadwozia pojazdu
    /// </summary>
    public string? KolorNadwozia { get; set; }

    /// <summary>
    /// Wyposażenie wnętrza pojazdu
    /// </summary>
    public string? WyposazenieWnetrza { get; set; }

    /// <summary>
    /// Dodatkowe akcesoria pojazdu
    /// </summary>
    public string? Akcesoria { get; set; }

    public int Cena { get; set; }

    public string? Opis { get; set; } = null;

    public byte[]? Zdjecie { get; set; } = null;

    public IEnumerable<Engine>? Silniki { get; set; } = null;

    public IEnumerable<VehicleFeatures>? Cechy { get; set; } = null;
} 