using System.Data;
using Dapper;
using KonfiguratorSamochodowy.Api.Repositories.Dto;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories;

internal class EngineRepository : IEngineRepository
{
    private readonly IDbConnection _connection;

    public EngineRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<Result<IEnumerable<Engine>>> GetAllAsync()
    {
        try
        {
            const string sql = "SELECT * FROM Silnik";
            var engines = await _connection.QueryAsync<Engine>(sql);
            return Result<IEnumerable<Engine>>.Success(engines);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Engine>>.Failure(new Error("DatabaseError", ex.Message));
        }
    }

    public async Task<Result<Engine>> GetByIdAsync(string id)
    {
        try
        {
            const string sql = "SELECT * FROM Silnik WHERE ID = @Id";
            var engine = await _connection.QueryFirstOrDefaultAsync<Engine>(sql, new { Id = id });
            
            if (engine == null)
                return Result<Engine>.Failure(new Error("NotFound", $"Silnik o ID {id} nie został znaleziony"));
                
            return Result<Engine>.Success(engine);
        }
        catch (Exception ex)
        {
            return Result<Engine>.Failure(new Error("DatabaseError", ex.Message));
        }
    }

    public async Task<Result<IEnumerable<Engine>>> GetFilteredAsync(FilterEnginesRequestDto filter)
    {
        try
        {
            var conditions = new List<string>();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(filter.Type))
            {
                conditions.Add("Typ = @Type");
                parameters.Add("@Type", filter.Type);
            }

            if (!string.IsNullOrEmpty(filter.FuelType))
            {
                conditions.Add("RodzajPaliwa LIKE @FuelType");
                parameters.Add("@FuelType", $"%{filter.FuelType}%");
            }

            if (filter.MinCapacity.HasValue)
            {
                conditions.Add("Pojemnosc >= @MinCapacity");
                parameters.Add("@MinCapacity", filter.MinCapacity.Value);
            }

            if (filter.MaxCapacity.HasValue)
            {
                conditions.Add("Pojemnosc <= @MaxCapacity");
                parameters.Add("@MaxCapacity", filter.MaxCapacity.Value);
            }

            if (filter.MinPower.HasValue)
            {
                conditions.Add("Moc >= @MinPower");
                parameters.Add("@MinPower", filter.MinPower.Value);
            }

            if (filter.MaxPower.HasValue)
            {
                conditions.Add("Moc <= @MaxPower");
                parameters.Add("@MaxPower", filter.MaxPower.Value);
            }

            if (!string.IsNullOrEmpty(filter.Transmission))
            {
                conditions.Add("Skrzynia = @Transmission");
                parameters.Add("@Transmission", filter.Transmission);
            }

            if (!string.IsNullOrEmpty(filter.DriveType))
            {
                conditions.Add("NapedzType = @DriveType");
                parameters.Add("@DriveType", filter.DriveType);
            }

            if (filter.IsActive.HasValue)
            {
                conditions.Add("JestAktywny = @IsActive");
                parameters.Add("@IsActive", filter.IsActive.Value);
            }

            var whereClause = conditions.Count > 0 ? "WHERE " + string.Join(" AND ", conditions) : string.Empty;
            var sql = $"SELECT * FROM Silnik {whereClause}";

            var engines = await _connection.QueryAsync<Engine>(sql, parameters);
            return Result<IEnumerable<Engine>>.Success(engines);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Engine>>.Failure(new Error("DatabaseError", ex.Message));
        }
    }

    public async Task<Result<Engine>> CreateAsync(Engine engine)
    {
        try
        {
            const string sql = @"
                INSERT INTO Silnik (Pojemnosc, Typ, Moc, RodzajPaliwa, Skrzynia, NapedzType, JestAktywny)
                VALUES (@Pojemnosc, @Typ, @Moc, @RodzajPaliwa, @Skrzynia, @NapedzType, @JestAktywny);
                SELECT LAST_INSERT_ID();
            ";

            var id = await _connection.ExecuteScalarAsync<int>(sql, engine);
            engine.ID = id;
            return Result<Engine>.Success(engine);
        }
        catch (Exception ex)
        {
            return Result<Engine>.Failure(new Error("DatabaseError", ex.Message));
        }
    }

    public async Task<Result<Engine>> UpdateAsync(string id, Engine engine)
    {
        try
        {
            const string sql = @"
                UPDATE Silnik
                SET Pojemnosc = @Pojemnosc,
                    Typ = @Typ,
                    Moc = @Moc,
                    RodzajPaliwa = @RodzajPaliwa,
                    Skrzynia = @Skrzynia,
                    NapedzType = @NapedzType,
                    JestAktywny = @JestAktywny
                WHERE ID = @Id
            ";

            var parameters = new DynamicParameters(engine);
            parameters.Add("@Id", id);

            var rowsAffected = await _connection.ExecuteAsync(sql, parameters);
            if (rowsAffected == 0)
                return Result<Engine>.Failure(new Error("NotFound", $"Silnik o ID {id} nie został znaleziony"));

            engine.ID = int.Parse(id);
            return Result<Engine>.Success(engine);
        }
        catch (Exception ex)
        {
            return Result<Engine>.Failure(new Error("DatabaseError", ex.Message));
        }
    }

    public async Task<Result<bool>> DeleteAsync(string id)
    {
        try
        {
            const string sql = "DELETE FROM Silnik WHERE ID = @Id";
            var rowsAffected = await _connection.ExecuteAsync(sql, new { Id = id });

            if (rowsAffected == 0)
                return Result<bool>.Failure(new Error("NotFound", $"Silnik o ID {id} nie został znaleziony"));

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(new Error("DatabaseError", ex.Message));
        }
    }

    public async Task<IEnumerable<Engine>> GetAllByVechicleIdAsync(int vehicleId)
    {
        const string sql = "SELECT * FROM Silnik WHERE PojazdID = @VehicleId";
        return await _connection.QueryAsync<Engine>(sql, new { VehicleId = vehicleId });
    }
}
