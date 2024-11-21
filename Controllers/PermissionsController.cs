using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMS.API.Data;
using System.Linq;

namespace MMS.API.Controllers
{
    [ApiController]
    [Route("api/permissions")]
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

            // Convert roleId from string to integer
            if (!int.TryParse(userRoleId, out int roleId))
            {
                return BadRequest("Invalid RoleId in token.");
            }

            // Fetch permissions using joins across the three tables
            var permissions = (from pr in _context.Permission_Role
                               join p in _context.Permissions on pr.PermissionId equals p.PermissionId
                               join r in _context.Roles on pr.RoleId equals r.RoleId
                               where pr.RoleId == roleId
                               select new
                               {
                                   PermissionName = p.PermissionName,
                                   RoleName = r.RoleName
                               }).ToList();

            if (!permissions.Any())
            {
                return NotFound("No permissions found for the specified role.");
            }

            return Ok(new
            {
                RoleId = roleId,
                Permissions = permissions
            });
        }
    }
}
