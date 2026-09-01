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
    public class GetUpcominEventsQueryHandler : IRequestHandler<GetUpcominEventsQuery, List<EventCardQueryResult>>
    {
        private readonly IEventRepository _eventRepository;

        public GetUpcominEventsQueryHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<List<EventCardQueryResult>> Handle(GetUpcominEventsQuery request, CancellationToken cancellationToken)
        {
            var values = await _eventRepository.GetUpcominEventsAsync();
            return values.Select(x => new EventCardQueryResult
            {
                EventId = x.EventId,
                Title = x.Title,
                Organizer = x.Organizer,
                Location = x.Location,
                EventDate = x.EventDate,
                Price = x.Price,
                Capacity = x.Capacity,
                ImageUrl = x.ImageUrl,
                Status = x.Status,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.CategoryName,
                CategoryIcon = x.Category.Icon,
                RemainingTickets = x.Capacity - x.Bookings.Sum(b => b.TicketQuantity)
            }).ToList();
        }
    }
}
