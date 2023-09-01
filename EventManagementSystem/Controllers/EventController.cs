using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManagementSystem.Data;
using EventManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventManagementContext _context;

        public EventController(EventManagementContext context)
        {
            _context = context;
        }

        // GET: api/event
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        // GET: api/event/{eventId}
        [HttpGet("{eventId}")]
        public async Task<ActionResult<Event>> GetEvent(Guid eventId)
        {
            var @event = await _context.Events.FindAsync(eventId);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        // POST: api/event
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(Event @event)
        {
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { eventId = @event.EventId }, @event);
        }

        // PUT: api/event/{eventId}
        [HttpPut("{eventId}")]
        public async Task<IActionResult> UpdateEvent(Guid eventId, Event @event)
        {
            if (eventId != @event.EventId)
            {
                return BadRequest();
            }

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(eventId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/event/{eventId}
        [HttpDelete("{eventId}")]
        public async Task<IActionResult> DeleteEvent(Guid eventId)
        {
            var @event = await _context.Events.FindAsync(eventId);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(Guid eventId)
        {
            return _context.Events.Any(e => e.EventId == eventId);
        }

        // GET: api/events/{eventId}/available-slots
    [HttpGet("{eventId}/available-slots")]
    public async Task<ActionResult<int>> GetAvailableSlots(int eventId)
    {
        var @event = await _context.Events.FindAsync(eventId);

        if (@event == null)
        {
            return NotFound();
        }

        // Calculate available slots based on registered users
        var registeredUsersCount = await _context.Users.Where(u => u.EventId.ToString() == eventId.ToString()).CountAsync();
        var availableSlots = @event.Capacity - registeredUsersCount;

        return availableSlots;
    }

    // GET: api/events/search?location=Nyeri
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Event>>> SearchEventsByLocation([FromQuery] string location)
    {
        var events = await _context.Events.Where(e => e.Location.Equals(location, StringComparison.OrdinalIgnoreCase)).ToListAsync();

        if (!events.Any())
        {
            return NotFound();
        }

        return events;
    }
    }
}
