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

        [HttpPost]
        [Route("Registrar")]
        public IActionResult RegistrarEmpleado(Empleado model)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var result = context.Execute("RegistrarEmpleado", new
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

                return Ok(new { Codigo = result > 0 ? 0 : -1 });
            }
        }
    }
}
