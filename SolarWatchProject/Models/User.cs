using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarWatchProjectBackend.Models
{
    [Table("users")]
    public class User: IdentityUser
    {
    }
}
