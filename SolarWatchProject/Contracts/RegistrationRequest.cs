using System.ComponentModel.DataAnnotations;

namespace SolarWatchProjectBackend.Contracts;

public record RegistrationRequest(
    [Required]string Email, 
    [Required]string Username, 
    [Required]string Password);