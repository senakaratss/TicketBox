using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBox.Application.Features.Events.Results
{
    public class GetEventSeatsQueryResult
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }

        public List<string> TakenSeats { get; set; } = new();
    }
}
