using Dapper;

using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using System.Data;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories;

/// <summary>
/// Implementacja repozytorium konfiguracji pojazdów
/// </summary>
internal sealed class ConfigurationRepository : IConfigurationRepository
{
    private readonly IDbConnection _connection;

    /// <summary>
    /// Inicjalizuje nową instancję klasy ConfigurationRepository
    /// </summary>
    /// <param name="connection">Połączenie do bazy danych</param>
    public ConfigurationRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    /// <summary>
    /// Pobiera konfigurację na podstawie identyfikatora
    /// </summary>
    /// <param name="id">Identyfikator konfiguracji</param>
    /// <returns>Obiekt konfiguracji lub null, jeśli nie znaleziono</returns>
    public async Task<Configuration?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM Konfiguracja WHERE ID = @Id";
        return await _connection.QueryFirstOrDefaultAsync<Configuration>(sql, new { Id = id });
    }

    /// <summary>
    /// Pobiera wszystkie konfiguracje dla danego użytkownika
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika</param>
    /// <returns>Kolekcja konfiguracji użytkownika</returns>
    public async Task<IEnumerable<Configuration>> GetByUserIdAsync(int userId)
    {
        const string sql = "SELECT * FROM Konfiguracja WHERE IDUzytkownika = @UserId";
        return await _connection.QueryAsync<Configuration>(sql, new { UserId = userId });
    }

    /// <summary>
    /// Tworzy nową konfigurację
    /// </summary>
    /// <param name="configuration">Obiekt konfiguracji do utworzenia</param>
    /// <returns>Identyfikator utworzonej konfiguracji</returns>
    public async Task<int> CreateAsync(Configuration configuration)
    {
        const string sql = @"
            INSERT INTO Konfiguracja (IDUzytkownika, IDPojazdu, DataUtworzenia, NazwaKonfiguracji, OpisKonfiguracji)
            VALUES (@IDUzytkownika, @IDPojazdu, @DataUtworzenia, @NazwaKonfiguracji, @OpisKonfiguracji)
            RETURNING ID";
        
        return await _connection.ExecuteScalarAsync<int>(sql, configuration);
    }

    /// <summary>
    /// Aktualizuje dane konfiguracji
    /// </summary>
    /// <param name="configuration">Obiekt konfiguracji z zaktualizowanymi danymi</param>
    public async Task UpdateAsync(Configuration configuration)
    {
        const string sql = @"
            UPDATE Konfiguracja 
            SET IDUzytkownika = @IDUzytkownika,
                IDPojazdu = @IDPojazdu,
                DataUtworzenia = @DataUtworzenia,
                NazwaKonfiguracji = @NazwaKonfiguracji,
                OpisKonfiguracji = @OpisKonfiguracji
            WHERE ID = @ID";
        
        await _connection.ExecuteAsync(sql, configuration);
    }

    /// <summary>
    /// Usuwa konfigurację na podstawie identyfikatora
    /// </summary>
    /// <param name="id">Identyfikator konfiguracji do usunięcia</param>
    public async Task DeleteAsync(int id)
    {
        const string sql = "DELETE FROM Konfiguracja WHERE ID = @Id";
        await _connection.ExecuteAsync(sql, new { Id = id });
    }

    /// <summary>
    /// Sprawdza, czy konfiguracja o podanym identyfikatorze istnieje
    /// </summary>
    /// <param name="id">Identyfikator konfiguracji</param>
    /// <returns>True, jeśli konfiguracja istnieje; w przeciwnym razie false</returns>
    public async Task<bool> ExistsAsync(int id)
    {
        const string sql = "SELECT COUNT(1) FROM Konfiguracja WHERE ID = @Id";
        return await _connection.ExecuteScalarAsync<bool>(sql, new { Id = id });
    }
} 