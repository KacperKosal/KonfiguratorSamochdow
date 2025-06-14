using System.Data;
using Dapper;
using KonfiguratorSamochodowy.Api.Repositories.Dto;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories;

public sealed class SqlEngineRepository : IEngineRepository
{
    private readonly IDbConnection _connection;

    public SqlEngineRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<Result<IEnumerable<Engine>>> GetAllAsync()
    {
        try
        {
            // Debug: Check if table exists and has data
            var countSql = @"SELECT COUNT(*) FROM ""Silnik""";
            var count = await _connection.QuerySingleAsync<int>(countSql);
            Console.WriteLine($"DEBUG: Found {count} engines in database");
            
            if (count == 0)
            {
                Console.WriteLine("DEBUG: No engines found, returning empty list");
                return Result<IEnumerable<Engine>>.Success(new List<Engine>());
            }
            const string sql = @"
                SELECT
                    CAST(""ID"" AS VARCHAR) AS Id,
                    COALESCE(""Nazwa""::TEXT, ""Pojemnosc""::TEXT) AS Name,
                    ""Typ"" AS Type,
                    ""Pojemnosc"" AS CapacityRaw,
                    COALESCE(""Moc""::TEXT, '150') AS Power,
                    COALESCE(""MomentObrotowy""::TEXT, '200') AS Torque,
                    COALESCE(""RodzajPaliwa"", 
                        CASE
                            WHEN ""Typ"" = 'benzyna' THEN 'Benzyna'
                            WHEN ""Typ"" = 'diesel' THEN 'Diesel'
                            WHEN ""Typ"" = 'elektryczny' THEN 'Elektryczny'
                            ELSE 'Benzyna'
                        END) AS FuelType,
                    COALESCE(""Cylindry""::TEXT, '4') AS Cylinders,
                    COALESCE(""Skrzynia"", 'Automatyczna') AS Transmission,
                    COALESCE(""Biegi""::TEXT, '6') AS Gears,
                    COALESCE(""NapedzType"", 'RWD') AS DriveType,
                    COALESCE(""ZuzyciePaliva"", '7.2') AS FuelConsumption,
                    COALESCE(""EmisjaCO2""::TEXT, '165') AS CO2Emission,
                    COALESCE(""Opis"", 'Opis silnika') AS Description,
                    COALESCE(""JestAktywny"", TRUE) AS IsActive,
                    NOW() - INTERVAL '60 days' * RANDOM() AS CreatedAt,
                    NULL AS UpdatedAt
                FROM ""Silnik""
                ORDER BY CAST(""Moc"" AS INTEGER) DESC NULLS LAST";
                
            var engineData = await _connection.QueryAsync(sql);
            Console.WriteLine($"DEBUG: Raw query returned {engineData.Count()} records");
            
            // Since engineData contains correct data, let's use direct mapping
            var engines = engineData.Select(data => new Engine
            {
                Id = ((IDictionary<string, object>)data)["id"]?.ToString(),
                Name = ((IDictionary<string, object>)data)["name"]?.ToString() ?? string.Empty,
                Type = ((IDictionary<string, object>)data)["type"]?.ToString() ?? string.Empty,
                Capacity = ParseCapacity(((IDictionary<string, object>)data)["capacityraw"]?.ToString()),
                Power = ParseInt(((IDictionary<string, object>)data)["power"]?.ToString(), 150),
                Torque = ParseInt(((IDictionary<string, object>)data)["torque"]?.ToString(), 200),
                FuelType = ((IDictionary<string, object>)data)["fueltype"]?.ToString() ?? string.Empty,
                Cylinders = ParseInt(((IDictionary<string, object>)data)["cylinders"]?.ToString(), 4),
                Transmission = ((IDictionary<string, object>)data)["transmission"]?.ToString() ?? string.Empty,
                Gears = ParseInt(((IDictionary<string, object>)data)["gears"]?.ToString(), 6),
                DriveType = ((IDictionary<string, object>)data)["drivetype"]?.ToString() ?? string.Empty,
                FuelConsumption = ParseDecimal(((IDictionary<string, object>)data)["fuelconsumption"]?.ToString(), 7.2m),
                CO2Emission = ParseInt(((IDictionary<string, object>)data)["co2emission"]?.ToString(), 165),
                Description = ((IDictionary<string, object>)data)["description"]?.ToString() ?? string.Empty,
                IsActive = (bool?)((IDictionary<string, object>)data)["isactive"] ?? true,
                CreatedAt = (DateTime?)((IDictionary<string, object>)data)["createdat"] ?? DateTime.Now,
                UpdatedAt = (DateTime?)((IDictionary<string, object>)data)["updatedat"]
            });
            
            return Result<IEnumerable<Engine>>.Success(engines);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Engine>>.Failure(new Error("DatabaseError", ex.Message));
        }
    }

