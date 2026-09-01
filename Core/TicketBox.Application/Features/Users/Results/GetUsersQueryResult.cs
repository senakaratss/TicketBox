using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBox.Application.Features.Users.Results
{
    public class GetUsersQueryResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public bool EmailConfirmed { get; set; }
        public int BookingCount { get; set; }
        public int TicketCount { get; set; }
    }
}
