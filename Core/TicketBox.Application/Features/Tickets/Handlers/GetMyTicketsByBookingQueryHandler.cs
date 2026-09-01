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
    public class GetMyTicketsByBookingQueryHandler : IRequestHandler<GetMyTicketsByBookingQuery, List<GetMyTicketsByBookingQueryResult>>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IIdentityService _identityService;

        public GetMyTicketsByBookingQueryHandler(ITicketRepository ticketRepository, IIdentityService identityService)
        {
            _ticketRepository = ticketRepository;
            _identityService = identityService;
        }

        public async Task<List<GetMyTicketsByBookingQueryResult>> Handle(GetMyTicketsByBookingQuery request, CancellationToken cancellationToken)
        {
            var userId = await _identityService.GetCurrentUserIdAsync();
            var userInfo = await _identityService.GetUserInfoAsync(userId);
            
            var values = await _ticketRepository.GetMyTicketsByBookingAsync(userId, request.BookingId);

            return values.Select(x => new GetMyTicketsByBookingQueryResult
            {
                TicketId = x.TicketId,
                SerialNumber = x.SerialNumber,
                SeatNumber = x.SeatNumber,
                QRCode = x.QRCode,
                TicketImageUrl = x.TicketImageUrl,
                CreatedDate = x.CreatedDate,
                Status = x.Status,

                Holder=userInfo.Name+" "+userInfo.Surname,  
                HolderEmail=userInfo.Email,

                CategoryName = x.Booking.Event.Category.CategoryName,
                
                EventId=x.Booking.Event.EventId,
                EventName = x.Booking.Event.Title,
                EventDate = x.Booking.Event.EventDate,
                EventImageUrl = x.Booking.Event.ImageUrl,
                EventLocation = x.Booking.Event.Location,

            }).ToList();
        }
    }
}
