using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using TicketBox.Application.Features.Categories.Queries;
using TicketBox.Application.Features.Events.Commands;
using TicketBox.Application.Features.Events.Queries;

namespace TicketBox.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class EventController : Controller
    {
        private readonly IMediator _mediator;

        public EventController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> EventList()
        {
            var values = await _mediator.Send(new GetEventsQuery());
            return View(values);
        }
        public async Task<IActionResult> CreateEvent()
        {
            var categories = await _mediator.Send(new GetCategoriesQuery());
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreateEventCommand command)
        {
            await _mediator.Send(command);
            return RedirectToAction("EventList");
        }
        public async Task<IActionResult> DeleteEvent(int id)
        {
            await _mediator.Send(new DeleteEventCommand { EventId = id });
            return RedirectToAction("EventList");
        }
        public async Task<IActionResult> UpdateEvent(int id)
        {
            var value = await _mediator.Send(new GetEventByIdQuery { EventId = id });

            var categories = await _mediator.Send(new GetCategoriesQuery());
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");

            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateEvent(UpdateEventCommand command)
        {
            await _mediator.Send(command);
            return RedirectToAction("EventList");
        }
    }
}
