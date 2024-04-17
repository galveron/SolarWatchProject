using SolarWatchProjectBackend.Data;
using System.Text.Json.Serialization;
using System.Text;
using SolarWatchProjectBackend.Services.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SolarWatchProjectBackend.Services.ProcessData;
using SolarWatchProjectBackend.Models;
using SolarWatchProjectBackend.Repositories;

var config =
    new ConfigurationBuilder()
        .AddUserSecrets<Program>()
        .Build();

var builder = WebApplication.CreateBuilder(args);


ConfigureSwagger();
AddServices();
AddCors();
AddAuthentication();
AddIdentity();

var app = builder.Build();

app.UseCors("MyAllowSpecificOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var authenticationSeeder = scope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();


authenticationSeeder.AddRoles();

authenticationSeeder.AddAdmin();

app.Run();


void AddServices()
{
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddTransient<IGeoRepository, GeoRepository>();
    builder.Services.AddTransient<ISolarWatchRepository, SolarWatchRepository>();
    builder.Services.AddTransient<ISunDataRepository, SunDataRepository>();
    builder.Services.AddTransient<IJsonProcessor, JsonProcessor>();
    builder.Services.AddScoped<AuthenticationSeeder>();
    builder.Services.AddDbContext<SolarWatchDbContext>((container, options) =>
        options.UseSqlServer(config["ConnectionString"]));

}

void ConfigureSwagger()
{
    builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    });
}
void AddCors()
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: "MyAllowSpecificOrigins",
            policy =>
            {
                policy
                    //.WithOrigins("*") //doesn't work with credentials included
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetIsOriginAllowed(origin =>
                    {
                        if (string.IsNullOrWhiteSpace(origin)) return false;
                        // Only add this to allow testing with localhost, remove this line in production!
                        if (origin.ToLower().StartsWith("http://localhost")) return true;
                        // Insert your production domain here.
                        if (origin.ToLower().StartsWith("https://dev.mydomain.com")) return true;
                        return false;
                    });
            });
    });
}

void AddAuthentication()
{
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var issuerSignInKey = config["IssuerSigningKey"] ?? Environment.GetEnvironmentVariable("ISSUERSIGNINGKEY");
        var validIssuer = config["ValidIssuer"] ?? Environment.GetEnvironmentVariable("VALIDISSUER");
        var validAudience = config["ValidAudience"] ?? Environment.GetEnvironmentVariable("VALIDAUDIENCE");

        options.TokenValidationParameters = new TokenValidationParameters()
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = validIssuer,
                ValidAudience = validAudience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(issuerSignInKey)
                ),
            };
        options.Events = new JwtBearerEvents();
        options.Events.OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("User"))
            {
                context.Token = context.Request.Cookies["User"];
            }

            return Task.CompletedTask;
        };
    });
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
}

void AddIdentity()
{
    builder.Services
        .AddIdentityCore<User>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<SolarWatchDbContext>();
}

public partial class Program { }