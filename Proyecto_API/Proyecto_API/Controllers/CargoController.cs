using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using Proyecto_API.Models;

namespace Proyecto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargoController : ControllerBase
    {
        private readonly IConfiguration _conf;

        public CargoController(IConfiguration conf)
        {
            _conf = conf;
        }

        [HttpGet("Listar")]
        public IActionResult ListarCargos()
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                var cargos = connection.Query<Cargo>("SELECT * FROM Cargo");
                return Ok(cargos);
            }
        }
    }
}
