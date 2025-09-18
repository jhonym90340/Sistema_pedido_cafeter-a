using Sistema_Cafeteria.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Cafeteria.Pricing
{
    public interface IPricingStrategy
    {
        // El método se declara SIN un cuerpo. 
        decimal CalcularTotal(Pedido pedido);
    }
}
