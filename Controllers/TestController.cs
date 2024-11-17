using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        // Public endpoint (accessible without authentication)
        [HttpGet("public")]
        public IActionResult PublicEndpoint()
        {
            return Ok("This is a public endpoint accessible by anyone.");
        }

        // Protected endpoint (requires authentication with RoleId = 2)
        [HttpGet("protected")]
        [Authorize] // Authentication required
        public IActionResult ProtectedEndpoint()
        {
            // Retrieve and log the current user's claims
            var userRole = User.FindFirst("RoleId")?.Value;
            var userName = User.Identity?.Name;

            if (userRole == "2")
            {
                return Ok($"Welcome, {userName}. You are authorized for protected access.");
            }
            else
            {
                return Forbid($"Your role ({userRole}) does not have access to this endpoint.");
            }
        }

        // Admin-only endpoint (only accessible by Admins with RoleId = 1)
        [HttpGet("admin-only")]
        [Authorize] // Authentication required
        public IActionResult AdminOnlyEndpoint()
        {
            // Retrieve and log the current user's claims
            var userRole = User.FindFirst("RoleId")?.Value;
            var userName = User.Identity?.Name;

            if (userRole == "1")
            {
                return Ok($"Welcome, {userName}. You are authorized as an Admin!");
            }
            else
            {
                return Forbid($"Your role ({userRole}) does not have admin privileges.");
            }
        }

        [HttpGet("debug-headers")]
        public IActionResult DebugHeaders()
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader))
            {
                return Ok("No Authorization Header found.");
            }
            return Ok(new { AuthorizationHeader = authHeader });
        }
    }
}
