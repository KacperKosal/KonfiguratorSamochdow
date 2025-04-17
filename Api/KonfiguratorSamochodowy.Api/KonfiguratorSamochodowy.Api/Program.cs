using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using KonfiguratorSamochodowy.Api.Repositories;
using KonfiguratorSamochodowy.Api.Endpoints;
using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "KonfiguratorSamochodowy.Api", Description = "API for car configurator", Version = "v1" });
});

builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddScoped<IUserCreateService, UserCreateService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
LoginEndpoint.MapEndPoint(app);
RegisterEndpoint.MapEndPoint(app);


app.MapSwagger();
app.Run();
