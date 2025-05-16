using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Endpoints;
using KonfiguratorSamochodowy.Api.Repositories;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Repositories;
using KonfiguratorSamochodowy.Api.Services;
using KonfiguratorSamochodowy.Api.Validators;
using Microsoft.OpenApi.Models;
using FluentValidation;
using KonfiguratorSamochodowy.Api.Endpoints;
using KonfiguratorSamochodowy.Api.Repositories;
using KonfiguratorSamochodowy.Api.Services;
using KonfiguratorSamochodowy.Api.Validators;

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
builder.Services.AddSingleton<ICarInteriorEquipmentRepository, InMemoryCarInteriorEquipmentRepository>();
builder.Services.AddScoped<CreateCarInteriorEquipmentValidator>();
builder.Services.AddScoped<UpdateCarInteriorEquipmentValidator>();
builder.Services.AddScoped<ICarInteriorEquipmentService, CarInteriorEquipmentService>();
builder.Services.AddSingleton<ICarAccessoryRepository, CarAccessoryRepository>();
builder.Services.AddScoped<CreateCarAccessoryValidator>();
builder.Services.AddScoped<UpdateCarAccessoryValidator>();
builder.Services.AddScoped<ICarAccessoryService, CarAccessoryService>();

// Rejestracja repozytoriów
builder.Services.AddSingleton<ICarModelRepository, InMemoryCarModelRepository>();
builder.Services.AddSingleton<IEngineRepository, InMemoryEngineRepository>();
builder.Services.AddSingleton<ICarModelEngineRepository, InMemoryCarModelEngineRepository>();

// Rejestracja walidatorów
builder.Services.AddScoped<CreateCarModelValidator>();
builder.Services.AddScoped<UpdateCarModelValidator>();
builder.Services.AddScoped<CreateEngineValidator>();
builder.Services.AddScoped<UpdateEngineValidator>();
builder.Services.AddScoped<AddCarModelEngineValidator>();
builder.Services.AddScoped<UpdateCarModelEngineValidator>();

// Rejestracja serwisów
builder.Services.AddScoped<ICarModelService, CarModelService>();
builder.Services.AddScoped<IEngineService, EngineService>();
builder.Services.AddScoped<ICarModelEngineService, CarModelEngineService>();
builder.Services.AddScoped<ICarConfigurationService, CarConfigurationService>();


var app = builder.Build();

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

app.UseHttpsRedirection();

LoginEndpoint.MapEndPoint(app);
RegisterEndpoint.MapEndPoint(app);
ModelsEndpoint.MapEndPoint(app);
RefreshJwtEndpoint.MapEndPoint(app);
ValidateJwtEndpoint.MapEndPoint(app);
app.MapCarAccessoryEndpoints();

app.MapSwagger();
app.Run();
