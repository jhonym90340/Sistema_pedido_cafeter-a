
using Sistema_Cafeteria.Domain;
using Sistema_Cafeteria.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Cafeteria.Pricing
{
    public class DescuentoPorCantidadStrategy : IPrecioStrategy
    {
        private readonly int _cantidadMinima;
        private readonly decimal _porcentajeDescuento;

        public DescuentoPorCantidadStrategy(int cantidadMinima, decimal porcentajeDescuento)
        {
            _cantidadMinima = cantidadMinima;
            _porcentajeDescuento = porcentajeDescuento;
        }



        public decimal CalcularTotal(Pedido pedido)
        {
            
            int totalItems = pedido.Lineas.Sum(l => l.Cantidad);

            if (totalItems >= _cantidadMinima)
            {
                // El descuento se aplica sobre el subtotal de todas las líneas.
                decimal subtotal = pedido.Lineas.Sum(l => l.Subtotal);
                return subtotal - (subtotal * _porcentajeDescuento);
            }

            // Si no se cumple la condición, el total es simplemente el subtotal.
            return pedido.Lineas.Sum(l => l.Subtotal);
        }



    }
}
