using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Proyecto_API.Models;
using System.Data;

namespace Proyecto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiembrosController : ControllerBase
    {
        private readonly IConfiguration _conf;
        public MiembrosController(IConfiguration conf)
        {
            _conf = conf;
        }

        [HttpGet]
        [Route("Miembro/{usuarioId}")]
        public async Task<IActionResult> Miembro(long usuarioId)
        {
            using (var connection = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                try
                {
                    var resultado = await connection.QueryFirstOrDefaultAsync<Miembro>(
                        "ConsultarMiembro",
                        new { UsuarioID = usuarioId },
                        commandType: CommandType.StoredProcedure
                    );

                    var respuesta = new Respuesta();

                    if (resultado != null)
                    {
                        respuesta.Codigo = 0;
                        respuesta.Contenido = resultado;
                    }
                    else
                    {
                        respuesta.Codigo = -1;
                        respuesta.Mensaje = "Miembro no encontrado.";
                    }

                    return Ok(respuesta);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { success = false, message = ex.Message });
                }
            }
        }
    }
}
