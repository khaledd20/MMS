using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMS.API.Data;
using MMS.API.Models;

namespace MMS.API.Controllers
{
    [ApiController]
    [Route("meetings/{meetingId}/minutes")]
    public class MeetingMinutesController : ControllerBase
    {
        private readonly MMSDbContext _context;

        public MeetingMinutesController(MMSDbContext context)
        {
            _context = context;
        }

        // GET: /meetings/{meetingId}/minutes
        [HttpGet]
        public async Task<IActionResult> GetMinutes(int meetingId)
        {
            var minutes = await _context.Minutes
                .Where(m => m.MeetingId == meetingId)
                .ToListAsync();

            if (minutes == null || !minutes.Any())
            {
                return NotFound("No minutes found for the specified meeting.");
            }

            return Ok(minutes);
        }

        // POST: /meetings/{meetingId}/minutes
        [HttpPost]
        public async Task<IActionResult> AddMinute(int meetingId, [FromBody] MeetingMinute minute)
        {
            if (minute == null || string.IsNullOrWhiteSpace(minute.Content))
            {
                return BadRequest("Invalid meeting minute data.");
            }

            var meeting = await _context.Meetings.FindAsync(meetingId);
            if (meeting == null)
            {
                return NotFound("Meeting not found.");
            }

            minute.MeetingId = meetingId;
            _context.Minutes.Add(minute);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMinutes), new { meetingId }, minute);
        }

        // PUT: /meetings/{meetingId}/minutes/{minuteId}
        [HttpPut("{minuteId}")]
        public async Task<IActionResult> UpdateMinute(int meetingId, int minuteId, [FromBody] MeetingMinute updatedMinute)
        {
            if (updatedMinute == null || string.IsNullOrWhiteSpace(updatedMinute.Content))
            {
                return BadRequest("Invalid meeting minute data.");
            }

            var existingMinute = await _context.Minutes
                .FirstOrDefaultAsync(m => m.MinuteId == minuteId && m.MeetingId == meetingId);

            if (existingMinute == null)
            {
                return NotFound("Minute not found.");
            }

            // Update the content and timestamp
            existingMinute.Content = updatedMinute.Content;
            existingMinute.Timestamp = DateTime.UtcNow;

            _context.Minutes.Update(existingMinute);
            await _context.SaveChangesAsync();

            return Ok(existingMinute);
        }


        // DELETE: /meetings/{meetingId}/minutes/{minuteId}
        [HttpDelete("{minuteId}")]
        public async Task<IActionResult> DeleteMinute(int meetingId, int minuteId)
        {
            var minute = await _context.Minutes
                .FirstOrDefaultAsync(m => m.MinuteId == minuteId && m.MeetingId == meetingId);

            if (minute == null)
            {
                return NotFound("Minute not found.");
            }

            _context.Minutes.Remove(minute);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
