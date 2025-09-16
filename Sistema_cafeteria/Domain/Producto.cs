using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Cafeteria.Domain
{
    public class Producto

    {
        // Propiedades autoimplementadas
        public string Codigo { get; } // Propiedad autoimplementada con acceso publico para lectura
        public string Nombre { get; private set; } // Propiedad con acceso publico para lectura y privado para escritura
        public decimal PrecioUnitario { get; private set; } // Propiedad con acceso publico para lectura y privado para escritura
        public int Stock { get; private set; } // Propiedad con acceso publico para lectura y privado para escritura

        public int UmbralBajo { get; init; } = 3; // Propiedad con acceso publico para lectura y escritura solo en el inicializador o constructor

        // Evento: avisar cuando el stock es bajo o llegue a un umbral

        public event EventHandler<string>? StockBajo; // Evento para notificar cuando el stock es bajo

        // Construtor
        public Producto(string codigo, string nombre, decimal precio, int stockInicial, int umbralBajo = 3) // Constructor publico
        {
            if (string.IsNullOrWhiteSpace(codigo)) // Validacion de datos
            {
                throw new ArgumentException("El codigo no puede estar vacio"); // Lanzar excepcion si el dato es invalido
            }
            if (string.IsNullOrWhiteSpace(nombre)) // Validacion de datos
            {
                throw new ArgumentException("El nombre no puede estar vacio"); // Lanzar excepcion si el dato es invalido
            }
            if (precio < 0) // Validacion de datos
            {
                throw new ArgumentException("El precio no puede ser negativo"); // Lanzar excepcion si el dato es invalido
            }
            if (stockInicial < 0) // Validacion de datos
            {
                throw new ArgumentException("El stock inicial no puede ser negativo"); // Lanzar excepcion si el dato es invalido
            }
            if (umbralBajo < 0) // Validacion de datos
            {
                throw new ArgumentException("El umbral bajo no puede ser negativo"); // Lanzar excepcion si el dato es invalido
            }
            Codigo = codigo.Trim().ToUpper(); // Asignacion de valor a la propiedad con eliminacion de espacios en blanco y conversion a mayusculas
            Nombre = nombre.Trim(); // Asignacion de valor a la propiedad con eliminacion de espacios en blanco
            PrecioUnitario = precio; // Asignacion de valor a la propiedad
            Stock = stockInicial; // Asignacion de valor a la propiedad
            UmbralBajo = umbralBajo > 0 ? umbralBajo : 3; // Asignacion de valor a la propiedad con validacion

        }
        // Metodo de modificación de stock
        public void CambiarNombre(string nuevo) => Nombre = string.IsNullOrWhiteSpace(nuevo) ? Nombre : nuevo.Trim(); // Metodo para cambiar el nombre del producto con validacion y formateo

        public void CambiarPrecio(decimal nuevo)
        {
            if (nuevo < 0) return; // Validacion de datos
            PrecioUnitario = nuevo; // Asignacion de nuevo valor a la propiedad
        }

        // Metodo para reponer y descontar stock
        public void Reponer(int cantidad)
        {
            if (cantidad <= 0) return; // Validacion de datos
            Stock += cantidad; // Incremento del stock
        }

        public void Descontar(int cantidad)
        {
            if (cantidad <= 0) return; // Validacion de datos
            if (cantidad > Stock) throw new InvalidOperationException($"Stock insuficiente de {Nombre}."); // Lanzar excepcion si no hay stock suficiente

            Stock -= cantidad; // Decremento del stock
            if (Stock <= UmbralBajo) StockBajo?.Invoke(this, Codigo); // Notificar si el stock es bajo
            
        }
        // Metodo sobrescrito para mostrar informacion del producto
        public override string ToString()
            => $"{Codigo} - {Nombre} ({PrecioUnitario.ToString("C")}) - Stock: {Stock}"; // Sobrescribir metodo ToString para mostrar informacion del producto

    }
}