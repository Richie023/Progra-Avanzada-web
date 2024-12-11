using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_WEB.Models;
using System.Net.Http;
using System.Text.Json;

namespace Proyecto_WEB.Controllers
{
    public class PlanEntrenamientoController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _conf;

        public PlanEntrenamientoController(IHttpClientFactory http, IConfiguration conf)
        {
            _http = http;
            _conf = conf;
        }

        [HttpGet]
        public IActionResult CrearPlan()
        {
            // Obtener la lista de usuarios desde la API
            using (var client = _http.CreateClient())
            {
                string url = _conf.GetSection("Variables:UrlApi").Value + "Usuario/ListaUsuarios"; // URL del nuevo endpoint
                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var usuarios = response.Content.ReadFromJsonAsync<List<Usuario>>().Result;
                    var model = new PlanEntrenamiento();

                    // Asignar la lista de usuarios al ViewData para usarla en el SelectList
                    ViewData["Usuarios"] = new SelectList(usuarios, "UsuarioID", "Username");
                    return View(model);
                }
                else
                {
                    ViewBag.ErrorMessage = "No se pudo obtener la lista de usuarios.";
                    return View(new PlanEntrenamiento());
                }
            }
        }

        [HttpPost]
        public IActionResult CrearPlan(PlanEntrenamiento model)
        {
            if (ModelState.IsValid)
            {
                using (var client = _http.CreateClient())
                {
                    string url = _conf.GetSection("Variables:UrlApi").Value + "PlanEntrenamiento/CrearPlan";

                    try
                    {
                        var response = client.PostAsJsonAsync(url, model).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("ListaPlan");
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "No se pudo registrar el plan de entrenamiento.";
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = $"Ocurrió un error al conectar con la API: {ex.Message}";
                    }
                }
            }

            return View(model);
        }

    [HttpGet]
        public IActionResult ListaPlan()
        {
            using (var client = _http.CreateClient())
            {
                string url = _conf.GetSection("Variables:UrlApi").Value + "PlanEntrenamiento/ListaPlan";

                try
                {
                    var response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadFromJsonAsync<JsonElement>().Result;

                        if (result.TryGetProperty("success", out var success) && success.GetBoolean() &&
                            result.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Array)
                        {
                            // Deserializar los planes de entrenamiento
                            var planes = JsonSerializer.Deserialize<List<PlanEntrenamiento>>(data.ToString());
                            return View(planes);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "No se encontraron planes de entrenamiento.";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Error al conectar con la API.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Ocurrió un error al conectarse con la API: {ex.Message}";
                }

                return View(new List<PlanEntrenamiento>());
            }
        }
    }
}