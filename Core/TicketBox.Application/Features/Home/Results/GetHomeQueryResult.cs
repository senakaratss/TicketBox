using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Features.Bookings.Results;
using TicketBox.Application.Features.Events.Results;
using TicketBox.Application.Features.Tickets.Results;

namespace TicketBox.Application.Features.Home.Results
{
    public class GetHomeQueryResult
    {
        public int ActiveTicketCount { get; set; }
        public int TotalBookingCount { get; set; }
        public int UpcomingBookingCount { get; set; }
        public List<EventCardQueryResult> MyUpcomingBookings { get; set; }
        public List<GetMyTicketQueryResult> MyActiveTickets { get; set; }
    }
}
