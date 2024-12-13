using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using Proyecto_API.Models;
using System.Data;

namespace Proyecto_API.Controllers
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

        // Registrar Producto
        [HttpPost]
        [Route("RegistrarProducto")]
        public IActionResult CrearProducto(Producto model)
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var result = connection.Execute("RegistrarProducto", new
                    {
                        model.Nombre,
                        model.Descripcion,
                        model.Precio,
                        model.Stock,
                        model.Imagen,
                        model.Activo
                    }, commandType: CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        return Ok(new { Message = "Producto registrado correctamente" });
                    }
                    else
                    {
                        return BadRequest(new { Message = "No se pudo registrar el producto" });
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = ex.Message });
                }
            }
        }

        // Consultar todos los productos
        [HttpGet]
        [Route("ConsultarProductos")]
        public async Task<IActionResult> ListaProductosAdmin()
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var productos = await connection.QueryAsync<Producto>(
                        "ConsultarProductos",
                        commandType: CommandType.StoredProcedure
                    );

                    if (productos == null || !productos.Any())
                    {
                        return NotFound(new { success = false, message = "No se encontraron productos." });
                    }

                    return Ok(new { success = true, data = productos });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { success = false, message = ex.Message });
                }
            }
        }

        // Consultar un producto por ID
        [HttpGet]
        [Route("ConsultarProducto/{productoId}")]
        public async Task<IActionResult> ConsultarProducto(int productoId)
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var producto = await connection.QuerySingleOrDefaultAsync<Producto>(
                        "ConsultarProducto",
                        new { ProductoID = productoId },
                        commandType: CommandType.StoredProcedure
                    );

                    if (producto == null)
                    {
                        return NotFound(new { success = false, message = "Producto no encontrado." });
                    }

                    return Ok(new { success = true, data = producto });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { success = false, message = ex.Message });
                }
            }
        }

        // Consultar productos activos
        [HttpGet]
        [Route("ConsultarProductosActivos")]
        public async Task<IActionResult> ListaProductos()
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var productos = await connection.QueryAsync<Producto>(
                        "ConsultarProductosActivos",
                        commandType: CommandType.StoredProcedure
                    );

                    if (productos == null || !productos.Any())
                    {
                        return NotFound(new { success = false, message = "No se encontraron productos activos." });
                    }

                    return Ok(new { success = true, data = productos });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { success = false, message = ex.Message });
                }
            }
        }

        // Actualizar Producto
        [HttpPut]
        [Route("ActualizarProducto")]
        public IActionResult EditarProducto(Producto model)
        {
            using (var connection = new SqlConnection(_conf.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var result = connection.Execute("ActualizarProducto", new
                    {
                        model.ProductoID,
                        model.Nombre,
                        model.Descripcion,
                        model.Precio,
                        model.Stock,
                        model.Imagen,
                        model.Activo
                    }, commandType: CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        return Ok(new { Message = "Producto actualizado correctamente" });
                    }
                    else
                    {
                        return BadRequest(new { Message = "No se pudo actualizar el producto" });
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = ex.Message });
                }
            }
        }
    }
}
