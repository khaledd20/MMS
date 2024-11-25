using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMS.API.Models
{
    public class Attendee
    {
        [Key]
        public int Id { get; set; }

        [Column("meeting_id")] // Map to the correct column name
        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; }

        [Column("user_id")] // Map to the correct column name
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
