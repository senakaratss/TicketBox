using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Features.Bookings.Results;
using TicketBox.Application.Features.Events.Results;
using TicketBox.Application.Features.Home.Queries;
using TicketBox.Application.Features.Home.Results;
using TicketBox.Application.Features.Tickets.Results;
using TicketBox.Application.Interfaces;

namespace TicketBox.Application.Features.Home.Handlers
{
    public class GetHomeQueryHandler : IRequestHandler<GetHomeQuery, GetHomeQueryResult>
    {
        private readonly IIdentityService _identityService;
        private readonly ITicketRepository _ticketRepository;
        private readonly IBookingRepository _bookingRepository;

        public GetHomeQueryHandler(IIdentityService identityService, ITicketRepository ticketRepository, IBookingRepository bookingRepository)
        {
            _identityService = identityService;
            _ticketRepository = ticketRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<GetHomeQueryResult> Handle(GetHomeQuery request, CancellationToken cancellationToken)
        {
            var userId = await _identityService.GetCurrentUserIdAsync();

            var activeTickets = await _ticketRepository.GetActiveTicketsByUserIdAsync(userId);
            var bookings = await _bookingRepository.GetBookingsByUserIdAsync(userId);
            var upcomingBookings = bookings.Where(x => x.Event.EventDate >= DateTime.Now).ToList();

            return new GetHomeQueryResult
            {
                MyActiveTickets = activeTickets.Select(x => new GetMyTicketQueryResult
                {
                    TicketId = x.TicketId,
                    EventName = x.Booking.Event.Title,
                    EventLocation = x.Booking.Event.Location,
                    EventDate = x.Booking.Event.EventDate,
                    SeatNumber = x.SeatNumber
                }).ToList(),
                MyUpcomingBookings = upcomingBookings.Select(x => new EventCardQueryResult
                {
                    EventId = x.EventId,
                    Title = x.Event.Title,
                    Organizer = x.Event.Organizer,
                    Location = x.Event.Location,
                    EventDate = x.Event.EventDate,
                    Price = x.Event.Price,
                    Capacity = x.Event.Capacity,
                    ImageUrl = x.Event.ImageUrl,
                    RemainingTickets = x.Event.Capacity - x.Event.Bookings.Sum(b=>b.TicketQuantity)
                }).ToList(),
                ActiveTicketCount = activeTickets.Count,
                TotalBookingCount = bookings.Count,
                UpcomingBookingCount = upcomingBookings.Count
            };
        }
    }
}
