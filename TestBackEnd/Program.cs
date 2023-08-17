// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
// print("Hello, World!"

/*
nombre = "Jhon"
apellido = "Snow"
edad = 10
*/

var Lista = new List<Persona>();

while (true)
{
    string? nombre;
    string? apellido;
    int edad;

    Console.Write("Ingresa tu Nombre: ");
    nombre = Console.ReadLine();
    Console.Write("Ingresa tu Apellido: ");
    apellido = Console.ReadLine();
    Console.Write("Ingresa tu Edad: ");
    edad = Convert.ToInt32(Console.ReadLine());


    // P = Persona() => Python
    var P = new Persona()
    {
        Nombre = nombre,
        Apellido = apellido,
        Edad = edad
    };


    Lista.Add(P);

    Console.Clear();
    Console.WriteLine("Listado de Personas:");
    foreach (var p in Lista)
    {
        Console.WriteLine(p.ToString());
    }

    Console.ReadLine();
}




//Console.WriteLine("---Resumen---");
//Console.WriteLine($"Nombre Completo: {nombre} {apellido}, Edad: {edad}");
//Console.WriteLine("Nombre Completo" + nombre + " " + apellido + ", Edad:" + edad);
Console.ReadLine();


public class Persona
{
    public int Edad { get; set; }
    public string? Apellido { get; set; }
    public string? Nombre { get; set; }

    public override string ToString()
    {
        return $"Nombre Completo: {Nombre} {Apellido}, " +
            $"Edad: {Edad}";
    }
}
