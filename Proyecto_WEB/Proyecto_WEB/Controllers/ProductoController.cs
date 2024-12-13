using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_WEB.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Proyecto_WEB.Controllers
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
        public IActionResult CrearProducto()
        {
            return View(new Producto());
        }

        [HttpPost]
        public IActionResult CrearProducto(IFormFile ImagenArchivo, Producto model)
        {
            var ext = string.Empty;
            var folder = string.Empty;

            if (ImagenArchivo != null)
            {
                ext = Path.GetExtension(Path.GetFileName(ImagenArchivo.FileName));
                folder = Path.Combine(_env.ContentRootPath, "wwwroot\\products");
                model.Imagen = "/products/";

                if (ext.ToLower() != ".png")
                {
                    ViewBag.Mensaje = "La imagen debe ser .png";
                    return View(model);
                }
            }

            using (var client = _http.CreateClient())
            {
                var url = _conf.GetSection("Variables:UrlApi").Value + "Producto/RegistrarProducto";
                JsonContent datos = JsonContent.Create(model);

                var response = client.PostAsync(url, datos).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    if (ImagenArchivo != null)
                    {
                        var archivo = Path.Combine(folder, result.Mensaje + ext);
                        using (Stream fs = new FileStream(archivo, FileMode.Create))
                        {
                            ImagenArchivo.CopyTo(fs);
                        }
                    }

                    return RedirectToAction("ListaProductosAdmin", "Producto");
                }
                else
                {
                    ViewBag.Mensaje = result!.Mensaje;
                    return View(model);
                }
            }
        }

        // Vista para consultar todos los productos
        [HttpGet]
        public IActionResult ListaProductosAdmin()
        {
            using (var client = _http.CreateClient())
            {
                string url = _conf.GetSection("Variables:UrlApi").Value + "Producto/ConsultarProductos";

                try
                {
                    var response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadFromJsonAsync<JsonElement>().Result;

                        if (result.TryGetProperty("success", out var success) && success.GetBoolean() &&
                            result.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Array)
                        {
                            var productos = JsonSerializer.Deserialize<List<Producto>>(data.ToString());
                            return View(productos);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "No se encontraron productos.";
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

                return View(new List<Producto>());
            }
        }

        // Vista para consultar un producto por ID
        [HttpGet]
        public IActionResult ConsultarProducto(int productoId)
        {
            using (var client = _http.CreateClient())
            {
                string url = _conf.GetSection("Variables:UrlApi").Value + $"Producto/ConsultarProducto/{productoId}";

                try
                {
                    var response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadFromJsonAsync<JsonElement>().Result;

                        if (result.TryGetProperty("success", out var success) && success.GetBoolean() &&
                            result.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Object)
                        {
                            var producto = JsonSerializer.Deserialize<Producto>(data.ToString());
                            return View(producto);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Producto no encontrado.";
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

                return View(new Producto());
            }
        }

        [HttpGet]
        public IActionResult ListaProductos()
        {
            using (var client = _http.CreateClient())
            {
                string url = _conf.GetSection("Variables:UrlApi").Value + "Producto/ConsultarProductosActivos";

                try
                {
                    var response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadFromJsonAsync<JsonElement>().Result;

                        if (result.TryGetProperty("success", out var success) && success.GetBoolean() &&
                            result.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Array)
                        {
                            var productos = JsonSerializer.Deserialize<List<Producto>>(data.ToString());
                            return View(productos);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "No se encontraron productos.";
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

                return View(new List<Producto>());
            }
        }

        // Acción para actualizar un producto
        [HttpPost]
        public IActionResult ActualizarProducto(Producto model)
        {
            if (ModelState.IsValid)
            {
                using (var client = _http.CreateClient())
                {
                    string url = _conf.GetSection("Variables:UrlApi").Value + "Producto/ActualizarProducto";

                    var response = client.PutAsJsonAsync(url, model).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("ListaProductos");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "No se pudo actualizar el producto.";
                    }
                }
            }
            return View(model);
        }
    }
}
