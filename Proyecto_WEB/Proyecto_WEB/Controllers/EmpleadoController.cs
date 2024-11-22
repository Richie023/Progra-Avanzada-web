using Microsoft.AspNetCore.Mvc;
using Proyecto_WEB.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace Proyecto_WEB.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EmpleadoController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult RegEmpleados()
        {
            // Muestra el formulario de registro
            return View("~/Views/Empleado/RegEmpleados.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> RegEmpleados(Empleado model)
        {
            if (ModelState.IsValid)
            {
                // Llama a la API para registrar el empleado
                using (var client = _httpClientFactory.CreateClient())
                {
                    client.BaseAddress = new Uri("https://localhost:5001/api/"); // Cambia por la URL de tu API
                    var response = await client.PostAsJsonAsync("Empleado/Registrar", model);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("ListEmpleados"); // Redirige a la lista de empleados tras guardar
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se pudo registrar el empleado.");
                    }
                }
            }

            // Si hay errores, vuelve a mostrar el formulario
            return View("~/Views/Empleado/RegEmpleados.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> ListEmpleados()
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                client.BaseAddress = new Uri("https://localhost:7179/api/"); // URL de tu API

                // Llamada a la API para obtener la lista de empleados
                var response = await client.GetAsync("Empleado/Listar");
                if (response.IsSuccessStatusCode)
                {
                    var empleados = await response.Content.ReadFromJsonAsync<IEnumerable<Empleado>>();
                    return View("~/Views/Empleado/ListEmpleados.cshtml", empleados);
                }
                else
                {
                    // Manejo de errores en caso de que la API falle
                    ModelState.AddModelError("", "No se pudo obtener la lista de empleados.");
                    return View("~/Views/Empleado/ListEmpleados.cshtml", new List<Empleado>());
                }
            }
        }

    }
}
