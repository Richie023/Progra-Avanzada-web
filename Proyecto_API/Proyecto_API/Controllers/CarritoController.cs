using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Proyecto_API.Models;
using System.Data;

namespace SApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase
    {
        private readonly IConfiguration _conf;
        public CarritoController(IConfiguration conf)
        {
            _conf = conf;
        }

        [HttpPost]
        [Route("AgregarCarrito")]
        public IActionResult AgregarCarrito(Carrito model)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.Execute("RegistrarCarrito", new { model.UsuarioID, model.ProductoID, model.Unidades });

                if (result > 0)
                {
                    respuesta.Codigo = 0;
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "El producto no se ha añadido correctamente en su carrito";
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("ConsultarCarrito/{usuarioId}")]
        public IActionResult ConsultarCarrito(long usuarioId)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.Query<Carrito>(
                    "ConsultarCarrito", 
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
                    respuesta.Mensaje = "No hay productos en su carrito";
                }

                return Ok(respuesta);
            }
        }

        [HttpPost]
        [Route("EliminarProductoCarrito")]
        public IActionResult EliminarProductoCarrito(Carrito model)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.Execute("RemoverProductoCarrito", new { model.UsuarioID, model.ProductoID });

                if (result > 0)
                {
                    respuesta.Codigo = 0;
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "El producto no se ha removido dn su carrito";
                }

                return Ok(respuesta);
            }
        }

        [HttpPost]
        [Route("PagarCarrito")]
        public IActionResult PagarCarrito(Carrito model)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.Execute("PagarCarrito", new { model.UsuarioID });

                if (result > 0)
                {
                    respuesta.Codigo = 0;
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "No se realizó el pago de su carrito";
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("ConsultarFacturas/{usuarioId}")]
        public IActionResult ConsultarFacturas(long usuarioId)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.Query<Carrito>(
                    "ConsultarFacturas",
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
                    respuesta.Mensaje = "No hay facturas registradas en este momento";
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("ConsultarDetallesFactura/{facturaId}")]
        public IActionResult ConsultarDetallesFactura(long facturaId)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.Query<Carrito>(
                    "ConsultarDetallesFactura",
                    new { FacturaID = facturaId },
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
                    respuesta.Mensaje = "No hay detalles para esa factura en este momento";
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("ConsultarFacturasAdmin")]
        public IActionResult ConsultarFacturasAdmin()
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.Query<Carrito>(
                    "ConsultarFacturasAdmin",
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
                    respuesta.Mensaje = "No hay facturas registradas en este momento";
                }

                return Ok(respuesta);
            }
        }
    }
}
