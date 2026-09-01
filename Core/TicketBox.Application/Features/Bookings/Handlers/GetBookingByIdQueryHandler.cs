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
    public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, GetBookingByIdQueryResult>
    {
       private readonly IBookingRepository _bookingRepository;
        private readonly IIdentityService _identityService;

        public GetBookingByIdQueryHandler(IBookingRepository bookingRepository, IIdentityService identityService)
        {
            _bookingRepository = bookingRepository;
            _identityService = identityService;
        }

        public async Task<GetBookingByIdQueryResult> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            var value = await _bookingRepository.GetBookingByIdAsync(request.BookingId);
            var user = await _identityService.GetUserInfoAsync(value.UserId);

            return new GetBookingByIdQueryResult
            {
                BookingId = request.BookingId,
                EventName = value.Event.Title,
                EventImageUrl = value.Event.ImageUrl,
                EventDate = value.Event.EventDate,
                EventLocation = value.Event.Location,
                TicketQuantity = value.TicketQuantity,
                TotalPrice = value.TotalPrice,
                BookingDate = value.BookingDate,
                UserId=value.UserId,
                UserFullName=user.Name+" "+user.Surname,
                UserEmail=user.Email,
                TicketSerials=value.Tickets.Select(t=>t.SerialNumber).ToArray()
            };
        }
    }
}
