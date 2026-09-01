using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TicketBox.Domain.Entities
{
    public class Booking
    {
        public int BookingId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        public int TicketQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime BookingDate { get; set; }

        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
