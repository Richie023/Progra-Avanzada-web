using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using Proyecto_API.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

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

        [HttpGet]
        [Route("ObtenerPlan/{id}")]
        public IActionResult ObtenerPlan(int id)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var plan = context.QueryFirstOrDefault<PlanEntrenamiento>(
                    "SELECT * FROM PlanEntrenamiento WHERE PlanEntrenamientoID = @Id", new { Id = id });

                if (plan == null)
                {
                    return NotFound(new { Mensaje = "No se encontró el plan de entrenamiento." });
                }

                return Ok(plan);
            }
        }

        [HttpPost]
        [Route("CrearPlan")]
        public IActionResult CrearPlan([FromBody] PlanEntrenamiento planEntrenamiento)
        {
            if (planEntrenamiento == null)
            {
                return BadRequest(new { Mensaje = "El plan de entrenamiento no puede ser nulo." });
            }

            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var query = "INSERT INTO PlanEntrenamiento (UsuarioID, Ejercicio, Repeticiones, Peso, Fecha) " +
                            "VALUES (@UsuarioID, @Ejercicio, @Repeticiones, @Peso, @Fecha); " +
                            "SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var planId = context.Query<int>(query, new
                {
                    UsuarioID = planEntrenamiento.UsuarioID,
                    Ejercicio = planEntrenamiento.Ejercicio,
                    Repeticiones = planEntrenamiento.Repeticiones,
                    Peso = planEntrenamiento.Peso,
                    Fecha = planEntrenamiento.Fecha
                }).Single();

                return CreatedAtAction("ObtenerPlan", new { id = planId }, new { Mensaje = "Plan creado exitosamente." });
            }
        }
    }
}
