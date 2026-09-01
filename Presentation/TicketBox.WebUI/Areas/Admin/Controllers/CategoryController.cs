using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketBox.Application.Features.Categories.Commands;
using TicketBox.Application.Features.Categories.Queries;

namespace TicketBox.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryCommand command)
        {
            var categoryName = command.CategoryName;

            var categoryId = await _mediator.Send(command);
            return RedirectToAction("CategoryList");
        }
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _mediator.Send(new DeleteCategoryCommand { CategoryId = id });
            return RedirectToAction("CategoryList");
        }
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var value = await _mediator.Send(new GetCategoryByIdQuery { CategoryId = id });
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryCommand command)
        {
            await _mediator.Send(command);
            return RedirectToAction("CategoryList");
        }

    }
}
