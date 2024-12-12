namespace Proyecto_API.Models
{
    public class Progreso
    {
        public long ProgresoID { get; set; }
        public long UsuarioID { get; set; }
        public int PlanEntrenamientoID { get; set; }
        public DateTime FechaRegistro { get; set; }
        public decimal PesoEntrenamiento { get; set; }
        public int RepeticionesCompletadas { get; set; }
        public int TiempoEntrenamiento { get; set; }
    }
}
