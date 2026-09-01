using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Domain.Entities;

namespace TicketBox.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task CreateBookingAsync(Booking booking);
        Task<List<Booking>> GetAllBookingAsync();
        Task<Booking> GetBookingByIdAsync(int id);
        Task<List<Booking>> GetBookingsByUserIdAsync(string userId);
    }
}
