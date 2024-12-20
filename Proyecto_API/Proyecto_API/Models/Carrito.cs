namespace Proyecto_API.Models
{
    public class Carrito
    {
        public long UsuarioID { get; set; }
        public long ProductoID { get; set; }
        public int Unidades { get; set; }
        public DateTime Fecha { get; set; }

        public long FacturaID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public decimal Total { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}
