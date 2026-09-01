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
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
    {
        private readonly IEventRepository _eventRepository;

        public UpdateEventCommandHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var value = await _eventRepository.GetEventByIdAsync(request.EventId);
            if (value == null)
            {
                throw new Exception("Event bulunamadı");
            }

            value.Title = request.Title;
            value.Description = request.Description;
            value.Organizer = request.Organizer;
            value.Location = request.Location;
            value.EventDate = request.EventDate;
            value.Price = request.Price;
            value.Capacity = request.Capacity;
            value.ImageUrl = request.ImageUrl;
            value.Status = request.Status;
            value.HasSeatSelection = request.HasSeatSelection;
            value.CategoryId = request.CategoryId;

            await _eventRepository.UpdateEventAsync(value);
        }
    }
}
