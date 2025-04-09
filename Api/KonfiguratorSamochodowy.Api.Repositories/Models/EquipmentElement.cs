namespace KonfiguratorSamochodowy.Api.Repositories.Models;

/// <summary>
/// Klasa reprezentująca element wyposażenia pojazdu
/// </summary>
public class EquipmentElement
{
    /// <summary>
    /// Unikalny identyfikator elementu wyposażenia
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nazwa elementu wyposażenia
    /// </summary>
    public string? Nazwa { get; set; }

    /// <summary>
    /// Szczegółowy opis elementu wyposażenia
    /// </summary>
    public string? Opis { get; set; }

    /// <summary>
    /// Cena elementu wyposażenia
    /// </summary>
    public decimal? Cena { get; set; }

    /// <summary>
    /// Kategoria, do której należy element wyposażenia
    /// </summary>
    public string? Kategoria { get; set; }

    /// <summary>
    /// Zdjęcie elementu wyposażenia w formacie binarnym
    /// </summary>
    public byte[]? Zdjecie { get; set; }
} 