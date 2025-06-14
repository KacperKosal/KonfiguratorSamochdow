using System.Data;
using Dapper;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using System.Text;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories;

public sealed class SqlCarAccessoryRepository : ICarAccessoryRepository
{
    private readonly IDbConnection _connection;

    public SqlCarAccessoryRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    // Metoda mapująca wynik zapytania na obiekt CarAccessory
    private CarAccessory MapToCarAccessory(IDataRecord reader)
    {
        return new CarAccessory
        {
            Id = reader["id"].ToString(),
            CarId = reader["car_id"].ToString(),
            CarModel = reader["car_model"].ToString(),
            Category = reader["category"].ToString(),
            Type = reader["type"].ToString(),
            Name = reader["name"].ToString(),
            Description = reader["description"].ToString(),
            Price = Convert.ToDecimal(reader["price"]),
            Manufacturer = reader["manufacturer"].ToString(),
            PartNumber = reader["part_number"].ToString(),
            IsOriginalBMWPart = Convert.ToBoolean(reader["is_original_bmw_part"]),
            IsInStock = Convert.ToBoolean(reader["is_in_stock"]),
            StockQuantity = Convert.ToInt32(reader["stock_quantity"]),
            ImageUrl = reader["image_url"].ToString(),
            Size = reader["size"].ToString(),
            Pattern = reader["pattern"].ToString(),
            Color = reader["color"].ToString(),
            Material = reader["material"].ToString(),
            Capacity = reader["capacity"] != DBNull.Value ? Convert.ToInt32(reader["capacity"]) : 0,
            Compatibility = reader["compatibility"].ToString(),
            AgeGroup = reader["age_group"].ToString(),
            MaxLoad = reader["max_load"] != DBNull.Value ? Convert.ToInt32(reader["max_load"]) : 0,
            IsUniversal = Convert.ToBoolean(reader["is_universal"]),
            InstallationDifficulty = reader["installation_difficulty"].ToString(),
            Warranty = reader["warranty"].ToString()
        };
    }

    public async Task<Result<IEnumerable<CarAccessory>>> GetAllAsync()
    {
        var accessories = new List<CarAccessory>();

        try
        {
            var query = @"SELECT * FROM car_accessories";

            using (var reader = await _connection.ExecuteReaderAsync(query))
            {
                while (reader.Read())
                {
                    accessories.Add(MapToCarAccessory(reader));
                }
            }

            return Result<IEnumerable<CarAccessory>>.Success(accessories);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CarAccessory>>.Failure(new Error("error", $"Błąd pobierania akcesoriów: {ex.Message}"));
        }
    }

    public async Task<Result<CarAccessory>> GetByIdAsync(string id)
    {
        try
        {
            var query = @"SELECT * FROM car_accessories WHERE id = @id";

            var accessory = await _connection.QueryFirstOrDefaultAsync<CarAccessory>(query, new { id });
            return accessory != null
                ? Result<CarAccessory>.Success(accessory)
                : Result<CarAccessory>.Failure(new Error("error", $"Nie znaleziono akcesorium o ID {id}."));
        }
        catch (Exception ex)
        {
            return Result<CarAccessory>.Failure(new Error("error", $"Błąd pobierania akcesorium: {ex.Message}"));
        }
    }

    public async Task<Result<IEnumerable<CarAccessory>>> GetByCarIdAsync(string carId)
    {
        var accessories = new List<CarAccessory>();

        try
        {
            var query = @"SELECT * FROM car_accessories WHERE car_id = @carId";

            var result = await _connection.QueryAsync<CarAccessory>(query, new { carId });
            accessories.AddRange(result);

            return Result<IEnumerable<CarAccessory>>.Success(accessories);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CarAccessory>>.Failure(new Error("error", $"Błąd pobierania akcesoriów dla samochodu: {ex.Message}"));
        }
    }

