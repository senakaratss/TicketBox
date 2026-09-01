using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Domain.Enums;

namespace TicketBox.Application.Features.Tickets.Results
{
    public class GetAllTicketsQueryResult
    {
        public int TicketId { get; set; }
        public string SerialNumber { get; set; }
        public string EventName { get; set; }
        public string UserId { get; set; }
        public string Holder { get; set; }
        public string SeatNumber { get; set; }
        public TicketStatus Status { get; set; }
    }
}
