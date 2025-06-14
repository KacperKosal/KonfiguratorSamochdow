using System.Data;
using Dapper;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories;

public class SqlCarInteriorEquipmentRepository : ICarInteriorEquipmentRepository
{
    private readonly IDbConnection _connection;

    public SqlCarInteriorEquipmentRepository(IDbConnection connection)
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
            return equipment != null
                ? Result<CarInteriorEquipment>.Success(equipment)
                : Result<CarInteriorEquipment>.Failure(new Error("NotFound", $"Wyposażenie wnętrza o ID {id} nie zostało znalezione"));
        }
        catch (Exception ex)
        {
            return Result<CarInteriorEquipment>.Failure(new Error("DatabaseError", ex.Message));
        }
    }

    public async Task<Result<IEnumerable<CarInteriorEquipment>>> GetByCarIdAsync(string carId)
    {
        try
        {
            const string sql = @"
                SELECT
                    cie.id AS Id,
                    p.id AS CarId,
                    p.model AS CarModel,
                    cie.type AS Type,
                    cie.value AS Value,
                    cie.additional_price AS AdditionalPrice,
                    cie.description AS Description,
                    FALSE AS IsDefault,
                    '' AS ColorCode,
                    NULL AS Intensity,
                    cie.has_navigation AS HasNavigation,
                    cie.has_premium_sound AS HasPremiumSound,
                    cie.control_type AS ControlType
                FROM car_interior_equipment cie
                JOIN pojazd p ON cie.id = ANY(STRING_TO_ARRAY(p.wyposazeniewnetrza, ',')) -- Assuming comma-separated IDs
                WHERE p.id = CAST(@CarId AS INTEGER)
                ORDER BY cie.type, cie.value";

            var equipment = await _connection.QueryAsync<CarInteriorEquipment>(sql, new { CarId = carId });
            return Result<IEnumerable<CarInteriorEquipment>>.Success(equipment);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CarInteriorEquipment>>.Failure(new Error("DatabaseError", ex.Message));
        }
    }

    public async Task<Result<IEnumerable<CarInteriorEquipment>>> GetByCarModelAsync(string carModel)
    {
        try
        {
            const string sql = @"
                SELECT
                    cie.id AS Id,
                    p.id AS CarId,
                    p.model AS CarModel,
                    cie.type AS Type,
                    cie.value AS Value,
                    cie.additional_price AS AdditionalPrice,
                    cie.description AS Description,
                    FALSE AS IsDefault,
                    '' AS ColorCode,
                    NULL AS Intensity,
                    cie.has_navigation AS HasNavigation,
                    cie.has_premium_sound AS HasPremiumSound,
                    cie.control_type AS ControlType
                FROM car_interior_equipment cie
                JOIN pojazd p ON cie.id = ANY(STRING_TO_ARRAY(p.wyposazeniewnetrza, ',')) -- Assuming comma-separated IDs
                WHERE p.model = @CarModel
                ORDER BY cie.type, cie.value";

            var equipment = await _connection.QueryAsync<CarInteriorEquipment>(sql, new { CarModel = carModel });
            return Result<IEnumerable<CarInteriorEquipment>>.Success(equipment);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CarInteriorEquipment>>.Failure(new Error("DatabaseError", ex.Message));
        }
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

            string sql = @"
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
                FROM car_interior_equipment";

            if (!string.IsNullOrEmpty(carId))
            {
                conditions.Add("id IN (SELECT UNNEST(STRING_TO_ARRAY(wyposazeniewnetrza, ',')) FROM pojazd WHERE id = CAST(@CarId AS INTEGER))");
                parameters.Add("@CarId", carId);
            }

            if (!string.IsNullOrEmpty(carModel))
            {
                conditions.Add("id IN (SELECT UNNEST(STRING_TO_ARRAY(wyposazeniewnetrza, ',')) FROM pojazd WHERE model = @CarModel)");
                parameters.Add("@CarModel", carModel);
            }

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

            if (conditions.Any())
            {
                sql += " WHERE " + string.Join(" AND ", conditions);
            }

            sql += " ORDER BY type, value";

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
            const string sql = @"
                INSERT INTO car_interior_equipment (
                    id, type, value, description, additional_price, has_navigation, has_premium_sound, control_type
                )
                VALUES (
                    @Id, @Type, @Value, @Description, @AdditionalPrice, @HasNavigation, @HasPremiumSound, @ControlType
                )";

            var rowsAffected = await _connection.ExecuteAsync(sql, equipment);
            if (rowsAffected == 0)
            {
                return Result<CarInteriorEquipment>.Failure(new Error("CreateFailed", "Nie udało się utworzyć wyposażenia wnętrza."));
            }
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
                SET
                    type = @Type,
                    value = @Value,
                    description = @Description,
                    additional_price = @AdditionalPrice,
                    has_navigation = @HasNavigation,
                    has_premium_sound = @HasPremiumSound,
                    control_type = @ControlType
                WHERE id = @Id";

            var rowsAffected = await _connection.ExecuteAsync(sql, equipment);
            if (rowsAffected == 0)
            {
                return Result<CarInteriorEquipment>.Failure(new Error("UpdateFailed", $"Nie znaleziono wyposażenia wnętrza o ID {id}."));
            }
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
            return Result<bool>.Success(rowsAffected > 0);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(new Error("DatabaseError", ex.Message));
        }
    }
}