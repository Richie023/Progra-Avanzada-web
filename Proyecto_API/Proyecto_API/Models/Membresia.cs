namespace Proyecto_API.Models
{
    public class Membresia
    {
        public int MembresiaID { get; set; }
        public string TipoMembresia { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string Duracion { get; set; } = string.Empty;
        public string Beneficios { get; set; } = string.Empty;
    }
}
