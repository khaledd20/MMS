using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMS.API.Models
{
    public class Role
    {

        [Column("role_id")] 

        public int RoleId { get; set; }

        [Column("role_name")] 

        public required string RoleName { get; set; }
    public required ICollection<Permission_Role> PermissionRole { get; set; }

    }
}
