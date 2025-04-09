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
    /// Marka pojazdu
    /// </summary>
    public string? Marka { get; set; }

    /// <summary>
    /// Model pojazdu
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// Rok produkcji pojazdu
    /// </summary>
    public int? RokProdukcji { get; set; }

    /// <summary>
    /// Numer VIN pojazdu
    /// </summary>
    public string? VIN { get; set; }

    /// <summary>
    /// Typ silnika pojazdu
    /// </summary>
    public string? TypSilnika { get; set; }

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
} 