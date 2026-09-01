using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Domain.Enums;

namespace TicketBox.Application.Features.Tickets.Results
{
    public class GetMyTicketsByBookingQueryResult
    {
        public int TicketId { get; set; }

        public string CategoryName { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventImageUrl { get; set; }
        public string EventLocation { get; set; }
        public DateTime EventDate { get; set; }

        public string Holder { get; set; }
        public string HolderEmail { get; set; }

        public string SerialNumber { get; set; } = string.Empty;
        public string? SeatNumber { get; set; }
        public string? QRCode { get; set; }
        public string? TicketImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public TicketStatus Status { get; set; }
    }
}
