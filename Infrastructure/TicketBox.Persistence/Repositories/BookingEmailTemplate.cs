using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Interfaces;

namespace TicketBox.Persistence.Repositories
{
    public class BookingEmailTemplate : IBookingEmailTemplate
    {
        public string CreateConfirmationEmail(string userName, string eventName, DateTime eventDate, string location,
            int bookingId, int ticketQuantity, decimal totalPrice)
        {
            return $@"
            <h2>Booking Confirmed!</h2>

            <p>Hello {userName},</p>

            <p>Your booking has been successfully created.</p>

            <p>
                <strong>Event:</strong> {eventName}
            </p>

            <p>
                <strong>Date:</strong>
                {eventDate:dd.MM.yyyy HH:mm}
            </p>

            <p>
                <strong>Location:</strong> {location}
            </p>

            <p>
                <strong>BookingId:</strong>
                {bookingId}
            </p>

            <p>
                <strong>Ticket Quantity:</strong>
                {ticketQuantity}
            </p>

            <p>
                <strong>Total Price:</strong>
                {totalPrice:C}
            </p>

            <p>
                Your digital ticket(s) are attached to this email.
            </p>

            <p>
                Thank you for choosing TicketBox.
            </p>
        ";
        }
    }
}
