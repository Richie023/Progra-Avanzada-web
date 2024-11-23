using Microsoft.AspNetCore.Mvc;

namespace Proyecto_WEB.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminIndex()
        {
            return View();
        }
    }
}
