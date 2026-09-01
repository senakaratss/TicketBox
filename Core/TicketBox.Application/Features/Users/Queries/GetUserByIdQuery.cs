using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Features.Users.Results;

namespace TicketBox.Application.Features.Users.Queries
{
    public class GetUserByIdQuery:IRequest<GetUserByIdQueryResult>
    {
        public string Id { get; set; }
    }
}
