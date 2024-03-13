using System.ComponentModel.DataAnnotations;

namespace SolarWatchProject.Contracts;

public record RegistrationRequest(
    [Required]string Email, 
    [Required]string Username, 
    [Required]string Password);