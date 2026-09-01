using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Domain.Entities;

namespace TicketBox.Application.Interfaces
{
    public interface IEventRepository
    {
        Task CreateEventAsync(Event @event);
        Task UpdateEventAsync(Event @event);
        Task DeleteEventAsync(int id);
        Task<List<Event>> GetAllEventsAsync(string? search, int? categoryId, string? location, DateTime? date);
        Task<List<Event>> GetFeaturedEventsAsync();
        Task<List<Event>> GetUpcominEventsAsync();
        Task<Event> GetEventByIdAsync(int id);
    }
}
