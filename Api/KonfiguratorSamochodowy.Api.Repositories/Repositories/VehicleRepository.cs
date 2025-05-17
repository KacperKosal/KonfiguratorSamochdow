using Dapper;
using KonfiguratorSamochodowy.Api.Repositories.Enums;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using KonfiguratorSamochodowy.Api.Repositories.Options;
using System.Data;
using System.Text;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories;

/// <summary>
/// Implementacja repozytorium pojazdów
/// </summary>
internal sealed class VehicleRepository(IDbConnection connection, IVechicleFeaturesRepository vechicleFeaturesRepository, IEngineRepository engineRepository) : IVehicleRepository
{
    /// <summary>
    /// Pobiera listę pojazdów na podstawie parametrów wyszukiwania
    /// </summary>
    /// <param name="sortingOptions">Parametry filtrowania i sortowania</param>
    /// <returns>Lista pojazdów spełniających kryteria</returns>
    public async Task<IEnumerable<Vehicle>> GetByFiltersAsync(SortingOptions sortingOptions)
    {
        var sql = new StringBuilder();
        var parameters = new DynamicParameters();

        sql.Append("SELECT * FROM Pojazd WHERE 1 = 1");

        // Filtrowanie po nazwie modelu (ILIKE — case-insensitive, PostgreSQL)
        if (!string.IsNullOrWhiteSpace(sortingOptions.ModelName))
        {
            sql.Append(" AND Model ILIKE @ModelName");
            parameters.Add("ModelName", $"%{sortingOptions.ModelName}%");
        }

        // Filtrowanie po przedziale cenowym
        if (sortingOptions.MinPrice.HasValue)
        {
            sql.Append(" AND Cena >= @MinPrice");
            parameters.Add("MinPrice", sortingOptions.MinPrice.Value);
        }
        if (sortingOptions.MaxPrice.HasValue)
        {
            sql.Append(" AND Cena <= @MaxPrice");
            parameters.Add("MaxPrice", sortingOptions.MaxPrice.Value);
        }

        // Filtrowanie, czy ma napęd 4x4
        if (sortingOptions.Has4x4.HasValue)
        {
            sql.Append(" AND Ma4x4 = @Has4x4");
            parameters.Add("Has4x4", sortingOptions.Has4x4.Value);
        }

        // Filtrowanie, czy jest elektryczny
        if (sortingOptions.IsElectrick.HasValue)
        {
            sql.Append(" AND JestElektryczny = @IsElectric");
            parameters.Add("IsElectric", sortingOptions.IsElectrick.Value);
        }

        // Dodanie sortowania
        if (sortingOptions.SortingOption.HasValue)
        {
            sql.Append(" ORDER BY ");
            switch (sortingOptions.SortingOption.Value)
            {
                case SortingOption.PriceAscending:
                    sql.Append("Cena ASC");
                    break;
                case SortingOption.PriceDescending:
                    sql.Append("Cena DESC");
                    break;
                case SortingOption.NameAscending:
                    sql.Append("Model ASC");
                    break;
                default:
                    sql.Append("ID"); // domyślnie po kluczu
                    break;
            }
        }

        var result = await connection.QueryAsync<Vehicle>(sql.ToString(), parameters);

        foreach (var vehicle in result)
        {
            vehicle.Cechy = await vechicleFeaturesRepository.GetAllByVechicleIdAsync(vehicle.Id);
            vehicle.Silniki = await engineRepository.GetAllByVechicleIdAsync(vehicle.Id);
        }

        return result;
    }


    /// <summary>
    /// Pobiera pojazd na podstawie identyfikatora
    /// </summary>
    /// <param name="id">Identyfikator pojazdu</param>
    /// <returns>Obiekt pojazdu lub null, jeśli nie znaleziono</returns>
    public async Task<Vehicle?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM Pojazd WHERE ID = @Id";
        return await connection.QueryFirstOrDefaultAsync<Vehicle>(sql, new { Id = id });
    }

    /// <summary>
    /// Pobiera wszystkie pojazdy
    /// </summary>
    /// <returns>Kolekcja wszystkich pojazdów</returns>
    public async Task<IEnumerable<Vehicle>> GetAllAsync()
    {
        const string sql = "SELECT * FROM Pojazd";
        return await connection.QueryAsync<Vehicle>(sql);
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
        
        return await connection.ExecuteScalarAsync<int>(sql, vehicle);
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
        
        await connection.ExecuteAsync(sql, vehicle);
    }

    /// <summary>
    /// Usuwa pojazd na podstawie identyfikatora
    /// </summary>
    /// <param name="id">Identyfikator pojazdu do usunięcia</param>
    public async Task DeleteAsync(int id)
    {
        const string sql = "DELETE FROM Pojazd WHERE ID = @Id";
        await connection.ExecuteAsync(sql, new { Id = id });
    }

    /// <summary>
    /// Sprawdza, czy pojazd o podanym identyfikatorze istnieje
    /// </summary>
    /// <param name="id">Identyfikator pojazdu</param>
    /// <returns>True, jeśli pojazd istnieje; w przeciwnym razie false</returns>
    public async Task<bool> ExistsAsync(int id)
    {
        const string sql = "SELECT COUNT(1) FROM Pojazd WHERE ID = @Id";
        return await connection.ExecuteScalarAsync<bool>(sql, new { Id = id });
    }

    public async Task<byte[]> GetImageByIdAsync(int id)
    {
        return await connection.QueryFirstOrDefaultAsync<byte[]>("SELECT Zdjecie FROM Pojazd WHERE ID = @Id", new { Id = id }) ?? [];
    }
} 