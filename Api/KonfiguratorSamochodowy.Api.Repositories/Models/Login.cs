namespace KonfiguratorSamochodowy.Api.Repositories.Models;

/// <summary>
/// Klasa reprezentująca próbę logowania użytkownika
/// </summary>
public class Login
{
    /// <summary>
    /// Unikalny identyfikator próby logowania
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identyfikator użytkownika, który próbował się zalogować
    /// </summary>
    public int IDUzytkownika { get; set; }

    /// <summary>
    /// Data i czas próby logowania
    /// </summary>
    public DateTime? DataLogowania { get; set; }

    /// <summary>
    /// Adres IP, z którego wykonano próbę logowania
    /// </summary>
    public string? AdresIP { get; set; }

    /// <summary>
    /// Status próby logowania (np. sukces, niepowodzenie)
    /// </summary>
    public string? Status { get; set; }
} 