using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketBox.Application.Features.Events.Queries;

namespace TicketBox.WebUI.ViewComponents.HomeViewComponents
{
    public class _UpcomingEventsComponentPartial:ViewComponent
    {
        private readonly IMediator _mediator;

        public _UpcomingEventsComponentPartial(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var upcomingEvents = await _mediator.Send(new GetUpcominEventsQuery());
            return View(upcomingEvents);
        }
    }
}
