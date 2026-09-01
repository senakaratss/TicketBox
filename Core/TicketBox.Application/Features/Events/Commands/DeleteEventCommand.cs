using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBox.Application.Features.Events.Commands
{
    public class DeleteEventCommand:IRequest
    {
        public int EventId { get; set; }
    }
}
