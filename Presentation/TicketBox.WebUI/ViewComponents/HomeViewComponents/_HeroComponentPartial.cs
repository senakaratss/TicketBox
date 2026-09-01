using Microsoft.AspNetCore.Mvc;

namespace TicketBox.WebUI.ViewComponents.HomeViewComponents
{
    public class _HeroComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
