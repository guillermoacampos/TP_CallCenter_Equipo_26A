using System;
using System.Collections.Generic;
using dominio;

namespace negocio
{
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void CrearIncidencia(Incidencias nuevaIncidencia, int usuarioLogueadoID)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                nuevaIncidencia.AsignadoUsuario = new Usuarios { IDUsuario = usuarioLogueadoID };

                // Cambiar el estado a "Abierto" automáticamente
                nuevaIncidencia.CambiarEstado("Crear");

                datos.SetearConsulta("INSERT INTO Incidencias (IDCliente, IDCreadorUsuario, IDUsuarioAsignado, IDTipoIncidencia, IDPrioridad, IDEstado, Descripcion, FechaAlta) " +
                                     "VALUES (@IDCliente, @IDCreadorUsuario, @IDUsuarioAsignado, @IDTipoIncidencia, @IDPrioridad, @IDEstado, @Descripcion, @FechaAlta)");

                datos.SetearParametro("@IDCliente", nuevaIncidencia.Cliente.IDCliente);
                datos.SetearParametro("@IDCreadorUsuario", nuevaIncidencia.CreadorUsuario.IDUsuario);
                datos.SetearParametro("@IDUsuarioAsignado", nuevaIncidencia.AsignadoUsuario.IDUsuario);
                datos.SetearParametro("@IDTipoIncidencia", nuevaIncidencia.TipoIncidencia.IDTipoIncidencia);
                datos.SetearParametro("@IDPrioridad", nuevaIncidencia.Prioridad.IDPrioridad);
                datos.SetearParametro("@IDEstado", nuevaIncidencia.Estado.IDEstado);
                datos.SetearParametro("@Descripcion", nuevaIncidencia.Descripcion);
                datos.SetearParametro("@FechaAlta", nuevaIncidencia.FechaAlta);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void ReasignarIncidencia(int idIncidencia, Usuarios nuevoUsuarioAsignado, Usuarios usuarioActual)
        {
            if (usuarioActual.Perfil.IDPerfil != 3 && usuarioActual.Perfil.IDPerfil != 2) // Supervisor o Administrador
            {
                throw new UnauthorizedAccessException("Solo los supervisores o administradores pueden reasignar incidencias.");
            }

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("UPDATE Incidencias SET IDUsuarioAsignado = @IDUsuarioAsignado, IDEstado = @IDEstado WHERE IDIncidencia = @IDIncidencia");
                datos.SetearParametro("@IDUsuarioAsignado", nuevoUsuarioAsignado.IDUsuario);
                datos.SetearParametro("@IDEstado", 5); // Estado Asignado
                datos.SetearParametro("@IDIncidencia", idIncidencia);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void ResolverIncidencia(int idIncidencia, string comentarioResolucion)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("UPDATE Incidencias SET IDEstado = @IDEstado, ComentarioResolucion = @ComentarioResolucion, FechaResolucion = @FechaResolucion WHERE IDIncidencia = @IDIncidencia");
                datos.SetearParametro("@IDEstado", 6); // Estado Resuelto
                datos.SetearParametro("@ComentarioResolucion", comentarioResolucion);
                datos.SetearParametro("@FechaResolucion", DateTime.Now);
                datos.SetearParametro("@IDIncidencia", idIncidencia);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void CerrarIncidencia(int idIncidencia, string comentarioCierre)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("UPDATE Incidencias SET IDEstado = @IDEstado, ComentarioCierre = @ComentarioCierre WHERE IDIncidencia = @IDIncidencia");
                datos.SetearParametro("@IDEstado", 3); // Estado Cerrado
                datos.SetearParametro("@ComentarioCierre", comentarioCierre);
                datos.SetearParametro("@IDIncidencia", idIncidencia);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public List<Incidencias> ObtenerTodas()
        {
            List<Incidencias> lista = new List<Incidencias>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(@"
                    SELECT i.IDIncidencia, i.Descripcion, i.FechaAlta, 
                           c.IDCliente, c.Nombre AS ClienteNombre, 
                           t.IDTipoIncidencia, t.Nombre AS TipoNombre, 
                           p.IDPrioridad, p.Nombre AS PrioridadNombre, 
                           e.IDEstado, e.Descripcion AS EstadoDescripcion
                    FROM Incidencias i
                    INNER JOIN Clientes c ON i.IDCliente = c.IDCliente
                    INNER JOIN TiposDeIncidencia t ON i.IDTipoIncidencia = t.IDTipoIncidencia
                    INNER JOIN Prioridades p ON i.IDPrioridad = p.IDPrioridad
                    INNER JOIN Estados e ON i.IDEstado = e.IDEstado");

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Incidencias incidencia = new Incidencias
                    {
                        IDIncidencia = (int)datos.Lector["IDIncidencia"],
                        Descripcion = (string)datos.Lector["Descripcion"],
                        FechaAlta = (DateTime)datos.Lector["FechaAlta"],
                        Cliente = new Clientes
                        {
                            IDCliente = (int)datos.Lector["IDCliente"],
                            Nombre = (string)datos.Lector["ClienteNombre"]
                        },
                        TipoIncidencia = new TiposDeIncidencia
                        {
                            IDTipoIncidencia = (int)datos.Lector["IDTipoIncidencia"],
                            Nombre = (string)datos.Lector["TipoNombre"]
                        },
                        Prioridad = new Prioridades
                        {
                            IDPrioridad = (int)datos.Lector["IDPrioridad"],
                            Nombre = (string)datos.Lector["PrioridadNombre"]
                        },
                        Estado = new Estados
                        {
                            IDEstado = (int)datos.Lector["IDEstado"],
                            Descripcion = (string)datos.Lector["EstadoDescripcion"]
                        }
                    };
                    lista.Add(incidencia);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
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
                    Incidencias incidencia = new Incidencias
                    {
                        IDIncidencia = (int)datos.Lector["IDIncidencia"],
                        Descripcion = (string)datos.Lector["Descripcion"],
                        FechaAlta = (DateTime)datos.Lector["FechaAlta"],
                        Estado = new Estados { IDEstado = (int)datos.Lector["IDEstado"] },
                        AsignadoUsuario = new Usuarios()
                    };
                    lista.Add(incidencia);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}