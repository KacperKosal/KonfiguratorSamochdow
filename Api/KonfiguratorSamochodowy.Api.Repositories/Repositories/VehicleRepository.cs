using Dapper;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using System.Data;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories;

/// <summary>
/// Implementacja repozytorium pojazdów
/// </summary>
internal sealed class VehicleRepository : IVehicleRepository
{
    private readonly IDbConnection _connection;

    /// <summary>
    /// Inicjalizuje nową instancję klasy VehicleRepository
    /// </summary>
    /// <param name="connection">Połączenie do bazy danych</param>
    public VehicleRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    /// <summary>
    /// Pobiera pojazd na podstawie identyfikatora
    /// </summary>
    /// <param name="id">Identyfikator pojazdu</param>
    /// <returns>Obiekt pojazdu lub null, jeśli nie znaleziono</returns>
    public async Task<Vehicle?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM Pojazd WHERE ID = @Id";
        return await _connection.QueryFirstOrDefaultAsync<Vehicle>(sql, new { Id = id });
    }

    /// <summary>
    /// Pobiera wszystkie pojazdy
    /// </summary>
    /// <returns>Kolekcja wszystkich pojazdów</returns>
    public async Task<IEnumerable<Vehicle>> GetAllAsync()
    {
        const string sql = "SELECT * FROM Pojazd";
        return await _connection.QueryAsync<Vehicle>(sql);
    }

    /// <summary>
    /// Tworzy nowy pojazd
    /// </summary>
    /// <param name="vehicle">Obiekt pojazdu do utworzenia</param>
    /// <returns>Identyfikator utworzonego pojazdu</returns>
    public async Task<int> CreateAsync(Vehicle vehicle)
    {
        const string sql = @"
            INSERT INTO Pojazd (Marka, Model, RokProdukcji, VIN, TypSilnika, KolorNadwozia, WyposazenieWnetrza, Akcesoria)
            VALUES (@Marka, @Model, @RokProdukcji, @VIN, @TypSilnika, @KolorNadwozia, @WyposazenieWnetrza, @Akcesoria)
            RETURNING ID";
        
        return await _connection.ExecuteScalarAsync<int>(sql, vehicle);
    }

    /// <summary>
    /// Aktualizuje dane pojazdu
    /// </summary>
    /// <param name="vehicle">Obiekt pojazdu z zaktualizowanymi danymi</param>
    public async Task UpdateAsync(Vehicle vehicle)
    {
        const string sql = @"
            UPDATE Pojazd 
            SET Marka = @Marka,
                Model = @Model,
                RokProdukcji = @RokProdukcji,
                VIN = @VIN,
                TypSilnika = @TypSilnika,
                KolorNadwozia = @KolorNadwozia,
                WyposazenieWnetrza = @WyposazenieWnetrza,
                Akcesoria = @Akcesoria
            WHERE ID = @ID";
        
        await _connection.ExecuteAsync(sql, vehicle);
    }

    /// <summary>
    /// Usuwa pojazd na podstawie identyfikatora
    /// </summary>
    /// <param name="id">Identyfikator pojazdu do usunięcia</param>
    public async Task DeleteAsync(int id)
    {
        const string sql = "DELETE FROM Pojazd WHERE ID = @Id";
        await _connection.ExecuteAsync(sql, new { Id = id });
    }

    /// <summary>
    /// Sprawdza, czy pojazd o podanym identyfikatorze istnieje
    /// </summary>
    /// <param name="id">Identyfikator pojazdu</param>
    /// <returns>True, jeśli pojazd istnieje; w przeciwnym razie false</returns>
    public async Task<bool> ExistsAsync(int id)
    {
        const string sql = "SELECT COUNT(1) FROM Pojazd WHERE ID = @Id";
        return await _connection.ExecuteScalarAsync<bool>(sql, new { Id = id });
    }
} 