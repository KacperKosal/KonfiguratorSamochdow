using Dapper;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories;

public class UserConfigurationRepository : IUserConfigurationRepository
{
    private readonly string _connectionString;

    public UserConfigurationRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Psql") ?? throw new ArgumentNullException("Connection string not found");
    }

    public async Task<Result<int>> SaveUserConfigurationAsync(UserConfiguration configuration)
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            // Sprawdź czy tabela istnieje, jeśli nie - stwórz ją
            await EnsureTableExists(connection);

            const string insertSql = @"
                INSERT INTO user_configurations 
                (user_id, configuration_name, car_model_id, car_model_name, engine_id, engine_name, 
                 exterior_color, exterior_color_name, selected_accessories, selected_interior_equipment, 
                 total_price, created_at, is_active)
                VALUES 
                (@UserId, @ConfigurationName, @CarModelId, @CarModelName, @EngineId, @EngineName,
                 @ExteriorColor, @ExteriorColorName, @SelectedAccessories, @SelectedInteriorEquipment,
                 @TotalPrice, @CreatedAt, @IsActive)
                RETURNING id";

            var id = await connection.QuerySingleAsync<int>(insertSql, configuration);
            return Helpers.Result.Success(id);
        }
        catch (Exception ex)
        {
            return Helpers.Result.Failure<int>(Helpers.Error.Failure("Database.SaveConfiguration", ex.Message));
        }
    }

    public async Task<Result<List<UserConfiguration>>> GetUserConfigurationsAsync(int userId)
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await EnsureTableExists(connection);

            const string selectSql = @"
                SELECT * FROM user_configurations 
                WHERE user_id = @UserId AND is_active = true 
                ORDER BY created_at DESC";

            var configurations = await connection.QueryAsync<UserConfiguration>(selectSql, new { UserId = userId });
            return Helpers.Result.Success(configurations.ToList());
        }
        catch (Exception ex)
        {
            return Helpers.Result.Failure<List<UserConfiguration>>(Helpers.Error.Failure("Database.GetUserConfigurations", ex.Message));
        }
    }

    public async Task<Result<UserConfiguration>> GetUserConfigurationByIdAsync(int configurationId, int userId)
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await EnsureTableExists(connection);

            const string selectSql = @"
                SELECT * FROM user_configurations 
                WHERE id = @ConfigurationId AND user_id = @UserId AND is_active = true";

            var configuration = await connection.QuerySingleOrDefaultAsync<UserConfiguration>(
                selectSql, new { ConfigurationId = configurationId, UserId = userId });

            if (configuration == null)
                return Helpers.Result.Failure<UserConfiguration>(Helpers.Error.NotFound("Configuration.NotFound", "Configuration not found"));

            return Helpers.Result.Success(configuration);
        }
        catch (Exception ex)
        {
            return Helpers.Result.Failure<UserConfiguration>(Helpers.Error.Failure("Database.GetUserConfiguration", ex.Message));
        }
    }

    public async Task<Result<bool>> DeleteUserConfigurationAsync(int configurationId, int userId)
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await EnsureTableExists(connection);

            const string deleteSql = @"
                UPDATE user_configurations 
                SET is_active = false, updated_at = @UpdatedAt 
                WHERE id = @ConfigurationId AND user_id = @UserId";

            var rowsAffected = await connection.ExecuteAsync(deleteSql, 
                new { ConfigurationId = configurationId, UserId = userId, UpdatedAt = DateTime.UtcNow });

            return Helpers.Result.Success(rowsAffected > 0);
        }
        catch (Exception ex)
        {
            return Helpers.Result.Failure<bool>(Helpers.Error.Failure("Database.DeleteConfiguration", ex.Message));
        }
    }

    public async Task<Result<bool>> UpdateUserConfigurationAsync(UserConfiguration configuration)
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await EnsureTableExists(connection);

            const string updateSql = @"
                UPDATE user_configurations 
                SET configuration_name = @ConfigurationName,
                    car_model_id = @CarModelId,
                    car_model_name = @CarModelName,
                    engine_id = @EngineId,
                    engine_name = @EngineName,
                    exterior_color = @ExteriorColor,
                    exterior_color_name = @ExteriorColorName,
                    selected_accessories = @SelectedAccessories,
                    selected_interior_equipment = @SelectedInteriorEquipment,
                    total_price = @TotalPrice,
                    updated_at = @UpdatedAt
                WHERE id = @Id AND user_id = @UserId";

            configuration.UpdatedAt = DateTime.UtcNow;
            var rowsAffected = await connection.ExecuteAsync(updateSql, configuration);

            return Result.Success(rowsAffected > 0);
        }
        catch (Exception ex)
        {
            return Helpers.Result.Failure<bool>(Helpers.Error.Failure("Database.UpdateConfiguration", ex.Message));
        }
    }

    private async Task EnsureTableExists(NpgsqlConnection connection)
    {
        const string createTableSql = @"
            CREATE TABLE IF NOT EXISTS user_configurations (
                id SERIAL PRIMARY KEY,
                user_id INTEGER NOT NULL,
                configuration_name VARCHAR(255) NOT NULL,
                car_model_id VARCHAR(255),
                car_model_name VARCHAR(255),
                engine_id VARCHAR(255),
                engine_name VARCHAR(255),
                exterior_color VARCHAR(50),
                exterior_color_name VARCHAR(100),
                selected_accessories TEXT,
                selected_interior_equipment TEXT,
                total_price DECIMAL(12, 2) NOT NULL DEFAULT 0,
                created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
                updated_at TIMESTAMP WITH TIME ZONE,
                is_active BOOLEAN DEFAULT true
            );

            CREATE INDEX IF NOT EXISTS idx_user_configurations_user_id ON user_configurations(user_id);
            CREATE INDEX IF NOT EXISTS idx_user_configurations_created_at ON user_configurations(created_at);";

        await connection.ExecuteAsync(createTableSql);
    }
}