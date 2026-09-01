using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBox.Application.Features.Bookings.Results
{
    public class GetMyBookingsQueryResult
    {
        public int BookingId { get; set; }

        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; }

        public int TicketQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
