using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SolarWatchProject.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SolarWatchProject.Data
{
    public class SolarWatchDbContext : IdentityDbContext
    {
        public DbSet<SunRiseAndSetTime> SunData { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<User> User { get; set; } = default!;

        public SolarWatchDbContext()
        { }
        public SolarWatchDbContext(DbContextOptions<SolarWatchDbContext> options)
            : base(options)
        {
        }
    }
}
