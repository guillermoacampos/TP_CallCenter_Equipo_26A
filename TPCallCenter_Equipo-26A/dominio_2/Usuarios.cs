using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Usuarios
    {
        public int IDUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; } // Cambiado de Contrasena a Contraseña
        public Perfil Perfil { get; set; } 
        public bool Activo { get; set; }
        public DateTime FechaDeAlta { get; set; } // Cambiado de fechaAlta a FechaDeAlta
    }
}
