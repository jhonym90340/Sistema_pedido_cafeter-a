using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Cafeteria.Domain
{
    public class LineaPedido
    {
        // Propiedad Producto
        public Producto Producto { get; } // Propiedad autoimplementada con acceso publico para lectura
        // Propiedad Cantidad
        public int Cantidad { get;  } // Propiedad con acceso publico para lectura 
        //Propiedad Calculada Subtotal
        public decimal Subtotal => Producto.PrecioUnitario * Cantidad; // Propiedad calculada para obtener el subtotal

        // Constructor

        public LineaPedido(Producto producto, int cantidad) // Constructor publico
        {
            Producto = producto ?? throw new ArgumentNullException(nameof(producto)); // Asignacion de valor a la propiedad con validacion
            if (cantidad <= 0)   throw new ArgumentException("Cantidad Inválida"); // Lanzar excepcion si el dato es invalido
            Cantidad = cantidad; // Asignacion de valor a la propiedad
        }
        //Se Sobrescribe ToString para mejorar la impresión en consola
        public override string ToString()
        {
            return $"{Producto.Nombre} x{Cantidad} = {Subtotal.ToString("C")}";
        }
    }
}