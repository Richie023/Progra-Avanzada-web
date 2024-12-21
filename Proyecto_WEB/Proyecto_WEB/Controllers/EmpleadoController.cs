using Microsoft.AspNetCore.Mvc;
using Proyecto_WEB.Models;
using Proyecto_WEB.Servicios;

namespace Proyecto_WEB.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly IMetodosComunes _metodosComunes;

        public EmpleadoController(IMetodosComunes metodosComunes)
        {
            _metodosComunes = metodosComunes;
        }

        public IActionResult RegEmpleados()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(Empleado empleado)
        {
            var response = await _metodosComunes.Post("Empleado/Registrar", empleado);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListEmpleado");
            }
            return View("RegEmpleados");
        }

        public async Task<IActionResult> ListEmpleados()
        {
            var empleados = await _metodosComunes.Get<List<Empleado>>("Empleado/Listar");
            return View(empleados);
        }

    }
}
