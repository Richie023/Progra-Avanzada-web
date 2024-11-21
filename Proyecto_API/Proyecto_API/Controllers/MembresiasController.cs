using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using Proyecto_API.Models;

namespace Proyecto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembresiasController : ControllerBase
    {
        private readonly IConfiguration _conf;

        public MembresiasController(IConfiguration conf)
        {
            _conf = conf;
        }

        [HttpGet]
        [Route("ObtenerMembresias")]
        public IActionResult ObtenerMembresias()
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var membresias = context.Query<Membresia>("SELECT * FROM Membresia").ToList();

                if (membresias == null || !membresias.Any())
                {
                    return NotFound(new { Mensaje = "No se encontraron membresías." });
                }

                return Ok(membresias);
            }
        }
    }
}