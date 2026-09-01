using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketBox.Application.Features.Bookings.Queries;
using TicketBox.Application.Features.Home.Queries;
using TicketBox.Application.Features.Tickets.Queries;
using TicketBox.WebUI.Models;

namespace TicketBox.WebUI.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var myStatistics = await _mediator.Send(new GetHomeQuery());

            var model = new DashboardViewModel
            {
                MyActiveTickets=myStatistics.MyActiveTickets,
                MyUpcomingBookings=myStatistics.MyUpcomingBookings,
                ActiveTicketCount=myStatistics.ActiveTicketCount,
                TotalBookingCount=myStatistics.TotalBookingCount,
                UpcomingBookingCount=myStatistics.UpcomingBookingCount,
                Username = User.Identity?.Name ?? "Guest"
            };

            ViewData["ActiveNav"] = "Home";
            return View(model);
        }
    }
}
