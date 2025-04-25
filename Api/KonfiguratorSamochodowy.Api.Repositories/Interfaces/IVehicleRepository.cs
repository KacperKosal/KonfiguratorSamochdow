using KonfiguratorSamochodowy.Api.Repositories.Models;
using KonfiguratorSamochodowy.Api.Repositories.Options;

namespace KonfiguratorSamochodowy.Api.Repositories.Interfaces;


/// <summary>
/// Interfejs repozytorium pojazdów
/// </summary>
public interface IVehicleRepository
{
    Task<byte[]> GetImageByIdAsync(int id);

    public Task<IEnumerable<Vehicle>> GetByFiltersAsync(SortingOptions sortingOptions);

    /// <summary>
    /// Pobiera pojazd na podstawie identyfikatora
    /// </summary>
    /// <param name="id">Identyfikator pojazdu</param>
    /// <returns>Obiekt pojazdu lub null, jeśli nie znaleziono</returns>
    Task<Vehicle?> GetByIdAsync(int id);

    /// <summary>
    /// Pobiera wszystkie pojazdy
    /// </summary>
    /// <returns>Kolekcja wszystkich pojazdów</returns>
    Task<IEnumerable<Vehicle>> GetAllAsync();

    /// <summary>
    /// Tworzy nowy pojazd
    /// </summary>
    /// <param name="vehicle">Obiekt pojazdu do utworzenia</param>
    /// <returns>Identyfikator utworzonego pojazdu</returns>
    Task<int> CreateAsync(Vehicle vehicle);

    /// <summary>
    /// Aktualizuje dane pojazdu
    /// </summary>
    /// <param name="vehicle">Obiekt pojazdu z zaktualizowanymi danymi</param>
    Task UpdateAsync(Vehicle vehicle);

    /// <summary>
    /// Usuwa pojazd na podstawie identyfikatora
    /// </summary>
    /// <param name="id">Identyfikator pojazdu do usunięcia</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Sprawdza, czy pojazd o podanym identyfikatorze istnieje
    /// </summary>
    /// <param name="id">Identyfikator pojazdu</param>
    /// <returns>True, jeśli pojazd istnieje; w przeciwnym razie false</returns>
    Task<bool> ExistsAsync(int id);
} 