using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMS.API.Models
{
    public class Permission_Role
    {
        public int Id { get; set; }

        [Column("role_id")]
        public int RoleId { get; set; }

        [Column("permission_id")]
        public int PermissionId { get; set; }

        // Navigation properties
        public required Role Role { get; set; }
        public required Permissions Permission { get; set; }
    }
}
