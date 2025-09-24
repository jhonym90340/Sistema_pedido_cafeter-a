#  Sistema de Gestión de Cafetería

Este es un proyecto de aplicación de consola desarrollado en C# para gestionar las operaciones de una cafetería. El sistema permite administrar el inventario de productos, procesar pedidos de clientes, aplicar descuentos y manejar la autenticación de empleados.

##  Funcionalidades Principales

* **Gestión de Inventario**: Permite agregar, modificar, reponer y eliminar productos del inventario.
* **Gestión de Pedidos**: Crea nuevos pedidos para los clientes, agregando múltiples productos y cantidades.
* **Sistema de Autenticación**: Un sistema de login para empleados basado en su documento y una clave.
* **Lógica de Negocio**: Implementa validaciones para asegurar la integridad de los datos (ej. stock, códigos únicos, valores numéricos).
* **Aplicación de Descuentos**: Calcula automáticamente descuentos basados en la cantidad total de productos en un pedido.

---

##  Diseño Orientado a Objetos (POO)

El proyecto está diseñado siguiendo los cuatro pilares fundamentales de la Programación Orientada a Objetos para garantizar un código modular, mantenible y escalable.

### **Abstracción**

Se han definido contratos esenciales a través de interfaces para ocultar la complejidad de la implementación.

* **`IRepositorioEmpleados`**: Define el contrato para el acceso a los datos de los empleados, permitiendo cambiar la fuente de datos (ej. de memoria a una base de datos) sin modificar la lógica del negocio.
* **`IMedioPago`**: Define la interfaz para los distintos métodos de pago (`Efectivo`, `TarjetaCredito`, etc.), asegurando que todos se comporten de manera similar con los métodos `Autorizar()` y `Capturar()`.

### **Encapsulamiento**

Las clases protegen su estado interno y exponen solo los métodos necesarios para interactuar con ellas, garantizando la consistencia de los datos.

* **Propiedades privadas (`private set`)**: Clases como `Producto` y `Empleado` restringen la modificación directa de propiedades cruciales como el código o el stock, forzando a usar métodos controlados (`Reponer()`, `Descontar()`).
* **Validaciones internas**: La clase `Pedido` valida la disponibilidad de stock antes de agregar una línea de pedido.

### **Herencia**

Se establecen jerarquías de clases para reutilizar código y modelar relaciones del mundo real.

* **`Persona`**: `Empleado` y `Cliente` heredan de la clase base `Persona`, compartiendo propiedades comunes como `Documento` y `Nombre`.

### **Polimorfismo**

Permite a los objetos de diferentes clases responder al mismo mensaje de maneras distintas, a través de interfaces y sobreescrituras.

* **Estrategia de precios**: Se utiliza un enfoque de **Estrategia** con la interfaz `IPricingStrategy` para aplicar diferentes lógicas de precio de forma flexible (ej. un descuento por cantidad). Esto permite extender la funcionalidad de precios sin modificar el código existente.
* **Medios de pago**: El programa procesa los pagos llamando a los métodos definidos en la interfaz `IMedioPago`, sin importar si el objeto es de tipo `Efectivo` o `TarjetaCredito`.

---

##  Validaciones de Datos

El proyecto incluye validaciones clave para prevenir errores y mantener la integridad de los datos.

* **Códigos de Producto Repetidos**: Se utiliza una consulta LINQ (`_productos.Any(...)`) para asegurar que no se pueda agregar un producto con un código que ya existe en el inventario.
* **Entrada de Datos Numérica**: Los métodos `decimal.TryParse()` e `int.TryParse()` validan que las entradas de usuario para precio y stock sean números válidos y mayores que cero, evitando que el programa falle.
* **Stock Insuficiente**: Antes de procesar un pedido, el sistema valida que haya suficiente stock disponible para cada producto solicitado. Si no, se lanza una excepción controlada.
* **Existencia de Objetos**: Se verifica si un producto o un empleado existe antes de intentar modificarlo, reponer su stock o autenticarlo.

---

##  Cómo Ejecutar el Proyecto

1.  **Clonar el repositorio**:
    ```bash
    git clone [https://github.com/tu_usuario/nombre_del_repositorio.git](https://github.com/tu_usuario/nombre_del_repositorio.git)
    ```
2.  **Abrir en Visual Studio**:
    * Abre Visual Studio 2022.
    * Selecciona `Abrir un proyecto o una solución` y navega hasta la carpeta clonada.
    * Selecciona el archivo `.sln` para abrir el proyecto.
3.  **Ejecutar**:
    * Asegúrate de que `Sistema_Cafeteria` sea el proyecto de inicio.
    * Presiona `F5` o el botón de `Iniciar` para compilar y ejecutar la aplicación.

El sistema se iniciará en la terminal y te pedirá las credenciales para iniciar sesión como empleado.
