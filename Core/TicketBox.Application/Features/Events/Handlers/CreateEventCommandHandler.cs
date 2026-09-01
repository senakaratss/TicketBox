using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Features.Events.Commands;
using TicketBox.Application.Interfaces;
using TicketBox.Domain.Entities;

namespace TicketBox.Application.Features.Events.Handlers
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand>
    {
        private readonly IEventRepository _eventRepository;

        public CreateEventCommandHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var value = new Event
            {
                Title = request.Title,
                Organizer=request.Organizer,
                Description = request.Description,
                Location = request.Location,
                EventDate = request.EventDate,
                Price = request.Price,
                Capacity = request.Capacity,
                ImageUrl = request.ImageUrl,
                Status = request.Status,
                HasSeatSelection=request.HasSeatSelection,
                CategoryId = request.CategoryId,
            };
            await _eventRepository.CreateEventAsync(value);
        }
    }
}
