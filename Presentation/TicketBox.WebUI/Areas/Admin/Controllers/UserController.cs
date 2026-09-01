using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketBox.Application.Features.Users.Queries;

namespace TicketBox.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> UserList()
        {
            var values = await _mediator.Send(new GetUsersQuery());
            return View(values);
        }
        public async Task<IActionResult> UserDetails(string id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery { Id = id });
            return View(user);
        }
    }
}
