using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;

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

        services.AddTransient<IDbConnection>(_ => new NpgsqlConnection(connectionString));
        
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IVehicleRepository, VehicleRepository>();
        services.AddTransient<IConfigurationRepository, ConfigurationRepository>();

        return services;
    }
} 