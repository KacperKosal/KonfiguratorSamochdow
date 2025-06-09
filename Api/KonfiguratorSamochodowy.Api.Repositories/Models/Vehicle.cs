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
    /// Body color of the vehicle (English property)
    /// </summary>
    public string? BodyColor { get; set; }

    /// <summary>
    /// Wyposażenie wnętrza pojazdu
    /// </summary>
    public string? WyposazenieWnetrza { get; set; }

    /// <summary>
    /// Interior equipment of the vehicle (English property)
    /// </summary>
    public string? InteriorEquipment { get; set; }

    /// <summary>
    /// Dodatkowe akcesoria pojazdu
    /// </summary>
    public string? Akcesoria { get; set; }

    /// <summary>
    /// Additional accessories of the vehicle (English property)
    /// </summary>
    public string? Accessories { get; set; }

    public int Cena { get; set; }

    /// <summary>
    /// Price of the vehicle (English property)
    /// </summary>
    public int Price { get; set; }

    public string? Opis { get; set; } = null;

    /// <summary>
    /// Description of the vehicle (English property)
    /// </summary>
    public string? Description { get; set; } = null;

    public byte[]? Zdjecie { get; set; } = null;

    /// <summary>
    /// Photo of the vehicle (English property)
    /// </summary>
    public byte[]? Photo { get; set; } = null;

    /// <summary>
    /// Engines available for the vehicle (English property)
    /// </summary>
    public IEnumerable<Engine>? Engines { get; set; } = null;

    /// <summary>
    /// Features of the vehicle (English property)
    /// </summary>
    public IEnumerable<VehicleFeatures>? Features { get; set; } = null;
} 