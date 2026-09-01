using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Features.Events.Results;

namespace TicketBox.Application.Features.Events.Queries
{
    public class GetEventsQuery:IRequest<List<GetEventsQueryResult>>
    {
        public string? Search { get; set; }
        public int? CategoryId { get; set; }
        public string? Location { get; set; }
        public DateTime? Date{ get; set; }
    }
}
