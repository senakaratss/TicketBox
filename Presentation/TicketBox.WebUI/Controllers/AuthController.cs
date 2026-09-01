using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketBox.Application.Features.Users.Commands;

namespace TicketBox.WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return RedirectToAction("ConfirmEmail", new { email = command.Email });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(command);
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result)
            {
                ModelState.AddModelError("", "Incorrect username or password");
                return View(command);
            }
            return RedirectToAction("Index", "Profile");
        }
        public async Task<IActionResult> Logout()
        {
            await _mediator.Send(new LogoutUserCommand());
            return RedirectToAction("Login");
        }
        public IActionResult ConfirmEmail(string email)
        {
            return View(new ConfirmEmailCommand { Email = email });
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand command)
        {
            await _mediator.Send(command);
            return RedirectToAction("Login");
        }
    }
}
