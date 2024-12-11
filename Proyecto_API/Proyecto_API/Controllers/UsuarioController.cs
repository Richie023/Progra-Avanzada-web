using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using Proyecto_API.Models;

namespace Proyecto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IConfiguration _conf;

        public UsuarioController(IConfiguration conf)
        {
            _conf = conf;
        }

        [HttpPost("Registrar")]
        public IActionResult RegistrarUsuario(Usuario model)
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                // Verificar si el RolID existe
                var rolExists = connection.ExecuteScalar<bool>(
                    "SELECT COUNT(1) FROM Rol WHERE RolID = @RolID",
                    new { model.RolID });

                if (!rolExists)
                {
                    return BadRequest(new { Message = "El RolID proporcionado no es válido." });
                }

                // Verificar si el Username ya existe
                var exists = connection.ExecuteScalar<bool>(
                    "SELECT COUNT(1) FROM Usuario WHERE Username = @Username",
                    new { model.Username });

                if (exists)
                {
                    return BadRequest(new { Message = "El nombre de usuario ya existe." });
                }

                // Asignar una fecha de vigencia válida si no se proporciona
                if (model.Vigencia == DateTime.MinValue)
                {
                    model.Vigencia = DateTime.Now.AddYears(1); // Ejemplo: Vigencia de 1 año
                }

                // Insertar el nuevo usuario
                var usuarioID = connection.ExecuteScalar<long>(
                    "INSERT INTO Usuario (Username, Contrasenna, Activo, ClaveTemp, Vigencia, RolID) " +
                    "OUTPUT INSERTED.UsuarioID " +
                    "VALUES (@Username, @Contrasenna, @Activo, @ClaveTemp, @Vigencia, @RolID);",
                    new
                    {
                        model.Username,
                        model.Contrasenna,
                        model.Activo,
                        model.ClaveTemp,
                        model.Vigencia,
                        model.RolID
                    });

                return Ok(usuarioID);
            }
        }


        [HttpGet("Existe")]
        public IActionResult Existe(string username)
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                var exists = connection.ExecuteScalar<bool>(
                    "SELECT COUNT(1) FROM Usuario WHERE Username = @Username",
                    new { Username = username });
                return Ok(exists);
            }
        }

        [HttpGet]
        [Route("ListaUsuarios")]
        public IActionResult ListaUsuarios()
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                var usuarios = connection.Query<Usuario>("SELECT UsuarioID, Username FROM Usuario").ToList();

                if (usuarios != null && usuarios.Any())
                {
                    return Ok(usuarios);
                }
                else
                {
                    return NotFound(new { Message = "No se encontraron usuarios." });
                }
            }
        }

        [HttpGet("ListarRoles")]
        public IActionResult ListarRoles()
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                var roles = connection.Query<Rol>("SELECT RolID, NombreRol FROM Rol").ToList();
                return Ok(roles);
            }
        }


    }
}
