using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MMS.API.Models
{   
    [Table("Permissions")] // Explicitly specify table name to avoid conflicts

    public class Permissions
    {
        [Key] // Define this as the primary key        
        [Column("permission_id")] 
        
        public int PermissionId { get; set; }

        [Column("permission_name")] 

        public required string PermissionName { get; set; }
        public required ICollection<Permission_Role> PermissionRole { get; set; }

    }
}
