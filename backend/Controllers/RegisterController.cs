using DataStore.Abstraction.Models;
using DataStore.Implementation.Models;
using FeatureObject.Abstraction.FeatureObject;
using FeatureObject.Implementation.FeatureObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Controllers
{
    [Route("api/register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IUserService _userService;

        public RegisterController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        public IActionResult Register([FromBody] User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest(new { Message = "Invalid user data in the request body" });
                }

                if (_userService.IsUsernameTaken(user.Username))
                {
                    return BadRequest(new { Message = "Username is already exist" });
                }

                if(_userService.IsEmailTaken(user.Email))
                {
                    return BadRequest(new { Message = "Email is already registered" });
                }
                _userService.AddUser(user);

                // Generate JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes("your-secret-key-that-is-at-least-32-characters-long"); // Use the same secret key as in Program.cs
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        // Add more claims if needed
                    }),
                    Expires = DateTime.UtcNow.AddDays(1), // Token expiration time
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new { Message = "Registration successful", UserId = user.Id, Token =tokenHandler.WriteToken(token) });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Registration failed:{ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(new{ user,UserId=user.Id});
        }
    }
}
