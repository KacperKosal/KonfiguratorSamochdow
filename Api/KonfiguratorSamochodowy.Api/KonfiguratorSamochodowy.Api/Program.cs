using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using KonfiguratorSamochodowy.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "KonfiguratorSamochodowy.Api", Description = "API for car configurator", Version = "v1" });
});

builder.Services.AddRepositories(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapSwagger();
app.Run();
