using KonfiguratorSamochodowy.Api.Models;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace KonfiguratorSamochodowy.Api.Repositories
{
    /// <summary>
    /// Entity Framework DbContext for the application
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<CarModel> CarModels { get; set; } = null!;
        public DbSet<Engine> Engines { get; set; } = null!;
        public DbSet<CarModelEngine> CarModelEngines { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Minimal configuration - we're primarily using Dapper for our repositories
            modelBuilder.Entity<CarModel>().ToTable("pojazd");
            modelBuilder.Entity<Engine>().ToTable("silnik");
            modelBuilder.Entity<CarModelEngine>().ToTable("modelsilnik");
        }
    }
}