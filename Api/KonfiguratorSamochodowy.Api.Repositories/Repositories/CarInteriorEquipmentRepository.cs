using System.Data;
using Dapper;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories;

public class CarInteriorEquipmentRepository : ICarInteriorEquipmentRepository
{
    private readonly IDbConnection _connection;

    public CarInteriorEquipmentRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<Result<IEnumerable<CarInteriorEquipment>>> GetAllAsync()
    {
        try
        {
            const string sql = @"
                SELECT
                    id AS Id,
                    '' AS CarId,
                    '' AS CarModel,
                    type AS Type,
                    value AS Value,
                    additional_price AS AdditionalPrice,
                    description AS Description,
                    FALSE AS IsDefault,
                    '' AS ColorCode,
                    NULL AS Intensity,
                    has_navigation AS HasNavigation,
                    has_premium_sound AS HasPremiumSound,
                    control_type AS ControlType
                FROM car_interior_equipment
                ORDER BY type, value";

            var equipment = await _connection.QueryAsync<CarInteriorEquipment>(sql);
            return Result<IEnumerable<CarInteriorEquipment>>.Success(equipment);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CarInteriorEquipment>>.Failure(new Error("DatabaseError", ex.Message));
        }
    }

    public async Task<Result<CarInteriorEquipment>> GetByIdAsync(string id)
    {
        try
        {
            const string sql = @"
                SELECT
                    id AS Id,
                    '' AS CarId,
                    '' AS CarModel,
                    type AS Type,
                    value AS Value,
                    additional_price AS AdditionalPrice,
                    description AS Description,
                    FALSE AS IsDefault,
                    '' AS ColorCode,
                    NULL AS Intensity,
                    has_navigation AS HasNavigation,
                    has_premium_sound AS HasPremiumSound,
                    control_type AS ControlType
                FROM car_interior_equipment
                WHERE id = @Id";

            var equipment = await _connection.QueryFirstOrDefaultAsync<CarInteriorEquipment>(sql, new { Id = id });
            
            if (equipment == null)
                return Result<CarInteriorEquipment>.Failure(new Error("NotFound", $"Wyposażenie wnętrza o ID {id} nie zostało znalezione"));
                
            return Result<CarInteriorEquipment>.Success(equipment);
        }
        catch (Exception ex)
        {
            return Result<CarInteriorEquipment>.Failure(new Error("DatabaseError", ex.Message));
        }
    }

    public async Task<Result<IEnumerable<CarInteriorEquipment>>> GetByCarIdAsync(string carId)
    {
        // Tabela nie ma pola car_id, więc zwróć wszystkie
        return await GetAllAsync();
    }

    public async Task<Result<IEnumerable<CarInteriorEquipment>>> GetByCarModelAsync(string carModel)
    {
        // Tabela nie ma pola car_model, więc zwróć wszystkie
        return await GetAllAsync();
    }

    public async Task<Result<IEnumerable<CarInteriorEquipment>>> GetByTypeAsync(string type)
    {
        try
        {
            const string sql = @"
                SELECT
                    id AS Id,
                    '' AS CarId,
                    '' AS CarModel,
                    type AS Type,
                    value AS Value,
                    additional_price AS AdditionalPrice,
                    description AS Description,
                    FALSE AS IsDefault,
                    '' AS ColorCode,
                    NULL AS Intensity,
                    has_navigation AS HasNavigation,
                    has_premium_sound AS HasPremiumSound,
                    control_type AS ControlType
                FROM car_interior_equipment
                WHERE type = @Type
                ORDER BY value";

            var equipment = await _connection.QueryAsync<CarInteriorEquipment>(sql, new { Type = type });
            return Result<IEnumerable<CarInteriorEquipment>>.Success(equipment);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CarInteriorEquipment>>.Failure(new Error("DatabaseError", ex.Message));
        }
    }

