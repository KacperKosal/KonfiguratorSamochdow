using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Repositories;
using KonfiguratorSamochodowy.Api.Repositories.Services;
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
        
        // Register transaction service
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<TransactionExamples>(sp => 
            new TransactionExamples(
                sp.GetRequiredService<ITransactionService>(),
                connectionString
            ));
        
        // Register existing repositories
        services.AddTransient<IUserRepository, SqlUserRepository>();
        services.AddTransient<IVehicleRepository, SqlVehicleRepository>();
        services.AddTransient<IConfigurationRepository, SqlConfigurationRepository>();
        services.AddTransient<IVechicleFeaturesRepository, SqlVechicleFeaturesRepository>();
        
        // Register our pure SQL implementations using Dapper
        services.AddTransient<ICarModelRepository, SqlCarModelRepository>();
        services.AddTransient<ICarModelEngineRepository, SqlCarModelEngineRepository>();
        services.AddTransient<IEngineRepository, SqlEngineRepository>();
        services.AddTransient<ICarAccessoryRepository, SqlCarAccessoryRepository>();
        services.AddTransient<ICarInteriorEquipmentRepository, SqlCarInteriorEquipmentRepository>();
        services.AddTransient<IUserConfigurationRepository, SqlUserConfigurationRepository>();
        services.AddTransient<ICarModelImageRepository, SqlCarModelImageRepository>();
        services.AddTransient<ICarModelColorRepository, CarModelColorRepository>();
        
        return services;
    }
} 