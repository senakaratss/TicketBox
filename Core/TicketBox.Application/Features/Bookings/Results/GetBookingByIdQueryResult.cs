using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBox.Application.Features.Bookings.Results
{
    public class GetBookingByIdQueryResult
    {
        public int BookingId { get; set; }

        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }

        public string EventName { get; set; }
        public string EventImageUrl { get; set; }
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; }

        public int TicketQuantity { get; set; }
        public string[] TicketSerials { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
