namespace Proyecto_WEB.Models
{
    public class Cliente
    {
        public long MiembroID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Contrasenna { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public char Genero { get; set; }
        public int Telefono { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;

    }
}
