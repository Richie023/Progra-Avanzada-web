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
        public async Task<IActionResult> RegEmpleados()
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001/api/");

                // Obtener los roles
                var rolesResponse = await client.GetAsync("Usuario/ListarRoles");

                if (rolesResponse.IsSuccessStatusCode)
                {
                    var roles = await rolesResponse.Content.ReadFromJsonAsync<IEnumerable<Rol>>();
                    ViewBag.Roles = roles; // Asignar al ViewBag
                }
                else
                {
                    ViewBag.Roles = new List<Rol>();
                    ModelState.AddModelError("", "No se pudieron cargar los roles.");
                }
            }

            return View("~/Views/Empleado/RegEmpleados.cshtml");
        }



        [HttpPost]
        public async Task<IActionResult> RegEmpleados(Empleado model, Usuario usuarioModel)
        {
            if (ModelState.IsValid)
            {
                using (var client = _httpClientFactory.CreateClient())
                {
                    // Base URL de la API
                    client.BaseAddress = new Uri("https://localhost:5001/api/");

                    // Registrar Usuario
                    var usuarioResponse = await client.PostAsJsonAsync("Usuario/Registrar", usuarioModel);
                    if (!usuarioResponse.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError("", "No se pudo registrar el usuario.");
                        return View("~/Views/Empleado/RegEmpleados.cshtml");
                    }

                    // Obtener el UsuarioID registrado
                    var usuarioID = await usuarioResponse.Content.ReadFromJsonAsync<long>();
                    model.UsuarioID = usuarioID;

                    // Registrar Empleado
                    var empleadoResponse = await client.PostAsJsonAsync("Empleado/Registrar", model);
                    if (empleadoResponse.IsSuccessStatusCode)
                    {
                        // Redirigir a la lista de empleados si el registro fue exitoso
                        return RedirectToAction("ListEmpleados");
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se pudo registrar el empleado.");
                    }
                }
            }

            // Si hay errores, recargar cargos y roles para mostrarlos en la vista
            using (var client = _httpClientFactory.CreateClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001/api/");
                var cargosResponse = await client.GetAsync("Cargo/Listar");
                var rolesResponse = await client.GetAsync("Usuario/ListarRoles");

                if (cargosResponse.IsSuccessStatusCode && rolesResponse.IsSuccessStatusCode)
                {
                    var cargos = await cargosResponse.Content.ReadFromJsonAsync<IEnumerable<Cargo>>();
                    var roles = await rolesResponse.Content.ReadFromJsonAsync<IEnumerable<Rol>>();

                    ViewBag.Cargos = cargos ?? new List<Cargo>();
                    ViewBag.Roles = roles ?? new List<Rol>();
                }
            }

            return View("~/Views/Empleado/RegEmpleados.cshtml", model);
        }
    }
}
