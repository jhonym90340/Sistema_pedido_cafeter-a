using Sistema_Cafeteria.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Cafeteria.Repositories
{
    public class RepositorioPedidosMemoria 
    {
        // Usamos una lista estática para guardar los pedidos en memoria
        private static readonly List<Pedido> _pedidos = new();

        public void Guardar(Pedido pedido)
        {
            _pedidos.Add(pedido);
        }

        // Este es el método que falta y causa el error
        public IEnumerable<Pedido> ObtenerTodos()
        {
            return _pedidos;
        }
    }
}
