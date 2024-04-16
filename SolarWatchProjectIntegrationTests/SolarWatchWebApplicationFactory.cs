using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using SolarWatchProject.Data;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SolarWatchProjectIntegrationTests
{
    public class SolarWatchWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<SolarWatchDbContext>));

                services.Remove(dbContextDescriptor);

                var config = new ConfigurationBuilder()
                    .AddUserSecrets<SolarWatchWebApplicationFactory>()
                    .Build();

                services.AddDbContext<SolarWatchDbContext>((container, options) =>
                {
                    options.UseSqlServer(config["TestConnectionString"] != null
                        ? config["TestConnectionString"] : Environment.GetEnvironmentVariable("TESTCONNECTIONSTRING"));
                });

                var serviceProvider = services.BuildServiceProvider();
                var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<SolarWatchDbContext>();
                var authSeeder = scope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
                authSeeder.AddRoles();
                authSeeder.AddAdmin();
            });
        }
    }
}
