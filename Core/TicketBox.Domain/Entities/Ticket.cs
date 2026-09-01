using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Domain.Enums;

namespace TicketBox.Domain.Entities
{
    public class Ticket
    {
        public int TicketId { get; set; }

        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        public string SerialNumber { get; set; } = string.Empty;
        public string? SeatNumber { get; set; }
        public string? QRCode { get; set; }
        public string? TicketImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public TicketStatus Status { get; set; }
    }
}
