using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Cafeteria.Domain
{
    public class TarjetaCredito : IMedioPago
    {
        //Propiedades propias de la tarjeta
        public string Titular { get; } // Propiedad con acceso publico para lectura 
        public string NumeroEnmascarado { get; } // Propiedad con acceso publico para lectura // ****_**** **** 1234
        public decimal LimiteDisponible { get; private set; } // Propiedad con acceso publico para lectura y privado para escritura

        // Propiedad Descripcion

        public string Descripcion => $"Tarjeta de {Titular} ({NumeroEnmascarado})"; // Propiedad autoimplementada con acceso publico para lectura

        // Constructor

        public TarjetaCredito(string titular, string ultimos4, decimal limite)

        {
            Titular = string.IsNullOrWhiteSpace(titular) ? "Sin Titular" : titular.Trim(); // Asignacion de valor a la propiedad con validacion y formateo
            NumeroEnmascarado = $"**** **** **** - {(string.IsNullOrWhiteSpace(ultimos4) ? "0000" : ultimos4.Trim())}"; // Asignacion de valor a la propiedad con validacion y formateo
            LimiteDisponible = limite < 0 ? 0 : limite; // Asignacion de valor a la propiedad con validacion
        }

        // Metodo Autorizar
        public bool Autorizar(decimal monto) => monto > 0 && monto <= LimiteDisponible; // Metodo para autorizar el pago

        // Metodo Capturar

        public void Capturar(decimal monto)
        {
            if (!Autorizar(monto)) throw new InvalidOperationException("No se puede capturar un monto no autorizado"); // Validacion de datos
            LimiteDisponible -= monto; // Descontar el monto del limite disponible
        }
    }
}