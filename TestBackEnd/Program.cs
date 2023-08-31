// Python => nombre = "test"

// Lenguaje Fuertemente Tipado

// Tipo Variable - Nombre = ValorPorDefecto

string Nombre = "Test";
int Edad = 10;
bool VerdaderoFalso = false;
float PI = 1.1416f;
char caracter = 'a';

List<string> Listado = new List<string>();

Listado.Add("Chile");
Listado.Add("Arg");

var apellido = "HOLA";
var flag = true;

// Control de Flujo

if (apellido.Contains("OL") || apellido.StartsWith("S"))
{
    Console.WriteLine("Verdadero");
}
else
{
    Console.WriteLine("Falso");
}

while (1 > 3)
{
    Console.WriteLine("ASD");
    break;
}

do
{
    Console.WriteLine("ASD");
    break;
} while (1 > 3);

string mes = "123";
switch (mes)
{
    case "Enero":
        Console.WriteLine("Vacaciones");
        break;

    case "Febrero":
        Console.WriteLine("Vacaciones");
        break;

    default:
        Console.WriteLine("Caso por Defecto");
        break;
}


//Ciclos

for (int i = 0; i < 10; i++)
{
    Console.Write($"{i}");
}

foreach (var item in Listado)
{
    Console.WriteLine(item);
}


var AlumnoSebastian = new Alumno("1-1", "Seba", "Imas");
var AlumnoPedro = new Alumno("1-1");
var AlumnoGonzalo = new Alumno("1-1", "Gonzalo", 23);

var contador = 0;


contador = 1;

Console.WriteLine(AlumnoSebastian.getInfo());
Console.WriteLine(AlumnoPedro.getInfo());


class Persona
{
    public string Rut { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }

    public Persona(string rut, string nombre, string apellido)
    {
        Rut = rut;
        Nombre = nombre;
        Apellido = apellido;
    }
    public Persona(string rut, string nombre, int edad)
    {
        Rut = rut;
        Nombre = nombre;

    }
    public Persona(string rut)
    {
        Rut = rut;
    }

    public string getInfo()
    {
        return $"Rut: {Rut} Nombres: {Nombre} Apellidos: {Apellido}";
    }

}

class Alumno : Persona
{
    public Alumno(string rut, string nombre, string apellido) : base(rut, nombre, apellido)
    {
    }
    public Alumno(string rut) : base(rut)
    {
    }

    public Alumno(string rut, string nombre, int edad) : base(rut, nombre, edad)
    {
    }

    public int CodigoMatricula { get; set; }
}

class Docente : Persona
{
    public Docente(string rut, string nombre, string apellido) : base(rut, nombre, apellido)
    {
    }

    public int NContrato { get; set; }
}

class Administrativo : Persona
{
    public Administrativo(string rut, string nombre, string apellido) : base(rut, nombre, apellido)
    {
    }
}






