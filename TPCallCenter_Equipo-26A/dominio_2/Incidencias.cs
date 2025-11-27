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
        public int NumeroReclamo { get; set; }
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

        // cambiar el estado automáticamente
        public void CambiarEstado(string accion)
        {
            switch (accion)
            {
                case "Crear":
                    Estado = new Estados { IDEstado = 1, Descripcion = "Abierto" }; // Estado Abierto
                    break;

                case "Resolver":
                    Estado = new Estados { IDEstado = 6, Descripcion = "Resuelto" }; // Estado Resuelto
                    FechaResolucion = DateTime.Now;
                    break;

                case "Cerrar":
                    Estado = new Estados { IDEstado = 3, Descripcion = "Cerrado" }; // Estado Cerrado
                    break;

                case "Reasignar":
                    Estado = new Estados { IDEstado = 5, Descripcion = "Asignado" }; // Estado Asignado
                    break;

                case "Modificar":
                    Estado = new Estados { IDEstado = 2, Descripcion = "En análisis" }; // Estado En Análisis
                    break;

                case "Reabrir":
                    Estado = new Estados { IDEstado = 4, Descripcion = "Reabierto" }; // Estado Reabierto
                    break;

                default:
                    throw new ArgumentException("Acción no válida para cambiar el estado.");
            }
        }
    }
}
