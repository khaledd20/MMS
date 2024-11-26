using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMS.API.Models
{
    public class MeetingMinute
    {
        [Key]
        [Column("minute_id")]
        public int MinuteId { get; set; } // Primary Key

        [Column("meeting_id")]
        public int MeetingId { get; set; } // Foreign Key referencing Meetings

        [ForeignKey("MeetingId")]
        public Meeting? Meeting { get; set; } // Navigation property

        [Required]
        public string Content { get; set; } = string.Empty; // Details of the meeting minute

        public DateTime Timestamp { get; set; } = DateTime.UtcNow; // Created/Modified timestamp
    }
}
