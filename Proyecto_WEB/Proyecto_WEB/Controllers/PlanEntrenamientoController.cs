using Microsoft.AspNetCore.Mvc;
using Proyecto_WEB.Models;
using System.Net.Http;
using System.Numerics;
using System.Text.Json;

namespace Proyecto_WEB.Controllers
{
    public class PlanEntrenamientoController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlApi;

        public PlanEntrenamientoController(IConfiguration configuration)
        {
            _urlApi = configuration["Variables:UrlApi"] ?? "https://localhost:5001/api/";
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_urlApi)
            };
        }
        public async Task<IActionResult> Index(int id)
        {
            var response = await _httpClient.GetAsync($"PlanEntrenamiento/ObtenerPlan/1");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var plan = JsonSerializer.Deserialize<PlanEntrenamiento>(jsonResponse);

                return View(plan);
            }
            else
            {
                ViewBag.Error = $"No se pudo obtener el plan de entrenamiento. Código de error: {response.StatusCode}";
                return View();
            }
        }
    }
}
