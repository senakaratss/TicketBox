using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Features.Tickets.Results;

namespace TicketBox.Application.Features.Tickets.Queries
{
    public class GetAllTicketsQuery:IRequest<List<GetAllTicketsQueryResult>>
    {
    }
}
