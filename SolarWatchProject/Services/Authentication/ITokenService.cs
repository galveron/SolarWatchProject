using Microsoft.AspNetCore.Identity;
using SolarWatchProject.Models;

namespace SolarWatchProject.Services.Authentication;

public interface ITokenService
{
    public string CreateToken(User user, string role);
}