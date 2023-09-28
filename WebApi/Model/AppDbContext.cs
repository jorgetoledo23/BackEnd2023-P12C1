using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Model
{
    //Class de Conexion a la BD
    public class AppDbContext : DbContext
    {
        //Tablas de la BD
        public DbSet<Departamento> TblDepartamentos { get; set; }
        public DbSet<Trabajador> TblTrabajadores { get; set; }
        public DbSet<Carga> TblCargas { get; set; }
        public DbSet<Contacto> TblContactos { get; set; }
        // --- Fin Tablas --
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ApiWeb2023P12C1;Integrated Security=True;");
        }
    }

    public class Departamento
    {
        [Key]
        public int Cod_Dpto { get; set; } //PK

        [StringLength(50, ErrorMessage ="Excede Maximo de Caracteres")]
        //[DataType(DataType.Password)]
        [Required]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

    }

    public class Trabajador
    {
        [Key]
        public string Rut { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; }
        public string Comuna { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }

        //Clave Foranea
        [ForeignKey("Departamento")]
        public int Cod_Dpto { get; set; }
        public Departamento Departamento { get; set; }


    }


    /*
     Crear las tablas Cargas Familiares y Contactos de Emergencia y enlazarlas a la tabla Trabajador
     */

    public class Carga
    {
        [Key]
        public string Rut { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }

        [ForeignKey("Trabajador")]
        public string RutTrabajador { get; set; }
        public Trabajador Trabajador { get; set; }

    }

    public class Contacto
    {
        [Key]
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Rut { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; }
        public string Comuna { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }


        [ForeignKey("Trabajador")]
        public string RutTrabajador { get; set; }
        public Trabajador Trabajador { get; set; }

    }



}
