using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Clientes
    {
        public int IDCliente { get; set; }
        public string Nombre { get; set; }
        public String Apellido { get; set; }
        public String Documento { get; set; }

        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public bool Activo { get; set; }
        public DateTime fechaAlta { get; set; }

    }
}
