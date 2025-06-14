using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Enums;
using Npgsql;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories
{
    public class LoggingRepository : ILoggingRepository
    {
        private readonly string _connectionString;

        public LoggingRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Psql") ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<Result<bool>> LogLoginAttemptAsync(int? userId, string ipAddress, Statustyp status)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"
                    INSERT INTO logowanie (iduzytkownika, datalogowania, adresip, status)
                    VALUES (@UserId, @DataLogowania, @AdresIP, @Status::statustyp);";

                var parameters = new
                {
                    UserId = userId,
                    DataLogowania = DateTime.UtcNow,
                    AdresIP = ipAddress,
                    Status = status.ToString() // Konwersja Enum na string dla PostgreSQL ENUM
                };

                await connection.ExecuteAsync(sql, parameters);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(new Error("LOGGING_ERROR", $"Failed to log login attempt: {ex.Message}"));
            }
        }
    }
} 