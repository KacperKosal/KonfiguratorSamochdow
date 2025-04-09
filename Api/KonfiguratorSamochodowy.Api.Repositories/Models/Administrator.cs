namespace KonfiguratorSamochodowy.Api.Repositories.Models;

/// <summary>
/// Klasa reprezentująca administratora systemu
/// </summary>
public class Administrator
{
    /// <summary>
    /// Unikalny identyfikator administratora
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identyfikator użytkownika powiązanego z administratorem
    /// </summary>
    public int IDUzytkownika { get; set; }

    /// <summary>
    /// Poziom uprawnień administratora w systemie
    /// </summary>
    public string? PoziomUprawnien { get; set; }

    /// <summary>
    /// Data i czas ostatniego logowania administratora
    /// </summary>
    public DateTime? OstatnieLogowanie { get; set; }
} 