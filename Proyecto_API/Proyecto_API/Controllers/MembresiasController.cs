using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Proyecto_API.Models;

namespace Proyecto_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembresiasController : ControllerBase
    {
        private readonly IConfiguration _conf;

        public MembresiasController(IConfiguration conf)
        {
            _conf = conf;
        }

        [HttpGet]
        [Route("ObtenerMembresias")]
        public async Task<IActionResult> ObtenerMembresias()
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var membresias = await connection.QueryAsync<Membresia>(
                        "ObtenerMembresias",
                        commandType: CommandType.StoredProcedure
                    );

                    if (membresias == null || !membresias.Any())
                    {
                        return NotFound(new { success = false, message = "No se encontraron membresías." });
                    }

                    return Ok(new { success = true, data = membresias });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { success = false, message = ex.Message });
                }
            }
        }
    }
}
