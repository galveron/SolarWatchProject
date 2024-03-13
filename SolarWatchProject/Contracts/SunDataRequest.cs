using System.ComponentModel.DataAnnotations;

namespace SolarWatchProject.Contracts;

public record SunDataRequest(
    [Required]string Date, 
    [Required]string City);