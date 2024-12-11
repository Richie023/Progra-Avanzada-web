using Microsoft.AspNetCore.Mvc;
using Proyecto_WEB.Models;
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

                        // Verificar si la respuesta es exitosa y contiene datos
                        if (result.TryGetProperty("success", out var success) && success.GetBoolean() &&
                            result.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Array)
                        {
                            // Deserializar la lista de membresías
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
    }
}
