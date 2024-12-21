using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_API.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

namespace Proyecto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaseController : ControllerBase
    {

        private readonly IConfiguration _conf;

        public ClaseController(IConfiguration conf)
        {
            _conf = conf;
        }
        [HttpPost]
        [Route("RegistrarMiembroEnClase")]
        public IActionResult RegistrarMiembroEnClase( MiembroClase miembroClase)
        {
            using (var connection = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();

                try
                {
                    connection.Open();

                    var miembro = connection.QueryFirstOrDefault<Miembro>("ConsultarMiembro",new { UsuarioID = miembroClase.UsuarioID });

                    if (miembro != null)
                    {
                        var result = connection.Execute("RegistrarMiembroEnClase",new { ClaseID = miembroClase.ClaseID, MiembroID = miembro.MiembroID });

                        if (result > 0)
                        {
                            respuesta.Codigo = 0;
                            respuesta.Mensaje = "Miembro registrado en la clase exitosamente.";
                        }
                        else
                        {
                            respuesta.Codigo = -1;
                            respuesta.Mensaje = "Error al registrar el miembro en la clase.";
                        }
                    }
                    else
                    {
                        respuesta.Codigo = -1;
                        respuesta.Mensaje = "Miembro no encontrado.";
                    }
                }
                catch (Exception ex)
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = $"Ocurrió un error: {ex.Message}";
                }
                finally
                {
                    connection.Close();
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("ConsultarClases")]
        public IActionResult ConsultarClases()
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.Query<Clase>("ConsultarClases", new { });

                if (result.Any())
                {
                    respuesta.Codigo = 0;
                    respuesta.Contenido = result;
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "No hay clases registrados en este momento";
                }

                return Ok(respuesta);
            }
        }
        [HttpPost]
        [Route("EliminarReservaClase")]
        public IActionResult EliminarReservaClase(MiembroClase miembroClase)
        {
            using (var connection = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();

                try
                {
                    connection.Open();

                    var miembro = connection.QueryFirstOrDefault<Miembro>("ConsultarMiembro", new { UsuarioID = miembroClase.UsuarioID });

                    if (miembro != null)
                    {
                        var result = connection.Execute("EliminarReservaClase", new { ClaseID = miembroClase.ClaseID, MiembroID = miembro.MiembroID });

                        if (result > 0)
                        {
                            respuesta.Codigo = 0;
                            respuesta.Mensaje = "Reserva eliminada exitosamente.";
                        }
                        else
                        {
                            respuesta.Codigo = -1;
                            respuesta.Mensaje = "Error al eliminar la reserva de la clase.";
                        }
                    }
                    else
                    {
                        respuesta.Codigo = -1;
                        respuesta.Mensaje = "Miembro no encontrado.";
                    }
                }
                catch (Exception ex)
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = $"Ocurrió un error: {ex.Message}";
                }
                finally
                {
                    connection.Close();
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("ConsultarUsuarioClases")]
        public IActionResult ConsultarUsuarioClases(int UsuarioID)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.Query<Clase>("ConsultarUsuarioClases", new { UsuarioID });

                if (result.Any())
                {
                    respuesta.Codigo = 0;
                    respuesta.Contenido = result;
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "No hay clases registradas en este momento";
                }

                return Ok(respuesta);
            }
        }
    }
}
