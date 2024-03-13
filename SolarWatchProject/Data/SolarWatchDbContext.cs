using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SolarWatchProject.Models;

namespace SolarWatchProject.Data
{
    public class SolarWatchDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public DbSet<SunRiseAndSetTime> SunData { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<User> User { get; set; }

        public SolarWatchDbContext()
        {

        }
        public SolarWatchDbContext(DbContextOptions<SolarWatchDbContext> options)
            : base(options)
        {
        }
    }
}
