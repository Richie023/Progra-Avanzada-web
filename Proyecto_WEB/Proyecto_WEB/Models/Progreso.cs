namespace Proyecto_WEB.Models
{
    public class Progreso
    {
        public long ProgresoID { get; set; }
        public long UsuarioID { get; set; }
        public decimal Peso { get; set; }
        public int CantidadEJercicios { get; set; }
        public int DuracionEntrenamiento { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
