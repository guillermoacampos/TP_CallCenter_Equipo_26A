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
        public Clientes Cliente { get; set; } 

        public Usuarios CreadorUsuario { get; set; } 
        public Usuarios AsignadoUsuario { get; set; }
        public TiposDeIncidencia TipoIncidencia { get; set; } 
        public Prioridades Prioridad { get; set; } 
        public Estados Estado { get; set; } 
        public string Descripcion { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaResolucion { get; set; }
        public string ComentarioResolucion { get; set; }
        public string ComentarioCierre { get; set; }
    }
}
