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
    public class GetMyTicketQueryHandler : IRequestHandler<GetMyTicketQuery, List<GetMyTicketQueryResult>>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IIdentityService _identityService;

        public GetMyTicketQueryHandler(ITicketRepository ticketRepository, IIdentityService identityService)
        {
            _ticketRepository = ticketRepository;
            _identityService = identityService;
        }

        public async Task<List<GetMyTicketQueryResult>> Handle(GetMyTicketQuery request, CancellationToken cancellationToken)
        {
            var userId = await _identityService.GetCurrentUserIdAsync();
            var values = await _ticketRepository.GetTicketsByUserIdAsync(userId);

            return values.Select(x => new GetMyTicketQueryResult
            {
                TicketId = x.TicketId,
                SerialNumber = x.SerialNumber,
                SeatNumber = x.SeatNumber,
                QRCode = x.QRCode,
                TicketImageUrl = x.TicketImageUrl,
                CreatedDate = x.CreatedDate,
                Status = x.Status,

                CategoryName = x.Booking.Event.Category.CategoryName,

                EventName = x.Booking.Event.Title,
                EventDate = x.Booking.Event.EventDate,
                EventImageUrl = x.Booking.Event.ImageUrl,
                EventLocation = x.Booking.Event.Location,

            }).ToList();

        }
    }
}
