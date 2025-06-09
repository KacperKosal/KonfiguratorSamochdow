using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Endpoints;
using KonfiguratorSamochodowy.Api.Repositories;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Services;
using KonfiguratorSamochodowy.Api.Validators;
using Microsoft.OpenApi.Models;
using Npgsql;
using Dapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "KonfiguratorSamochodowy.Api", Description = "API for car configurator", Version = "v1" });
});

builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddScoped<IUserCreateService, UserCreateService>();
builder.Services.AddScoped<ILoginUserService, LoginUserService>();
builder.Services.AddScoped<IModelsService, ModelsService>();
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddScoped<IRefreshJwtService, RefreshJwtService>();
builder.Services.AddScoped<CreateCarInteriorEquipmentValidator>();
builder.Services.AddScoped<UpdateCarInteriorEquipmentValidator>();
builder.Services.AddScoped<ICarInteriorEquipmentService, CarInteriorEquipmentService>();
builder.Services.AddScoped<CreateCarAccessoryValidator>();
builder.Services.AddScoped<UpdateCarAccessoryValidator>();
builder.Services.AddScoped<ICarAccessoryService, CarAccessoryService>();

builder.Services.AddScoped<CreateCarModelValidator>();
builder.Services.AddScoped<UpdateCarModelValidator>();
builder.Services.AddScoped<CreateEngineValidator>();
builder.Services.AddScoped<UpdateEngineValidator>();
builder.Services.AddScoped<AddCarModelEngineValidator>();
builder.Services.AddScoped<UpdateCarModelEngineValidator>();
builder.Services.AddScoped<IChangePasswordService, ChangePasswordService>();
builder.Services.AddScoped<ICarModelService, CarModelService>();
builder.Services.AddScoped<IEngineService, EngineService>();
builder.Services.AddScoped<ICarModelEngineService, CarModelEngineService>();
builder.Services.AddScoped<ICarConfigurationService, CarConfigurationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserConfigurationService, UserConfigurationService>();
builder.Services.AddScoped<ICarModelImageService, CarModelImageService>();

builder.Services.AddCors();

var app = builder.Build();

// Ensure required tables exist
await EnsureTablesExist(app.Services);

app.UseCors(cfg => 
{
    cfg.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:5173");
});

// Obsługa plików statycznych
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarInteriorEquipmentEndpoints();
app.MapCarModelEndpoints();
app.MapEngineEndpoints();
app.MapCarModelEngineEndpoints();
app.MapCarConfigurationEndpoints();
app.MapImageEndpoints();
app.MapCarModelImageEndpoints();

app.UseHttpsRedirection();

LoginEndpoint.MapEndPoint(app);
RegisterEndpoint.MapEndPoint(app);
RefreshJwtEndpoint.MapEndPoint(app);
ValidateJwtEndpoint.MapEndPoint(app);
ChangePasswordEndpoint.MapEndPoint(app);
ContactEndpoint.MapEndPoint(app);
UserConfigurationEndpoints.MapEndPoint(app);
app.MapCarAccessoryEndpoints();

app.MapSwagger();
app.Run();

