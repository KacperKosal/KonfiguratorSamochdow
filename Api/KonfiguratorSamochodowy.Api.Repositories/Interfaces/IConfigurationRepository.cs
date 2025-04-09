using KonfiguratorSamochodowy.Api.Repositories.Models;

namespace KonfiguratorSamochodowy.Api.Repositories.Interfaces;

/// <summary>
/// Interfejs repozytorium konfiguracji pojazdów
/// </summary>
public interface IConfigurationRepository
{
    /// <summary>
    /// Pobiera konfigurację na podstawie identyfikatora
    /// </summary>
    /// <param name="id">Identyfikator konfiguracji</param>
    /// <returns>Obiekt konfiguracji lub null, jeśli nie znaleziono</returns>
    Task<Configuration?> GetByIdAsync(int id);

    /// <summary>
    /// Pobiera wszystkie konfiguracje dla danego użytkownika
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika</param>
    /// <returns>Kolekcja konfiguracji użytkownika</returns>
    Task<IEnumerable<Configuration>> GetByUserIdAsync(int userId);

    /// <summary>
    /// Tworzy nową konfigurację
    /// </summary>
    /// <param name="configuration">Obiekt konfiguracji do utworzenia</param>
    /// <returns>Identyfikator utworzonej konfiguracji</returns>
    Task<int> CreateAsync(Configuration configuration);

    /// <summary>
    /// Aktualizuje dane konfiguracji
    /// </summary>
    /// <param name="configuration">Obiekt konfiguracji z zaktualizowanymi danymi</param>
    Task UpdateAsync(Configuration configuration);

    /// <summary>
    /// Usuwa konfigurację na podstawie identyfikatora
    /// </summary>
    /// <param name="id">Identyfikator konfiguracji do usunięcia</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Sprawdza, czy konfiguracja o podanym identyfikatorze istnieje
    /// </summary>
    /// <param name="id">Identyfikator konfiguracji</param>
    /// <returns>True, jeśli konfiguracja istnieje; w przeciwnym razie false</returns>
    Task<bool> ExistsAsync(int id);
} 