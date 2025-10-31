using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Incidencias
    {
        public int IDIncidencia { get; set; }
        public int NumReclamo { get; set; }
        public int IDCliente { get; set; }

        public int IDCreadorUsuario { get; set; }
        public int IDAsignadoUsuario { get; set; }
        public int IDTipoIncidencia { get; set; }
        public int IDPrioridad { get; set; }
        public int IDEstado { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaResolucion { get; set; }
        public string ComentarioResolucion { get; set; }
        public string ComentarioCierre { get; set; }
    }
}
