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
                connection.Execute(
                    "EXEC RegistrarEmpleado @Nombre, @Apellidos, @FechaNacimiento, @Telefono, @Email, @Direccion, @FechaContratacion, @CargoID, @UsuarioID",
                    new
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
                    });

                return Ok(new { Message = "Empleado registrado exitosamente." });
            }
        }

        [HttpGet("Listar")]
        public IActionResult ListarEmpleados()
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                var empleados = connection.Query<Empleado>(
                    "SELECT e.*, c.NombreCargo FROM Empleado e LEFT JOIN Cargo c ON e.CargoID = c.CargoID");
                return Ok(empleados);
            }
        }
    }
}
