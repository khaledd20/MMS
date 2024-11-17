namespace MMS.API.Models
{
    public class Meeting
    {
        public int MeetingId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int OrganizerId { get; set; }
        public User Organizer { get; set; }
        public string Status { get; set; }
    }
}
