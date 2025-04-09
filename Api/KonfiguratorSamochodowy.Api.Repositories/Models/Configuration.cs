namespace KonfiguratorSamochodowy.Api.Repositories.Models;

/// <summary>
/// Klasa reprezentująca konfigurację pojazdu
/// </summary>
public class Configuration
{
    /// <summary>
    /// Unikalny identyfikator konfiguracji
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identyfikator użytkownika, który utworzył konfigurację
    /// </summary>
    public int IDUzytkownika { get; set; }

    /// <summary>
    /// Identyfikator pojazdu, dla którego utworzono konfigurację
    /// </summary>
    public int IDPojazdu { get; set; }

    /// <summary>
    /// Data i czas utworzenia konfiguracji
    /// </summary>
    public DateTime? DataUtworzenia { get; set; }

    /// <summary>
    /// Nazwa nadana konfiguracji przez użytkownika
    /// </summary>
    public string? NazwaKonfiguracji { get; set; }

    /// <summary>
    /// Opis konfiguracji zawierający dodatkowe informacje
    /// </summary>
    public string? OpisKonfiguracji { get; set; }
} 