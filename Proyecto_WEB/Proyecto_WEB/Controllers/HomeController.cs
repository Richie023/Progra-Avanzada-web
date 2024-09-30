using Microsoft.AspNetCore.Mvc;
using Proyecto_WEB.Models;
using System.Diagnostics;

namespace Proyecto_WEB.Controllers
{
    public class HomeController : Controller
    {
        

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Registrar()
        {
            return View();
        }

        
    }
}
