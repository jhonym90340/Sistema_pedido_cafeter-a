using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Cafeteria.Domain
{
    public class Empleado : Persona
    {
        // Propiedad Cargo
        public string Cargo { get; private set; }
        public string Clave { get; private set; }

        
        public Empleado(string documento, string nombre, string cargo, string clave)
            : base(documento, nombre)
        {
            // Asignacion de la propiedad Cargo con validacion y formateo
            Cargo = string.IsNullOrWhiteSpace(cargo) ? "General" : cargo.Trim();
          
            Clave = clave;
        }
    }
}