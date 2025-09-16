using Sistema_Cafeteria.Domain;
using Sistema_Cafeteria.Pricing;
using Sistema_Cafeteria.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Cafeteria.Pricing
{
    public class PrecioBaseStrategy : IPrecioStrategy
    {
        public decimal CalcularTotal(Pedido pedido)
        {
            // Solo devuelve el total del pedido sin aplicar ninguna regla
            return pedido.Lineas.Sum(l => l.Subtotal);
        }
    }
}
