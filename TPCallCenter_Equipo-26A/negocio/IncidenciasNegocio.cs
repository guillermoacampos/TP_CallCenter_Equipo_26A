using System;
using System.Collections.Generic;
using dominio;

namespace negocio
{
    public class IncidenciasNegocio
    {
        // IDs de estado (ajusta si difieren)
        private const int ESTADO_ABIERTO = 1;
        private const int ESTADO_EN_ANALISIS = 2;
        private const int ESTADO_CERRADO = 3;
        private const int ESTADO_REABIERTO = 4;
        private const int ESTADO_ASIGNADO = 5;
        private const int ESTADO_RESUELTO = 6;

        // CREAR INCIDENCIA
        public int CrearIncidencia(Incidencias inc, int idCreadorUsuario)
        {
            if (inc == null) throw new ArgumentNullException(nameof(inc));
            if (inc.Cliente == null || inc.Cliente.IDCliente <= 0) throw new ArgumentException("Falta el cliente.", nameof(inc.Cliente));
            if (inc.TipoIncidencia == null || inc.TipoIncidencia.IDTipoIncidencia <= 0) throw new ArgumentException("Falta el tipo.", nameof(inc.TipoIncidencia));
            if (inc.Prioridad == null || inc.Prioridad.IDPrioridad <= 0) throw new ArgumentException("Falta la prioridad.", nameof(inc.Prioridad));

            int idEstado = inc.Estado?.IDEstado > 0 ? inc.Estado.IDEstado : ESTADO_ABIERTO;
            int idAsignado = inc.AsignadoUsuario?.IDUsuario > 0 ? inc.AsignadoUsuario.IDUsuario : idCreadorUsuario;

            int nuevoIdIncidencia = 0;
            int numeroReclamo = 0;

            var datos = new AccesoDatos();
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
            var datos = new AccesoDatos();
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
                    lista.Add(MapearIncidencia(datos));
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
            var datos = new AccesoDatos();
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

                return datos.Lector.Read() ? MapearIncidencia(datos) : null;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // MÉTODO GENÉRICO USADO POR TU CODE-BEHIND (soluciona los CS1061)
        public void Actualizar(Incidencias inc)
        {
            if (inc == null) throw new ArgumentNullException(nameof(inc));
            if (inc.IDIncidencia <= 0) throw new ArgumentException("ID inválido.", nameof(inc.IDIncidencia));
            if (inc.Estado == null) throw new ArgumentException("Estado requerido.", nameof(inc.Estado));

            var datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
                    UPDATE Incidencias SET
                        Descripcion = @desc,
                        ComentarioResolucion = @cres,
                        ComentarioCierre = @ccie,
                        IDEstado = @estado
                    WHERE IDIncidencia = @id");
                datos.SetearParametro("@desc", (inc.Descripcion ?? "").Trim());
                datos.SetearParametro("@cres", (object)inc.ComentarioResolucion ?? DBNull.Value);
                datos.SetearParametro("@ccie", (object)inc.ComentarioCierre ?? DBNull.Value);
                datos.SetearParametro("@estado", inc.Estado.IDEstado);
                datos.SetearParametro("@id", inc.IDIncidencia);
                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // MODIFICAR DESCRIPCIÓN + EN ANÁLISIS
        public void ModificarDescripcionYEstado(int idIncidencia, string nuevaDescripcion, int? estadoAnalisisOverride = null)
        {
            if (string.IsNullOrWhiteSpace(nuevaDescripcion))
                throw new ArgumentException("La descripción no puede estar vacía.");
            int estadoFinal = estadoAnalisisOverride ?? ESTADO_EN_ANALISIS;

            var datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("UPDATE Incidencias SET Descripcion=@desc, IDEstado=@estado WHERE IDIncidencia=@id");
                datos.SetearParametro("@desc", nuevaDescripcion.Trim());
                datos.SetearParametro("@estado", estadoFinal);
                datos.SetearParametro("@id", idIncidencia);
                datos.EjecutarAccion();
            }
            finally { datos.CerrarConexion(); }
        }

        // RESOLVER
        public void ResolverIncidenciaConComentario(int idIncidencia, string comentarioResolucion, Usuarios usuarioAccion, int? estadoResueltoOverride = null)
        {
            if (string.IsNullOrWhiteSpace(comentarioResolucion))
                throw new ArgumentException("Comentario de resolución obligatorio.");
            int estadoFinal = estadoResueltoOverride ?? ESTADO_RESUELTO;

            var datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
                    UPDATE Incidencias SET 
                        ComentarioResolucion=@coment,
                        FechaResolucion=@fecha,
                        IDEstado=@estado
                    WHERE IDIncidencia=@id");
                datos.SetearParametro("@coment", comentarioResolucion.Trim());
                datos.SetearParametro("@fecha", DateTime.Now);
                datos.SetearParametro("@estado", estadoFinal);
                datos.SetearParametro("@id", idIncidencia);
                datos.EjecutarAccion();
            }
            finally { datos.CerrarConexion(); }
        }

        // CERRAR
        public void CerrarIncidenciaConComentario(int idIncidencia, string comentarioCierre, Usuarios usuarioAccion, int? estadoCerradoOverride = null)
        {
            if (string.IsNullOrWhiteSpace(comentarioCierre))
                throw new ArgumentException("Comentario de cierre obligatorio.");
            int estadoFinal = estadoCerradoOverride ?? ESTADO_CERRADO;

            var datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
                    UPDATE Incidencias SET 
                        ComentarioCierre=@coment,
                        IDEstado=@estado
                    WHERE IDIncidencia=@id");
                datos.SetearParametro("@coment", comentarioCierre.Trim());
                datos.SetearParametro("@estado", estadoFinal);
                datos.SetearParametro("@id", idIncidencia);
                datos.EjecutarAccion();
            }
            finally { datos.CerrarConexion(); }
        }

        // REASIGNAR
        public void ReasignarIncidencia(int idIncidencia, int nuevoUsuarioAsignadoId, Usuarios supervisor)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
                    UPDATE Incidencias SET 
                        IDUsuarioAsignado=@nuevo,
                        IDEstado=@estado
                    WHERE IDIncidencia=@id");
                datos.SetearParametro("@nuevo", nuevoUsuarioAsignadoId);
                datos.SetearParametro("@estado", ESTADO_ASIGNADO);
                datos.SetearParametro("@id", idIncidencia);
                datos.EjecutarAccion();
            }
            finally { datos.CerrarConexion(); }
        }

        // Mapeo
        private Incidencias MapearIncidencia(AccesoDatos datos)
        {
            var inc = new Incidencias
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
                    IDCliente = Convert.ToInt32(datos.Lector["IDCliente"]),
                    Nombre = datos.Lector["ClienteNombre"] == DBNull.Value ? "" : (string)datos.Lector["ClienteNombre"]
                },
                TipoIncidencia = new TiposDeIncidencia
                {
                    IDTipoIncidencia = Convert.ToInt32(datos.Lector["IDTipoIncidencia"]),
                    Nombre = datos.Lector["TipoNombre"] == DBNull.Value ? "" : (string)datos.Lector["TipoNombre"]
                },
                Prioridad = new Prioridades
                {
                    IDPrioridad = Convert.ToInt32(datos.Lector["IDPrioridad"]),
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
            return inc;
        }
    }
}