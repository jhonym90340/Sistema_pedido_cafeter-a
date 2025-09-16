using Sistema_Cafeteria.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Cafeteria.Repositories
{
    public interface IRepositorioPedidos
    {
        void Guardar(Pedido pedido);

        IEnumerable<Pedido> ObtenerTodos();
        Pedido? BuscarPorId(int id);
        List<Pedido> ListarTodos();

    }
}
