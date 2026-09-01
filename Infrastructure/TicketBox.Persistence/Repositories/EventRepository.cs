using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Interfaces;
using TicketBox.Domain.Entities;
using TicketBox.Persistence.Context;

namespace TicketBox.Persistence.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly TicketContext _context;

        public EventRepository(TicketContext context)
        {
            _context = context;
        }

        public async Task CreateEventAsync(Event @event)
        {
            await _context.Events.AddAsync(@event);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(int id)
        {
            var value = await _context.Events.FindAsync(id);
            _context.Events.Remove(value);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Event>> GetAllEventsAsync(string? search, int? categoryId, string? location, DateTime? date)
        {
            var query = _context.Events.Include(x => x.Category).Include(x => x.Bookings).AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Title.Contains(search));
            }
            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(x => x.Location.Contains(location));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }
            if (date.HasValue)
            {
                var startDate = date.Value.Date;
                var endDate = date.Value.Date.AddDays(1);

                query = query.Where(x => x.EventDate >= startDate && x.EventDate < endDate);
            }
            return await query.ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _context.Events.Include(x => x.Category).Include(x => x.Bookings).FirstOrDefaultAsync(x => x.EventId == id);
        }

        public async Task<List<Event>> GetFeaturedEventsAsync()
        {
            return await _context.Events.Include(x => x.Category).Include(x => x.Bookings)
                .OrderByDescending(x => x.Bookings.Sum(x => x.TicketQuantity))
                .Take(6).ToListAsync();
        }

        public async Task<List<Event>> GetUpcominEventsAsync()
        {
            var today = DateTime.Today;
            var startOfNextMonth = new DateTime(today.Year, today.Month, 1).AddMonths(1);

            return await _context.Events.Include(x => x.Category).Include(x => x.Bookings)
                .Where(x => x.EventDate >= today && x.EventDate <= startOfNextMonth)
                .OrderBy(x => x.EventDate).Take(3).ToListAsync();
        }

        public async Task UpdateEventAsync(Event @event)
        {
            _context.Events.Update(@event);
            await _context.SaveChangesAsync();
        }
    }
}
