using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketBox.Application.Features.Users.Commands;
using TicketBox.Application.Features.Users.Queries;

namespace TicketBox.WebUI.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _mediator.Send(new GetMyProfileQuery());
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(UpdateProfileCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result)
            {
                TempData["Error"] = "Profil güncellenirken bir hata oluştu.";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Profil bilgileriniz başarıyla güncellendi.";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result)
            {
                TempData["PasswordError"] = "Şifre değiştirilemedi.";
                return RedirectToAction("Index");
            }
            TempData["PasswordSuccess"] = "Şifreniz başarıyla değiştirildi.";
            return RedirectToAction("Index");
        }
    }
}
