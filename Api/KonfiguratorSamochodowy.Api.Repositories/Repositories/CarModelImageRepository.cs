using System.Data;
using Dapper;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories
{
    public class CarModelImageRepository : ICarModelImageRepository
    {
        private readonly string _connectionString;

        public CarModelImageRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Psql") 
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<Result<List<CarModelImage>>> GetImagesByCarModelIdAsync(string carModelId)
        {
            try
            {
                Console.WriteLine($"Repository: Getting images for carModelId: '{carModelId}'");
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                Console.WriteLine("Database connection opened successfully");
                
                // Check if table exists
                var tableExistsQuery = @"
                    SELECT EXISTS (
                        SELECT FROM information_schema.tables 
                        WHERE table_schema = 'public' 
                        AND table_name = 'pojazd_zdjecie'
                    );";
                var tableExists = await connection.QuerySingleAsync<bool>(tableExistsQuery);
                Console.WriteLine($"Table pojazd_zdjecie exists: {tableExists}");
                
                if (tableExists)
                {
                    // Check table structure
                    var columnsQuery = @"
                        SELECT column_name, data_type 
                        FROM information_schema.columns 
                        WHERE table_name = 'pojazd_zdjecie' 
                        ORDER BY ordinal_position;";
                    var columns = await connection.QueryAsync(columnsQuery);
                    Console.WriteLine($"Table structure: {string.Join(", ", columns.Select(c => $"{c.column_name}({c.data_type})"))}");
                }
                
                const string sql = @"
                    SELECT ""Id"", ""CarModelId"", ""ImageUrl"", ""Color"", ""DisplayOrder"", ""IsMainImage"", ""CreatedAt"", ""UpdatedAt""
                    FROM pojazd_zdjecie
                    WHERE ""CarModelId"" = @CarModelId
                    ORDER BY ""DisplayOrder""";

                Console.WriteLine($"Executing SQL: {sql}");
                var images = await connection.QueryAsync<CarModelImage>(sql, new { CarModelId = carModelId });
                var imagesList = images.ToList();
                Console.WriteLine($"Query returned {imagesList.Count} images");
                return Result<List<CarModelImage>>.Success(imagesList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error in GetImagesByCarModelIdAsync: {ex}");
                return Result<List<CarModelImage>>.Failure(
                    new Error("DATABASE_ERROR", $"Error getting images for car model {carModelId}: {ex.Message}")
                );
            }
        }

        public async Task<Result<CarModelImage>> GetImageByIdAsync(string imageId)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                const string sql = @"
                    SELECT ""Id"", ""CarModelId"", ""ImageUrl"", ""Color"", ""DisplayOrder"", ""IsMainImage"", ""CreatedAt"", ""UpdatedAt""
                    FROM pojazd_zdjecie
                    WHERE ""Id"" = @Id";

                var image = await connection.QuerySingleOrDefaultAsync<CarModelImage>(sql, new { Id = imageId });
                
                if (image == null)
                {
                    return Result<CarModelImage>.Failure(
                        new Error("NOT_FOUND", $"Image with ID {imageId} not found")
                    );
                }

                return Result<CarModelImage>.Success(image);
            }
            catch (Exception ex)
            {
                return Result<CarModelImage>.Failure(
                    new Error("DATABASE_ERROR", $"Error getting image {imageId}: {ex.Message}")
                );
            }
        }

        public async Task<Result<CarModelImage>> AddImageAsync(CarModelImage image)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                
                const string sql = @"
                    INSERT INTO pojazd_zdjecie (""Id"", ""CarModelId"", ""ImageUrl"", ""Color"", ""DisplayOrder"", ""IsMainImage"", ""CreatedAt"")
                    VALUES (@Id, @CarModelId, @ImageUrl, @Color, @DisplayOrder, @IsMainImage, @CreatedAt)
                    RETURNING ""Id"", ""CarModelId"", ""ImageUrl"", ""Color"", ""DisplayOrder"", ""IsMainImage"", ""CreatedAt"", ""UpdatedAt""";

                var insertedImage = await connection.QuerySingleAsync<CarModelImage>(sql, image);
                return Result<CarModelImage>.Success(insertedImage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error in AddImageAsync: {ex}");
                return Result<CarModelImage>.Failure(
                    new Error("DATABASE_ERROR", $"Error adding image: {ex.Message}")
                );
            }
        }

        public async Task<Result<bool>> DeleteImageAsync(string imageId)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                const string sql = "DELETE FROM pojazd_zdjecie WHERE \"Id\" = @Id";

                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = imageId });
                return Result<bool>.Success(rowsAffected > 0);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(
                    new Error("DATABASE_ERROR", $"Error deleting image {imageId}: {ex.Message}")
                );
            }
        }

        public async Task<Result<bool>> UpdateImageOrderAsync(string imageId, int newOrder)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                const string sql = @"
                    UPDATE pojazd_zdjecie 
                    SET ""DisplayOrder"" = @DisplayOrder, ""UpdatedAt"" = @UpdatedAt
                    WHERE ""Id"" = @Id";

                var rowsAffected = await connection.ExecuteAsync(sql, new 
                { 
                    Id = imageId, 
                    DisplayOrder = newOrder, 
                    UpdatedAt = DateTime.UtcNow 
                });

                return Result<bool>.Success(rowsAffected > 0);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(
                    new Error("DATABASE_ERROR", $"Error updating image order: {ex.Message}")
                );
            }
        }

        public async Task<Result<bool>> SetMainImageAsync(string carModelId, string imageId)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                using var transaction = await connection.BeginTransactionAsync();

                try
                {
                    const string resetSql = @"
                        UPDATE pojazd_zdjecie 
                        SET ""IsMainImage"" = false, ""UpdatedAt"" = @UpdatedAt
                        WHERE ""CarModelId"" = @CarModelId";

                    await connection.ExecuteAsync(resetSql, new 
                    { 
                        CarModelId = carModelId, 
                        UpdatedAt = DateTime.UtcNow 
                    }, transaction);

                    const string setSql = @"
                        UPDATE pojazd_zdjecie 
                        SET ""IsMainImage"" = true, ""UpdatedAt"" = @UpdatedAt
                        WHERE ""Id"" = @Id AND ""CarModelId"" = @CarModelId";

                    var rowsAffected = await connection.ExecuteAsync(setSql, new 
                    { 
                        Id = imageId, 
                        CarModelId = carModelId, 
                        UpdatedAt = DateTime.UtcNow 
                    }, transaction);

                    await transaction.CommitAsync();
                    return Result<bool>.Success(rowsAffected > 0);
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(
                    new Error("DATABASE_ERROR", $"Error setting main image: {ex.Message}")
                );
            }
        }

        public async Task<Result<int>> GetImageCountByCarModelIdAsync(string carModelId)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                const string sql = "SELECT COUNT(*) FROM pojazd_zdjecie WHERE \"CarModelId\" = @CarModelId";

                var count = await connection.QuerySingleAsync<int>(sql, new { CarModelId = carModelId });
                return Result<int>.Success(count);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error in GetImageCountByCarModelIdAsync: {ex}");
                return Result<int>.Failure(
                    new Error("DATABASE_ERROR", $"Error getting image count: {ex.Message}")
                );
            }
        }

        public async Task<Result<int>> GetImageCountByCarModelIdAndColorAsync(string carModelId, string color)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                const string sql = "SELECT COUNT(*) FROM pojazd_zdjecie WHERE \"CarModelId\" = @CarModelId AND \"Color\" = @Color";

                var count = await connection.QuerySingleAsync<int>(sql, new { CarModelId = carModelId, Color = color });
                return Result<int>.Success(count);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error in GetImageCountByCarModelIdAndColorAsync: {ex}");
                return Result<int>.Failure(
                    new Error("DATABASE_ERROR", $"Error getting image count by color: {ex.Message}")
                );
            }
        }
    }
}