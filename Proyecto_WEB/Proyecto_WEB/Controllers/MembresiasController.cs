using Microsoft.AspNetCore.Mvc;
using Proyecto_WEB.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Proyecto_WEB.Controllers
{
    public class MembresiasController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _conf;

        public MembresiasController(IHttpClientFactory http, IConfiguration conf)
        {
            _http = http;
            _conf = conf;
        }

        [HttpGet]
        public IActionResult Index()
        {
            using (var client = _http.CreateClient())
            {
                string url = _conf.GetSection("Variables:UrlApi").Value + "Membresias/ObtenerMembresias";

                try
                {
                    var response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadFromJsonAsync<JsonElement>().Result;

                        if (result.TryGetProperty("success", out var success) && success.GetBoolean() &&
                            result.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Array)
                        {
                            var datosContenido = JsonSerializer.Deserialize<List<Membresia>>(data.ToString());
                            return View(datosContenido);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "No se encontraron membresías.";
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

                return View(new List<Membresia>());
            }
        }

        [HttpGet]
        public IActionResult SinMembresia()
        {
            var consecutivo = HttpContext.Session.GetString("Consecutivo");
            if (string.IsNullOrEmpty(consecutivo))
            {
                return RedirectToAction("Login", "Login");
            }

            long usuarioId = long.Parse(consecutivo);

            using (var client = _http.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", consecutivo);

                string url = _conf.GetSection("Variables:UrlApi").Value + $"Miembros/Miembro/{usuarioId}";

                try
                {
                    var response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadFromJsonAsync<JsonElement>().Result;

                        if (result.TryGetProperty("Codigo", out var codigo) && codigo.GetInt32() == 0 &&
                            result.TryGetProperty("Contenido", out var contenido) && contenido.ValueKind == JsonValueKind.Object)
                        {
                            var miembro = JsonSerializer.Deserialize<Miembro>(contenido.ToString());
                            return View(miembro);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Miembro no encontrado o error en la respuesta de la API.";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Error al obtener los datos del miembro desde la API.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Ocurrió un error al intentar conectarse con la API: {ex.Message}";
                }
            }

            return View(new Miembro());
        }


        [HttpPost]
        public IActionResult ActualizarSinMembresia(Miembro model)
        {
            using (var client = _http.CreateClient())
            {
                var url = _conf.GetSection("Variables:UrlApi").Value + "Membresias/ActualizarSinMembresia";

                JsonContent datos = JsonContent.Create(model);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Consecutivo"));
                var response = client.PutAsync(url, datos).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    return RedirectToAction("Index", "Membresias");
                }
                else
                {
                    ViewBag.Mensaje = result!.Mensaje;
                    return View("Index", "Membresias");
                }
            }
        }

        [HttpGet]
        public IActionResult MembresiaRegular()
        {
            var consecutivo = HttpContext.Session.GetString("Consecutivo");
            if (string.IsNullOrEmpty(consecutivo))
            {
                return RedirectToAction("Login", "Login");
            }

            long usuarioId = long.Parse(consecutivo);

            using (var client = _http.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", consecutivo);

                string url = _conf.GetSection("Variables:UrlApi").Value + $"Miembros/Miembro/{usuarioId}";

                try
                {
                    var response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadFromJsonAsync<JsonElement>().Result;

                        if (result.TryGetProperty("Codigo", out var codigo) && codigo.GetInt32() == 0 &&
                            result.TryGetProperty("Contenido", out var contenido) && contenido.ValueKind == JsonValueKind.Object)
                        {
                            var miembro = JsonSerializer.Deserialize<Miembro>(contenido.ToString());
                            return View(miembro);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Miembro no encontrado o error en la respuesta de la API.";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Error al obtener los datos del miembro desde la API.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Ocurrió un error al intentar conectarse con la API: {ex.Message}";
                }
            }

            return View(new Miembro());
        }

        [HttpPost]
        public IActionResult ActualizarMembresiaRegular(Miembro model)
        {
            using (var client = _http.CreateClient())
            {
                var url = _conf.GetSection("Variables:UrlApi").Value + "Membresias/ActualizarMembresiaRegular";

                JsonContent datos = JsonContent.Create(model);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Consecutivo"));
                var response = client.PutAsync(url, datos).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    return RedirectToAction("Index", "Membresias");
                }
                else
                {
                    ViewBag.Mensaje = result!.Mensaje;
                    return View("Index", "Membresias");
                }
            }
        }

        [HttpGet]
        public IActionResult MembresiaPremium()
        {
            var consecutivo = HttpContext.Session.GetString("Consecutivo");
            if (string.IsNullOrEmpty(consecutivo))
            {
                return RedirectToAction("Login", "Login");
            }

            long usuarioId = long.Parse(consecutivo);

            using (var client = _http.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", consecutivo);

                string url = _conf.GetSection("Variables:UrlApi").Value + $"Miembros/Miembro/{usuarioId}";

                try
                {
                    var response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadFromJsonAsync<JsonElement>().Result;

                        if (result.TryGetProperty("Codigo", out var codigo) && codigo.GetInt32() == 0 &&
                            result.TryGetProperty("Contenido", out var contenido) && contenido.ValueKind == JsonValueKind.Object)
                        {
                            var miembro = JsonSerializer.Deserialize<Miembro>(contenido.ToString());
                            return View(miembro);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Miembro no encontrado o error en la respuesta de la API.";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Error al obtener los datos del miembro desde la API.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Ocurrió un error al intentar conectarse con la API: {ex.Message}";
                }
            }

            return View(new Miembro());
        }

        [HttpPost]
        public IActionResult ActualizarMembresiaPremium(Miembro model)
        {
            using (var client = _http.CreateClient())
            {
                var url = _conf.GetSection("Variables:UrlApi").Value + "Membresias/ActualizarMembresiaPremium";

                JsonContent datos = JsonContent.Create(model);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Consecutivo"));
                var response = client.PutAsync(url, datos).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    return RedirectToAction("Index", "Membresias");
                }
                else
                {
                    ViewBag.Mensaje = result!.Mensaje;
                    return View("Index", "Membresias");
                }
            }
        }
    }
}
