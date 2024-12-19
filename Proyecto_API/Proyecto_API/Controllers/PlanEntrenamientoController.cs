using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using Proyecto_API.Models;
using System.Data;

namespace Proyecto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanEntrenamientoController : ControllerBase
    {
        private readonly IConfiguration _conf;

        public PlanEntrenamientoController(IConfiguration conf)
        {
            _conf = conf;
        }

        [HttpPost]
        [Route("CrearPlan")]
        public IActionResult CrearPlan(PlanEntrenamiento model)
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                var result = connection.Execute("RegistrarPlanEntenamiento", new
                {
                    model.UsuarioID,
                    model.Ejercicio,
                    model.Repeticiones,
                    model.Peso,
                    FechaCreacion = model.FechaCreacion == default ? (DateTime?)null : model.FechaCreacion
                }, commandType: System.Data.CommandType.StoredProcedure);

                if (result > 0)
                {
                    return Ok(new { Message = "Plan de entrenamiento creado correctamente" });
                }
                else
                {
                    return BadRequest(new { Message = "No se pudo crear el plan de entrenamiento" });
                }
            }
        }

        [HttpGet]
        [Route("ListaPlan")]
        public async Task<IActionResult> ListaPlan()
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var planes = await connection.QueryAsync<PlanEntrenamiento>(
                        "ConsultarPlanEntenamientoAdmin",
                        commandType: CommandType.StoredProcedure
                    );

                    if (planes == null || !planes.Any())
                    {
                        return NotFound(new { success = false, message = "No se encontraron los planes de entrenamiento." });
                    }

                    return Ok(new { success = true, data = planes });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { success = false, message = ex.Message });
                }
            }
        }

        [HttpGet]
        [Route("PlanUsuario/{usuarioId}")]
        public async Task<IActionResult> PlanUsuario(long usuarioId)
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var planes = await connection.QueryAsync<PlanEntrenamiento>(
                        "ConsultarPlanEntrenamientoUsuario",
                        new { UsuarioID = usuarioId },
                        commandType: CommandType.StoredProcedure
                    );

                    if (planes == null || !planes.Any())
                    {
                        return NotFound(new { success = false, message = "No se encontraron planes de entrenamiento para el usuario." });
                    }

                    return Ok(new { success = true, data = planes });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { success = false, message = ex.Message });
                }
            }
        }
    }
}
