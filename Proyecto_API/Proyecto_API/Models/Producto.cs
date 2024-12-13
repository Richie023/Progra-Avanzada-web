namespace Proyecto_API.Models
{
    public class Producto
    {
        public int ProductoID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}
