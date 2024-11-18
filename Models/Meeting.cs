using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMS.API.Models
{
    public class Meeting
    {
        [Column("meeting_id")] 

        public int MeetingId { get; set; }
        public required string Title { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }

        [Column("organizer_id")] 

        public int OrganizerId { get; set; }
        public required User Organizer { get; set; }
        public required string Status { get; set; }
    }
}
