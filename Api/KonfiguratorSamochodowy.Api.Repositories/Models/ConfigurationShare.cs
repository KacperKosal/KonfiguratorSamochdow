namespace KonfiguratorSamochodowy.Api.Repositories.Models;

/// <summary>
/// Klasa reprezentująca udostępnienie konfiguracji pojazdu
/// </summary>
public class ConfigurationShare
{
    /// <summary>
    /// Unikalny identyfikator udostępnienia
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identyfikator udostępnianej konfiguracji
    /// </summary>
    public int IDKonfiguracji { get; set; }

    /// <summary>
    /// Link do udostępnionej konfiguracji
    /// </summary>
    public string? Link { get; set; }

    /// <summary>
    /// Adres odbiorcy udostępnienia (np. email)
    /// </summary>
    public string? Odbiorca { get; set; }

    /// <summary>
    /// Data i czas udostępnienia konfiguracji
    /// </summary>
    public DateTime? DataUdostepnienia { get; set; }
} 