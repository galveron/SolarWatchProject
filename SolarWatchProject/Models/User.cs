using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarWatchProject.Models
{
    [Table("users")]
    public class User: IdentityUser
    {
    }
}
