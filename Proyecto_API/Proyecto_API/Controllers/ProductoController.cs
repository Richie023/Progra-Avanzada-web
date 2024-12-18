using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Proyecto_API.Models;

namespace SApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IConfiguration _conf;
        public ProductoController(IConfiguration conf)
        {
            _conf = conf;
        }

        [HttpGet]
        [Route("ListaProductos")]
        public IActionResult ListaProductos()
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.Query<Producto>("ConsultarProductos", new { });

                if (result.Any())
                {
                    respuesta.Codigo = 0;
                    respuesta.Contenido = result;
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "No hay productos registrados en este momento";
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("ListaProductosActivos")]
        public IActionResult ListaProductosActivos()
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.Query<Producto>("ConsultarProductosActivos", new { });

                if (result.Any())
                {
                    respuesta.Codigo = 0;
                    respuesta.Contenido = result;
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "No hay productos registrados en este momento";
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("Producto")]
        public IActionResult Producto(int ProductoID)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.QueryFirstOrDefault<Producto>("ConsultarProducto", new { ProductoID });

                if (result != null)
                {
                    respuesta.Codigo = 0;
                    respuesta.Contenido = result;
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "No hay productos registrados en este momento";
                }

                return Ok(respuesta);
            }
        }

        [HttpPut]
        [Route("ActualizarEstado")]
        public IActionResult ActualizarEstado(Producto model)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();

                var result = context.Execute("ActualizarEstado", new
                {
                    model.ProductoID
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

        [HttpPost]
        [Route("CrearProducto")]
        public IActionResult CrearProducto(Producto model)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.QueryFirstOrDefault<Producto>("RegistrarProducto", new { model.Nombre, model.Precio, model.Stock, model.Imagen });

                if (result != null)
                {
                    respuesta.Codigo = 0;
                    respuesta.Mensaje = result.ProductoID.ToString();
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "La información del producto no se ha registrado correctamente";
                }

                return Ok(respuesta);
            }                       

        }

        [HttpPut]
        [Route("EditarProducto")]
        public IActionResult EditarProducto(Producto model)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();

                var result = context.Execute("ActualizarProducto", new
                {
                    model.ProductoID,
                    model.Nombre,
                    model.Precio,
                    model.Stock
                });

                if (result > 0)
                {
                    respuesta.Codigo = 0;
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "La información del producto no se ha actualizado correctamente";
                }

                return Ok(respuesta);
            }
        }
    }
}