static async Task EnsureTablesExist(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("Psql");
    
    if (string.IsNullOrEmpty(connectionString))
        return;
        
    try
    {
        using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        
        // Extend Silnik table with additional columns
        await ExtendSilnikTable(connection);
        
        // Create car model images table
        await CreateCarModelImagesTable(connection);
        
        // Check if car_interior_equipment table exists
        var checkSql = @"
            SELECT EXISTS (
                SELECT FROM information_schema.tables 
                WHERE table_schema = 'public' 
                AND table_name = 'car_interior_equipment'
            );";

        // With this corrected line:
        var exists = await connection.ExecuteScalarAsync<bool>(checkSql);

        if (!exists)
        {
            Console.WriteLine("Creating car_interior_equipment table...");
            
            var createSql = @"
                CREATE TABLE car_interior_equipment (
                    id VARCHAR(255) PRIMARY KEY,
                    type VARCHAR(50) NOT NULL,
                    value VARCHAR(255) NOT NULL,
                    description TEXT,
                    additional_price DECIMAL(10, 2) NOT NULL DEFAULT 0,
                    has_navigation BOOLEAN DEFAULT FALSE,
                    has_premium_sound BOOLEAN DEFAULT FALSE,
                    control_type VARCHAR(50)
                );
                
                INSERT INTO car_interior_equipment (id, type, value, description, additional_price, has_navigation, has_premium_sound, control_type) VALUES
                ('1', 'Audio', 'System audio podstawowy', 'Standardowy system audio z radiem FM/AM', 0, FALSE, FALSE, 'Przyciski'),
                ('2', 'Audio', 'System audio Premium', 'System audio wysokiej jakości z 12 głośnikami', 2500, FALSE, TRUE, 'Dotykowy'),
                ('3', 'Nawigacja', 'Nawigacja podstawowa', 'System nawigacji z mapami Europy', 1500, TRUE, FALSE, 'Dotykowy'),
                ('4', 'Nawigacja', 'Nawigacja Premium', 'Zaawansowany system nawigacji z Traffic Live', 3000, TRUE, TRUE, 'Głosowy'),
                ('5', 'Klimatyzacja', 'Klimatyzacja automatyczna', 'Dwustrefowa automatyczna klimatyzacja', 2000, FALSE, FALSE, 'Automatyczny'),
                ('6', 'Oświetlenie', 'Oświetlenie ambientowe', 'Kolorowe oświetlenie wnętrza z regulacją', 800, FALSE, FALSE, 'Dotykowy');";
            
            await connection.ExecuteAsync(createSql);
            Console.WriteLine("car_interior_equipment table created successfully.");
        }
        else
        {
            Console.WriteLine("car_interior_equipment table already exists.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error ensuring tables exist: {ex.Message}");
    }
}

static async Task ExtendSilnikTable(NpgsqlConnection connection)
{
    try
    {
        // First check if Silnik table exists
        var checkTableSql = @"
            SELECT EXISTS (
                SELECT FROM information_schema.tables 
                WHERE table_schema = 'public' 
                AND table_name = 'Silnik'
            );";
            
        var tableExists = await connection.ExecuteScalarAsync<bool>(checkTableSql);
        
        if (!tableExists)
        {
            Console.WriteLine("Creating Silnik table...");
            
            var createTableSql = @"
                CREATE TABLE ""Silnik"" (
                    ""ID"" SERIAL PRIMARY KEY,
                    ""PojazdID"" INT REFERENCES Pojazd(ID),
                    ""Pojemnosc"" VARCHAR(10),
                    ""Typ"" VARCHAR(20),
                    ""Moc"" SMALLINT,
                    ""Nazwa"" VARCHAR(255),
                    ""MomentObrotowy"" INT,
                    ""RodzajPaliwa"" VARCHAR(50),
                    ""Cylindry"" INT,
                    ""Skrzynia"" VARCHAR(50),
                    ""Biegi"" INT,
                    ""NapedzType"" VARCHAR(50),
                    ""ZuzyciePaliva"" VARCHAR(10),
                    ""EmisjaCO2"" INT,
                    ""Opis"" TEXT,
                    ""JestAktywny"" BOOLEAN DEFAULT TRUE
                );";
                
            await connection.ExecuteAsync(createTableSql);
            Console.WriteLine("Silnik table created successfully.");
            return; // Table is new, no need to extend
        }
        
        Console.WriteLine("Extending Silnik table with additional columns...");
        
        var alterSql = @"
            -- Add columns if they don't exist
            DO $$ 
            BEGIN 
                -- Name (nazwa silnika)
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Silnik' AND column_name = 'Nazwa') THEN
                    ALTER TABLE ""Silnik"" ADD COLUMN ""Nazwa"" VARCHAR(255);
                END IF;
                
                -- Torque (moment obrotowy)
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Silnik' AND column_name = 'MomentObrotowy') THEN
                    ALTER TABLE ""Silnik"" ADD COLUMN ""MomentObrotowy"" INT;
                END IF;
                
                -- FuelType (rodzaj paliwa) 
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'silnik' AND column_name = 'RodzajPaliwa') THEN
                    ALTER TABLE ""Silnik"" ADD COLUMN ""RodzajPaliwa"" VARCHAR(50);
                END IF;
                
                -- Cylinders (liczba cylindrów)
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'silnik' AND column_name = 'Cylindry') THEN
                    ALTER TABLE ""Silnik"" ADD COLUMN ""Cylindry"" INT;
                END IF;
                
                -- Transmission (skrzynia biegów)
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'silnik' AND column_name = 'Skrzynia') THEN
                    ALTER TABLE ""Silnik"" ADD COLUMN ""Skrzynia"" VARCHAR(50);
                END IF;
                
                -- Gears (liczba biegów)
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'silnik' AND column_name = 'Biegi') THEN
                    ALTER TABLE ""Silnik"" ADD COLUMN ""Biegi"" INT;
                END IF;
                
                -- DriveType (napęd)
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'silnik' AND column_name = 'NapedzType') THEN
                    ALTER TABLE ""Silnik"" ADD COLUMN ""NapedzType"" VARCHAR(50);
                END IF;
                
                -- FuelConsumption (zużycie paliwa)
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'silnik' AND column_name = 'ZuzyciePaliva') THEN
                    ALTER TABLE ""Silnik"" ADD COLUMN ""ZuzyciePaliva"" VARCHAR(10);
                END IF;
                
                -- CO2Emission (emisja CO2)
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'silnik' AND column_name = 'EmisjaCO2') THEN
                    ALTER TABLE ""Silnik"" ADD COLUMN ""EmisjaCO2"" INT;
                END IF;
                
                -- Description (opis)
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'silnik' AND column_name = 'Opis') THEN
                    ALTER TABLE ""Silnik"" ADD COLUMN ""Opis"" TEXT;
                END IF;
                
                -- IsActive (czy aktywny)
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'silnik' AND column_name = 'JestAktywny') THEN
                    ALTER TABLE ""Silnik"" ADD COLUMN ""JestAktywny"" BOOLEAN DEFAULT TRUE;
                END IF;
            END $$;";
        
        await connection.ExecuteAsync(alterSql);
        
        // Fix existing ZuzyciePaliva column to be VARCHAR and convert comma separators
        var fixColumnSql = @"
            DO $$ 
            BEGIN 
                -- If column exists and is not VARCHAR, alter it
                IF EXISTS (
                    SELECT 1 FROM information_schema.columns 
                    WHERE table_name = 'Silnik' 
                    AND column_name = 'ZuzyciePaliva' 
                    AND data_type != 'character varying'
                ) THEN
                    -- First create a temp column
                    ALTER TABLE ""Silnik"" ADD COLUMN ""ZuzyciePaliva_temp"" VARCHAR(10);
                    
                    -- Copy data with comma replacement
                    UPDATE ""Silnik"" 
                    SET ""ZuzyciePaliva_temp"" = REPLACE(""ZuzyciePaliva""::TEXT, ',', '.');
                    
                    -- Drop old column and rename temp
                    ALTER TABLE ""Silnik"" DROP COLUMN ""ZuzyciePaliva"";
                    ALTER TABLE ""Silnik"" RENAME COLUMN ""ZuzyciePaliva_temp"" TO ""ZuzyciePaliva"";
                END IF;
            END $$;";
            
        await connection.ExecuteAsync(fixColumnSql);
        
        Console.WriteLine("Silnik table extended successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error extending Silnik table: {ex.Message}");
    }
}

