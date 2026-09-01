using TicketBox.Application.Features.Bookings.Results;
using TicketBox.Application.Features.Events.Results;
using TicketBox.Application.Features.Tickets.Results;

namespace TicketBox.WebUI.Models
{
    public class DashboardViewModel
    {
        public string Username { get; set; }
        public int ActiveTicketCount { get; set; }
        public int TotalBookingCount { get; set; }
        public int UpcomingBookingCount { get; set; }
        public List<EventCardQueryResult> MyUpcomingBookings { get; set; }
        public List<GetMyTicketQueryResult> MyActiveTickets { get; set; }
    }
}
