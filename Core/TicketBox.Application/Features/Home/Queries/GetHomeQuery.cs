using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Features.Home.Results;

namespace TicketBox.Application.Features.Home.Queries
{
    public class GetHomeQuery:IRequest<GetHomeQueryResult>
    {
    }
}
