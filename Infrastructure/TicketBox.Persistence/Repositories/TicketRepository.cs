using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Interfaces;
using TicketBox.Domain.Entities;
using TicketBox.Domain.Enums;
using TicketBox.Persistence.Context;

namespace TicketBox.Persistence.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketContext _context;

        public TicketRepository(TicketContext context)
        {
            _context = context;
        }

        public async Task<List<Ticket>> GetActiveTicketsByUserIdAsync(string userId)
        {
            return await _context.Tickets.Include(x => x.Booking).ThenInclude(x => x.Event)
                .Where(x => x.Booking.UserId == userId && x.Status == TicketStatus.Active).ToListAsync();
        }

        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            return await _context.Tickets.Include(x => x.Booking).ThenInclude(x => x.Event).ToListAsync();
        }

        public async Task<Ticket> GetMyTicketByIdAsync(string userId, int ticketId)
        {
            return await _context.Tickets.Include(x => x.Booking).ThenInclude(x => x.Event).ThenInclude(x => x.Category)
                .FirstOrDefaultAsync(x => x.Booking.UserId == userId && x.TicketId == ticketId);
        }

        public async Task<List<Ticket>> GetMyTicketsByBookingAsync(string userId, int bookingId)
        {
            return await _context.Tickets.Include(x => x.Booking).ThenInclude(x => x.Event).ThenInclude(x => x.Category)
                .Where(x => x.Booking.UserId == userId && x.BookingId == bookingId).ToListAsync();
        }

        public async Task<List<string>> GetTakenSeatsByEventIdAsync(int eventId)
        {
            return await _context.Tickets.Where(x => x.Booking.EventId == eventId && x.SeatNumber != null)
                .Select(x => x.SeatNumber).ToListAsync();
        }

        public async Task<List<Ticket>> GetTicketsByUserIdAsync(string userId)
        {
            return await _context.Tickets.Include(x => x.Booking).ThenInclude(x => x.Event).ThenInclude(x => x.Category)
                .Where(x => x.Booking.UserId == userId).ToListAsync();
        }
    }
}
