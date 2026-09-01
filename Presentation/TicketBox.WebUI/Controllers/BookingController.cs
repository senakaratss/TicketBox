using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketBox.Application.Features.Bookings.Commands;
using TicketBox.Application.Features.Bookings.Queries;
using TicketBox.Application.Features.Events.Queries;

namespace TicketBox.WebUI.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> MyBookings()
        {
            var values = await _mediator.Send(new GetMyBookingsQuery());
            return View(values);
        }
        public async Task<IActionResult> CreateBooking(int eventId,string? seatNumbers)
        {
            var value = await _mediator.Send(new GetEventByIdQuery { EventId = eventId });
            ViewBag.SeatNumbers = seatNumbers;
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBooking(CreateBookingCommand command)
        {
            await _mediator.Send(command);
            return RedirectToAction("MyBookings");
        }
        public async Task<IActionResult> SelectSeats(int id)
        {
            var value = await _mediator.Send(new GetEventSeatsQuery { EventId = id });
            return View(value);
        }
    }
}
