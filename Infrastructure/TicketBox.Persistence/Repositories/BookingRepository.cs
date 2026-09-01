using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Interfaces;
using TicketBox.Domain.Entities;
using TicketBox.Persistence.Context;

namespace TicketBox.Persistence.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly TicketContext _context;

        public BookingRepository(TicketContext context)
        {
            _context = context;
        }

        public async Task CreateBookingAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Booking>> GetAllBookingAsync()
        {
            return await _context.Bookings.Include(x=>x.Event).ToListAsync();
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings.Include(x => x.Event).Include(x=>x.Tickets).FirstOrDefaultAsync(x => x.BookingId == id);
        }

        public async Task<List<Booking>> GetBookingsByUserIdAsync(string userId)
        {
            return await _context.Bookings.Where(x => x.UserId == userId).Include(x => x.Event).ToListAsync();
        }
    }
}
