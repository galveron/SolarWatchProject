namespace SolarWatchProject.Service.Authentication;

public record AuthResponse(string Email, string UserName, string Token);