using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBox.Application.Features.Events.Results
{
    public class GetEventByIdQueryResult
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Organizer { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime EventDate { get; set; }
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public string ImageUrl { get; set; }
        public bool Status { get; set; }
        public bool HasSeatSelection { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryIcon { get; set; }

        public int RemainingTickets { get; set; }
        public int SoldTickets { get; set; }
    }
}
