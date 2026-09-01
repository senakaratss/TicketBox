using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBox.Domain.Entities
{
    public class Event
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
        public Category Category { get; set; }

        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
