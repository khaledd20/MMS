using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MMS.API.Data;
using MMS.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
            // Validate user
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null || user.Password != request.Password)
            {
                return Unauthorized("Invalid email or password.");
            }

            // Generate JWT Token using the helper method
            var tokenString = GenerateJwtToken(user);

            // Return token in a detailed format
            return Ok(new
            {
                accessToken = tokenString,
                tokenType = "Bearer",
                expiresIn = 3600 // 1 hour in seconds
            });
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]); // Replace with a secure key from configuration

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("RoleId", user.RoleId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

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