    public async Task<Result<Engine>> GetByIdAsync(string id)
    {
        try
        {
            if (!int.TryParse(id, out _))
                return Result<Engine>.Failure(new Error("InvalidId", $"ID '{id}' nie jest poprawnym numerem"));

            const string sql = @"
                SELECT
                    CAST(""ID"" AS VARCHAR) AS Id,
                    ""Nazwa"" AS Name,
                    ""Typ"" AS Type,
                    ""Pojemnosc"" AS CapacityRaw,
                    ""Moc"" AS Power,
                    ""MomentObrotowy"" AS Torque,
                    ""RodzajPaliwa"" AS FuelType,
                    ""Cylindry"" AS Cylinders,
                    ""Skrzynia"" AS Transmission,
                    ""Biegi"" AS Gears,
                    ""NapedzType"" AS DriveType,
                    ""ZuzyciePaliva"" AS FuelConsumption,
                    ""EmisjaCO2"" AS CO2Emission,
                    ""Opis"" AS Description,
                    ""JestAktywny"" AS IsActive,
                    CURRENT_TIMESTAMP AS CreatedAt,
                    NULL AS UpdatedAt
                FROM ""Silnik""
                WHERE ""ID"" = @Id";

            var engineData = await _connection.QueryFirstOrDefaultAsync(sql, new { Id = int.Parse(id) });
            
            if (engineData == null)
                return Result<Engine>.Failure(new Error("NotFound", $"Silnik o ID {id} nie został znaleziony"));
                
            var engine = new Engine
            {
                Id = ((IDictionary<string, object>)engineData)["id"]?.ToString(),
                Name = ((IDictionary<string, object>)engineData)["name"]?.ToString() ?? string.Empty,
                Type = ((IDictionary<string, object>)engineData)["type"]?.ToString() ?? string.Empty,
                Capacity = ParseCapacity(((IDictionary<string, object>)engineData)["capacityraw"]?.ToString()),
                Power = ParseInt(((IDictionary<string, object>)engineData)["power"]?.ToString(), 150),
                Torque = ParseInt(((IDictionary<string, object>)engineData)["torque"]?.ToString(), 200),
                FuelType = ((IDictionary<string, object>)engineData)["fueltype"]?.ToString() ?? string.Empty,
                Cylinders = ParseInt(((IDictionary<string, object>)engineData)["cylinders"]?.ToString(), 4),
                Transmission = ((IDictionary<string, object>)engineData)["transmission"]?.ToString() ?? string.Empty,
                Gears = ParseInt(((IDictionary<string, object>)engineData)["gears"]?.ToString(), 6),
                DriveType = ((IDictionary<string, object>)engineData)["drivetype"]?.ToString() ?? string.Empty,
                FuelConsumption = ParseDecimal(((IDictionary<string, object>)engineData)["fuelconsumption"]?.ToString(), 7.2m),
                CO2Emission = ParseInt(((IDictionary<string, object>)engineData)["co2emission"]?.ToString(), 165),
                Description = ((IDictionary<string, object>)engineData)["description"]?.ToString() ?? string.Empty,
                IsActive = (bool?)((IDictionary<string, object>)engineData)["isactive"] ?? true,
                CreatedAt = (DateTime?)((IDictionary<string, object>)engineData)["createdat"] ?? DateTime.Now,
                UpdatedAt = (DateTime?)((IDictionary<string, object>)engineData)["updatedat"]
            };
                
            return Result<Engine>.Success(engine);
        }
        catch (Exception ex)
        {
            return Result<Engine>.Failure(new Error("DatabaseError", ex.Message));
        }
    }

