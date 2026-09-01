using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBox.Application.Interfaces
{
    public interface IBookingEmailTemplate
    {
        string CreateConfirmationEmail(string userName, string eventName, DateTime eventDate, string location, 
            int bookingId, int ticketQuantity, decimal totalPrice);
    }
}
