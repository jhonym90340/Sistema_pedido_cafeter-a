using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Cafeteria.Domain
{
    public sealed class Efectivo : IMedioPago
    { 
        // Propiedad Descripción

        public string Descripcion => "Efectivo"; 

        // Metodo Autorizar

        public bool Autorizar(decimal monto) => monto > 0; // Metodo para autorizar el pago

        // Metodo Capturar
        public void Capturar(decimal monto) {}
    }
}

