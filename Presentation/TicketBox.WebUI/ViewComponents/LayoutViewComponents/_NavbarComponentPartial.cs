using Microsoft.AspNetCore.Mvc;

namespace TicketBox.WebUI.ViewComponents.LayoutViewComponents
{
    public class _NavbarComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
