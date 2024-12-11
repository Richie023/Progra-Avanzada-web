using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using Proyecto_API.Models;

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
                    model.Fecha
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
        public IActionResult ListaPlan([FromQuery] long usuarioID, [FromQuery] int rolID)
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                IEnumerable<PlanEntrenamiento> planes;

                if (rolID == 1)
                {
                    planes = connection.Query<PlanEntrenamiento>("ConsultarPlanEntenamiento", new { UsuarioID = usuarioID, RolID = rolID }, commandType: System.Data.CommandType.StoredProcedure);
                }
                else
                {
                    planes = connection.Query<PlanEntrenamiento>("ConsultarPlanEntenamiento", new { UsuarioID = usuarioID, RolID = rolID }, commandType: System.Data.CommandType.StoredProcedure);
                }

                return Ok(planes);
            }
        }
    }
}
