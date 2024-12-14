using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MMS.API.Data;
using MMS.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace MMS.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly MMSDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(MMSDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.Users
                .Include(u => u.Role) // Include the Role navigation property
                .FirstOrDefault(u => u.Email == request.Email);

            if (user == null || user.Password != request.Password)
            {
                return Unauthorized("Invalid email or password.");
            }

            // Fetch permissions based on the user's role
            var permissions = _context.Permission_Role
                .Include(pr => pr.Permission) // Include Permission entity
                .Where(pr => pr.RoleId == user.RoleId)
                .Select(pr => pr.Permission.PermissionName) // Access PermissionName
                .ToList();

            // Generate JWT token with permissions
            var token = GenerateJwtToken(user, permissions);

            return Ok(new
            {
                Bearer = token,
                TokenType = "Bearer",
                ExpiresIn = 3600
            });
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            // Check if the email is already registered
            if (_context.Users.Any(u => u.Email == request.Email))
            {
                return Conflict("Email is already registered.");
            }

            // Create a new user
            var newUser = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password, // NOTE: Hash the password in a real-world application
                RoleId = request.RoleId
            };

            // Add user to the database
            _context.Users.Add(newUser);
            _context.SaveChanges();
             return Ok();

        }
         // GET ALL USERS ENDPOINT
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users.Select(u => new
            {
                u.UserId,
                u.Name,
                u.Email,
                u.Password,
                u.RoleId
            }).ToList();

            return Ok(users);
        }

        [HttpGet("search")]
        public IActionResult SearchUser(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query cannot be empty.");
            }

            var user = _context.Users
                .Where(u => u.Name.Contains(query) || u.Email.Contains(query))
                .Select(u => new
                {
                    u.UserId,
                    u.Name,
                    u.Email
                })
                .ToList();

            if (!user.Any())
            {
                return NotFound("No user found matching the query.");
            }

            return Ok(user);
        }
        [HttpPut("users/{userId}")]
        public IActionResult UpdateUser(int userId, [FromBody] User updateRequest)
        {
            Console.WriteLine($"Received update request for UserId: {userId}");
            Console.WriteLine($"Name: {updateRequest.Name}");
            Console.WriteLine($"Email: {updateRequest.Email}");
            Console.WriteLine($"Password: {updateRequest.Password}");

            // Find the user by ID
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Validate and update fields only if provided
            if (!string.IsNullOrWhiteSpace(updateRequest.Name))
            {
                user.Name = updateRequest.Name;
            }

            if (!string.IsNullOrWhiteSpace(updateRequest.Email))
            {
                // Ensure email is not already in use by another user
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == updateRequest.Email && u.UserId != userId);
                if (existingUser != null)
                {
                    return Conflict("Email is already in use.");
                }
                user.Email = updateRequest.Email;
            }

            if (!string.IsNullOrWhiteSpace(updateRequest.Password))
            {
                // Directly save the password without hashing (not recommended for production)
                user.Password = updateRequest.Password;
            }

            try
            {
                _context.SaveChanges();
                return Ok(new { message = "User updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("users/{userId}/role")]
        public IActionResult UpdateUserRole(int userId, [FromBody] int newRoleId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Update RoleId
            if (newRoleId != 0)
            {
                user.RoleId = newRoleId;
            }
            else
            {
                return BadRequest("Invalid RoleId.");
            }

            try
            {
                _context.SaveChanges();
                return Ok(new { message = "User role updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpDelete("users/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok();
        }

        private string GenerateJwtToken(User user, List<string> permissions)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("RoleId", user.RoleId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.UserId.ToString())
            };

            // Add permissions as claims
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("Permission", permission));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
