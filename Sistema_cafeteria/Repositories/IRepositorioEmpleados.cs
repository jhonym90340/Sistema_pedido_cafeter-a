using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistema_Cafeteria.Domain;

namespace Sistema_Cafeteria.Repositories
{
    internal interface IRepositorioEmpleados
    {
        Empleado ObtenerPorDocumento(string documento);

    }
}
