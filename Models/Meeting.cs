using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMS.API.Models
{
    public class Meeting
    {
        [Key]
        [Column("meeting_id")]
        public int MeetingId { get; set; }

        public required string Title { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        [Column("organizer_id")]
        public int OrganizerId { get; set; } // FK to Users table

        [ForeignKey("OrganizerId")]
        public User? Organizer { get; set; } // Navigation property

        public required string Status { get; set; }

        public string? Description { get; set; } // Optional field for meeting description
    }
}
