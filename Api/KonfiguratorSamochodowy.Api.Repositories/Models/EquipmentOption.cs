namespace KonfiguratorSamochodowy.Api.Repositories.Models;

/// <summary>
/// Klasa reprezentująca opcję wyposażenia dla pojazdu
/// </summary>
public class EquipmentOption
{
    /// <summary>
    /// Unikalny identyfikator opcji wyposażenia
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identyfikator pojazdu, dla którego dostępna jest opcja
    /// </summary>
    public int IDPojazdu { get; set; }

    /// <summary>
    /// Identyfikator elementu wyposażenia powiązanego z opcją
    /// </summary>
    public int IDElementuWyposazenia { get; set; }

    /// <summary>
    /// Wartość opcji wyposażenia
    /// </summary>
    public string? Wartosc { get; set; }

    /// <summary>
    /// Dodatkowa cena za wybraną opcję wyposażenia
    /// </summary>
    public decimal? CenaDodatkowa { get; set; }
} 