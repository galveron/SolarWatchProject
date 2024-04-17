using SolarWatchProjectBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SolarWatchProjectBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public UserController(
        UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("GetUser")]
        public async Task<ActionResult<User>> GetUser(string userName)
        {
            try
            {
                var user = await _userManager.Users
                    .SingleOrDefaultAsync(user1 => user1.UserName == userName);
                Console.WriteLine(user);
                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}
