using Microsoft.AspNetCore.Mvc;
using Proyecto_WEB.Models;
using System.Net.Http.Headers;
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
        [HttpPost]
        public IActionResult Index(int claseId) 
        {
            if (HttpContext.Session.GetString("Consecutivo") == null)
            {
                return RedirectToAction("Login", "Login");
            }

            using (var client = _http.CreateClient())
            {
                var url = _conf.GetSection("Variables:UrlApi").Value + "Clase/RegistrarMiembroEnClase";
                var UsuarioID = long.Parse(HttpContext.Session.GetString("Consecutivo")!.ToString());

                var miembroClase = new MiembroClase
                {
                    ClaseID = claseId, 
                    UsuarioID = UsuarioID
                };

                JsonContent datos = JsonContent.Create(miembroClase);
                var response = client.PostAsync(url, datos).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    return RedirectToAction("Index", "Clase");
                }
                else
                {
                    ViewBag.Error = result!.Mensaje;
                    return View("Index", "Clase");
                }
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
                    if (result.Contenido != null)
                    {
                        var datosContenido = JsonSerializer.Deserialize<List<Clase>>((JsonElement)result.Contenido!);
                        if (datosContenido == null || datosContenido.Count == 0)
                        {
                            ViewBag.Mensaje = "No hay clases registradas.";
                            return View(new List<Clase>());
                        }

                        return View(datosContenido);
                    }
                    else
                    {
                        ViewBag.Mensaje = "No hay clases registradas";
                        return View(new List<Clase>());
                    }
                }

                ViewBag.Mensaje = "No se pudo obtener la información de clases";
                return View(new List<Clase>());
            }
        }



    }
}
