using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using TicketBox.Application.Features.Categories.Queries;
using TicketBox.Application.Features.Events.Queries;
using TicketBox.Application.Features.Events.Results;

namespace TicketBox.WebUI.Controllers
{
    public class EventController : Controller
    {
        private readonly IMediator _mediator;

        public EventController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> EventList(string? search, int? categoryId, string? location, DateTime? date)
        {
            var query = new GetEventsQuery
            {
                Search = search,
                CategoryId = categoryId,
                Date = date,
                Location = location
            };
            var values = await _mediator.Send(query);

            var categories = await _mediator.Send(new GetCategoriesQuery());
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");

            return View(values);
        }
        public async Task<IActionResult> Details(int id)
        {
            var value = await _mediator.Send(new GetEventByIdQuery { EventId = id });
            return View(value);
        }
    }
}