    public async Task<Result<IEnumerable<CarAccessory>>> GetByCarModelAsync(string carModel)
    {
        var accessories = new List<CarAccessory>();

        try
        {
            var query = @"SELECT * FROM car_accessories WHERE car_model = @carModel";

            var result = await _connection.QueryAsync<CarAccessory>(query, new { carModel });
            accessories.AddRange(result);

            return Result<IEnumerable<CarAccessory>>.Success(accessories);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CarAccessory>>.Failure(new Error("error", $"Błąd pobierania akcesoriów dla modelu: {ex.Message}"));
        }
    }

    public async Task<Result<IEnumerable<CarAccessory>>> GetByCategoryAsync(string category)
    {
        var accessories = new List<CarAccessory>();

        try
        {
            var query = @"SELECT * FROM car_accessories WHERE category = @category";

            var result = await _connection.QueryAsync<CarAccessory>(query, new { category });
            accessories.AddRange(result);

            return Result<IEnumerable<CarAccessory>>.Success(accessories);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CarAccessory>>.Failure(new Error("error", $"Błąd pobierania akcesoriów według kategorii: {ex.Message}"));
        }
    }

    public async Task<Result<IEnumerable<CarAccessory>>> GetByTypeAsync(string type)
    {
        var accessories = new List<CarAccessory>();

        try
        {
            var query = @"SELECT * FROM car_accessories WHERE type = @type";

            var result = await _connection.QueryAsync<CarAccessory>(query, new { type });
            accessories.AddRange(result);

            return Result<IEnumerable<CarAccessory>>.Success(accessories);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CarAccessory>>.Failure(new Error("error", $"Błąd pobierania akcesoriów według typu: {ex.Message}"));
        }
    }

    public async Task<Result<IEnumerable<CarAccessory>>> GetFilteredAsync(
        string? carId = null,
        string? carModel = null,
        string? category = null,
        string? type = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        bool? isOriginalBMWPart = null,
        bool? isInStock = null,
        string? installationDifficulty = null)
    {
        var accessories = new List<CarAccessory>();

        try
        {
            var sqlBuilder = new StringBuilder("SELECT * FROM car_accessories WHERE 1=1");
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(carId))
            {
                sqlBuilder.Append(" AND car_id = @CarId");
                parameters.Add("@CarId", carId);
            }

            if (!string.IsNullOrEmpty(carModel))
            {
                sqlBuilder.Append(" AND car_model = @CarModel");
                parameters.Add("@CarModel", carModel);
            }

            if (!string.IsNullOrEmpty(category))
            {
                sqlBuilder.Append(" AND category = @Category");
                parameters.Add("@Category", category);
            }

            if (!string.IsNullOrEmpty(type))
            {
                sqlBuilder.Append(" AND type = @Type");
                parameters.Add("@Type", type);
            }

            if (minPrice.HasValue)
            {
                sqlBuilder.Append(" AND price >= @MinPrice");
                parameters.Add("@MinPrice", minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                sqlBuilder.Append(" AND price <= @MaxPrice");
                parameters.Add("@MaxPrice", maxPrice.Value);
            }

            if (isOriginalBMWPart.HasValue)
            {
                sqlBuilder.Append(" AND is_original_bmw_part = @IsOriginalBMWPart");
                parameters.Add("@IsOriginalBMWPart", isOriginalBMWPart.Value);
            }

            if (isInStock.HasValue)
            {
                sqlBuilder.Append(" AND is_in_stock = @IsInStock");
                parameters.Add("@IsInStock", isInStock.Value);
            }

            if (!string.IsNullOrEmpty(installationDifficulty))
            {
                sqlBuilder.Append(" AND installation_difficulty = @InstallationDifficulty");
                parameters.Add("@InstallationDifficulty", installationDifficulty);
            }

            sqlBuilder.Append(" ORDER BY name");

            var result = await _connection.QueryAsync<CarAccessory>(sqlBuilder.ToString(), parameters);
            accessories.AddRange(result);

            return Result<IEnumerable<CarAccessory>>.Success(accessories);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CarAccessory>>.Failure(new Error("error", $"Błąd filtrowania akcesoriów: {ex.Message}"));
        }
    }

