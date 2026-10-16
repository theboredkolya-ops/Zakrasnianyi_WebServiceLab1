using Microsoft.AspNetCore.Mvc;

namespace WebServiceLab1.Components
{
    public class NavbarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}   