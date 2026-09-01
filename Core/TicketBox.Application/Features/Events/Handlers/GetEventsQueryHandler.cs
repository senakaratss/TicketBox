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
    public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, List<GetEventsQueryResult>>
    {
        private readonly IEventRepository _eventRepository;

        public GetEventsQueryHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<List<GetEventsQueryResult>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {
            var values = await _eventRepository.GetAllEventsAsync(request.Search,request.CategoryId,request.Location,request.Date);
            return values.Select(x => new GetEventsQueryResult
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
                RemainingTickets=x.Capacity-x.Bookings.Sum(x=>x.TicketQuantity),
                SoldTickets=x.Bookings.Sum(x=>x.TicketQuantity)
            }).ToList();
        }
    }
}
