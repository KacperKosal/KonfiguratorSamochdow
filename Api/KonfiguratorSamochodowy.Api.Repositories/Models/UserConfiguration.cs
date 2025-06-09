namespace KonfiguratorSamochodowy.Api.Repositories.Models;

/// <summary>
/// Klasa reprezentująca szczegółową konfigurację pojazdu użytkownika
/// </summary>
public class UserConfiguration
{
    /// <summary>
    /// Unikalny identyfikator konfiguracji
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identyfikator użytkownika, który utworzył konfigurację
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Nazwa konfiguracji nadana przez użytkownika
    /// </summary>
    public string? ConfigurationName { get; set; }

    /// <summary>
    /// Identyfikator modelu samochodu
    /// </summary>
    public string? CarModelId { get; set; }

    /// <summary>
    /// Nazwa modelu samochodu
    /// </summary>
    public string? CarModelName { get; set; }

    /// <summary>
    /// Identyfikator silnika
    /// </summary>
    public string? EngineId { get; set; }

    /// <summary>
    /// Nazwa silnika
    /// </summary>
    public string? EngineName { get; set; }

    /// <summary>
    /// Kolor nadwozia (hex)
    /// </summary>
    public string? ExteriorColor { get; set; }

    /// <summary>
    /// Nazwa koloru nadwozia
    /// </summary>
    public string? ExteriorColorName { get; set; }

    /// <summary>
    /// Lista wybranych akcesoriów (JSON)
    /// </summary>
    public string? SelectedAccessories { get; set; }

    /// <summary>
    /// Lista wybranego wyposażenia wnętrza (JSON)
    /// </summary>
    public string? SelectedInteriorEquipment { get; set; }

    /// <summary>
    /// Całkowita cena konfiguracji
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Data utworzenia konfiguracji
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Data ostatniej modyfikacji
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Czy konfiguracja jest aktywna
    /// </summary>
    public bool IsActive { get; set; } = true;
}