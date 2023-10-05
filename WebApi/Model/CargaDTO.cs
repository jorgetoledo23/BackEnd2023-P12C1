using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Model
{
    public class CargaDTO
    {
        public string Rut { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string RutTrabajador { get; set; } 
    }
}
