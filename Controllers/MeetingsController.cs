using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMS.API.Data;
using MMS.API.Models;
using System.IdentityModel.Tokens.Jwt;

namespace MMS.API.Controllers
{
    [ApiController]
    [Route("meetings")]
    public class MeetingsController : ControllerBase
    {
        private readonly MMSDbContext _context;
        private readonly IConfiguration _configuration;
        public MeetingsController(MMSDbContext context)
        {
            _context = context;
        }

        // GET: api/meetings
        [HttpGet]
        public async Task<IActionResult> GetMeetings()
        {
            var meetings = await _context.Meetings
                .Include(m => m.Organizer) // Include related organizer details
                .ToListAsync();

            return Ok(meetings);
        }

        // GET: api/meetings/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMeeting(int id)
        {
            var meeting = await _context.Meetings
                .Include(m => m.Organizer)
                .FirstOrDefaultAsync(m => m.MeetingId == id);

            if (meeting == null)
            {
                return NotFound($"Meeting with ID {id} not found.");
            }

            return Ok(meeting);
        }

        // GET: api/meetings/organizer/{organizerId}
        [HttpGet("organizer/{organizerId}")]
        public async Task<IActionResult> GetMeetingsByOrganizer(int organizerId)
        {
            var meetings = await _context.Meetings
                .Where(m => m.OrganizerId == organizerId)
                .Include(m => m.Organizer) // Include organizer details
                .ToListAsync();

            if (!meetings.Any())
            {
                return NotFound("No meetings found for this organizer.");
            }

            return Ok(meetings);
        }

        // POST: api/meetings
        [HttpPost]
        public async Task<IActionResult> CreateMeeting([FromBody] Meeting meeting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate that the organizer exists
            var organizer = await _context.Users.FindAsync(meeting.OrganizerId);
            if (organizer == null)
            {
                return BadRequest("Invalid organizer ID.");
            }

            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMeeting), new { id = meeting.MeetingId }, meeting);
        }



        // PUT: api/meetings/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMeeting(int id, [FromBody] Meeting meeting)
        {
            if (id != meeting.MeetingId)
            {
                return BadRequest("Meeting ID mismatch.");
            }

            var existingMeeting = await _context.Meetings
                    .AsNoTracking() // Prevent EF from tracking relationships like Attendees
                    .FirstOrDefaultAsync(m => m.MeetingId == id);            
            
            if (existingMeeting == null)
            {
                return NotFound($"Meeting with ID {id} not found.");
            }

             // Update only the fields that are part of the Meeting
            existingMeeting.Title = meeting.Title;
            existingMeeting.Date = meeting.Date;
            existingMeeting.Time = meeting.Time;
            existingMeeting.Status = meeting.Status;
            existingMeeting.Description = meeting.Description;
            existingMeeting.MeetingURL = meeting.MeetingURL;
            existingMeeting.Agenda = meeting.Agenda;
            existingMeeting.OrganizerId = meeting.OrganizerId;

            _context.Entry(existingMeeting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(existingMeeting); // Return updated Meeting
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Error updating meeting: {ex.Message}");
            }
        }


        // DELETE: api/meetings/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeeting(int id)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting == null)
            {
                return NotFound($"Meeting with ID {id} not found.");
            }

            _context.Meetings.Remove(meeting);
            await _context.SaveChangesAsync();

            return Ok("Meeting deleted successfully.");
        }

     }
}
