using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBox.Application.Features.Categories.Commands
{
    public class DeleteCategoryCommand:IRequest
    {
        public int CategoryId { get; set; }
    }
}
