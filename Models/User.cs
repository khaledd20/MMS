using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMS.API.Models
{
    [Table("Users")] // Explicitly map the model to the "Users" table
    public class User
    {
        [Key] // Define this as the primary key
        [Column("user_id")] // Map to the database column "user_id"
        public int UserId { get; set; }

        [Column("name")] // Map to the database column "name"
        public required string Name { get; set; }

        [Column("email")] // Map to the database column "email"
        public required string Email { get; set; }

        [Column("password")] // Map to the database column "password"
        public required string Password { get; set; }

        [Column("role_id")] // Map to the database column "role_id"
        public int RoleId { get; set; }

        // Navigation property to Role
        public Role? Role { get; set; }
    }
}
