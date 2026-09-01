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
    public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, GetEventByIdQueryResult>
    {
        private readonly IEventRepository _eventRepository;

        public GetEventByIdQueryHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<GetEventByIdQueryResult> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            var value = await _eventRepository.GetEventByIdAsync(request.EventId);
            var soldTickets = value.Bookings?.Sum(x => x.TicketQuantity) ?? 0;

            return new GetEventByIdQueryResult
            {
                EventId = value.EventId,
                Title = value.Title,
                Organizer = value.Organizer,
                Description = value.Description,
                Location = value.Location,
                EventDate = value.EventDate,
                Price = value.Price,
                Capacity = value.Capacity,
                ImageUrl = value.ImageUrl,
                Status = value.Status,
                CategoryId = value.CategoryId,
                CategoryName=value.Category.CategoryName,
                CategoryIcon=value.Category.Icon,
                RemainingTickets=value.Capacity-soldTickets,
                SoldTickets=soldTickets,
                HasSeatSelection=value.HasSeatSelection
            };
        }
    }
}
