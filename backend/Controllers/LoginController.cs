using DataStore.Abstraction.Models;
using DataStore.Implementation.Models;
using FeatureObject.Abstraction.FeatureObject;
using FeatureObject.Implementation.FeatureObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }
        //[HttpGet]
        [HttpPost]
        public IActionResult Login([FromBody] SingleUser user)
        {
            try
            {
                var loggedInUser = _loginService.Login(user.Username, user.Email, user.Password);
                if (loggedInUser != null)
                {
                    // Return UserId along with the response
                    return Ok(new { Message = "Login successful", UserId = loggedInUser.Id, isAdmin = loggedInUser.IsAdmin });
                }
                else
                {
                    return Unauthorized(new { Message = "Invalid email or password" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Login failed: {ex.Message}" });
            }
        }

        /*[HttpGet("{username}")]
        public async Task<ActionResult<IUser>> GetUserByName(string username)
        {
            try
            {
                // Call the service method to retrieve user details by username
                var user = await _loginService.GetUserByName(username);

                if (user == null)
                {
                    return NotFound($"User with username '{username}' not found.");
                }

                // For security reasons, you may want to remove sensitive data like password
                // before returning the user details
                user.Password = null;

                return Ok(user);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "An error occurred while retrieving user details.");
            }
        }*/

        [HttpGet("{id}")]
        public IActionResult GetloginUserById(int id)
        {
            var user = _loginService.GetloginUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(new { user, UserId = user.Id });
        }

    }
}
