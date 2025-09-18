// Espacios de nombres (using)
using System; // Contiene clase basicas del sistema como Console, String, ArgumentException, etc.
using System.Collections.Generic; // Colecciones genericas como List<T>, Dictionary<TKey, TValue>, etc.
using System.Linq; // Consultas LINQ para manipular colecciones
using System.Security.Cryptography.X509Certificates;
using System.Text; // Utilidades de texto y codificacion
using System.Threading.Tasks; // Programacion asincrona y paralela con Task y async/await

namespace Sistema_Cafeteria.Domain 
{
    // Definicion de la clase Persona como abstracta para que no pueda ser instanciada directamente
    public abstract class Persona 

{
       
        // Encapsular datos de la persona
    public string Documento { get; } // Propiedad autoimplementada con acceso publico para lectura 
    public string Nombre { get; private set; } // Propiedad con acceso publico para lectura y privado para escritura
        // Constructo protegido
    protected Persona(string documento, string nombre) // Constructor protegido para que solo clases derivadas puedan instanciar
    {
        if (string.IsNullOrWhiteSpace(documento)) // Validacion de datos
        {
            throw new ArgumentException("El documento no puede estar vacio"); // Lanzar excepcion si el dato es invalido
        }
        if (string.IsNullOrWhiteSpace(nombre)) // Validacion de datos
        {
            throw new ArgumentException("El nombre no puede estar vacio"); // Lanzar excepcion si el dato es invalido
        }
        Documento = documento; // Asignacion de valor a la propiedad
        Nombre = nombre.Trim(); // Asignacion de valor a la propiedad con eliminacion de espacios en blanco
    }
        // Metodo para cambiar el nombre de la persona

        public void CambiarNombre(string nuevoNombre) // Metodo publico para cambiar el nombre
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre)) return; // Validacion de datos

            Nombre = nuevoNombre.Trim(); // Asignacion de nuevo valor a la propiedad con eliminacion de espacios en blanco
        }

        // Metodo sobrescrito para mostrar informacion de la persona o sobrescribir ToString
        public override string ToString() => $"{Nombre} ({Documento})"; // Sobrescribir metodo ToString para mostrar informacion de la persona
}

}

