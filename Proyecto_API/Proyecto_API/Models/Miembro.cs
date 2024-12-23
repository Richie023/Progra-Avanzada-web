﻿namespace Proyecto_API.Models
{
    public class Miembro
    {
        public long MiembroID { get; set; }
        public long UsuarioID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public char Genero { get; set; }
        public int Telefono { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public int MembresiaID { get; set; }

        public string NombreCompleto { get; set; } = string.Empty;
        public string TipoMembresia { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string Duracion { get; set; } = string.Empty;
        public string Beneficios { get; set; } = string.Empty;
    }
}