static async Task CreateCarModelImagesTable(NpgsqlConnection connection)
{
    try
    {
        Console.WriteLine("=== CreateCarModelImagesTable: Starting ===");
        var checkSql = @"
            SELECT EXISTS (
                SELECT FROM information_schema.tables 
                WHERE table_schema = 'public' 
                AND table_name = 'pojazd_zdjecie'
            );";

        var exists = await connection.ExecuteScalarAsync<bool>(checkSql);
        Console.WriteLine($"=== CreateCarModelImagesTable: Table exists check returned: {exists} ===");

        if (!exists)
        {
            Console.WriteLine("Creating pojazd_zdjecie table...");
            
            var createSql = @"
                CREATE TABLE pojazd_zdjecie (
                    ""Id"" VARCHAR(255) PRIMARY KEY,
                    ""CarModelId"" VARCHAR(255) NOT NULL,
                    ""ImageUrl"" VARCHAR(500) NOT NULL,
                    ""DisplayOrder"" INTEGER NOT NULL DEFAULT 1,
                    ""IsMainImage"" BOOLEAN NOT NULL DEFAULT FALSE,
                    ""CreatedAt"" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
                    ""UpdatedAt"" TIMESTAMP WITH TIME ZONE
                );
                
                CREATE INDEX idx_pojazd_zdjecie_carmodelid ON pojazd_zdjecie(""CarModelId"");
                CREATE INDEX idx_pojazd_zdjecie_displayorder ON pojazd_zdjecie(""DisplayOrder"");";
            
            await connection.ExecuteAsync(createSql);
            Console.WriteLine("=== CreateCarModelImagesTable: Table created successfully ===");
        }
        else 
        {
            Console.WriteLine("=== CreateCarModelImagesTable: Table already exists, checking structure ===");
            
            // Check if the table has the correct column structure
            var columnsQuery = @"
                SELECT column_name, data_type 
                FROM information_schema.columns 
                WHERE table_name = 'pojazd_zdjecie' 
                ORDER BY ordinal_position;";
            
            var columns = await connection.QueryAsync(columnsQuery);
            var columnNames = columns.Select(c => c.column_name).ToList();
            Console.WriteLine($"Existing columns: {string.Join(", ", columnNames)}");
            
            // Check if we have the expected quoted columns
            var expectedColumns = new[] { "Id", "CarModelId", "ImageUrl", "DisplayOrder", "IsMainImage", "CreatedAt", "UpdatedAt" };
            var hasCorrectStructure = expectedColumns.All(expected => columnNames.Contains(expected));
            
            if (!hasCorrectStructure)
            {
                Console.WriteLine("=== Table structure is incorrect, recreating table ===");
                
                // Drop and recreate table with correct structure
                var dropSql = "DROP TABLE IF EXISTS pojazd_zdjecie CASCADE;";
                await connection.ExecuteAsync(dropSql);
                
                var createSql = @"
                    CREATE TABLE pojazd_zdjecie (
                        ""Id"" VARCHAR(255) PRIMARY KEY,
                        ""CarModelId"" VARCHAR(255) NOT NULL,
                        ""ImageUrl"" VARCHAR(500) NOT NULL,
                        ""DisplayOrder"" INTEGER NOT NULL DEFAULT 1,
                        ""IsMainImage"" BOOLEAN NOT NULL DEFAULT FALSE,
                        ""CreatedAt"" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
                        ""UpdatedAt"" TIMESTAMP WITH TIME ZONE
                    );
                    
                    CREATE INDEX idx_pojazd_zdjecie_carmodelid ON pojazd_zdjecie(""CarModelId"");
                    CREATE INDEX idx_pojazd_zdjecie_displayorder ON pojazd_zdjecie(""DisplayOrder"");";
                
                await connection.ExecuteAsync(createSql);
                Console.WriteLine("=== CreateCarModelImagesTable: Table recreated with correct structure ===");
            }
            else
            {
                Console.WriteLine("=== CreateCarModelImagesTable: Table structure is correct ===");
            }
        }
        Console.WriteLine("=== CreateCarModelImagesTable: Completed ===");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"=== CreateCarModelImagesTable: ERROR - {ex.Message} ===");
        Console.WriteLine($"=== CreateCarModelImagesTable: FULL ERROR - {ex} ===");
    }
}
