namespace Proyecto_WEB.Models
{
    public class Empleado
    {
        public int EmpleadoID { get; set; }
        public long UsuarioID { get; set; }
        public string Username { get; set; }
        public string Contrasenna { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaContratacion { get; set; }
        public int CargoID { get; set; }
        public string NombreCargo { get; set; }
    }
}
