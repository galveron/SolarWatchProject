namespace SolarWatchProjectBackend.Services.Authentication;

public record AuthResponse(string Email, string UserName, string Token);