using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMS.API.Data;
using MMS.API.Models;
using System.Linq;

namespace MMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly MMSDbContext _context;

        public PermissionsController(MMSDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize] // Requires authentication
        public IActionResult GetPermissions()
        {
            // Retrieve the user's role from JWT claims
            var userRoleId = User.FindFirst("RoleId")?.Value;

            if (string.IsNullOrEmpty(userRoleId))
            {
                return Unauthorized("User role not found in token.");
            }

            // Fetch permissions based on the user's role
            var roleId = int.Parse(userRoleId);
            var permissions = _context.Permission_Role
                .Where(pr => pr.RoleId == roleId)
                .Select(pr => pr.Permission.Name)
                .ToList();

            if (permissions == null || !permissions.Any())
            {
                return NotFound("No permissions found for the specified role.");
            }

            return Ok(new { RoleId = roleId, Permissions = permissions });
        }
    }
}
