using System.Data;
using Dapper;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories
{
    public class CarModelColorRepository : ICarModelColorRepository
    {
        private readonly string _connectionString;

        public CarModelColorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Psql") 
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<Result<List<CarModelColor>>> GetColorsByCarModelIdAsync(string carModelId)
        {
            try
            {
                Console.WriteLine($"CarModelColorRepository: Getting colors for carModelId: '{carModelId}'");
                
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                
                const string sql = @"
                    SELECT ""Id"", ""CarModelId"", ""ColorName"", ""Price"", ""CreatedAt"", ""UpdatedAt""
                    FROM car_model_colors
                    WHERE ""CarModelId"" = @CarModelId
                    ORDER BY ""ColorName""";

                var colors = await connection.QueryAsync<CarModelColor>(sql, new { CarModelId = carModelId });
                var colorsList = colors.ToList();
                
                Console.WriteLine($"Found {colorsList.Count} colors for model {carModelId}");
                return Result<List<CarModelColor>>.Success(colorsList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error in GetColorsByCarModelIdAsync: {ex}");
                return Result<List<CarModelColor>>.Failure(
                    new Error("DATABASE_ERROR", $"Error getting colors for car model {carModelId}: {ex.Message}")
                );
            }
        }

        public async Task<Result<CarModelColor>> GetColorByCarModelIdAndNameAsync(string carModelId, string colorName)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                
                const string sql = @"
                    SELECT ""Id"", ""CarModelId"", ""ColorName"", ""Price"", ""CreatedAt"", ""UpdatedAt""
                    FROM car_model_colors
                    WHERE ""CarModelId"" = @CarModelId AND ""ColorName"" = @ColorName";

                var color = await connection.QuerySingleOrDefaultAsync<CarModelColor>(sql, new { 
                    CarModelId = carModelId, 
                    ColorName = colorName 
                });
                
                if (color == null)
                {
                    return Result<CarModelColor>.Failure(
                        new Error("NOT_FOUND", $"Color {colorName} not found for car model {carModelId}")
                    );
                }

                return Result<CarModelColor>.Success(color);
            }
            catch (Exception ex)
            {
                return Result<CarModelColor>.Failure(
                    new Error("DATABASE_ERROR", $"Error getting color {colorName} for car model {carModelId}: {ex.Message}")
                );
            }
        }

        public async Task<Result<CarModelColor>> CreateOrUpdateColorAsync(CarModelColor color)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                
                // Check if color already exists
                var existingResult = await GetColorByCarModelIdAndNameAsync(color.CarModelId, color.ColorName);
                
                if (existingResult.IsSuccess)
                {
                    // Update existing color
                    const string updateSql = @"
                        UPDATE car_model_colors 
                        SET ""Price"" = @Price, ""UpdatedAt"" = @UpdatedAt
                        WHERE ""CarModelId"" = @CarModelId AND ""ColorName"" = @ColorName
                        RETURNING ""Id"", ""CarModelId"", ""ColorName"", ""Price"", ""CreatedAt"", ""UpdatedAt""";

                    var updatedColor = await connection.QuerySingleAsync<CarModelColor>(updateSql, new
                    {
                        color.Price,
                        UpdatedAt = DateTime.UtcNow,
                        color.CarModelId,
                        color.ColorName
                    });

                    Console.WriteLine($"Updated color {color.ColorName} for model {color.CarModelId} with price {color.Price}");
                    return Result<CarModelColor>.Success(updatedColor);
                }
                else
                {
                    // Create new color
                    color.Id = Guid.NewGuid().ToString();
                    color.CreatedAt = DateTime.UtcNow;
                    
                    const string insertSql = @"
                        INSERT INTO car_model_colors (""Id"", ""CarModelId"", ""ColorName"", ""Price"", ""CreatedAt"")
                        VALUES (@Id, @CarModelId, @ColorName, @Price, @CreatedAt)
                        RETURNING ""Id"", ""CarModelId"", ""ColorName"", ""Price"", ""CreatedAt"", ""UpdatedAt""";

                    var createdColor = await connection.QuerySingleAsync<CarModelColor>(insertSql, color);

                    Console.WriteLine($"Created color {color.ColorName} for model {color.CarModelId} with price {color.Price}");
                    return Result<CarModelColor>.Success(createdColor);
                }
            }
            catch (Exception ex)
            {
                return Result<CarModelColor>.Failure(
                    new Error("DATABASE_ERROR", $"Error creating or updating color: {ex.Message}")
                );
            }
        }

        public async Task<Result<bool>> DeleteColorAsync(string id)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                
                const string sql = @"DELETE FROM car_model_colors WHERE ""Id"" = @Id";
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });

                return Result<bool>.Success(rowsAffected > 0);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(
                    new Error("DATABASE_ERROR", $"Error deleting color: {ex.Message}")
                );
            }
        }

        public async Task<Result<List<string>>> GetAvailableColorsWithPricesForModelAsync(string carModelId)
        {
            try
            {
                Console.WriteLine($"GetAvailableColorsWithPricesForModelAsync called with carModelId: '{carModelId}'");
                
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                
                // Get colors that have both images and price definitions
                const string sql = @"
                    SELECT DISTINCT c.""ColorName""
                    FROM car_model_colors c
                    INNER JOIN pojazd_zdjecie i ON c.""CarModelId"" = i.""CarModelId"" AND c.""ColorName"" = i.""Color""
                    WHERE c.""CarModelId"" = @CarModelId
                    ORDER BY c.""ColorName""";

                var colors = await connection.QueryAsync<string>(sql, new { CarModelId = carModelId });
                var colorsList = colors.ToList();
                
                Console.WriteLine($"Available colors with prices for model {carModelId}: [{string.Join(", ", colorsList)}]");
                return Result<List<string>>.Success(colorsList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error in GetAvailableColorsWithPricesForModelAsync: {ex}");
                return Result<List<string>>.Failure(
                    new Error("DATABASE_ERROR", $"Error getting available colors with prices: {ex.Message}")
                );
            }
        }
    }
}