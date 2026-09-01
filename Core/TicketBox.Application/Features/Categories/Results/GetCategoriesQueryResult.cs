using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBox.Application.Features.Categories.Results
{
    public class GetCategoriesQueryResult
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Icon { get; set; }
        public int EventsCount { get; set; }

    }
}
