using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Features.Events.Queries;
using TicketBox.Application.Features.Events.Results;
using TicketBox.Application.Interfaces;

namespace TicketBox.Application.Features.Events.Handlers
{
    public class GetEventSeatsQueryHandler : IRequestHandler<GetEventSeatsQuery, GetEventSeatsQueryResult>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IEventRepository _eventRepository;

        public GetEventSeatsQueryHandler(ITicketRepository ticketRepository, IEventRepository eventRepository)
        {
            _ticketRepository = ticketRepository;
            _eventRepository = eventRepository;
        }

        public async Task<GetEventSeatsQueryResult> Handle(GetEventSeatsQuery request, CancellationToken cancellationToken)
        {
            var eventValue = await _eventRepository.GetEventByIdAsync(request.EventId);
            var takenSeats = await _ticketRepository.GetTakenSeatsByEventIdAsync(request.EventId);

            return new GetEventSeatsQueryResult
            {
                EventId = eventValue.EventId,
                EventName = eventValue.Title,
                EventDate = eventValue.EventDate,
                Location = eventValue.Location,
                Price = eventValue.Price,
                TakenSeats = takenSeats
            };
        }
    }
}
