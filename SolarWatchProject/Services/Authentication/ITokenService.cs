using Microsoft.AspNetCore.Identity;
using SolarWatchProjectBackend.Models;

namespace SolarWatchProjectBackend.Services.Authentication;

public interface ITokenService
{
    public string CreateToken(User user, string role);
}