    public async Task<Result<IEnumerable<Engine>>> GetFilteredAsync(FilterEnginesRequestDto filter)
    {
        try
        {
            var conditions = new List<string>();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(filter.Type))
            {
                conditions.Add(@"""Typ"" = @Type");
                parameters.Add("@Type", filter.Type);
            }

            if (filter.MinPower.HasValue)
            {
                conditions.Add(@"""Moc"" >= @MinPower");
                parameters.Add("@MinPower", filter.MinPower.Value);
            }

            if (filter.MaxPower.HasValue)
            {
                conditions.Add(@"""Moc"" <= @MaxPower");
                parameters.Add("@MaxPower", filter.MaxPower.Value);
            }

            var whereClause = conditions.Count > 0 ? "WHERE " + string.Join(" AND ", conditions) : string.Empty;
            
            var sql = $@"
                SELECT
                    CAST(""ID"" AS VARCHAR) AS Id,
                    COALESCE(""Nazwa"", ""Pojemnosc"") AS Name,
                    ""Typ"" AS Type,
                    CASE
                        WHEN ""Pojemnosc"" LIKE '%L%' THEN 
                            CAST(SUBSTRING(TRIM(""Pojemnosc""), 1, POSITION('L' IN ""Pojemnosc"") - 1) AS FLOAT)::INT * 1000
                        WHEN ""Pojemnosc"" = '—' THEN 0
                        WHEN ""Pojemnosc"" ~ E'^\\\\d+$' THEN CAST(""Pojemnosc"" AS INT)
                        ELSE 0
                    END AS Capacity,
                    ""Moc"" AS Power,
                    COALESCE(""MomentObrotowy"", ""Moc"" * 1.5) AS Torque,
                    COALESCE(""RodzajPaliwa"", 'Benzyna') AS FuelType,
                    COALESCE(""Cylindry"", 4) AS Cylinders,
                    COALESCE(""Skrzynia"", 'Automatyczna') AS Transmission,
                    COALESCE(""Biegi"", 6) AS Gears,
                    COALESCE(""NapedzType"", 'RWD') AS DriveType,
                    COALESCE(""ZuzyciePaliva"", 7.2) AS FuelConsumption,
                    COALESCE(""EmisjaCO2"", 165) AS CO2Emission,
                    COALESCE(""Opis"", 'Opis silnika') AS Description,
                    COALESCE(""JestAktywny"", TRUE) AS IsActive,
                    NOW() AS CreatedAt,
                    NULL AS UpdatedAt
                FROM ""Silnik""
                {whereClause}
                ORDER BY ""Moc"" DESC";

            var engines = await _connection.QueryAsync<Engine>(sql, parameters);
            return Result<IEnumerable<Engine>>.Success(engines);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Engine>>.Failure(new Error("DatabaseError", ex.Message));
        }
    }

    public async Task<Result<Engine>> CreateAsync(Engine engine)
    {
        try
        {
            // Konwersja capacity z int (cm³) na format string używany w bazie (L)
            var capacityString = engine.Capacity switch
            {
                null => "2.0L",
                0 => "—",
                var c when c >= 1000 => $"{(c / 1000.0):F1}L", // np. 2000 -> "2.0L"
                var c => $"{c}cm³" // mniejsze pojemności jako cm³
            };

            const string sql = @"
                INSERT INTO ""Silnik"" (""PojazdID"", ""Pojemnosc"", ""Typ"", ""Moc"", ""Nazwa"", ""MomentObrotowy"", ""RodzajPaliwa"", ""Cylindry"", ""Skrzynia"", ""Biegi"", ""NapedzType"", ""ZuzyciePaliva"", ""EmisjaCO2"", ""Opis"", ""JestAktywny"")
                VALUES (1, @Capacity, @Type, @Power, @Name, @Torque, @FuelType, @Cylinders, @Transmission, @Gears, @DriveType, @FuelConsumption, @CO2Emission, @Description, @IsActive)
                RETURNING CAST(""ID"" AS VARCHAR) AS Id";

            var parameters = new DynamicParameters();
            parameters.Add("@Capacity", capacityString);
            parameters.Add("@Type", engine.Type?.ToLower() switch
            {
                "benzyna" or "petrol" => "benzyna",
                "diesel" => "diesel", 
                "elektryczny" or "electric" => "elektryczny",
                _ => "benzyna"
            });
            parameters.Add("@Power", engine.Power);
            parameters.Add("@Name", engine.Name ?? "Silnik");
            parameters.Add("@Torque", engine.Torque);
            parameters.Add("@FuelType", engine.FuelType ?? "Benzyna");
            parameters.Add("@Cylinders", engine.Cylinders ?? 4);
            parameters.Add("@Transmission", engine.Transmission ?? "Automatyczna");
            parameters.Add("@Gears", engine.Gears);
            parameters.Add("@DriveType", engine.DriveType ?? "RWD");
            parameters.Add("@FuelConsumption", engine.FuelConsumption);
            parameters.Add("@CO2Emission", engine.CO2Emission);
            parameters.Add("@Description", engine.Description ?? "Opis silnika");
            parameters.Add("@IsActive", engine.IsActive);

            var newId = await _connection.QuerySingleOrDefaultAsync<string>(sql, parameters);
            engine.Id = newId;
            
            return Result<Engine>.Success(engine);
        }
        catch (Exception ex)
        {
            return Result<Engine>.Failure(new Error("DatabaseError", ex.Message));
        }
    }

    public async Task<Result<Engine>> UpdateAsync(string id, Engine engine)
    {
        try
        {
            if (!int.TryParse(id, out _))
                return Result<Engine>.Failure(new Error("InvalidId", $"ID '{id}' nie jest poprawnym numerem"));

            // Konwersja capacity z int (cm³) na format string używany w bazie (L)
            var capacityString = engine.Capacity switch
            {
                null => "2.0L",
                0 => "—",
                var c when c >= 1000 => $"{(c / 1000.0):F1}L", // np. 2000 -> "2.0L"
                var c => $"{c}cm³" // mniejsze pojemności jako cm³
            };
            
            const string sql = @"
                UPDATE ""Silnik""
                SET ""Pojemnosc"" = @Capacity,
                    ""Typ"" = @Type,
                    ""Moc"" = @Power,
                    ""Nazwa"" = @Name,
                    ""MomentObrotowy"" = @Torque,
                    ""RodzajPaliwa"" = @FuelType,
                    ""Cylindry"" = @Cylinders,
                    ""Skrzynia"" = @Transmission,
                    ""Biegi"" = @Gears,
                    ""NapedzType"" = @DriveType,
                    ""ZuzyciePaliva"" = @FuelConsumption,
                    ""EmisjaCO2"" = @CO2Emission,
                    ""Opis"" = @Description,
                    ""JestAktywny"" = @IsActive
                WHERE ""ID"" = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", int.Parse(id));
            parameters.Add("@Capacity", capacityString);
            parameters.Add("@Type", engine.Type?.ToLower() switch
            {
                "benzyna" or "petrol" => "benzyna",
                "diesel" => "diesel", 
                "elektryczny" or "electric" => "elektryczny",
                _ => "benzyna"
            });
            parameters.Add("@Power", engine.Power);
            parameters.Add("@Name", engine.Name ?? "Silnik");
            parameters.Add("@Torque", engine.Torque);
            parameters.Add("@FuelType", engine.FuelType ?? "Benzyna");
            parameters.Add("@Cylinders", engine.Cylinders ?? 4);
            parameters.Add("@Transmission", engine.Transmission ?? "Automatyczna");
            parameters.Add("@Gears", engine.Gears);
            parameters.Add("@DriveType", engine.DriveType ?? "RWD");
            parameters.Add("@FuelConsumption", engine.FuelConsumption);
            parameters.Add("@CO2Emission", engine.CO2Emission);
            parameters.Add("@Description", engine.Description ?? "Opis silnika");
            parameters.Add("@IsActive", engine.IsActive);
            
            Console.WriteLine($"EngineRepository.UpdateAsync: ID={id}, Name={engine.Name}, Type={engine.Type}, Power={engine.Power}");

            var rowsAffected = await _connection.ExecuteAsync(sql, parameters);
            if (rowsAffected == 0)
                return Result<Engine>.Failure(new Error("NotFound", $"Silnik o ID {id} nie został znaleziony"));

            engine.Id = id;
            return Result<Engine>.Success(engine);
        }
        catch (Exception ex)
        {
            return Result<Engine>.Failure(new Error("DatabaseError", ex.Message));
        }
    }

    public async Task<Result<bool>> DeleteAsync(string id)
    {
        try
        {
            if (!int.TryParse(id, out _))
                return Result<bool>.Failure(new Error("InvalidId", $"ID '{id}' nie jest poprawnym numerem"));

            var checkRelatedQuery = @"
                SELECT COUNT(*) 
                FROM modelsilnik ms 
                WHERE ms.silnikid = CAST(@Id AS INTEGER)";

            var relatedCount = await _connection.ExecuteScalarAsync<int>(checkRelatedQuery, new { Id = id });

            if (relatedCount > 0)
            {
                return Result<bool>.Failure(new Error("ReferenceConstraint", $"Cannot delete engine with id {id} because it has related records in modelsilnik table"));
            }

            const string sql = "DELETE FROM \"Silnik\" WHERE \"ID\" = @Id";
            var rowsAffected = await _connection.ExecuteAsync(sql, new { Id = int.Parse(id) });

            if (rowsAffected == 0)
                return Result<bool>.Failure(new Error("NotFound", $"Silnik o ID {id} nie został znaleziony"));

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(new Error("DatabaseError", ex.Message));
        }
    }

    public async Task<Result<bool>> DeleteByVehicleIdAsync(string vehicleId)
    {
        try
        {
            if (!int.TryParse(vehicleId, out _))
                return Result<bool>.Failure(new Error("InvalidId", $"Vehicle ID '{vehicleId}' nie jest poprawnym numerem"));
            
            // Najpierw usuń powiązania z modelsilnik, aby uniknąć problemów z kluczem obcym
            const string deleteModelSilnikSql = "DELETE FROM modelsilnik WHERE silnikid IN (SELECT \"ID\" FROM \"Silnik\" WHERE \"PojazdID\" = @VehicleId)";
            await _connection.ExecuteAsync(deleteModelSilnikSql, new { VehicleId = int.Parse(vehicleId) });

            // Teraz usuń same silniki
            const string sql = "DELETE FROM \"Silnik\" WHERE \"PojazdID\" = @VehicleId";
            var rowsAffected = await _connection.ExecuteAsync(sql, new { VehicleId = int.Parse(vehicleId) });

            return Result<bool>.Success(rowsAffected > 0);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(new Error("DatabaseError", $"Failed to delete engines by vehicle ID {vehicleId}: {ex.Message}"));
        }
    }

    public async Task<IEnumerable<Engine>> GetAllByVechicleIdAsync(int vehicleId)
    {
        const string sql = @"
            SELECT
                CAST(""ID"" AS VARCHAR) AS Id,
                COALESCE(""Nazwa"", ""Pojemnosc"") AS Name,
                ""Typ"" AS Type,
                CASE
                    WHEN ""Pojemnosc"" LIKE '%L%' THEN 
                        CAST(SUBSTRING(TRIM(""Pojemnosc""), 1, POSITION('L' IN ""Pojemnosc"") - 1) AS FLOAT)::INT * 1000
                    WHEN ""Pojemnosc"" = '—' THEN 0
                    WHEN ""Pojemnosc"" ~ E'^\\\\d+$' THEN CAST(""Pojemnosc"" AS INT)
                    ELSE 0
                END AS Capacity,
                ""Moc"" AS Power,
                COALESCE(""MomentObrotowy"", ""Moc"" * 1.5) AS Torque,
                COALESCE(""RodzajPaliwa"", 'Benzyna') AS FuelType,
                COALESCE(""Cylindry"", 4) AS Cylinders,
                COALESCE(""Skrzynia"", 'Automatyczna') AS Transmission,
                COALESCE(""Biegi"", 6) AS Gears,
                COALESCE(""NapedzType"", 'RWD') AS DriveType,
                COALESCE(""ZuzyciePaliva"", 7.2) AS FuelConsumption,
                COALESCE(""EmisjaCO2"", 165) AS CO2Emission,
                COALESCE(""Opis"", 'Opis silnika') AS Description,
                COALESCE(""JestAktywny"", TRUE) AS IsActive,
                NOW() AS CreatedAt,
                NULL AS UpdatedAt
            FROM ""Silnik"" 
            WHERE ""PojazdID"" = @VehicleId
            ORDER BY ""Moc"" DESC";
            
        return await _connection.QueryAsync<Engine>(sql, new { VehicleId = vehicleId });
    }
    
    private static decimal ParseDecimal(string value, decimal defaultValue)
    {
        if (string.IsNullOrWhiteSpace(value))
            return defaultValue;
            
        // Replace comma with dot for decimal parsing
        value = value.Replace(',', '.');
        
        if (decimal.TryParse(value, System.Globalization.NumberStyles.Float, 
            System.Globalization.CultureInfo.InvariantCulture, out var result))
        {
            return result;
        }
        
        return defaultValue;
    }
    
    private static int ParseInt(string value, int defaultValue)
    {
        if (string.IsNullOrWhiteSpace(value))
            return defaultValue;
            
        if (int.TryParse(value, out var result))
        {
            return result;
        }
        
        return defaultValue;
    }
    
    private static int ParseCapacity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return 0;
            
        try
        {
            if (value.Contains("L"))
            {
                // Extract number before L (like "2,0L" -> "2,0")
                var numberPart = value.Substring(0, value.IndexOf('L')).Trim();
                numberPart = numberPart.Replace(',', '.');
                
                if (decimal.TryParse(numberPart, System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out var liters))
                {
                    return (int)(liters * 1000); // Convert to cm³
                }
            }
            else if (value == "—" || value == "-")
            {
                return 0;
            }
            else if (int.TryParse(value, out var capacity))
            {
                return capacity;
            }
        }
        catch
        {
            // Ignore parsing errors
        }
        
        return 0;
    }
}