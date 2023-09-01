using EventManagementSystem.Data;
using EventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using EventManagementSystem.Controllers;
using EventManagementSystem.Data;

namespace EventManagementSystem.Services
{
    public class EventService : IEventService
    {
        private readonly EventManagementContext _context;

        public EventService(EventManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(Guid eventId)
        {
            return await _context.Events.FindAsync(eventId);
        }

        public async Task<Event> CreateEventAsync(Event eventModel)
        {
            _context.Events.Add(eventModel);
            await _context.SaveChangesAsync();
            return eventModel;
        }

        public async Task<bool> UpdateEventAsync(Guid eventId, Event eventModel)
        {
            if (eventId != eventModel.EventId)
            {
                return false;
            }

            _context.Entry(eventModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(eventId))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        public async Task<bool> DeleteEventAsync(Guid eventId)
        {
            var eventModel = await _context.Events.FindAsync(eventId);
            if (eventModel == null)
            {
                return false;
            }

            _context.Events.Remove(eventModel);
            await _context.SaveChangesAsync();
            return true;
        }

        private bool EventExists(Guid eventId)
        {
            return _context.Events.Any(e => e.EventId == eventId);
        }

        public async Task<int> GetAvailableSlotsAsync(Guid eventId)
        {
            var @event = await _context.Events.FindAsync(eventId);

            if (@event == null)
            {
                return -1; // Event not found
            }

            // Calculate available slots based on registered users
            var registeredUsersCount = await _context.Users.Where(u => u.EventId == eventId).CountAsync();
            var availableSlots = @event.Capacity - registeredUsersCount;

            return availableSlots;
        }

        public async Task<IEnumerable<Event>> SearchEventsByLocationAsync(string location)
        {
            var events = await _context.Events
            .Where(e => e.Location.Equals(location, StringComparison.OrdinalIgnoreCase))
            .ToListAsync();

            return events;
        }
    }
}
