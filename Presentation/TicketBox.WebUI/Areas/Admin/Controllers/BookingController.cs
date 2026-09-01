using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketBox.Application.Features.Bookings.Queries;

namespace TicketBox.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class BookingController : Controller
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> BookingList()
        {
            var values = await _mediator.Send(new GetBookingsQuery());
            return View(values);
        }
        public async Task<IActionResult> Details(int id)
        {
            var value = await _mediator.Send(new GetBookingByIdQuery { BookingId = id });
            return View(value);
        }
    }
}
