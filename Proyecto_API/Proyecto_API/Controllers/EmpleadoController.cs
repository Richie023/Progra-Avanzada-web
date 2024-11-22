using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using Proyecto_API.Models;

namespace Proyecto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly IConfiguration _conf;

        public EmpleadoController(IConfiguration conf)
        {
            _conf = conf;
        }

        [HttpPost("Registrar")]
        public IActionResult RegistrarEmpleado(Empleado model)
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                // Ejecuta el procedimiento almacenado para registrar un empleado
                var result = connection.Execute("RegistrarEmpleado", new
                {
                    model.Nombre,
                    model.Apellidos,
                    model.FechaNacimiento,
                    model.Telefono,
                    model.Email,
                    model.Direccion,
                    model.FechaContratacion,
                    model.CargoID,
                    model.UsuarioID
                }, commandType: System.Data.CommandType.StoredProcedure);

                if (result > 0)
                {
                    return Ok(new { Message = "Empleado registrado correctamente" });
                }
                else
                {
                    return BadRequest(new { Message = "No se pudo registrar el empleado" });
                }
            }
        }

        [HttpGet("Listar")]
        public IActionResult ListarEmpleados()
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                var empleados = connection.Query<Empleado>("SELECT * FROM Empleado");
                return Ok(empleados);
            }
        }

    }
}
