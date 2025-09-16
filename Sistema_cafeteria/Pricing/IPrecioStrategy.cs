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
    public interface IPrecioStrategy
    {
        decimal CalcularTotal(Pedido pedido);
    }
}
