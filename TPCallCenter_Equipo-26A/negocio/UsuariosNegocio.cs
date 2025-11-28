using System;
using System.Collections.Generic;
using dominio;

namespace negocio
{
    public class UsuariosNegocio
    {
        private AccesoDatos datos;

        // Lock estático para serializar la asignación manual de IDs.
        private static readonly object _idLock = new object();

        // Listar usuarios
        public List<Usuarios> listar()
        {
            var lista = new List<Usuarios>();
            datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
                    SELECT U.IDUsuario, U.Nombre, U.Apellido, U.Email, U.[Contraseña] AS Contrasena, 
                           U.IDPerfil, U.Activo, U.FechaDeAlta AS FechaAlta,
                           P.IDPerfil AS PerfilID, P.Descripcion AS PerfilDescripcion
                    FROM Usuarios U
                    INNER JOIN Perfil P ON P.IDPerfil = U.IDPerfil");
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                {
                    var u = new Usuarios
                    {
                        IDUsuario = Convert.ToInt32(datos.Lector["IDUsuario"]),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        Email = datos.Lector["Email"].ToString(),
                        Contrasena = datos.Lector["Contrasena"].ToString(),
                        Activo = Convert.ToBoolean(datos.Lector["Activo"]),
                        FechaAlta = Convert.ToDateTime(datos.Lector["FechaAlta"]),
                        Perfil = new Perfil
                        {
                            IDPerfil = Convert.ToInt32(datos.Lector["PerfilID"]),
                            Descripcion = datos.Lector["PerfilDescripcion"].ToString()
                        }
                    };
                    lista.Add(u);
                }
                return lista;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // Login (texto plano, adaptar a hashing si lo deseas)
        public Usuarios Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
                    SELECT TOP 1 U.IDUsuario, U.Nombre, U.Apellido, U.Email, U.[Contraseña] AS Contrasena, 
                                 U.Activo, U.FechaDeAlta AS FechaAlta,
                                 P.IDPerfil, P.Descripcion AS PerfilDescripcion
                    FROM Usuarios U
                    INNER JOIN Perfil P ON P.IDPerfil = U.IDPerfil
                    WHERE U.Email = @email AND U.[Contraseña] = @pass");
                datos.SetearParametro("@email", email.Trim());
                datos.SetearParametro("@pass", password.Trim());

                datos.EjecutarLectura();
                if (!datos.Lector.Read()) return null;

                return new Usuarios
                {
                    IDUsuario = Convert.ToInt32(datos.Lector["IDUsuario"]),
                    Nombre = datos.Lector["Nombre"].ToString(),
                    Apellido = datos.Lector["Apellido"].ToString(),
                    Email = datos.Lector["Email"].ToString(),
                    Contrasena = datos.Lector["Contrasena"].ToString(),
                    Activo = Convert.ToBoolean(datos.Lector["Activo"]),
                    FechaAlta = Convert.ToDateTime(datos.Lector["FechaAlta"]),
                    Perfil = new Perfil
                    {
                        IDPerfil = Convert.ToInt32(datos.Lector["IDPerfil"]),
                        Descripcion = datos.Lector["PerfilDescripcion"].ToString()
                    }
                };
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // Verifica si ya existe un email (para evitar duplicados)
        public bool EmailExiste(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT COUNT(*) FROM Usuarios WHERE Email = @mail");
                datos.SetearParametro("@mail", email.Trim());
                datos.EjecutarLectura();
                if (datos.Lector.Read())
                {
                    int count = Convert.ToInt32(datos.Lector[0]);
                    return count > 0;
                }
                return false;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // Obtiene el próximo ID manualmente (sin identity)
        private int ObtenerSiguienteIdUsuario()
        {
            datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT ISNULL(MAX(IDUsuario),0) + 1 FROM Usuarios");
                datos.EjecutarLectura();
                if (datos.Lector.Read())
                    return Convert.ToInt32(datos.Lector[0]);
                return 1;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // Método agregar con asignación manual de ID
        public void agregar(Usuarios nuevo)
        {
            if (nuevo == null) throw new ArgumentNullException(nameof(nuevo));
            if (nuevo.Perfil == null || nuevo.Perfil.IDPerfil <= 0)
                throw new ArgumentException("Perfil inválido (IDPerfil).", nameof(nuevo.Perfil));
            if (string.IsNullOrWhiteSpace(nuevo.Email))
                throw new ArgumentException("Email obligatorio.", nameof(nuevo.Email));
            if (string.IsNullOrWhiteSpace(nuevo.Contrasena))
                throw new ArgumentException("Contraseña obligatoria.", nameof(nuevo.Contrasena));

            // Validación de email duplicado
            if (EmailExiste(nuevo.Email))
                throw new InvalidOperationException("Ya existe un usuario con ese email.");

            int nuevoId;
            // Bloque crítico para evitar colisiones con inserts simultáneos
            lock (_idLock)
            {
                nuevoId = ObtenerSiguienteIdUsuario();
            }

            datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
                    INSERT INTO Usuarios (IDUsuario, Nombre, Apellido, Email, [Contraseña], IDPerfil, Activo, FechaDeAlta)
                    VALUES (@id, @nom, @ape, @mail, @pass, @perfil, @activo, @fecha)");
                datos.SetearParametro("@id", nuevoId);
                datos.SetearParametro("@nom", (nuevo.Nombre ?? "").Trim());
                datos.SetearParametro("@ape", (nuevo.Apellido ?? "").Trim());
                datos.SetearParametro("@mail", (nuevo.Email ?? "").Trim());
                datos.SetearParametro("@pass", (nuevo.Contrasena ?? "").Trim());
                datos.SetearParametro("@perfil", nuevo.Perfil.IDPerfil);
                datos.SetearParametro("@activo", nuevo.Activo);
                datos.SetearParametro("@fecha", nuevo.FechaAlta == default ? DateTime.Now.Date : nuevo.FechaAlta.Date);

                datos.EjecutarAccion();
                nuevo.IDUsuario = nuevoId;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // Eliminar (baja lógica o física)
        public void eliminar(int idUsuario, bool bajaLogica = true)
        {
            datos = new AccesoDatos();
            try
            {
                if (bajaLogica)
                    datos.SetearConsulta("UPDATE Usuarios SET Activo = 0 WHERE IDUsuario = @id");
                else
                    datos.SetearConsulta("DELETE FROM Usuarios WHERE IDUsuario = @id");

                datos.SetearParametro("@id", idUsuario);
                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}