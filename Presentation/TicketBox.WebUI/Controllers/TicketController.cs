using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketBox.Application.Features.Tickets.Queries;

namespace TicketBox.WebUI.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        private readonly IMediator _mediator;

        public TicketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> MyTickets()
        {
            var values = await _mediator.Send(new GetMyTicketQuery());
            return View(values);
        }

        public async Task<IActionResult> GetTicketsByBooking(int id)
        {
            var values = await _mediator.Send(new GetMyTicketsByBookingQuery { BookingId = id });
            return View(values);
        }

        public async Task<IActionResult> Details(int id)
        {
            var value = await _mediator.Send(new GetMyTicketDetailsQuery { TicketId = id });
            return View(value);
        }
    }
}
