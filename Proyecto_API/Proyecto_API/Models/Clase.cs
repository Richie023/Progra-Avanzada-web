namespace Proyecto_API.Models
{
    public class Clase
    {
      public int ClaseID { get; set; }
      public string Nombre { get; set; } = string.Empty;
      public string Descripcion {  get; set; } = string.Empty;
      public int Duracion {  get; set; }
      public DateTime Horario { get; set; }
      public string Entrenador { get; set; } = string.Empty;
    }
}
