using System;
using System.Collections.Generic;
using dominio;

namespace negocio
{
    public class IncidenciasNegocio
    {
        // IDs de estado (ajusta si difieren en tu tabla Estados)
        private const int ESTADO_ABIERTO = 1;
        private const int ESTADO_EN_ANALISIS = 2;
        private const int ESTADO_CERRADO = 3;
        private const int ESTADO_REABIERTO = 4;
        private const int ESTADO_ASIGNADO = 5;
        private const int ESTADO_RESUELTO = 6;

        // CREAR INCIDENCIA: devuelve NumeroReclamo (identity) y setea inc.IDIncidencia
        public int CrearIncidencia(Incidencias inc, int idCreadorUsuario)
        {
            if (inc == null) throw new ArgumentNullException(nameof(inc));
            if (inc.Cliente == null || inc.Cliente.IDCliente <= 0) throw new ArgumentException("Falta el cliente o su ID.", nameof(inc.Cliente));
            if (inc.TipoIncidencia == null || inc.TipoIncidencia.IDTipoIncidencia <= 0) throw new ArgumentException("Falta el tipo de incidencia o su ID.", nameof(inc.TipoIncidencia));
            if (inc.Prioridad == null || inc.Prioridad.IDPrioridad <= 0) throw new ArgumentException("Falta la prioridad o su ID.", nameof(inc.Prioridad));

            int idEstado = inc.Estado?.IDEstado > 0 ? inc.Estado.IDEstado : ESTADO_ABIERTO;
            int idAsignado = inc.AsignadoUsuario?.IDUsuario > 0 ? inc.AsignadoUsuario.IDUsuario : idCreadorUsuario;

            int nuevoIdIncidencia = 0;
            int numeroReclamo = 0;

            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT ISNULL(MAX(IDIncidencia), 0) + 1 FROM Incidencias");
                datos.EjecutarLectura();
                if (datos.Lector.Read())
                    nuevoIdIncidencia = Convert.ToInt32(datos.Lector[0]);
                datos.CerrarConexion();

                datos = new AccesoDatos();
                datos.SetearConsulta(@"
                    INSERT INTO Incidencias
                        (IDIncidencia, IDCliente, IDCreadorUsuario, IDUsuarioAsignado,
                         IDTipoIncidencia, IDPrioridad, IDEstado, Descripcion, FechaAlta)
                    VALUES
                        (@idInc, @idCli, @idCreador, @idAsignado,
                         @idTipo, @idPrioridad, @idEstado, @desc, @fechaAlta);

                    SELECT CAST(SCOPE_IDENTITY() AS INT) AS NumeroReclamo;");

                datos.SetearParametro("@idInc", nuevoIdIncidencia);
                datos.SetearParametro("@idCli", inc.Cliente.IDCliente);
                datos.SetearParametro("@idCreador", idCreadorUsuario);
                datos.SetearParametro("@idAsignado", idAsignado);
                datos.SetearParametro("@idTipo", inc.TipoIncidencia.IDTipoIncidencia);
                datos.SetearParametro("@idPrioridad", inc.Prioridad.IDPrioridad);
                datos.SetearParametro("@idEstado", idEstado);
                datos.SetearParametro("@desc", (inc.Descripcion ?? "").Trim());
                datos.SetearParametro("@fechaAlta", DateTime.Now.Date);

                datos.EjecutarLectura();
                if (datos.Lector.Read())
                    numeroReclamo = Convert.ToInt32(datos.Lector["NumeroReclamo"]);

                inc.IDIncidencia = nuevoIdIncidencia;
                return numeroReclamo;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // LISTAR TODAS
        public List<Incidencias> ObtenerTodas()
        {
            var lista = new List<Incidencias>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
                    SELECT I.IDIncidencia,
                           I.NumeroReclamo,
                           I.IDCliente,
                           I.IDCreadorUsuario,
                           I.IDUsuarioAsignado,
                           I.IDTipoIncidencia,
                           I.IDPrioridad,
                           I.IDEstado,
                           I.Descripcion,
                           I.FechaAlta,
                           I.FechaResolucion,
                           I.ComentarioResolucion,
                           I.ComentarioCierre,
                           C.Nombre   AS ClienteNombre,
                           T.Nombre   AS TipoNombre,
                           P.Nombre   AS PrioridadNombre,
                           E.Descripcion AS EstadoDesc,
                           UC.IDUsuario AS CreadorID,
                           UC.Nombre  AS CreadorNombre,
                           UA.IDUsuario AS AsignadoID,
                           UA.Nombre  AS AsignadoNombre
                    FROM Incidencias I
                    JOIN Clientes C          ON C.IDCliente = I.IDCliente
                    JOIN TiposDeIncidencia T ON T.IDTipoIncidencia = I.IDTipoIncidencia
                    JOIN Prioridades P       ON P.IDPrioridad = I.IDPrioridad
                    JOIN Estados E           ON E.IDEstado = I.IDEstado
                    JOIN Usuarios UC         ON UC.IDUsuario = I.IDCreadorUsuario
                    JOIN Usuarios UA         ON UA.IDUsuario = I.IDUsuarioAsignado
                    ORDER BY I.NumeroReclamo DESC");

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    var inc = MapearIncidencia(datos);
                    lista.Add(inc);
                }
                return lista;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // DETALLE POR ID
        public Incidencias ObtenerIncidenciaPorId(int idIncidencia)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
                    SELECT I.IDIncidencia,
                           I.NumeroReclamo,
                           I.IDCliente,
                           I.IDCreadorUsuario,
                           I.IDUsuarioAsignado,
                           I.IDTipoIncidencia,
                           I.IDPrioridad,
                           I.IDEstado,
                           I.Descripcion,
                           I.FechaAlta,
                           I.FechaResolucion,
                           I.ComentarioResolucion,
                           I.ComentarioCierre,
                           C.Nombre   AS ClienteNombre,
                           T.Nombre   AS TipoNombre,
                           P.Nombre   AS PrioridadNombre,
                           E.Descripcion AS EstadoDesc,
                           UC.IDUsuario AS CreadorID,
                           UC.Nombre  AS CreadorNombre,
                           UA.IDUsuario AS AsignadoID,
                           UA.Nombre  AS AsignadoNombre
                    FROM Incidencias I
                    JOIN Clientes C          ON C.IDCliente = I.IDCliente
                    JOIN TiposDeIncidencia T ON T.IDTipoIncidencia = I.IDTipoIncidencia
                    JOIN Prioridades P       ON P.IDPrioridad = I.IDPrioridad
                    JOIN Estados E           ON E.IDEstado = I.IDEstado
                    JOIN Usuarios UC         ON UC.IDUsuario = I.IDCreadorUsuario
                    JOIN Usuarios UA         ON UA.IDUsuario = I.IDUsuarioAsignado
                    WHERE I.IDIncidencia = @id");
                datos.SetearParametro("@id", idIncidencia);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                    return MapearIncidencia(datos);

                return null;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // MODIFICAR DESCRIPCIÓN Y PASAR A EN ANÁLISIS
        public void ModificarDescripcionYEstado(int idIncidencia, string nuevaDescripcion, int? estadoAnalisisOverride = null)
        {
            if (string.IsNullOrWhiteSpace(nuevaDescripcion))
                throw new ArgumentException("La descripción no puede estar vacía.", nameof(nuevaDescripcion));

            int estadoFinal = estadoAnalisisOverride ?? ESTADO_EN_ANALISIS;

            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("UPDATE Incidencias SET Descripcion = @desc, IDEstado = @estado WHERE IDIncidencia = @id");
                datos.SetearParametro("@desc", nuevaDescripcion.Trim());
                datos.SetearParametro("@estado", estadoFinal);
                datos.SetearParametro("@id", idIncidencia);
                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // RESOLVER (requiere comentario, guarda fecha)
        public void ResolverIncidenciaConComentario(int idIncidencia, string comentarioResolucion, Usuarios usuarioAccion, int? estadoResueltoOverride = null)
        {
            if (string.IsNullOrWhiteSpace(comentarioResolucion))
                throw new ArgumentException("El comentario de resolución es obligatorio.", nameof(comentarioResolucion));

            int estadoFinal = estadoResueltoOverride ?? ESTADO_RESUELTO;

            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"UPDATE Incidencias 
                                       SET ComentarioResolucion = @coment,
                                           FechaResolucion = @fecha,
                                           IDEstado = @estado
                                       WHERE IDIncidencia = @id");
                datos.SetearParametro("@coment", comentarioResolucion.Trim());
                datos.SetearParametro("@fecha", DateTime.Now.Date);
                datos.SetearParametro("@estado", estadoFinal);
                datos.SetearParametro("@id", idIncidencia);
                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // CERRAR (requiere comentario)
        public void CerrarIncidenciaConComentario(int idIncidencia, string comentarioCierre, Usuarios usuarioAccion, int? estadoCerradoOverride = null)
        {
            if (string.IsNullOrWhiteSpace(comentarioCierre))
                throw new ArgumentException("El comentario de cierre es obligatorio.", nameof(comentarioCierre));

            int estadoFinal = estadoCerradoOverride ?? ESTADO_CERRADO;

            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"UPDATE Incidencias 
                                       SET ComentarioCierre = @coment,
                                           IDEstado = @estado
                                       WHERE IDIncidencia = @id");
                datos.SetearParametro("@coment", comentarioCierre.Trim());
                datos.SetearParametro("@estado", estadoFinal);
                datos.SetearParametro("@id", idIncidencia);
                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // REASIGNAR (cambia asignado y estado a Asignado)
        public void ReasignarIncidencia(int idIncidencia, int nuevoUsuarioAsignadoId, Usuarios supervisor)
        {
            // Validación de perfil si la quieres estricta:
            // if (supervisor?.Perfil?.IDPerfil != 3) throw new UnauthorizedAccessException("Solo supervisor puede reasignar.");

            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"UPDATE Incidencias 
                                       SET IDUsuarioAsignado = @nuevo, IDEstado = @estado
                                       WHERE IDIncidencia = @id");
                datos.SetearParametro("@nuevo", nuevoUsuarioAsignadoId);
                datos.SetearParametro("@estado", ESTADO_ASIGNADO);
                datos.SetearParametro("@id", idIncidencia);
                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // Wrappers antiguos si los usabas en otras partes
        public void ResolverIncidencia(int idIncidencia, string comentario, Usuarios usuarioAccion)
            => ResolverIncidenciaConComentario(idIncidencia, comentario, usuarioAccion, ESTADO_RESUELTO);

        public void CerrarIncidencia(int idIncidencia, string comentario, Usuarios usuarioAccion)
            => CerrarIncidenciaConComentario(idIncidencia, comentario, usuarioAccion, ESTADO_CERRADO);

        // Mapeo reutilizable
        private Incidencias MapearIncidencia(AccesoDatos datos)
        {
            return new Incidencias
            {
                IDIncidencia = Convert.ToInt32(datos.Lector["IDIncidencia"]),
                NumeroReclamo = Convert.ToInt32(datos.Lector["NumeroReclamo"]),
                Descripcion = datos.Lector["Descripcion"] == DBNull.Value ? null : (string)datos.Lector["Descripcion"],
                ComentarioResolucion = datos.Lector["ComentarioResolucion"] == DBNull.Value ? null : (string)datos.Lector["ComentarioResolucion"],
                ComentarioCierre = datos.Lector["ComentarioCierre"] == DBNull.Value ? null : (string)datos.Lector["ComentarioCierre"],
                FechaAlta = Convert.ToDateTime(datos.Lector["FechaAlta"]),
                FechaResolucion = datos.Lector["FechaResolucion"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(datos.Lector["FechaResolucion"]),
                Estado = new Estados
                {
                    IDEstado = Convert.ToInt32(datos.Lector["IDEstado"]),
                    Descripcion = datos.Lector["EstadoDesc"] == DBNull.Value ? "" : (string)datos.Lector["EstadoDesc"]
                },
                Cliente = new Clientes
                {
                    Nombre = datos.Lector["ClienteNombre"] == DBNull.Value ? "" : (string)datos.Lector["ClienteNombre"]
                },
                TipoIncidencia = new TiposDeIncidencia
                {
                    Nombre = datos.Lector["TipoNombre"] == DBNull.Value ? "" : (string)datos.Lector["TipoNombre"]
                },
                Prioridad = new Prioridades
                {
                    Nombre = datos.Lector["PrioridadNombre"] == DBNull.Value ? "" : (string)datos.Lector["PrioridadNombre"]
                },
                CreadorUsuario = new Usuarios
                {
                    IDUsuario = Convert.ToInt32(datos.Lector["CreadorID"]),
                    Nombre = datos.Lector["CreadorNombre"] == DBNull.Value ? "" : (string)datos.Lector["CreadorNombre"]
                },
                AsignadoUsuario = new Usuarios
                {
                    IDUsuario = Convert.ToInt32(datos.Lector["AsignadoID"]),
                    Nombre = datos.Lector["AsignadoNombre"] == DBNull.Value ? "" : (string)datos.Lector["AsignadoNombre"]
                }
            };
        }
    }
}