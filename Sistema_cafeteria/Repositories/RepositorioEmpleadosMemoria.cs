using Sistema_Cafeteria.Repositories;
using System.Collections.Generic;
using System.Linq;
using Sistema_Cafeteria.Domain;

public class RepositorioEmpleadosMemoria : IRepositorioEmpleados
{
    private static readonly List<Empleado> _empleados = new()
    {
        
        new Empleado("1020304050", "Laura Restrepo", "Vendedora", "clave123"),
        new Empleado("1020304051", "Juan Perez", "Gerente", "juan123"),
        new Empleado("1020304052", "Maria Lopez", "Cajero", "maria123")
    };

    public Empleado ObtenerPorDocumento(string documento)
    {
        return _empleados.FirstOrDefault(e => e.Documento == documento);
    }
}