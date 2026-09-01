using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Features.Bookings.Queries;
using TicketBox.Application.Features.Bookings.Results;
using TicketBox.Application.Interfaces;

namespace TicketBox.Application.Features.Bookings.Handlers
{
    public class GetMyBookingsQueryHandler : IRequestHandler<GetMyBookingsQuery, List<GetMyBookingsQueryResult>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IIdentityService _identityService;

        public GetMyBookingsQueryHandler(IBookingRepository bookingRepository, IIdentityService identityService)
        {
            _bookingRepository = bookingRepository;
            _identityService = identityService;
        }

        public async Task<List<GetMyBookingsQueryResult>> Handle(GetMyBookingsQuery request, CancellationToken cancellationToken)
        {
            var userId = await _identityService.GetCurrentUserIdAsync();
            var bookings = await _bookingRepository.GetBookingsByUserIdAsync(userId);

            return bookings.Select(x => new GetMyBookingsQueryResult
            {
                BookingId = x.BookingId,
                BookingDate = x.BookingDate,
                TicketQuantity = x.TicketQuantity,
                TotalPrice = x.TotalPrice,
                EventId = x.EventId,
                EventName = x.Event.Title,
                EventDate = x.Event.EventDate,
                EventLocation = x.Event.Location
            }).ToList();
        }
    }
}
