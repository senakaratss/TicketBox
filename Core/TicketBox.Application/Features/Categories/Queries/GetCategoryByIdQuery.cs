using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Features.Categories.Results;

namespace TicketBox.Application.Features.Categories.Queries
{
    public class GetCategoryByIdQuery:IRequest<GetCategoryByIdQueryResult>
    {
        public int CategoryId { get; set; }
    }
}
