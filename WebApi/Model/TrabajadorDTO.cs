using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Model
{
    public class TrabajadorDTO
    {
        public string Rut { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; }
        public string Comuna { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public int Cod_Dpto { get; set; }
    }
}
