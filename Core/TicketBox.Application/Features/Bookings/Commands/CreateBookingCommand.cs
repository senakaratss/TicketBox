using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBox.Application.Features.Bookings.Commands
{
    public class CreateBookingCommand:IRequest
    {
        public int EventId { get; set; }
        public int TicketQuantity { get; set; }
        public List<string> SeatNumbers { get; set; } = new();
    }
}
