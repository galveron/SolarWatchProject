using Microsoft.EntityFrameworkCore;
using SolarWatchProject.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SolarWatchProject.Data
{
    public class SolarWatchDbContext : DbContext
    {
        public SolarWatchDbContext(DbContextOptions<SolarWatchDbContext> options) : base(options)
        {
        }
        public DbSet<SunRiseAndSetTime> SunRiseAndSetTimes { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = WebApplication.CreateBuilder();
            var connectionString = builder.Configuration["SolarWatchProgram:ConnectionString"];
            optionsBuilder.UseSqlServer(connectionString);

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<City>()
                .HasIndex(u => u.Name)
                .IsUnique()
                .IsCreatedOnline();
        }
    }
}
