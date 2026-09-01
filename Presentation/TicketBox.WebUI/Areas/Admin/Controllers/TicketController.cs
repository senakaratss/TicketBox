using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketBox.Application.Features.Tickets.Queries;

namespace TicketBox.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class TicketController : Controller
    {
        private readonly IMediator _mediator;

        public TicketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> TicketList()
        {
            var values = await _mediator.Send(new GetAllTicketsQuery());
            return View(values);
        }
    }
}
