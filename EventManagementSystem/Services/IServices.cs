using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Models;

namespace EventManagementSystem.Services
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event> GetEventByIdAsync(Guid eventId);
        Task<Event> CreateEventAsync(Event eventModel);
        Task<bool> UpdateEventAsync(Guid eventId, Event eventModel);
        Task<bool> DeleteEventAsync(Guid eventId);
        Task<int> GetAvailableSlotsAsync(Guid eventId); 
        Task<IEnumerable<Event>> SearchEventsByLocationAsync(string location);
    }

    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(Guid userId, User user);
        Task<bool> DeleteUserAsync(Guid userId);
    }
}
