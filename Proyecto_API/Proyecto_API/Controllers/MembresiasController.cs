using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Proyecto_API.Models;

namespace Proyecto_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembresiasController : ControllerBase
    {
        private readonly IConfiguration _conf;

        public MembresiasController(IConfiguration conf)
        {
            _conf = conf;
        }

        [HttpGet]
        [Route("ObtenerMembresias")]
        public async Task<IActionResult> ObtenerMembresias()
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var membresias = await connection.QueryAsync<Membresia>(
                        "ObtenerMembresias",
                        commandType: CommandType.StoredProcedure
                    );

                    if (membresias == null || !membresias.Any())
                    {
                        return NotFound(new { success = false, message = "No se encontraron membresías." });
                    }

                    return Ok(new { success = true, data = membresias });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { success = false, message = ex.Message });
                }
            }
        }

        [HttpPut]
        [Route("ActualizarSinMembresia")]
        public IActionResult ActualizarSinMembresia(Miembro model)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();

                var result = context.Execute("ActualizarSinMembresia", new
                {
                    model.UsuarioID
                });

                if (result > 0)
                {
                    respuesta.Codigo = 0;
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "El estado del producto no se ha actualizado correctamente";
                }

                return Ok(respuesta);
            }
        }
        
        [HttpPut]
        [Route("ActualizarMembresiaRegular")]
        public IActionResult ActualizarMembresiaRegular(Miembro model)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();

                var result = context.Execute("ActualizarMembresiaRegular", new
                {
                    model.UsuarioID
                });

                if (result > 0)
                {
                    respuesta.Codigo = 0;
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "El estado del producto no se ha actualizado correctamente";
                }

                return Ok(respuesta);
            }
        }
        
        [HttpPut]
        [Route("ActualizarMembresiaPremium")]
        public IActionResult ActualizarMembresiaPremium(Miembro model)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();

                var result = context.Execute("ActualizarMembresiaPremium", new
                {
                    model.UsuarioID
                });

                if (result > 0)
                {
                    respuesta.Codigo = 0;
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "El estado del producto no se ha actualizado correctamente";
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("ConsultarMembresiaMiembro/{usuarioId}")]
        public IActionResult ConsultarMembresiaMiembro(long usuarioId)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.Query<Miembro>(
                    "ConsultarMembresiaMiembro", 
                    new { UsuarioID = usuarioId },
                    commandType: CommandType.StoredProcedure
                    );

                if (result.Any())
                {
                    respuesta.Codigo = 0;
                    respuesta.Contenido = result;
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "Este miembro no tiene membresia";
                }

                return Ok(respuesta);
            }
        }
    }
}
