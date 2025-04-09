using KonfiguratorSamochodowy.Api.Repositories.Models;

namespace KonfiguratorSamochodowy.Api.Repositories.Interfaces;

/// <summary>
/// Interfejs repozytorium użytkowników
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Pobiera użytkownika na podstawie identyfikatora
    /// </summary>
    /// <param name="id">Identyfikator użytkownika</param>
    /// <returns>Obiekt użytkownika lub null, jeśli nie znaleziono</returns>
    Task<User?> GetByIdAsync(int id);

    /// <summary>
    /// Pobiera użytkownika na podstawie adresu email
    /// </summary>
    /// <param name="email">Adres email użytkownika</param>
    /// <returns>Obiekt użytkownika lub null, jeśli nie znaleziono</returns>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Tworzy nowego użytkownika
    /// </summary>
    /// <param name="user">Obiekt użytkownika do utworzenia</param>
    /// <returns>Identyfikator utworzonego użytkownika</returns>
    Task<int> CreateAsync(User user);

    /// <summary>
    /// Aktualizuje dane użytkownika
    /// </summary>
    /// <param name="user">Obiekt użytkownika z zaktualizowanymi danymi</param>
    Task UpdateAsync(User user);

    /// <summary>
    /// Usuwa użytkownika na podstawie identyfikatora
    /// </summary>
    /// <param name="id">Identyfikator użytkownika do usunięcia</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Sprawdza, czy użytkownik o podanym identyfikatorze istnieje
    /// </summary>
    /// <param name="id">Identyfikator użytkownika</param>
    /// <returns>True, jeśli użytkownik istnieje; w przeciwnym razie false</returns>
    Task<bool> ExistsAsync(int id);
} 