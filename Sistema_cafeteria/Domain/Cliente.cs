using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Cafeteria.Domain
{
    public class Cliente : Persona
    {
        // Propiedad Email 
        public string Email { get; private set; } 

        // Constructor
        public Cliente(string documento, string nombre, string email) : base(documento, nombre) // Constructor publico que llama al constructor base de Persona
        {
            // Inicializacion de la propiedad Email con validacion y formateo
            Email = string.IsNullOrWhiteSpace(email) ? "sin correo@na" : email.Trim().ToLower(); // Asignacion de valor a la propiedad con validacion y formateo
        }

    }
}