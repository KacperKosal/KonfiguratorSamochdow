namespace KonfiguratorSamochodowy.Api.Repositories.Models;

/// <summary>
/// Klasa reprezentująca rejestrację użytkownika w systemie
/// </summary>
public class Registration
{
    /// <summary>
    /// Unikalny identyfikator rejestracji
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identyfikator zarejestrowanego użytkownika
    /// </summary>
    public int IDUzytkownika { get; set; }

    /// <summary>
    /// Data i czas rejestracji użytkownika
    /// </summary>
    public DateTime? DataRejestracji { get; set; }

    /// <summary>
    /// Adres IP, z którego wykonano rejestrację
    /// </summary>
    public string? AdresIP { get; set; }

    /// <summary>
    /// Status rejestracji (np. zakończona, oczekująca na potwierdzenie)
    /// </summary>
    public string? Status { get; set; }
} 