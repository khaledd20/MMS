namespace MMS.API.Models
{
    public class RegisterRequest
    {
        public string Name { get; set; } // User's full name

        public string Email { get; set; } // User's email address (must be unique)

        public string Password { get; set; } // User's password (to be hashed in production)

        public int RoleId { get; set; } // Role ID (e.g., 2 for Organizer, 3 for Participant)
    }
}
