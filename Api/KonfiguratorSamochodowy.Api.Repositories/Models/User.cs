namespace KonfiguratorSamochodowy.Api.Repositories.Models;

/// <summary>
/// Klasa reprezentująca użytkownika systemu
/// </summary>
public class User
{
    /// <summary>
    /// Unikalny identyfikator użytkownika
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Imię i nazwisko użytkownika
    /// </summary>
    public string? ImieNazwisko { get; set; }

    /// <summary>
    /// Adres email użytkownika
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Zaszyfrowane hasło użytkownika
    /// </summary>
    public string? Haslo { get; set; }

    /// <summary>
    /// Rola użytkownika w systemie
    /// </summary>
    public string? Rola { get; set; }

    /// <summary>
    /// Status włączenia weryfikacji dwuetapowej (2FA)
    /// </summary>
    public bool Status2FA { get; set; }
} 