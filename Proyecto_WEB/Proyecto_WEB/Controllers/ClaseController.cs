using Microsoft.AspNetCore.Mvc;
using Proyecto_WEB.Models;
using System.Text.Json;

namespace Proyecto_WEB.Controllers
{
    public class ClaseController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _conf;

        public ClaseController(IHttpClientFactory http, IConfiguration conf)
        {
            _http = http;
            _conf = conf;

        }
        [HttpGet]
        public IActionResult Index()
        {
            using (var client = _http.CreateClient())
            {
                string url = _conf.GetSection("Variables:UrlApi").Value + "Clase/ConsultarClases";

                var response = client.GetAsync(url).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    var datosContenido = JsonSerializer.Deserialize<List<Clase>>((JsonElement)result.Contenido!);
                    return View(datosContenido);
                }

                return View(new List<Clase>());
            }
        }
        [HttpGet]
        public IActionResult ClasesUsuario()
        {
            using (var client = _http.CreateClient())
            {
                var UsuarioID = long.Parse(HttpContext.Session.GetString("Consecutivo")!.ToString());
                string url = _conf.GetSection("Variables:UrlApi").Value + "Clase/ConsultarUsuarioClases?UsuarioID=";

                var response = client.GetAsync(url).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    var datosContenido = JsonSerializer.Deserialize<List<Clase>>((JsonElement)result.Contenido!);
                    return View(datosContenido);
                }

                return View(new List<Clase>());
            }
        }

    }
}
