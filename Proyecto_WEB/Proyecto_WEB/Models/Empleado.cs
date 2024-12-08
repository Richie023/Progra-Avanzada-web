namespace Proyecto_WEB.Models
{
    public class Empleado
    {
        public int EmpleadoID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }
        public int Telefono { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public DateTime? FechaContratacion { get; set; }
        public int CargoID { get; set; }
        public long UsuarioID { get; set; }

        public Usuario Usuario { get; set; } = new Usuario();
    }
}
