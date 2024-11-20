namespace Proyecto_API.Models
{
    public class Usuario
    {
        public long UsuarioID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Contrasenna { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public bool ClaveTemp { get; set; } = false;
        public DateTime Vigencia { get; set; }

    }
}
