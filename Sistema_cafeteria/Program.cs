

using Sistema_Cafeteria.Domain;
using Sistema_Cafeteria.Pricing;
using Sistema_Cafeteria.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sistema_Cafeteria
{
    internal class Program
    {
        private static List<Producto> _productos = new();
        private static RepositorioPedidosMemoria _repositorioPedidos = new();

        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CultureInfo.CurrentCulture = new CultureInfo("es-CO");
            CultureInfo.CurrentUICulture = new CultureInfo("es-CO");

            InicializarProductos();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Sistema de Cafetería ===");
                Console.WriteLine("1. Crear nuevo pedido");
                Console.WriteLine("2. Listar pedidos existentes");
                Console.WriteLine("3. Listar inventario");
                Console.WriteLine("4. Agregar nuevo producto al inventario");
                Console.WriteLine("5. Modificar producto existente");
                Console.WriteLine("6. Reponer stock de un producto");
                Console.WriteLine("7. Eliminar producto");
                Console.WriteLine("8. Salir");
                Console.Write("\nSeleccione una opción: ");

                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        CrearPedido();
                        break;
                    case "2":
                        ListarPedidos();
                        break;
                    case "3":
                        ListarInventario();
                        Console.WriteLine("\nPresione cualquier tecla para volver al menú principal..."); // 👈 LÍNEA AGREGADA
                        Console.ReadKey(); // 👈 LÍNEA AGREGADA
                        break;
                    case "4":
                        AgregarProducto();
                        break;
                    case "5":
                        ModificarProducto();
                        break;
                    case "6":
                        ReponerStock();
                        break;
                    case "7":
                        EliminarProducto();
                        break;
                    case "8":
                        return;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void InicializarProductos()
        {
            _productos.Add(new Producto("CAF001", "Café Americano", 5_500m, stockInicial: 10));
            _productos.Add(new Producto("PAN002", "Pan de Queso", 3_000m, stockInicial: 15));
            _productos.Add(new Producto("CAP003", "Capuccino", 8_000m, stockInicial: 8));

            foreach (var producto in _productos)
            {
                producto.StockBajo += ManejarAlertaStock;
            }
        }



        private static void CrearPedido()
        {
            Console.Clear();
            Console.WriteLine("--- CREAR UN NUEVO PEDIDO ---\n");

            Console.Write("Ingrese documento del cliente: ");
            var documento = Console.ReadLine();
            Console.Write("Ingrese nombre del cliente: ");
            var nombre = Console.ReadLine();
            var cliente = new Cliente(documento, nombre, "sin correo@na");

            var nuevoPedido = new Pedido(cliente);

            bool seguirAgregando = true;
            while (seguirAgregando)
            {
                ListarInventario();

                Console.Write("\nIngrese el código del producto a agregar (o '0' para terminar): ");
                var codigo = Console.ReadLine().ToUpper();

                if (codigo == "0")
                {
                    seguirAgregando = false;
                }
                else
                {
                    var productoSeleccionado = _productos.FirstOrDefault(p => p.Codigo == codigo);

                    if (productoSeleccionado != null)
                    {
                        Console.Write($"Ingrese la cantidad de '{productoSeleccionado.Nombre}': ");
                        if (int.TryParse(Console.ReadLine(), out int cantidad) && cantidad > 0)
                        {
                            try
                            {
                                nuevoPedido.AgregarLinea(productoSeleccionado, cantidad);
                                Console.WriteLine("✔️ Producto agregado. Presione cualquier tecla para agregar otro.");
                            }
                            catch (InvalidOperationException ex)
                            {
                                Console.WriteLine($"❌ Error: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("⚠️ Cantidad no válida.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("⚠️ Código de producto no válido.");
                    }
                }
                Console.ReadKey();
            }

            if (nuevoPedido.Lineas.Any())
            {
                // 👇 1. Aplicamos la estrategia de descuento
                // Esta línea detecta si la cantidad de productos es mayor a 6 y calcula el nuevo total
                var estrategiaDescuento = new DescuentoPorCantidadStrategy(cantidadMinima: 6, porcentajeDescuento: 0.10m);
                var totalConDescuento = estrategiaDescuento.CalcularTotal(nuevoPedido);

                // 2. Calculamos el valor del descuento
                var descuentoAplicado = nuevoPedido.Total - totalConDescuento;

                Console.WriteLine("\n--- RESUMEN DEL PEDIDO ---");
                Console.WriteLine($"Pedido #{nuevoPedido.Id} - {nuevoPedido.Cliente.Nombre} - {nuevoPedido.Fecha:g}");
                Console.WriteLine("--------------------------");

                foreach (var linea in nuevoPedido.Lineas)
                {
                    Console.WriteLine($"  - {linea}");
                }

                Console.WriteLine("--------------------------");

                // 👇 3. Imprimimos el subtotal SIN el descuento
                Console.WriteLine($"Subtotal: {nuevoPedido.Total.ToString("C")}");

                // 👇 4. Imprimimos el descuento SOLO si fue aplicado
                if (descuentoAplicado > 0)
                {
                    Console.WriteLine($"Descuento (10% por 6+ productos): -{descuentoAplicado.ToString("C")}");
                }

                // 👇 5. Imprimimos el total final, que ya tiene el descuento
                Console.WriteLine($"Total Neto: {totalConDescuento.ToString("C")}");
                Console.WriteLine("--------------------------\n");


                var mediosDePago = new List<IMedioPago>
        {
            new Efectivo(),
            new TarjetaCredito(nuevoPedido.Cliente.Nombre, nuevoPedido.Cliente.Documento.Substring(nuevoPedido.Cliente.Documento.Length - 4), limite: 1_000_000m),
            new Transferencia("Bancolombia", $"TRX-{Guid.NewGuid().ToString().Substring(0, 8)}")
        };

                // 👇 6. Pasamos el total ya descontado al método de pago
                var pagoExitoso = SeleccionarYProcesarPago(totalConDescuento, mediosDePago);

                if (pagoExitoso)
                {
                    _repositorioPedidos.Guardar(nuevoPedido);
                    Console.WriteLine($"\n✅ Pedido #{nuevoPedido.Id} creado y pagado exitosamente.");
                }
                else
                {
                    Console.WriteLine("\n❌ El pedido no pudo ser procesado. Vuelva a intentar.");
                }
            }
            else
            {
                Console.WriteLine("\nEl pedido está vacío. Volviendo al menú principal.");
            }

            Console.WriteLine("Presione cualquier tecla para volver al menú principal...");
            Console.ReadKey();
        }





        private static void ListarPedidos()
        {
            Console.Clear();
            Console.WriteLine("--- LISTADO DE PEDIDOS ---");

            if (!_repositorioPedidos.ObtenerTodos().Any())
            {
                Console.WriteLine("\nNo hay pedidos registrados.");
            }
            else
            {
                // 1. Itera sobre cada pedido guardado
                foreach (var pedido in _repositorioPedidos.ObtenerTodos())
                {
                    // 2. Aplica la estrategia de descuento a cada pedido
                    var estrategiaDescuento = new DescuentoPorCantidadStrategy(cantidadMinima: 6, porcentajeDescuento: 0.10m);
                    var totalConDescuento = estrategiaDescuento.CalcularTotal(pedido);
                    var descuentoAplicado = pedido.Total - totalConDescuento;

                    // 3. Imprime el resumen detallado de cada pedido
                    Console.WriteLine($"\nPedido #{pedido.Id} - {pedido.Cliente.Nombre} - {pedido.Fecha:g}");
                    Console.WriteLine($"Subtotal: {pedido.Total.ToString("C")}");

                    // Muestra el descuento si se aplicó
                    if (descuentoAplicado > 0)
                    {
                        Console.WriteLine($"Descuento ({estrategiaDescuento.GetType().Name}): -{descuentoAplicado.ToString("C")}");
                    }

                    // 4. Imprime el total neto que sí tiene el descuento
                    Console.WriteLine($"Total Neto: {totalConDescuento.ToString("C")}");

                    // 5. Imprime las líneas del pedido
                    foreach (var linea in pedido.Lineas)
                    {
                        Console.WriteLine($"  - {linea}");
                    }
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para volver al menú principal...");
            Console.ReadKey();
        }







        private static void ListarInventario()
        {
            Console.Clear();
            Console.WriteLine("\n--- INVENTARIO DISPONIBLE ---");
            if (!_productos.Any())
            {
                Console.WriteLine("No hay productos en el inventario.");
            }
            else
            {
                foreach (var producto in _productos)
                {
                    Console.WriteLine(producto);
                }
            }
            Console.WriteLine("-----------------------------");
        }

        private static void ManejarAlertaStock(object? sender, string codigoProducto)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n🚨 ALERTA DE STOCK: El producto '{codigoProducto}' está por debajo del umbral.");
            Console.ResetColor();
        }

        private static bool SeleccionarYProcesarPago(decimal total, List<IMedioPago> mediosDePago)
        {
            Console.WriteLine("--- PROCESANDO PAGO ---");
            Console.WriteLine("Seleccione un medio de pago:");

            for (int i = 0; i < mediosDePago.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {mediosDePago[i].Descripcion}");
            }

            Console.Write("\nSeleccione una opción: ");
            if (int.TryParse(Console.ReadLine(), out int seleccion) && seleccion > 0 && seleccion <= mediosDePago.Count)
            {
                var medioElegido = mediosDePago[seleccion - 1];

                if (medioElegido.Autorizar(total))
                {
                    try
                    {
                        medioElegido.Capturar(total);
                        Console.WriteLine($"\n✔️ Pago de {total.ToString("C")} autorizado y capturado con {medioElegido.Descripcion}.");
                        return true;
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine($"\n❌ Error en la captura: {ex.Message}");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine($"\n❌ No se pudo autorizar el pago con {medioElegido.Descripcion}.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("\n⚠️ Opción de pago no válida.");
                return false;
            }
        }

        private static void AgregarProducto()
        {
            Console.Clear();
            Console.WriteLine("--- AGREGAR NUEVO PRODUCTO ---");
            Console.Write("Ingrese código del producto: ");
            var codigo = Console.ReadLine().ToUpper();

            if (_productos.Any(p => p.Codigo == codigo))
            {
                Console.WriteLine("❌ Error: Ya existe un producto con este código. Presione cualquier tecla...");
                Console.ReadKey();
                return;
            }

            Console.Write("Ingrese nombre: ");
            var nombre = Console.ReadLine();
            Console.Write("Ingrese precio: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal precio) || precio <= 0)
            {
                Console.WriteLine("❌ Precio no válido. Presione cualquier tecla...");
                Console.ReadKey();
                return;
            }
            Console.Write("Ingrese stock inicial: ");
            if (!int.TryParse(Console.ReadLine(), out int stock) || stock < 0)
            {
                Console.WriteLine("❌ Stock no válido. Presione cualquier tecla...");
                Console.ReadKey();
                return;
            }

            var nuevoProducto = new Producto(codigo, nombre, precio, stock);
            nuevoProducto.StockBajo += ManejarAlertaStock;
            _productos.Add(nuevoProducto);

            Console.WriteLine("✅ Producto agregado con éxito. Presione cualquier tecla...");
            Console.ReadKey();
        }

        private static void ModificarProducto()
        {
            Console.Clear();
            Console.WriteLine("--- MODIFICAR PRODUCTO ---");
            ListarInventario();
            Console.Write("\nIngrese código del producto a modificar: ");
            var codigo = Console.ReadLine().ToUpper();

            var producto = _productos.FirstOrDefault(p => p.Codigo == codigo);

            if (producto == null)
            {
                Console.WriteLine("❌ Producto no encontrado. Presione cualquier tecla...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Producto seleccionado: {producto}");
            Console.Write("Ingrese nuevo nombre (dejar vacío para no cambiar): ");
            var nuevoNombre = Console.ReadLine();
            producto.CambiarNombre(nuevoNombre);

            Console.Write("Ingrese nuevo precio (dejar vacío para no cambiar): ");
            var nuevoPrecioStr = Console.ReadLine();
            if (decimal.TryParse(nuevoPrecioStr, out decimal nuevoPrecio) && nuevoPrecio > 0)
            {
                producto.CambiarPrecio(nuevoPrecio);
            }

            Console.WriteLine("✅ Producto modificado con éxito. Presione cualquier tecla...");
            Console.ReadKey();
        }

        private static void ReponerStock()
        {
            Console.Clear();
            Console.WriteLine("--- REPONER STOCK ---");
            ListarInventario();
            Console.Write("\nIngrese código del producto a reponer: ");
            var codigo = Console.ReadLine().ToUpper();

            var producto = _productos.FirstOrDefault(p => p.Codigo == codigo);

            if (producto == null)
            {
                Console.WriteLine("❌ Producto no encontrado. Presione cualquier tecla...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Producto seleccionado: {producto}");
            Console.Write("Ingrese cantidad a reponer: ");
            if (int.TryParse(Console.ReadLine(), out int cantidad) && cantidad > 0)
            {
                producto.Reponer(cantidad);
                Console.WriteLine("✅ Stock repuesto con éxito. Presione cualquier tecla...");
            }
            else
            {
                Console.WriteLine("❌ Cantidad no válida. Presione cualquier tecla...");
            }
            Console.ReadKey();
        }

        private static void EliminarProducto()
        {
            Console.Clear();
            Console.WriteLine("--- ELIMINAR PRODUCTO ---");
            ListarInventario();
            Console.Write("\nIngrese código del producto a eliminar: ");
            var codigo = Console.ReadLine().ToUpper();

            var producto = _productos.FirstOrDefault(p => p.Codigo == codigo);

            if (producto == null)
            {
                Console.WriteLine("❌ Producto no encontrado. Presione cualquier tecla...");
                Console.ReadKey();
                return;
            }

            _productos.Remove(producto);
            Console.WriteLine($"✅ Producto '{producto.Nombre}' eliminado con éxito. Presione cualquier tecla...");
            Console.ReadKey();
        }
    }
}