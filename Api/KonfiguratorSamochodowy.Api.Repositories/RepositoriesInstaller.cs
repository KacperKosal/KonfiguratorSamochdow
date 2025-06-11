using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace KonfiguratorSamochodowy.Api.Repositories;

/// <summary>
/// Klasa odpowiedzialna za rejestrację repozytoriów w kontenerze DI
/// </summary>
public static class RepositoriesInstaller
{
    /// <summary>
    /// Rejestruje repozytoria w kontenerze DI
    /// </summary>
    /// <param name="services">Kolekcja usług</param>
    /// <param name="configuration">Konfiguracja aplikacji</param>
    /// <returns>Kolekcja usług z zarejestrowanymi repozytoriami</returns>
    /// <exception cref="InvalidOperationException">Wyrzucany, gdy nie skonfigurowano parametrów połączenia z bazą danych</exception>
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Psql");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'Psql' is not configured.");
        }

        // Register DbContext for Entity Framework (minimal use)
        services.AddDbContext<AppDbContext>(options => 
            options.UseNpgsql(connectionString));

        // Register IDbConnection for Dapper (main data access)
        services.AddTransient<IDbConnection>(_ => new NpgsqlConnection(connectionString));
        
        // Register existing repositories
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IVehicleRepository, VehicleRepository>();
        services.AddTransient<IConfigurationRepository, ConfigurationRepository>();
        services.AddTransient<IVechicleFeaturesRepository, VechicleFeaturesRepository>();
        
        // Register our pure SQL implementations using Dapper
        services.AddTransient<ICarModelRepository, SqlCarModelRepository>();
        services.AddTransient<ICarModelEngineRepository, SqlCarModelEngineRepository>();
        services.AddTransient<IEngineRepository, EngineRepository>();
        services.AddTransient<ICarAccessoryRepository, CarAccessoryRepository>();
        services.AddTransient<ICarInteriorEquipmentRepository, CarInteriorEquipmentRepository>();
        services.AddTransient<IUserConfigurationRepository, UserConfigurationRepository>();
        services.AddTransient<ICarModelImageRepository, CarModelImageRepository>();
        services.AddTransient<ICarModelColorRepository, CarModelColorRepository>();
        
        return services;
    }
} 