using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketBox.Application.Features.Categories.Commands;
using TicketBox.Application.Features.Categories.Queries;

namespace TicketBox.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> CategoryList()
        {
            var values = await _mediator.Send(new GetCategoriesQuery());
            return View(values);
        }
        public async Task<IActionResult> CategoryDetail(int id)
        {
            var value = await _mediator.Send(new GetCategoryByIdQuery { CategoryId = id });
            return View(value);
        }
    }
}
