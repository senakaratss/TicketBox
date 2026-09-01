using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBox.Application.Features.Categories.Commands
{
    public class CreateCategoryCommand:IRequest<int> //oluşturulan category id'sini döndür
    {
        public string CategoryName { get; set; }
        public string Icon { get; set; }
    }
}
