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
builder.Services.AddScoped<ICarModelColorService, CarModelColorService>();

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
app.MapCarModelColorEndpoints();

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
        
        
        // Check if car_interior_equipment table exists
        var checkSql = @"
            SELECT EXISTS (
                SELECT FROM information_schema.tables 
                WHERE table_schema = 'public' 
                AND table_name = 'car_interior_equipment'
            );";

        // With this corrected line:
        var exists = await connection.ExecuteScalarAsync<bool>(checkSql);

        
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
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Silnik' AND column_name = 'RodzajPaliwa') THEN
                    ALTER TABLE ""Silnik"" ADD COLUMN ""RodzajPaliwa"" VARCHAR(50);
                END IF;
                
                -- Cylinders (liczba cylindrów)
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Silnik' AND column_name = 'Cylindry') THEN
                    ALTER TABLE ""Silnik"" ADD COLUMN ""Cylindry"" INT;
                END IF;
                
                -- Transmission (skrzynia biegów)
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Silnik' AND column_name = 'Skrzynia') THEN
                    ALTER TABLE ""Silnik"" ADD COLUMN ""Skrzynia"" VARCHAR(50);
                END IF;
                
                -- Gears (liczba biegów)
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Silnik' AND column_name = 'Biegi') THEN
                    ALTER TABLE ""Silnik"" ADD COLUMN ""Biegi"" INT;
                END IF;
                
                -- DriveType (napęd)
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Silnik' AND column_name = 'NapedzType') THEN
                    ALTER TABLE ""Silnik"" ADD COLUMN ""NapedzType"" VARCHAR(50);
                END IF;
                
                -- FuelConsumption (zużycie paliwa)
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Silnik' AND column_name = 'ZuzyciePaliva') THEN
                    ALTER TABLE ""Silnik"" ADD COLUMN ""ZuzyciePaliva"" VARCHAR(10);
                END IF;
                
                -- CO2Emission (emisja CO2)
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Silnik' AND column_name = 'EmisjaCO2') THEN
                    ALTER TABLE ""Silnik"" ADD COLUMN ""EmisjaCO2"" INT;
                END IF;
                
                -- Description (opis)
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Silnik' AND column_name = 'Opis') THEN
                    ALTER TABLE ""Silnik"" ADD COLUMN ""Opis"" TEXT;
                END IF;
                
                -- IsActive (czy aktywny)
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Silnik' AND column_name = 'JestAktywny') THEN
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


