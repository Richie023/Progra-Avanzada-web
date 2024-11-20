using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.Data.SqlClient;
using Proyecto_API.Models;

namespace Proyecto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _conf;

        public LoginController(IConfiguration conf)
        {
            _conf = conf;
        }


        [HttpPost]
        [Route("CrearCliente")]
        public IActionResult CrearCliente(Cliente model)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                    var result = context.Execute("CrearCliente", new 
                {
                    model.Username,
                    model.Contrasenna,
                    model.Nombre,
                    model.Apellidos,
                    model.FechaNacimiento,
                    model.Genero,
                    model.Telefono,
                    model.Email,
                    model.Direccion
                });
                if (result > 0)
                {
                    respuesta.Codigo = 0;
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "No se ha podido registrar el cliente";
                }
                return Ok(respuesta);
            }
        }
        [HttpPost]
        [Route("IniciarSesion")]
        public IActionResult IniciarSesion(Usuario model)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.QueryFirstOrDefault<Usuario>("IniciarSesion", new { model.Username, model.Contrasenna });

                if (result != null)
                {
                    if (result.ClaveTemp && result.Vigencia < DateTime.Now)
                    {
                        respuesta.Codigo = -1;
                        respuesta.Mensaje = "Su información de acceso temporal ha expirado";
                    }
                    else
                    {
                        respuesta.Codigo = 0;
                        respuesta.Contenido = result;
                    }
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "Su información no se ha validado correctamente";
                }

                return Ok(respuesta);
            }
        }
    }
}
