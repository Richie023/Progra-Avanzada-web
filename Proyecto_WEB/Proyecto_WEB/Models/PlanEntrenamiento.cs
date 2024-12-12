namespace Proyecto_WEB.Models
{
    public class PlanEntrenamiento
    {
        public int PlanEntrenamientoID { get; set; }
        public long UsuarioID { get; set; }
        public string Ejercicio { get; set; } = string.Empty;
        public int Repeticiones { get; set; }
        public decimal Peso { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
