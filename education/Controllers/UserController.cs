using Microsoft.AspNetCore.Mvc;
using education.Users;
using System.Threading.Tasks;

namespace education.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync(UserEntity newUser)
        {
            bool result = await _userService.RegisterUserAsync(newUser);
            if (result)
            {
                return Ok("User registered successfully");
            }
            else
            {
                return BadRequest("Failed to register user");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(string username, string password)
        {
            bool result = await _userService.LoginAsync(username, password);
            if (result)
            {
                return Ok("Login successful");
            }
            else
            {
                return Unauthorized("Invalid username or password");
            }
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserDetailsAsync(string username)
        {
            var userDetails = await _userService.GetUserDetailsAsync(username);
            if (userDetails != null)
            {
                return Ok(userDetails);
            }
            else
            {
                return NotFound("User not found");
            }
        }
    }
}
