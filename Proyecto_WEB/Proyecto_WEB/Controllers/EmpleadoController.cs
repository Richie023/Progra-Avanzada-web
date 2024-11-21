using Microsoft.AspNetCore.Mvc;
using Proyecto_WEB.Models;

namespace Proyecto_WEB.Controllers
{
    public class EmpleadoController : Controller
    {
     
        [HttpGet]
        public IActionResult RegEmpleados()
        {
         
            return View("~/Views/Empleados/RegEmpleados.cshtml");
        }

   
        [HttpPost]
        public IActionResult RegEmpleados(Empleado model)
        {
            if (ModelState.IsValid)
            {
                
                return RedirectToAction("ListEmpleados"); 
            }

           
            return View("~/Views/Empleados/RegEmpleados.cshtml", model);
        }
    }
}
