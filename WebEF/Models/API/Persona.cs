using System.Text.Json.Serialization;

namespace WebEF.Models.API
{
    public class Persona
    {
        
        public string? Nombres { get; set; }
                
        public string? Apellidos { get; set; }

        public string? TipoDocumento { get; set; }

        public string? NroDocumento { get; set; }

        public string? EstadoCivil { get; set; }

        public string? FechaNacimiento { get; set; }

        public string? Fecha { get; set; } 

        public decimal ValorGanar { get; set; }
    }
}
