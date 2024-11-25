using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMS.API.Data;
using MMS.API.Models;

namespace MMS.API.Controllers
{
    [ApiController]
    [Route("meetings/{meetingId}/attendees")]
    public class AttendeesController : ControllerBase
    {
        private readonly MMSDbContext _context;

        public AttendeesController(MMSDbContext context)
        {
            _context = context;
        }

        // GET: meetings/{meetingId}/attendees
        [HttpGet]
        public async Task<IActionResult> GetAttendees(int meetingId)
        {
            var meeting = await _context.Meetings
                .Include(m => m.Attendees)
                .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(m => m.MeetingId == meetingId);

            if (meeting == null)
            {
                return NotFound($"Meeting with ID {meetingId} not found.");
            }

            var attendees = meeting.Attendees.Select(a => new
            {
                a.Id,
                a.UserId,
                a.User.Name,
                a.User.Email
            }).ToList();

            return Ok(attendees);
        }

        // POST: meetings/{meetingId}/attendees
        [HttpPost]
        public async Task<IActionResult> AddAttendee(int meetingId, [FromBody] AttendeeRequest request)
        {
            if (request == null || request.UserId <= 0 || meetingId <= 0)
            {
                return BadRequest(new { message = "Invalid user or meeting data." });
            }

            var meeting = await _context.Meetings.FindAsync(meetingId);
            if (meeting == null)
            {
                return NotFound(new { message = "Meeting not found." });
            }

            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return BadRequest(new { message = "Invalid User ID." });
            }

            var existingAttendee = await _context.Attendees
                .FirstOrDefaultAsync(a => a.MeetingId == meetingId && a.UserId == request.UserId);

            if (existingAttendee != null)
            {
                return Conflict(new { message = "User is already an attendee." });
            }

            var attendee = new Attendee
            {
                MeetingId = meetingId,
                UserId = request.UserId
            };

            _context.Attendees.Add(attendee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAttendees), new { meetingId }, attendee);
        }


        // DELETE: meetings/{meetingId}/attendees/{attendeeId}
        [HttpDelete("{attendeeId}")]
        public async Task<IActionResult> RemoveAttendee(int meetingId, int attendeeId)
        {
            var attendee = await _context.Attendees
                .FirstOrDefaultAsync(a => a.Id == attendeeId && a.MeetingId == meetingId);

            if (attendee == null)
            {
                return NotFound($"Attendee with ID {attendeeId} not found for Meeting ID {meetingId}.");
            }

            _context.Attendees.Remove(attendee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }   
}
    

