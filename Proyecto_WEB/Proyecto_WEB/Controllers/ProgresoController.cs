using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_WEB.Models;
using System.Net.Http;
using System.Text.Json;

namespace Proyecto_WEB.Controllers
{
    public class ProgresoController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _conf;

        public ProgresoController(IHttpClientFactory http, IConfiguration conf)
        {
            _http = http;
            _conf = conf;
        }

        [HttpGet]
        public IActionResult CrearProgreso()
        {
            long usuarioId = long.Parse(HttpContext.Session.GetString("Consecutivo")!);

            using (var client = _http.CreateClient())
            {
                string urlUsuarios = _conf.GetSection("Variables:UrlApi").Value + "Usuario/ListaUsuarios";
                var responseUsuarios = client.GetAsync(urlUsuarios).Result;

                if (responseUsuarios.IsSuccessStatusCode)
                {
                    var usuarios = responseUsuarios.Content.ReadFromJsonAsync<List<Usuario>>().Result;
                    var model = new Progreso
                    {
                        UsuarioID = Convert.ToInt32(HttpContext.Session.GetString("Consecutivo"))
                    };
                    
                    return View(model);                   
                }
                else
                {
                    ViewBag.ErrorMessage = "No se pudieron obtener los datos necesarios para registrar el progreso.";
                    return View(new Progreso());
                }
            }
        }

        [HttpPost]
        public IActionResult CrearProgreso(Progreso model)
        {
            if (ModelState.IsValid)
            {
                using (var client = _http.CreateClient())
                {
                    string url = _conf.GetSection("Variables:UrlApi").Value + "Progreso/CrearProgreso";

                    var response = client.PostAsJsonAsync(url, model).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("ListaProgreso");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "No se pudo registrar el progreso.";
                    }                    
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ListaProgreso()
        {
            long usuarioId = long.Parse(HttpContext.Session.GetString("Consecutivo")!);

            using (var client = _http.CreateClient())
            {
                string url = _conf.GetSection("Variables:UrlApi").Value + $"Progreso/ListaProgreso/{usuarioId}";

                try
                {
                    var response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadFromJsonAsync<JsonElement>().Result;

                        if (result.TryGetProperty("success", out var success) && success.GetBoolean() &&
                            result.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Array)
                        {
                            var progresos = JsonSerializer.Deserialize<List<Progreso>>(data.ToString());
                            return View(progresos);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "No se encontraron registros de progreso.";
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

                return View(new List<Progreso>());
            }
        }
    }
}
