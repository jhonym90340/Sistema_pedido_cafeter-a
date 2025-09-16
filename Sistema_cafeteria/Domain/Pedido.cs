using Sistema_Cafeteria.Domain;
using Sistema_Cafeteria.Repositories;
using Sistema_Cafeteria.Pricing;
using System; // tipos basicos como DateTime, EventHandler, etc
using System.Collections.Generic; // colecciones genericas como List<T>, Dictionary<TKey, TValue>, etc
using System.Linq;// para hacer consultas LINQ como sum(), where(), select(), etc|sobre otras listas
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Cafeteria.Domain
{
    public class Pedido
    {
        // Campo estático para IDs
        private static int _contadorId = 0; // Campo estático para llevar el conteo de IDs

        // Propiedades

        public int Id { get; } // Propiedad autoimplementada con acceso publico para lectura
        public Cliente Cliente { get; } // Propiedad autoimplementada con acceso publico para lectura
        public DateTime Fecha { get; } // Propiedad autoimplementada con acceso publico para lectura
        public List<LineaPedido> Lineas { get; } = new(); // Propiedad autoimplementada con acceso publico para lectura

        // Total sin descuento; se mantiene como una propiedad separada para la lógica del descuento
        public decimal SubtotalSinDescuento => Lineas.Sum(l => l.Subtotal);

        // Nuevo: Monto del descuento aplicado
        public decimal DescuentoAplicado =>
            Lineas.Count >= 6 ? SubtotalSinDescuento * 0.10m : 0m;

        // Total con descuento aplicado si corresponde
        public decimal Total => SubtotalSinDescuento - DescuentoAplicado;

        // Constructor
        public Pedido(Cliente cliente) // Constructor publico
        {
            Id = ++_contadorId; // Asignacion de valor a la propiedad con incremento del contador
            Cliente = cliente ?? throw new ArgumentNullException(nameof(cliente)); // Asignacion de valor a la propiedad con validacion
            Fecha = DateTime.Now; // Asignacion de valor a la propiedad con la fecha actual
        }

        // Metodo para agregar una linea al pedido

        public void AgregarLinea(Producto producto, int cantidad) // Metodo publico para agregar una linea al pedido
        {
            // Aseguramos integridad de datos: descontar stock del producto antes de "cerrar" la linea
            producto.Descontar(cantidad); // Descontar stock del producto
            Lineas.Add(new LineaPedido(producto, cantidad)); // Agregar la linea al pedido
        }

        // Representacion en texto del pedido

        public override string ToString() // Metodo para representar el pedido en texto
            => $"Pedido #{Id} - {Cliente.Nombre} - {Fecha:g} - Total Neto: {Total.ToString("C")}";
    }
}