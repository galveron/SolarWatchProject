using Microsoft.AspNetCore.Identity;

namespace SolarWatchProject.Service.Authentication;

public interface ITokenService
{
    public string CreateToken(IdentityUser user, string role);
}