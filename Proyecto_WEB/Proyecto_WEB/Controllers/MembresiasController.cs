using Microsoft.AspNetCore.Mvc;
using Proyecto_WEB.Models;
using System.Text.Json;

namespace Proyecto_WEB.Controllers
{
    public class MembresiasController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlApi = "https://localhost:7179/api/Membresias/ObtenerMembresias";

        public MembresiasController(IConfiguration configuration)
        {
            _urlApi = configuration["Variables:UrlApi"] ?? "https://localhost:7179/api/";
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_urlApi)
            };
        }
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("Membresias/ObtenerMembresias");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var membresias = JsonSerializer.Deserialize<List<Membresia>>(jsonResponse);

                return View(membresias);
            }
            else
            {
                ViewBag.Error = "No se pudieron obtener las membresías.";
                return View(new List<Membresia>());
            }
        }
    }
}
