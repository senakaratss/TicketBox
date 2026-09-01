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
    public class GetAllTicketsQueryHandler : IRequestHandler<GetAllTicketsQuery, List<GetAllTicketsQueryResult>>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IIdentityService _identityService;

        public GetAllTicketsQueryHandler(ITicketRepository ticketRepository, IIdentityService identityService)
        {
            _ticketRepository = ticketRepository;
            _identityService = identityService;
        }

        public async Task<List<GetAllTicketsQueryResult>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
        {
            var values = await _ticketRepository.GetAllTicketsAsync();
            var users = await _identityService.GetAllUsersAsync();
            var userDictionary = users.ToDictionary(x => x.Id);

            var result = values.Select(x =>
            {
                var userInfo = userDictionary[x.Booking.UserId];

                return new GetAllTicketsQueryResult
                {
                    TicketId = x.TicketId,
                    SeatNumber = x.SeatNumber,
                    SerialNumber = x.SerialNumber,
                    Status = x.Status,
                    EventName = x.Booking.Event.Title,
                    Holder = $"{userInfo.Name} {userInfo.Surname}"
                };
            }).ToList();

            return result;
        }
    }
}
