using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistema_Cafeteria.Domain;

namespace Sistema_Cafeteria.Domain
{
    public class Transferencia : IMedioPago
    {
        //Propiedades específicas de la transferencia
        public string Banco { get; } // Propiedad con acceso publico para lectura|
        public string Referencia { get; } // Propiedad con acceso publico para lectura

        // Propiedad Descripcion

        public string Descripcion => $"Transferencia bancaria {Banco} Ref:{Referencia}"; // Propiedad autoimplementada con acceso publico para lectura

        // Constructor

        public Transferencia(string banco, string referencia) // Constructor publico
        {
            Banco = string.IsNullOrWhiteSpace(banco) ? "NA" : banco.Trim(); // Asignacion de valor a la propiedad con validacion y formateo
            Referencia = string.IsNullOrWhiteSpace(referencia) ? "000000" : referencia.Trim(); // Asignacion de valor a la propiedad con validacion y formateo
        }

        // Metodo Autorizar

        public bool Autorizar(decimal monto) => monto > 0; // Metodo para autorizar el pago

        // Metodo Capturar

        public void Capturar(decimal monto) { /* nada que hacer */ }
    }
}
