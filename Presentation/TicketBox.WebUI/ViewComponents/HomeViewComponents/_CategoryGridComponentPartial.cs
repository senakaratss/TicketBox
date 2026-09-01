using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketBox.Application.Features.Categories.Queries;

namespace TicketBox.WebUI.ViewComponents.HomeViewComponents
{
    public class _CategoryGridComponentPartial:ViewComponent
    {
        private readonly IMediator _mediator;

        public _CategoryGridComponentPartial(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _mediator.Send(new GetCategoriesQuery());
            return View(categories);
        }
    }
}
