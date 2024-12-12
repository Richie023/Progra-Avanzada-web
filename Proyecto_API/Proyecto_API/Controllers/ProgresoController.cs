using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using Proyecto_API.Models;
using System.Data;

namespace Proyecto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgresoController : ControllerBase
    {
        private readonly IConfiguration _conf;

        public ProgresoController(IConfiguration conf)
        {
            _conf = conf;
        }

        [HttpPost]
        [Route("CrearProgreso")]
        public IActionResult CrearProgreso(Progreso model)
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var result = connection.Execute("RegistrarProgreso", new
                    {
                        model.UsuarioID,
                        model.Peso,
                        model.CantidadEJercicios,
                        model.DuracionEntrenamiento
                    }, commandType: CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        return Ok(new { Message = "Progreso registrado correctamente" });
                    }
                    else
                    {
                        return BadRequest(new { Message = "No se pudo registrar el progreso" });
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = ex.Message });
                }
            }
        }

        [HttpGet]
        [Route("ListaProgreso/{usuarioId}")]
        public async Task<IActionResult> ListaProgreso(long usuarioId)
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var progreso = await connection.QueryAsync<Progreso>(
                        "ConsultarProgreso",
                        new { UsuarioID = usuarioId },
                        commandType: CommandType.StoredProcedure
                    );

                    if (progreso == null || !progreso.Any())
                    {
                        return NotFound(new { success = false, message = "No se encontraron registros de progreso para el usuario." });
                    }

                    return Ok(new { success = true, data = progreso });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { success = false, message = ex.Message });
                }
            }
        }
    }
}
