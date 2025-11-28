using System;
using System.Collections.Generic;
using dominio;

namespace negocio
{
    public class UsuariosNegocio
    {
        private AccesoDatos datos;

        // Listar todos los usuarios con su perfil
        public List<Usuarios> listar()
        {
            var lista = new List<Usuarios>();
            datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
                    SELECT U.IDUsuario,
                           U.Nombre,
                           U.Apellido,
                           U.Email,
                           U.[Contraseña] AS Contrasena,
                           U.Activo,
                           U.[FechaDeAlta] AS FechaAlta,
                           P.IDPerfil,
                           P.Descripcion AS PerfilDescripcion
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
                            IDPerfil = Convert.ToInt32(datos.Lector["IDPerfil"]),
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

        // Login (email + contraseña)
        public Usuarios Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
                    SELECT TOP 1 U.IDUsuario,
                                 U.Nombre,
                                 U.Apellido,
                                 U.Email,
                                 U.[Contraseña] AS Contrasena,
                                 U.Activo,
                                 U.[FechaDeAlta] AS FechaAlta,
                                 P.IDPerfil,
                                 P.Descripcion AS PerfilDescripcion
                    FROM Usuarios U
                    INNER JOIN Perfil P ON P.IDPerfil = U.IDPerfil
                    WHERE U.Email = @email AND U.[Contraseña] = @pass");
                datos.SetearParametro("@email", email.Trim());
                datos.SetearParametro("@pass", password.Trim()); // Si luego usas hash, cambia esta comparación

                datos.EjecutarLectura();
                if (!datos.Lector.Read())
                    return null;

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

        // Agregar un nuevo usuario
        public void agregar(Usuarios nuevo)
        {
            if (nuevo == null) throw new ArgumentNullException(nameof(nuevo));
            if (nuevo.Perfil == null || nuevo.Perfil.IDPerfil <= 0)
                throw new ArgumentException("Perfil inválido.");
            if (string.IsNullOrWhiteSpace(nuevo.Email))
                throw new ArgumentException("Email obligatorio.");
            if (string.IsNullOrWhiteSpace(nuevo.Contrasena))
                throw new ArgumentException("Contraseña obligatoria.");

            datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
                    INSERT INTO Usuarios (Nombre, Apellido, Email, [Contraseña], IDPerfil, Activo, [FechaDeAlta])
                    VALUES (@nom, @ape, @mail, @pass, @perfil, @activo, @fecha)");
                datos.SetearParametro("@nom", (nuevo.Nombre ?? "").Trim());
                datos.SetearParametro("@ape", (nuevo.Apellido ?? "").Trim());
                datos.SetearParametro("@mail", (nuevo.Email ?? "").Trim());
                datos.SetearParametro("@pass", (nuevo.Contrasena ?? "").Trim());
                datos.SetearParametro("@perfil", nuevo.Perfil.IDPerfil);
                datos.SetearParametro("@activo", nuevo.Activo);
                datos.SetearParametro("@fecha", nuevo.FechaAlta == default ? DateTime.Now : nuevo.FechaAlta);
                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // Eliminar / baja lógica
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