    public async Task<Result<CarAccessory>> CreateAsync(CarAccessory accessory)
    {
        try
        {
            var query = @"
                INSERT INTO car_accessories (
                    id, car_id, car_model, category, type, name, description, price, manufacturer,
                    part_number, is_original_bmw_part, is_in_stock, stock_quantity, image_url,
                    size, pattern, color, material, capacity, compatibility, age_group, max_load,
                    is_universal, installation_difficulty, warranty
                )
                VALUES (
                    @Id, @CarId, @CarModel, @Category, @Type, @Name, @Description, @Price, @Manufacturer,
                    @PartNumber, @IsOriginalBMWPart, @IsInStock, @StockQuantity, @ImageUrl,
                    @Size, @Pattern, @Color, @Material, @Capacity, @Compatibility, @AgeGroup, @MaxLoad,
                    @IsUniversal, @InstallationDifficulty, @Warranty
                )";

            var rowsAffected = await _connection.ExecuteAsync(query, accessory);

            if (rowsAffected == 0)
            {
                return Result<CarAccessory>.Failure(new Error("error", "Nie udało się utworzyć akcesorium."));
            }

            return Result<CarAccessory>.Success(accessory);
        }
        catch (Exception ex)
        {
            return Result<CarAccessory>.Failure(new Error("error", $"Błąd tworzenia akcesorium: {ex.Message}"));
        }
    }

    public async Task<Result<CarAccessory>> UpdateAsync(string id, CarAccessory accessory)
    {
        try
        {
            var query = @"
                UPDATE car_accessories
                SET
                    car_id = @CarId,
                    car_model = @CarModel,
                    category = @Category,
                    type = @Type,
                    name = @Name,
                    description = @Description,
                    price = @Price,
                    manufacturer = @Manufacturer,
                    part_number = @PartNumber,
                    is_original_bmw_part = @IsOriginalBMWPart,
                    is_in_stock = @IsInStock,
                    stock_quantity = @StockQuantity,
                    image_url = @ImageUrl,
                    size = @Size,
                    pattern = @Pattern,
                    color = @Color,
                    material = @Material,
                    capacity = @Capacity,
                    compatibility = @Compatibility,
                    age_group = @AgeGroup,
                    max_load = @MaxLoad,
                    is_universal = @IsUniversal,
                    installation_difficulty = @InstallationDifficulty,
                    warranty = @Warranty
                WHERE id = @Id";

            var rowsAffected = await _connection.ExecuteAsync(query, accessory);

            if (rowsAffected == 0)
            {
                return Result<CarAccessory>.Failure(new Error("error", $"Nie znaleziono akcesorium o ID {id} do aktualizacji."));
            }

            return Result<CarAccessory>.Success(accessory);
        }
        catch (Exception ex)
        {
            return Result<CarAccessory>.Failure(new Error("error", $"Błąd aktualizacji akcesorium: {ex.Message}"));
        }
    }

    public async Task<Result<bool>> DeleteAsync(string id)
    {
        try
        {
            var query = @"DELETE FROM car_accessories WHERE id = @id";

            var rowsAffected = await _connection.ExecuteAsync(query, new { id });

            return Result<bool>.Success(rowsAffected > 0);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(new Error("error", $"Błąd usuwania akcesorium: {ex.Message}"));
        }
    }

    public async Task<Result<bool>> IsPartNumberUniqueAsync(string partNumber, string? excludeId = null)
    {
        try
        {
            var sqlBuilder = new StringBuilder("SELECT COUNT(*) FROM car_accessories WHERE part_number = @PartNumber");
            var parameters = new DynamicParameters();
            parameters.Add("@PartNumber", partNumber);

            if (!string.IsNullOrEmpty(excludeId))
            {
                sqlBuilder.Append(" AND id != @ExcludeId");
                parameters.Add("@ExcludeId", excludeId);
            }

            var count = await _connection.ExecuteScalarAsync<int>(sqlBuilder.ToString(), parameters);

            return Result<bool>.Success(count == 0);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(new Error("error", $"Błąd sprawdzania unikalności numeru części: {ex.Message}"));
        }
    }
}