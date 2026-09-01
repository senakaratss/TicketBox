using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Features.Tickets.Queries;
using TicketBox.Application.Features.Tickets.Results;
using TicketBox.Application.Interfaces;

namespace TicketBox.Application.Features.Tickets.Handlers
{
    public class GetMyTicketDetailsQueryHandler : IRequestHandler<GetMyTicketDetailsQuery, GetMyTicketDetailsQueryResult>
    {
        private readonly IIdentityService _identityService;
        private readonly ITicketRepository _ticketRepository;

        public GetMyTicketDetailsQueryHandler(IIdentityService identityService, ITicketRepository ticketRepository)
        {
            _identityService = identityService;
            _ticketRepository = ticketRepository;
        }

        public async Task<GetMyTicketDetailsQueryResult> Handle(GetMyTicketDetailsQuery request, CancellationToken cancellationToken)
        {
            var userId = await _identityService.GetCurrentUserIdAsync();
            var userInfo = await _identityService.GetUserInfoAsync(userId);

            var value = await _ticketRepository.GetMyTicketByIdAsync(userId, request.TicketId);
            return new GetMyTicketDetailsQueryResult
            {
                TicketId = value.TicketId,
                SerialNumber = value.SerialNumber,
                SeatNumber = value.SeatNumber,
                QRCode = value.QRCode,
                TicketImageUrl = value.TicketImageUrl,
                CreatedDate = value.CreatedDate,
                Status = value.Status,

                Holder = userInfo.Name + " " + userInfo.Surname,
                HolderEmail = userInfo.Email,

                CategoryName = value.Booking.Event.Category.CategoryName,

                EventId = value.Booking.Event.EventId,
                EventName = value.Booking.Event.Title,
                EventDate = value.Booking.Event.EventDate,
                EventImageUrl = value.Booking.Event.ImageUrl,
                EventLocation = value.Booking.Event.Location,
            };
        }
    }
}
