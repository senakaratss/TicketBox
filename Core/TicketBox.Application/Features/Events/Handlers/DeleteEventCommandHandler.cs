using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Features.Events.Commands;
using TicketBox.Application.Interfaces;

namespace TicketBox.Application.Features.Events.Handlers
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
    {
        private readonly IEventRepository _eventRepository;

        public DeleteEventCommandHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            await _eventRepository.DeleteEventAsync(request.EventId);
        }
    }
}
