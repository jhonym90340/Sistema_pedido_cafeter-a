using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Cafeteria.Domain
{
    public interface IMedioPago
    {
        //Propiedad Descripción
        string Descripcion { get; }

        // Método Autorizar

        bool Autorizar(decimal monto); // Método para autorizar el pago

        // Método Capturar
        void Capturar(decimal monto); // Método para capturar el pago
    }
}