    public async Task<Result<IEnumerable<CarInteriorEquipment>>> GetFilteredAsync(string? carId = null, string? carModel = null, string? type = null, bool? isDefault = null, decimal? maxPrice = null)
    {
        try
        {
            var conditions = new List<string>();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(type))
            {
                conditions.Add("type = @Type");
                parameters.Add("@Type", type);
            }

            if (maxPrice.HasValue)
            {
                conditions.Add("additional_price <= @MaxPrice");
                parameters.Add("@MaxPrice", maxPrice.Value);
            }

            var whereClause = conditions.Count > 0 ? "WHERE " + string.Join(" AND ", conditions) : string.Empty;
            
            var sql = $@"
                SELECT
                    id AS Id,
                    '' AS CarId,
                    '' AS CarModel,
                    type AS Type,
                    value AS Value,
                    additional_price AS AdditionalPrice,
                    description AS Description,
                    FALSE AS IsDefault,
                    '' AS ColorCode,
                    NULL AS Intensity,
                    has_navigation AS HasNavigation,
                    has_premium_sound AS HasPremiumSound,
                    control_type AS ControlType
                FROM car_interior_equipment
                {whereClause}
                ORDER BY type, value";

            var equipment = await _connection.QueryAsync<CarInteriorEquipment>(sql, parameters);
            return Result<IEnumerable<CarInteriorEquipment>>.Success(equipment);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CarInteriorEquipment>>.Failure(new Error("DatabaseError", ex.Message));
        }
    }

    public async Task<Result<CarInteriorEquipment>> CreateAsync(CarInteriorEquipment equipment)
    {
        try
        {
            equipment.Id = Guid.NewGuid().ToString();
            
            const string sql = @"
                INSERT INTO car_interior_equipment (id, type, value, description, additional_price, has_navigation, has_premium_sound, control_type)
                VALUES (@Id, @Type, @Value, @Description, @AdditionalPrice, @HasNavigation, @HasPremiumSound, @ControlType)";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", equipment.Id);
            parameters.Add("@Type", equipment.Type);
            parameters.Add("@Value", equipment.Value);
            parameters.Add("@Description", equipment.Description);
            parameters.Add("@AdditionalPrice", equipment.AdditionalPrice);
            parameters.Add("@HasNavigation", equipment.HasNavigation ?? false);
            parameters.Add("@HasPremiumSound", equipment.HasPremiumSound ?? false);
            parameters.Add("@ControlType", equipment.ControlType);

            await _connection.ExecuteAsync(sql, parameters);
            return Result<CarInteriorEquipment>.Success(equipment);
        }
        catch (Exception ex)
        {
            return Result<CarInteriorEquipment>.Failure(new Error("DatabaseError", ex.Message));
        }
    }

    public async Task<Result<CarInteriorEquipment>> UpdateAsync(string id, CarInteriorEquipment equipment)
    {
        try
        {
            const string sql = @"
                UPDATE car_interior_equipment
                SET type = @Type,
                    value = @Value,
                    description = @Description,
                    additional_price = @AdditionalPrice,
                    has_navigation = @HasNavigation,
                    has_premium_sound = @HasPremiumSound,
                    control_type = @ControlType
                WHERE id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@Type", equipment.Type);
            parameters.Add("@Value", equipment.Value);
            parameters.Add("@Description", equipment.Description);
            parameters.Add("@AdditionalPrice", equipment.AdditionalPrice);
            parameters.Add("@HasNavigation", equipment.HasNavigation ?? false);
            parameters.Add("@HasPremiumSound", equipment.HasPremiumSound ?? false);
            parameters.Add("@ControlType", equipment.ControlType);

            var rowsAffected = await _connection.ExecuteAsync(sql, parameters);
            if (rowsAffected == 0)
                return Result<CarInteriorEquipment>.Failure(new Error("NotFound", $"Wyposażenie wnętrza o ID {id} nie zostało znalezione"));

            equipment.Id = id;
            return Result<CarInteriorEquipment>.Success(equipment);
        }
        catch (Exception ex)
        {
            return Result<CarInteriorEquipment>.Failure(new Error("DatabaseError", ex.Message));
        }
    }

    public async Task<Result<bool>> DeleteAsync(string id)
    {
        try
        {
            const string sql = "DELETE FROM car_interior_equipment WHERE id = @Id";
            var rowsAffected = await _connection.ExecuteAsync(sql, new { Id = id });

            if (rowsAffected == 0)
                return Result<bool>.Failure(new Error("NotFound", $"Wyposażenie wnętrza o ID {id} nie zostało znalezione"));

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(new Error("DatabaseError", ex.Message));
        }
    }
}