using Microsoft.AspNetCore.Mvc;

namespace TicketBox.WebUI.ViewComponents.LayoutViewComponents
{
    public class _FooterComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
