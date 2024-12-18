using Microsoft.AspNetCore.Mvc;
using Proyecto_WEB.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SWeb.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _conf;
        private readonly IHostEnvironment _env;
        public ProductoController(IHttpClientFactory http, IConfiguration conf, IHostEnvironment env)
        {
            _http = http;
            _conf = conf;
            _env = env;
        }

        [HttpGet]
        public IActionResult ListaProductos()
        {
            return View(ObtenerProductosActivos());
        }

        [HttpGet]
        public IActionResult ListaProductosAdmin()
        {
            return View(ObtenerProductos());
        }

        [HttpPost]
        public IActionResult ActualizarEstado(Producto model)
        {
            using (var client = _http.CreateClient())
            {
                var url = _conf.GetSection("Variables:UrlApi").Value + "Producto/ActualizarEstado";

                JsonContent datos = JsonContent.Create(model);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Consecutivo"));
                var response = client.PutAsync(url, datos).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    return RedirectToAction("ListaProductosAdmin", "Producto");
                }
                else
                {
                    ViewBag.Mensaje = result!.Mensaje;
                    return View("ListaProductosAdmin", ObtenerProductos());
                }
            }
        }

        [HttpGet]
        public IActionResult CrearProducto()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearProducto(IFormFile ImagenProducto, Producto model)
        {
            var ext = string.Empty;
            var folder = string.Empty;

            if (ImagenProducto != null)
            {
                ext = Path.GetExtension(Path.GetFileName(ImagenProducto.FileName));
                folder = Path.Combine(_env.ContentRootPath, "wwwroot\\products");
                model.Imagen = "/products/";

                if (ext.ToLower() != ".png")
                {
                    ViewBag.Mensaje = "La imagen debe ser .png";
                    return View();
                }
            }

            using (var client = _http.CreateClient())
            {
                var url = _conf.GetSection("Variables:UrlApi").Value + "Producto/CrearProducto";

                JsonContent datos = JsonContent.Create(model);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Consecutivo"));
                var response = client.PostAsync(url, datos).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    if (ImagenProducto != null)
                    {
                        var archivo = Path.Combine(folder, result.Mensaje + ext);
                        using (Stream fs = new FileStream(archivo, FileMode.Create))
                        {
                            ImagenProducto.CopyTo(fs);
                        }
                    }

                    return RedirectToAction("ListaProductosAdmin", "Producto");
                }
                else
                {
                    ViewBag.Mensaje = result!.Mensaje;
                    return View();
                }
            }
        }

        [HttpGet]
        public IActionResult EditarProducto(long ProductoID)
        {
            return View(ObtenerProducto(ProductoID));
        }

        [HttpPost]
        public IActionResult EditarProducto(IFormFile ImagenProducto, Producto model)
        {
            var ext = string.Empty;
            var folder = string.Empty;

            if (ImagenProducto != null)
            {
                ext = Path.GetExtension(Path.GetFileName(ImagenProducto.FileName));
                folder = Path.Combine(_env.ContentRootPath, "wwwroot\\products");

                if (ext.ToLower() != ".png")
                {
                    ViewBag.Mensaje = "La imagen debe ser .png";
                    return View();
                }
            }

            using (var client = _http.CreateClient())
            {
                var url = _conf.GetSection("Variables:UrlApi").Value + "Producto/EditarProducto";

                JsonContent datos = JsonContent.Create(model);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Consecutivo"));
                var response = client.PutAsync(url, datos).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    if (ImagenProducto != null)
                    {
                        var archivo = Path.Combine(folder, model.ProductoID + ext);
                        using (Stream fs = new FileStream(archivo, FileMode.Create))
                        {
                            ImagenProducto.CopyTo(fs);
                        }
                    }

                    return RedirectToAction("ListaProductosAdmin", "Producto");
                }
                else
                {
                    ViewBag.Mensaje = result!.Mensaje;
                    return View();
                }
            }
        }

        private List<Producto> ObtenerProductos()
        {
            using (var client = _http.CreateClient())
            {
                string url = _conf.GetSection("Variables:UrlApi").Value + "Producto/ListaProductos";

                var response = client.GetAsync(url).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    var datosContenido = JsonSerializer.Deserialize<List<Producto>>((JsonElement)result.Contenido!);
                    return datosContenido!;
                }

                return new List<Producto>();
            }
        }

        private List<Producto> ObtenerProductosActivos()
        {
            using (var client = _http.CreateClient())
            {
                string url = _conf.GetSection("Variables:UrlApi").Value + "Producto/ListaProductosActivos";

                var response = client.GetAsync(url).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    var datosContenido = JsonSerializer.Deserialize<List<Producto>>((JsonElement)result.Contenido!);
                    return datosContenido!;
                }

                return new List<Producto>();
            }
        }

        private Producto? ObtenerProducto(long ProductoID)
        {
            using (var client = _http.CreateClient())
            {
                string url = _conf.GetSection("Variables:UrlApi").Value + "Producto/Producto?ProductoID=" + ProductoID;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Consecutivo"));
                var response = client.GetAsync(url).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    return JsonSerializer.Deserialize<Producto>((JsonElement)result.Contenido!);
                }

                return new Producto();
            }
        }
    }
}
