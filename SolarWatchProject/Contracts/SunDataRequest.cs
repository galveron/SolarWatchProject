using System.ComponentModel.DataAnnotations;

namespace SolarWatchProjectBackend.Contracts;

public record SunDataRequest(
    [Required]string Date, 
    [Required]string City);