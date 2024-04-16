using SolarWatchProject.Models;
using SolarWatchProject.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SolarWatchProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<User> _userManager;

        public UserController(
        ILogger<UserController> logger,
        UserManager<User> userManager)
        {
            _logger = logger;
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
