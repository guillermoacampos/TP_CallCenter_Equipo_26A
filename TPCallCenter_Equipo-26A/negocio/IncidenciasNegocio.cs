using System;
using System.Collections.Generic;
using dominio;

namespace negocio
{
    // Reemplaza este archivo completo por este contenido.
    // Atención: usa la clase AccesoDatos existente en el proyecto.
    public class IncidenciasNegocio
    {
        public List<int> ObtenerCantidadPorTipo()
        {
            List<int> cantidades = new List<int>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT IDTipoIncidencia, COUNT(*) AS Cantidad FROM Incidencias GROUP BY IDTipoIncidencia ORDER BY IDTipoIncidencia");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    cantidades.Add((int)datos.Lector["Cantidad"]);
                }

                return cantidades;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public int CrearIncidencia(dominio.Incidencias nuevaIncidencia, int usuarioLogueadoID)
        {
            if (nuevaIncidencia == null) throw new ArgumentNullException(nameof(nuevaIncidencia));

            int nextId = 1;
            AccesoDatos calc = new AccesoDatos();
            try
            {
                calc.SetearConsulta("SELECT ISNULL(MAX(IDIncidencia), 0) + 1 AS NextId FROM Incidencias");
                calc.EjecutarLectura();
                if (calc.Lector.Read())
                {
                    object o = calc.Lector["NextId"];
                    nextId = (o == null || o == DBNull.Value) ? 1 : Convert.ToInt32(o);
                }
            }
            finally
            {
                calc.CerrarConexion();
            }

            int idEstadoInicial = ObtenerIdEstadoPorNombre_Public("Abierto");

            AccesoDatos datos = new AccesoDatos();
            try
            {
                string sql = @"
INSERT INTO Incidencias (
    IDIncidencia, IDCliente, IDCreadorUsuario, IDUsuarioAsignado, IDTipoIncidencia, IDPrioridad, IDEstado, Descripcion, FechaAlta, FechaResolucion, ComentarioResolucion, ComentarioCierre
)
VALUES (
    @IDIncidencia, @IDCliente, @IDCreadorUsuario, @IDUsuarioAsignado, @IDTipoIncidencia, @IDPrioridad, @IDEstado, @Descripcion, @FechaAlta, @FechaResolucion, @ComentarioResolucion, @ComentarioCierre
);
SELECT CAST(ISNULL((SELECT MAX(NumeroReclamo) FROM Incidencias), 0) AS INT) AS NumeroReclamo;";

                datos.SetearConsulta(sql);

                datos.SetearParametro("@IDIncidencia", nextId);
                datos.SetearParametro("@IDCliente", nuevaIncidencia.Cliente?.IDCliente ?? (object)DBNull.Value);
                datos.SetearParametro("@IDCreadorUsuario", usuarioLogueadoID);
                int asignadoId = nuevaIncidencia.AsignadoUsuario?.IDUsuario ?? usuarioLogueadoID;
                datos.SetearParametro("@IDUsuarioAsignado", asignadoId);
                datos.SetearParametro("@IDTipoIncidencia", nuevaIncidencia.TipoIncidencia?.IDTipoIncidencia ?? (object)DBNull.Value);
                datos.SetearParametro("@IDPrioridad", nuevaIncidencia.Prioridad?.IDPrioridad ?? (object)DBNull.Value);
                datos.SetearParametro("@IDEstado", idEstadoInicial);
                datos.SetearParametro("@Descripcion", (object)nuevaIncidencia.Descripcion ?? string.Empty);
                datos.SetearParametro("@FechaAlta", nuevaIncidencia.FechaAlta);
                datos.SetearParametro("@FechaResolucion", DBNull.Value);
                datos.SetearParametro("@ComentarioResolucion", DBNull.Value);
                datos.SetearParametro("@ComentarioCierre", DBNull.Value);

                datos.EjecutarLectura();

                int numeroReclamo = 0;
                if (datos.Lector != null && datos.Lector.Read())
                {
                    object val = datos.Lector["NumeroReclamo"];
                    numeroReclamo = (val == null || val == DBNull.Value) ? 0 : Convert.ToInt32(val);
                }

                return numeroReclamo;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void ReasignarIncidencia(int idIncidencia, Usuarios nuevoUsuarioAsignado, Usuarios usuarioActual)
        {
            if (usuarioActual == null) throw new UnauthorizedAccessException("No user context.");
            int perfil = usuarioActual.Perfil?.IDPerfil ?? -1;
            if (perfil != 3 && perfil != 2)
                throw new UnauthorizedAccessException("Solo los supervisores o administradores pueden reasignar incidencias.");

            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("UPDATE Incidencias SET IDUsuarioAsignado = @IDUsuarioAsignado, IDEstado = @IDEstado WHERE IDIncidencia = @IDIncidencia");
                datos.SetearParametro("@IDUsuarioAsignado", nuevoUsuarioAsignado.IDUsuario);
                datos.SetearParametro("@IDEstado", 5);
                datos.SetearParametro("@IDIncidencia", idIncidencia);
                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void ResolverIncidencia(int idIncidencia, string comentarioResolucion, Usuarios usuarioActual)
        {
            if (usuarioActual == null) throw new UnauthorizedAccessException("No user context.");
            int perfil = usuarioActual.Perfil?.IDPerfil ?? -1;
            if (perfil != 3 && perfil != 2)
                throw new UnauthorizedAccessException("Solo supervisores o administradores pueden resolver.");

            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("UPDATE Incidencias SET IDEstado = @IDEstado, ComentarioResolucion = @ComentarioResolucion, FechaResolucion = @FechaResolucion WHERE IDIncidencia = @IDIncidencia");
                datos.SetearParametro("@IDEstado", 6);
                datos.SetearParametro("@ComentarioResolucion", comentarioResolucion ?? string.Empty);
                datos.SetearParametro("@FechaResolucion", DateTime.Now);
                datos.SetearParametro("@IDIncidencia", idIncidencia);
                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void CerrarIncidencia(int idIncidencia, string comentarioCierre, Usuarios usuarioActual)
        {
            if (usuarioActual == null) throw new UnauthorizedAccessException("No user context.");
            int perfil = usuarioActual.Perfil?.IDPerfil ?? -1;
            if (perfil != 3 && perfil != 2)
                throw new UnauthorizedAccessException("Solo supervisores o administradores pueden cerrar.");

            if (string.IsNullOrWhiteSpace(comentarioCierre))
                throw new ArgumentException("El cierre requiere un comentario final.");

            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("UPDATE Incidencias SET IDEstado = @IDEstado, ComentarioCierre = @ComentarioCierre WHERE IDIncidencia = @IDIncidencia");
                datos.SetearParametro("@IDEstado", 3);
                datos.SetearParametro("@ComentarioCierre", comentarioCierre);
                datos.SetearParametro("@IDIncidencia", idIncidencia);
                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // ObtenerTodas ahora incluye el ID/Nombre del creador y del asignado (necesario para filtrar por usuario)
        public List<Incidencias> ObtenerTodas()
        {
            List<Incidencias> lista = new List<Incidencias>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(@"
                    SELECT i.IDIncidencia, i.NumeroReclamo, i.Descripcion, i.FechaAlta,
                           i.IDCliente, c.Nombre AS ClienteNombre,
                           i.IDTipoIncidencia, t.Nombre AS TipoNombre,
                           i.IDPrioridad, p.Nombre AS PrioridadNombre,
                           i.IDEstado, e.Descripcion AS EstadoDescripcion,
                           i.IDCreadorUsuario, cu.Nombre AS CreadorNombre,
                           i.IDUsuarioAsignado, au.Nombre AS AsignadoNombre
                    FROM Incidencias i
                    LEFT JOIN Clientes c ON i.IDCliente = c.IDCliente
                    LEFT JOIN TiposDeIncidencia t ON i.IDTipoIncidencia = t.IDTipoIncidencia
                    LEFT JOIN Prioridades p ON i.IDPrioridad = p.IDPrioridad
                    LEFT JOIN Estados e ON i.IDEstado = e.IDEstado
                    LEFT JOIN Usuarios cu ON i.IDCreadorUsuario = cu.IDUsuario
                    LEFT JOIN Usuarios au ON i.IDUsuarioAsignado = au.IDUsuario
                    ORDER BY i.FechaAlta DESC");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Incidencias inc = new Incidencias
                    {
                        IDIncidencia = datos.Lector["IDIncidencia"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDIncidencia"]),
                        NumeroReclamo = datos.Lector["NumeroReclamo"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["NumeroReclamo"]),
                        Descripcion = datos.Lector["Descripcion"] == DBNull.Value ? null : (string)datos.Lector["Descripcion"],
                        FechaAlta = datos.Lector["FechaAlta"] == DBNull.Value ? DateTime.MinValue : (DateTime)datos.Lector["FechaAlta"],
                        Cliente = new Clientes
                        {
                            IDCliente = datos.Lector["IDCliente"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDCliente"]),
                            Nombre = datos.Lector["ClienteNombre"] == DBNull.Value ? null : (string)datos.Lector["ClienteNombre"]
                        },
                        TipoIncidencia = new TiposDeIncidencia
                        {
                            IDTipoIncidencia = datos.Lector["IDTipoIncidencia"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDTipoIncidencia"]),
                            Nombre = datos.Lector["TipoNombre"] == DBNull.Value ? null : (string)datos.Lector["TipoNombre"]
                        },
                        Prioridad = new Prioridades
                        {
                            IDPrioridad = datos.Lector["IDPrioridad"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDPrioridad"]),
                            Nombre = datos.Lector["PrioridadNombre"] == DBNull.Value ? null : (string)datos.Lector["PrioridadNombre"]
                        },
                        Estado = new Estados
                        {
                            IDEstado = datos.Lector["IDEstado"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDEstado"]),
                            Descripcion = datos.Lector["EstadoDescripcion"] == DBNull.Value ? null : (string)datos.Lector["EstadoDescripcion"]
                        },
                        CreadorUsuario = new Usuarios
                        {
                            IDUsuario = datos.Lector["IDCreadorUsuario"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDCreadorUsuario"]),
                            Nombre = datos.Lector["CreadorNombre"] == DBNull.Value ? null : (string)datos.Lector["CreadorNombre"]
                        },
                        AsignadoUsuario = new Usuarios
                        {
                            IDUsuario = datos.Lector["IDUsuarioAsignado"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDUsuarioAsignado"]),
                            Nombre = datos.Lector["AsignadoNombre"] == DBNull.Value ? null : (string)datos.Lector["AsignadoNombre"]
                        }
                    };

                    lista.Add(inc);
                }

                return lista;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public List<Incidencias> ObtenerPorEstado(int idEstado)
        {
            List<Incidencias> lista = new List<Incidencias>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT * FROM Incidencias WHERE IDEstado = @IDEstado");
                datos.SetearParametro("@IDEstado", idEstado);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Incidencias inc = new Incidencias
                    {
                        IDIncidencia = datos.Lector["IDIncidencia"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDIncidencia"]),
                        Descripcion = datos.Lector["Descripcion"] == DBNull.Value ? null : (string)datos.Lector["Descripcion"],
                        FechaAlta = datos.Lector["FechaAlta"] == DBNull.Value ? DateTime.MinValue : (DateTime)datos.Lector["FechaAlta"],
                        Estado = new Estados { IDEstado = datos.Lector["IDEstado"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDEstado"]) },
                        AsignadoUsuario = new Usuarios()
                    };
                    lista.Add(inc);
                }

                return lista;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
        public void ReasignarIncidencia(int idIncidencia, int nuevoUsuarioId, dominio.Usuarios supervisor)
        {
            if (supervisor?.Perfil?.IDPerfil != 3) // SOLO supervisor
                throw new UnauthorizedAccessException("Solo el supervisor puede reasignar incidencias.");

            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("UPDATE Incidencias SET IDUsuarioAsignado = @nuevo WHERE IDIncidencia = @id");
                datos.SetearParametro("@nuevo", nuevoUsuarioId);
                datos.SetearParametro("@id", idIncidencia);
                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
        public Incidencias ObtenerIncidenciaPorId(int idIncidencia)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
                    SELECT i.IDIncidencia, i.NumeroReclamo, i.Descripcion, i.FechaAlta,
                           i.IDCliente, c.Nombre AS ClienteNombre,
                           i.IDTipoIncidencia, t.Nombre AS TipoNombre,
                           i.IDPrioridad, p.Nombre AS PrioridadNombre,
                           i.IDEstado, e.Descripcion AS EstadoDescripcion,
                           i.IDCreadorUsuario, cu.Nombre AS CreadorNombre,
                           i.IDUsuarioAsignado, au.Nombre AS AsignadoNombre
                    FROM Incidencias i
                    LEFT JOIN Clientes c ON i.IDCliente = c.IDCliente
                    LEFT JOIN TiposDeIncidencia t ON i.IDTipoIncidencia = t.IDTipoIncidencia
                    LEFT JOIN Prioridades p ON i.IDPrioridad = p.IDPrioridad
                    LEFT JOIN Estados e ON i.IDEstado = e.IDEstado
                    LEFT JOIN Usuarios cu ON i.IDCreadorUsuario = cu.IDUsuario
                    LEFT JOIN Usuarios au ON i.IDUsuarioAsignado = au.IDUsuario
                    WHERE i.IDIncidencia = @id");
                datos.SetearParametro("@id", idIncidencia);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    Incidencias inc = new Incidencias
                    {
                        IDIncidencia = datos.Lector["IDIncidencia"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDIncidencia"]),
                        NumeroReclamo = datos.Lector["NumeroReclamo"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["NumeroReclamo"]),
                        Descripcion = datos.Lector["Descripcion"] == DBNull.Value ? null : (string)datos.Lector["Descripcion"],
                        FechaAlta = datos.Lector["FechaAlta"] == DBNull.Value ? DateTime.MinValue : (DateTime)datos.Lector["FechaAlta"],
                        Cliente = new Clientes
                        {
                            IDCliente = datos.Lector["IDCliente"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDCliente"]),
                            Nombre = datos.Lector["ClienteNombre"] == DBNull.Value ? null : (string)datos.Lector["ClienteNombre"]
                        },
                        TipoIncidencia = new TiposDeIncidencia
                        {
                            IDTipoIncidencia = datos.Lector["IDTipoIncidencia"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDTipoIncidencia"]),
                            Nombre = datos.Lector["TipoNombre"] == DBNull.Value ? null : (string)datos.Lector["TipoNombre"]
                        },
                        Prioridad = new Prioridades
                        {
                            IDPrioridad = datos.Lector["IDPrioridad"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDPrioridad"]),
                            Nombre = datos.Lector["PrioridadNombre"] == DBNull.Value ? null : (string)datos.Lector["PrioridadNombre"]
                        },
                        Estado = new Estados
                        {
                            IDEstado = datos.Lector["IDEstado"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDEstado"]),
                            Descripcion = datos.Lector["EstadoDescripcion"] == DBNull.Value ? null : (string)datos.Lector["EstadoDescripcion"]
                        },
                        CreadorUsuario = new Usuarios
                        {
                            IDUsuario = datos.Lector["IDCreadorUsuario"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDCreadorUsuario"]),
                            Nombre = datos.Lector["CreadorNombre"] == DBNull.Value ? null : (string)datos.Lector["CreadorNombre"]
                        },
                        AsignadoUsuario = new Usuarios
                        {
                            IDUsuario = datos.Lector["IDUsuarioAsignado"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDUsuarioAsignado"]),
                            Nombre = datos.Lector["AsignadoNombre"] == DBNull.Value ? null : (string)datos.Lector["AsignadoNombre"]
                        }
                    };

                    return inc;
                }

                return null;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public int ObtenerIdEstadoPorNombre_Public(string nombreEstado)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT IDEstado FROM Estados WHERE Descripcion = @nombre");
                datos.SetearParametro("@nombre", nombreEstado);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    object val = datos.Lector["IDEstado"];
                    if (val != null && int.TryParse(val.ToString(), out int id)) return id;
                }

                throw new Exception($"Estado '{nombreEstado}' no encontrado en la tabla Estados.");
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}