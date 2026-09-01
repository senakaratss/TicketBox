using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Features.Bookings.Results;

namespace TicketBox.Application.Features.Bookings.Queries
{
    public class GetMyBookingsQuery:IRequest<List<GetMyBookingsQueryResult>>
    {
    }
}
