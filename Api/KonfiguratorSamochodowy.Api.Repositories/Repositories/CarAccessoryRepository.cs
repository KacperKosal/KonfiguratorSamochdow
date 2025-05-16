using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Text;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories
{
    public class CarAccessoryRepository(IConfiguration configuration) : ICarAccessoryRepository
    {
        private readonly string _connectionString = configuration.GetConnectionString("Psql") ?? string.Empty;

        // Metoda mapująca wynik zapytania na obiekt CarAccessory
        private CarAccessory MapToCarAccessory(NpgsqlDataReader reader)
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
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"SELECT * FROM car_accessories";

                    using (var command = new NpgsqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            accessories.Add(MapToCarAccessory(reader));
                        }
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
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"SELECT * FROM car_accessories WHERE id = @id";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return Result<CarAccessory>.Success(MapToCarAccessory(reader));
                            }
                            else
                            {
                                return Result<CarAccessory>.Failure(new Error("error", $"Nie znaleziono akcesorium o ID {id}."));
                            }
                        }
                    }
                }
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
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"SELECT * FROM car_accessories WHERE car_id = @carId";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@carId", carId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                accessories.Add(MapToCarAccessory(reader));
                            }
                        }
                    }
                }

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
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"SELECT * FROM car_accessories WHERE car_model = @carModel";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@carModel", carModel);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                accessories.Add(MapToCarAccessory(reader));
                            }
                        }
                    }
                }

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
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"SELECT * FROM car_accessories WHERE category = @category";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@category", category);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                accessories.Add(MapToCarAccessory(reader));
                            }
                        }
                    }
                }

                return Result<IEnumerable<CarAccessory>>.Success(accessories);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<CarAccessory>>.Failure(new Error("error", $"Błąd pobierania akcesoriów dla kategorii: {ex.Message}"));
            }
        }

        public async Task<Result<IEnumerable<CarAccessory>>> GetByTypeAsync(string type)
        {
            var accessories = new List<CarAccessory>();

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"SELECT * FROM car_accessories WHERE type = @type";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@type", type);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                accessories.Add(MapToCarAccessory(reader));
                            }
                        }
                    }
                }

                return Result<IEnumerable<CarAccessory>>.Success(accessories);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<CarAccessory>>.Failure(new Error("error", $"Błąd pobierania akcesoriów dla typu: {ex.Message}"));
            }
        }

        public async Task<Result<IEnumerable<CarAccessory>>> GetFilteredAsync(
            string carId = null,
            string carModel = null,
            string category = null,
            string type = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            bool? isOriginalBMWPart = null,
            bool? isInStock = null,
            string installationDifficulty = null)
        {
            var accessories = new List<CarAccessory>();

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var queryBuilder = new StringBuilder("SELECT * FROM car_accessories WHERE 1=1");
                    var parameters = new List<NpgsqlParameter>();

                    if (!string.IsNullOrEmpty(carId))
                    {
                        queryBuilder.Append(" AND car_id = @carId");
                        parameters.Add(new NpgsqlParameter("@carId", carId));
                    }

                    if (!string.IsNullOrEmpty(carModel))
                    {
                        queryBuilder.Append(" AND car_model = @carModel");
                        parameters.Add(new NpgsqlParameter("@carModel", carModel));
                    }

                    if (!string.IsNullOrEmpty(category))
                    {
                        queryBuilder.Append(" AND category = @category");
                        parameters.Add(new NpgsqlParameter("@category", category));
                    }

                    if (!string.IsNullOrEmpty(type))
                    {
                        queryBuilder.Append(" AND type = @type");
                        parameters.Add(new NpgsqlParameter("@type", type));
                    }

                    if (minPrice.HasValue)
                    {
                        queryBuilder.Append(" AND price >= @minPrice");
                        parameters.Add(new NpgsqlParameter("@minPrice", minPrice.Value));
                    }

                    if (maxPrice.HasValue)
                    {
                        queryBuilder.Append(" AND price <= @maxPrice");
                        parameters.Add(new NpgsqlParameter("@maxPrice", maxPrice.Value));
                    }

                    if (isOriginalBMWPart.HasValue)
                    {
                        queryBuilder.Append(" AND is_original_bmw_part = @isOriginalBMWPart");
                        parameters.Add(new NpgsqlParameter("@isOriginalBMWPart", isOriginalBMWPart.Value));
                    }

                    if (isInStock.HasValue)
                    {
                        queryBuilder.Append(" AND is_in_stock = @isInStock");
                        parameters.Add(new NpgsqlParameter("@isInStock", isInStock.Value));
                    }

                    if (!string.IsNullOrEmpty(installationDifficulty))
                    {
                        queryBuilder.Append(" AND installation_difficulty = @installationDifficulty");
                        parameters.Add(new NpgsqlParameter("@installationDifficulty", installationDifficulty));
                    }

                    using (var command = new NpgsqlCommand(queryBuilder.ToString(), connection))
                    {
                        command.Parameters.AddRange(parameters.ToArray());

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                accessories.Add(MapToCarAccessory(reader));
                            }
                        }
                    }
                }

                return Result<IEnumerable<CarAccessory>>.Success(accessories);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<CarAccessory>>.Failure(new Error("error", $"Błąd pobierania filtrowanych akcesoriów: {ex.Message}"));
            }
        }

        public async Task<Result<CarAccessory>> CreateAsync(CarAccessory accessory)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"
                        INSERT INTO car_accessories (
                            id, car_id, car_model, category, type, name, description, price, 
                            manufacturer, part_number, is_original_bmw_part, is_in_stock, 
                            stock_quantity, image_url, size, pattern, color, material, 
                            capacity, compatibility, age_group, max_load, is_universal, 
                            installation_difficulty, warranty
                        ) VALUES (
                            @id, @carId, @carModel, @category, @type, @name, @description, @price, 
                            @manufacturer, @partNumber, @isOriginalBMWPart, @isInStock, 
                            @stockQuantity, @imageUrl, @size, @pattern, @color, @material, 
                            @capacity, @compatibility, @ageGroup, @maxLoad, @isUniversal, 
                            @installationDifficulty, @warranty
                        ) RETURNING *";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", accessory.Id ?? Guid.NewGuid().ToString());
                        command.Parameters.AddWithValue("@carId", accessory.CarId);
                        command.Parameters.AddWithValue("@carModel", accessory.CarModel);
                        command.Parameters.AddWithValue("@category", accessory.Category);
                        command.Parameters.AddWithValue("@type", accessory.Type);
                        command.Parameters.AddWithValue("@name", accessory.Name);
                        command.Parameters.AddWithValue("@description", accessory.Description ?? "");
                        command.Parameters.AddWithValue("@price", accessory.Price);
                        command.Parameters.AddWithValue("@manufacturer", accessory.Manufacturer ?? "");
                        command.Parameters.AddWithValue("@partNumber", accessory.PartNumber ?? "");
                        command.Parameters.AddWithValue("@isOriginalBMWPart", accessory.IsOriginalBMWPart);
                        command.Parameters.AddWithValue("@isInStock", accessory.IsInStock);
                        command.Parameters.AddWithValue("@stockQuantity", accessory.StockQuantity);
                        command.Parameters.AddWithValue("@imageUrl", accessory.ImageUrl ?? "");
                        command.Parameters.AddWithValue("@size", accessory.Size ?? "");
                        command.Parameters.AddWithValue("@pattern", accessory.Pattern ?? "");
                        command.Parameters.AddWithValue("@color", accessory.Color ?? "");
                        command.Parameters.AddWithValue("@material", accessory.Material ?? "");
                        command.Parameters.AddWithValue("@capacity", accessory.Capacity == 0 ? DBNull.Value : (object)accessory.Capacity);
                        command.Parameters.AddWithValue("@compatibility", accessory.Compatibility ?? "");
                        command.Parameters.AddWithValue("@ageGroup", accessory.AgeGroup ?? "");
                        command.Parameters.AddWithValue("@maxLoad", accessory.MaxLoad == 0 ? DBNull.Value : (object)accessory.MaxLoad);
                        command.Parameters.AddWithValue("@isUniversal", accessory.IsUniversal);
                        command.Parameters.AddWithValue("@installationDifficulty", accessory.InstallationDifficulty ?? "");
                        command.Parameters.AddWithValue("@warranty", accessory.Warranty ?? "");

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return Result<CarAccessory>.Success(MapToCarAccessory(reader));
                            }
                            else
                            {
                                return Result<CarAccessory>.Failure(new Error("error", "Nie udało się utworzyć akcesorium."));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Result<CarAccessory>.Failure(new Error("error",$"Błąd tworzenia akcesorium: {ex.Message}"));
            }
        }

        public async Task<Result<CarAccessory>> UpdateAsync(string id, CarAccessory accessory)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"
                        UPDATE car_accessories SET 
                            car_id = @carId, 
                            car_model = @carModel, 
                            category = @category, 
                            type = @type, 
                            name = @name, 
                            description = @description, 
                            price = @price, 
                            manufacturer = @manufacturer, 
                            part_number = @partNumber, 
                            is_original_bmw_part = @isOriginalBMWPart, 
                            is_in_stock = @isInStock, 
                            stock_quantity = @stockQuantity, 
                            image_url = @imageUrl, 
                            size = @size, 
                            pattern = @pattern, 
                            color = @color, 
                            material = @material, 
                            capacity = @capacity, 
                            compatibility = @compatibility, 
                            age_group = @ageGroup, 
                            max_load = @maxLoad, 
                            is_universal = @isUniversal, 
                            installation_difficulty = @installationDifficulty, 
                            warranty = @warranty
                        WHERE id = @id
                        RETURNING *";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@carId", accessory.CarId);
                        command.Parameters.AddWithValue("@carModel", accessory.CarModel);
                        command.Parameters.AddWithValue("@category", accessory.Category);
                        command.Parameters.AddWithValue("@type", accessory.Type);
                        command.Parameters.AddWithValue("@name", accessory.Name);
                        command.Parameters.AddWithValue("@description", accessory.Description ?? "");
                        command.Parameters.AddWithValue("@price", accessory.Price);
                        command.Parameters.AddWithValue("@manufacturer", accessory.Manufacturer ?? "");
                        command.Parameters.AddWithValue("@partNumber", accessory.PartNumber ?? "");
                        command.Parameters.AddWithValue("@isOriginalBMWPart", accessory.IsOriginalBMWPart);
                        command.Parameters.AddWithValue("@isInStock", accessory.IsInStock);
                        command.Parameters.AddWithValue("@stockQuantity", accessory.StockQuantity);
                        command.Parameters.AddWithValue("@imageUrl", accessory.ImageUrl ?? "");
                        command.Parameters.AddWithValue("@size", accessory.Size ?? "");
                        command.Parameters.AddWithValue("@pattern", accessory.Pattern ?? "");
                        command.Parameters.AddWithValue("@color", accessory.Color ?? "");
                        command.Parameters.AddWithValue("@material", accessory.Material ?? "");
                        command.Parameters.AddWithValue("@capacity", accessory.Capacity == 0 ? DBNull.Value : (object)accessory.Capacity);
                        command.Parameters.AddWithValue("@compatibility", accessory.Compatibility ?? "");
                        command.Parameters.AddWithValue("@ageGroup", accessory.AgeGroup ?? "");
                        command.Parameters.AddWithValue("@maxLoad", accessory.MaxLoad == 0 ? DBNull.Value : (object)accessory.MaxLoad);
                        command.Parameters.AddWithValue("@isUniversal", accessory.IsUniversal);
                        command.Parameters.AddWithValue("@installationDifficulty", accessory.InstallationDifficulty ?? "");
                        command.Parameters.AddWithValue("@warranty", accessory.Warranty ?? "");

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return Result<CarAccessory>.Success(MapToCarAccessory(reader));
                            }
                            else
                            {
                                return Result<CarAccessory>.Failure(new Error("error", $"Nie znaleziono akcesorium o ID {id}."));
                            }
                        }
                    }
                }
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
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"DELETE FROM car_accessories WHERE id = @id";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        var affectedRows = await command.ExecuteNonQueryAsync();

                        if (affectedRows > 0)
                        {
                            return Result<bool>.Success(true);
                        }
                        else
                        {
                            return Result<bool>.Failure(new Error("error", $"Nie znaleziono akcesorium o ID {id}."));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(new Error("error", $"Błąd usuwania akcesorium: {ex.Message}"));
            }
        }
    }
}