using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace TicketBox.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var isAuthenticated = User.Identity?.IsAuthenticated;
            if (isAuthenticated==true)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }
    }
}
