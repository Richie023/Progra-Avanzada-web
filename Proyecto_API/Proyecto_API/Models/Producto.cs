namespace Proyecto_API.Models
{
    public class Producto
    {
        public long ProductoID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
