using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketBox.Application.Features.Events.Queries;

namespace TicketBox.WebUI.ViewComponents.HomeViewComponents
{
    public class _FeaturedEventsComponentPartial:ViewComponent
    {
        private readonly IMediator _mediator;

        public _FeaturedEventsComponentPartial(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var featuredEvents = await _mediator.Send(new GetFeaturedEventsQuery());
            return View(featuredEvents);
        }
    }
}
