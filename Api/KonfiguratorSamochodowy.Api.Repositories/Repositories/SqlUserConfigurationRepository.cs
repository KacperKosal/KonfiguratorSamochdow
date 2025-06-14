using Dapper;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using System.Data;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories;

public sealed class SqlUserConfigurationRepository : IUserConfigurationRepository
{
    private readonly IDbConnection _connection;

    public SqlUserConfigurationRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<Result<int>> SaveUserConfigurationAsync(UserConfiguration configuration)
    {
        try
        {
            const string insertSql = @"
                INSERT INTO user_configurations 
                (user_id, configuration_name, car_model_id, car_model_name, engine_id, engine_name, 
                 exterior_color, exterior_color_name, selected_accessories, selected_interior_equipment, 
                 total_price, created_at, is_active)
                VALUES 
                (@UserId, @ConfigurationName, @CarModelId, @CarModelName, @EngineId, @EngineName,
                 @ExteriorColor, @ExteriorColorName, @SelectedAccessories::jsonb, @SelectedInteriorEquipment::jsonb,
                 @TotalPrice, @CreatedAt, @IsActive)
                RETURNING id";

            var id = await _connection.QuerySingleAsync<int>(insertSql, configuration);
            return Result<int>.Success(id);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure(Error.Failure("Database.SaveConfiguration", ex.Message));
        }
    }

    public async Task<Result<List<UserConfiguration>>> GetUserConfigurationsAsync(int userId)
    {
        try
        {
            const string selectSql = @"
                SELECT 
                    id as Id,
                    user_id as UserId,
                    configuration_name as ConfigurationName,
                    car_model_id as CarModelId,
                    car_model_name as CarModelName,
                    engine_id as EngineId,
                    engine_name as EngineName,
                    exterior_color as ExteriorColor,
                    exterior_color_name as ExteriorColorName,
                    selected_accessories::text as SelectedAccessories,
                    selected_interior_equipment::text as SelectedInteriorEquipment,
                    total_price as TotalPrice,
                    created_at as CreatedAt,
                    updated_at as UpdatedAt,
                    is_active as IsActive
                FROM user_configurations 
                WHERE user_id = @UserId AND is_active = true 
                ORDER BY created_at DESC";

            var configurations = await _connection.QueryAsync<UserConfiguration>(selectSql, new { UserId = userId });
            return Result<List<UserConfiguration>>.Success(configurations.ToList());
        }
        catch (Exception ex)
        {
            return Result<List<UserConfiguration>>.Failure(Error.Failure("Database.GetUserConfigurations", ex.Message));
        }
    }

    public async Task<Result<UserConfiguration>> GetUserConfigurationByIdAsync(int configurationId, int userId)
    {
        try
        {
            const string selectSql = @"
                SELECT 
                    id as Id,
                    user_id as UserId,
                    configuration_name as ConfigurationName,
                    car_model_id as CarModelId,
                    car_model_name as CarModelName,
                    engine_id as EngineId,
                    engine_name as EngineName,
                    exterior_color as ExteriorColor,
                    exterior_color_name as ExteriorColorName,
                    selected_accessories::text as SelectedAccessories,
                    selected_interior_equipment::text as SelectedInteriorEquipment,
                    total_price as TotalPrice,
                    created_at as CreatedAt,
                    updated_at as UpdatedAt,
                    is_active as IsActive
                FROM user_configurations 
                WHERE id = @ConfigurationId AND user_id = @UserId AND is_active = true";

            var configuration = await _connection.QuerySingleOrDefaultAsync<UserConfiguration>(
                selectSql, new { ConfigurationId = configurationId, UserId = userId });

            if (configuration == null)
                return Result<UserConfiguration>.Failure(Error.NotFound("Configuration.NotFound", "Configuration not found"));

            return Result<UserConfiguration>.Success(configuration);
        }
        catch (Exception ex)
        {
            return Result<UserConfiguration>.Failure(Error.Failure("Database.GetUserConfiguration", ex.Message));
        }
    }

    public async Task<Result<bool>> DeleteUserConfigurationAsync(int configurationId, int userId)
    {
        try
        {
            const string deleteSql = @"
                UPDATE user_configurations 
                SET is_active = false, updated_at = @UpdatedAt 
                WHERE id = @ConfigurationId AND user_id = @UserId";

            var rowsAffected = await _connection.ExecuteAsync(deleteSql, 
                new { ConfigurationId = configurationId, UserId = userId, UpdatedAt = DateTime.UtcNow });

            return Result<bool>.Success(rowsAffected > 0);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(Error.Failure("Database.DeleteConfiguration", ex.Message));
        }
    }

    public async Task<Result<bool>> UpdateUserConfigurationAsync(UserConfiguration configuration)
    {
        try
        {
            const string updateSql = @"
                UPDATE user_configurations 
                SET configuration_name = @ConfigurationName,
                    car_model_id = @CarModelId,
                    car_model_name = @CarModelName,
                    engine_id = @EngineId,
                    engine_name = @EngineName,
                    exterior_color = @ExteriorColor,
                    exterior_color_name = @ExteriorColorName,
                    selected_accessories = @SelectedAccessories::jsonb,
                    selected_interior_equipment = @SelectedInteriorEquipment::jsonb,
                    total_price = @TotalPrice,
                    updated_at = @UpdatedAt
                WHERE id = @Id AND user_id = @UserId";

            configuration.UpdatedAt = DateTime.UtcNow;
            
            var rowsAffected = await _connection.ExecuteAsync(updateSql, configuration);

            return Result<bool>.Success(rowsAffected > 0);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(Error.Failure("Database.UpdateConfiguration", ex.Message));
        }
    }
} 