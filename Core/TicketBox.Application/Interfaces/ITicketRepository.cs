using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Domain.Entities;

namespace TicketBox.Application.Interfaces
{
    public interface ITicketRepository
    {
        Task<List<Ticket>> GetAllTicketsAsync();
        Task<List<Ticket>> GetTicketsByUserIdAsync(string userId);
        Task<List<Ticket>> GetMyTicketsByBookingAsync(string userId,int bookingId);
        Task<List<Ticket>> GetActiveTicketsByUserIdAsync(string userId);
        Task<Ticket> GetMyTicketByIdAsync(string userId, int ticketId);
        Task<List<string>> GetTakenSeatsByEventIdAsync(int eventId);
    }
}
