using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Features.Bookings.Queries;
using TicketBox.Application.Features.Bookings.Results;
using TicketBox.Application.Interfaces;
using TicketBox.Domain.Entities;

namespace TicketBox.Application.Features.Bookings.Handlers
{
    public class GetBookingsQueryHandler : IRequestHandler<GetBookingsQuery, List<GetBookingsQueryResult>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IIdentityService _identityService;

        public GetBookingsQueryHandler(IBookingRepository bookingRepository, IIdentityService identityService)
        {
            _bookingRepository = bookingRepository;
            _identityService = identityService;
        }

        public async Task<List<GetBookingsQueryResult>> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _bookingRepository.GetAllBookingAsync();

            // Get all users at once instead of querying the database for each booking.
            // Use a Dictionary instead of FirstOrDefault to quickly find the user by UserId.
            //TO PREVENT N+1 PROBLEM
            var users = await _identityService.GetAllUsersAsync();
            var userDictionary = users.ToDictionary(x => x.Id);

            var result = bookings.Select(booking =>
            {
                var user = userDictionary[booking.UserId];

                return new GetBookingsQueryResult
                {
                    BookingId = booking.BookingId,
                    UserId = booking.UserId,
                    UserFullName = user.Name + " " + user.Surname,
                    UserEmail = user.Email,
                    EventName = booking.Event.Title,
                    BookingDate = booking.BookingDate,
                    TicketQuantity = booking.TicketQuantity,
                    TotalPrice = booking.TotalPrice
                };
            }).ToList();
            return result;
        }
    }
}
