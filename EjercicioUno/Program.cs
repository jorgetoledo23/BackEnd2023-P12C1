//List<Alumno>()
bool continuar = true;
do
{
    Console.WriteLine("[1] - Ingresar Alumno");
    Console.WriteLine("[2] - Ingresar Nota");
    Console.WriteLine("[3] - Ver Promedio Final");
    Console.WriteLine("[0] - Salir");

    Console.Write("Ingresa la Opcion: ");
    var opcion = Console.ReadLine();


    switch (opcion)
    {
        case "1":
            //TODO: Generar Implementacion Opcion Ingresar Alumno al Listado
            break;
        case "2":
            //TODO: Generar Implementacion Opcion Ingresar Nota
            break;
        case "3":
            //TODO: Generar Implementacion Opcion Promedio
            break;
        case "0":
            continuar = false;
            break;
        default:
            Console.WriteLine("Esa opcion NO es valida!");
            break;
    }
} while (continuar);



/*

Console.Write("Ingresa tu Nombre: ");
var name = Console.ReadLine();

try
{
    Console.Write("Ingresa tu Edad: ");
    int edad = Convert.ToInt32(Console.ReadLine());
}
catch (Exception E)
{
    Console.WriteLine(E.Message);
}

*/
/*
    
Crear una class llamada Alumno con los atributos y metodos que estime conveniente
La idea es ingresar Notas a los objetos creados a partir de la class Alumno
Y crear un metodo que retorne el promedio final de notas (si es que tiene notas ingresadas)
(Generar una especie de Menu para realizar las distintas opciones por el usuario)
 
 */



