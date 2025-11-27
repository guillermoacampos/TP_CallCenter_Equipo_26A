using System;
using System.Collections.Generic;
using System.Text;

namespace negocio
{
    // Clase de negocio UsuariosNegocio (versión determinista y simple)
    // Usa los nombres de columnas EXACTOS según tu tabla:
    // IDUsuario, Nombre, Apellido, Email, Contraseña, IDPerfil, Activo, FechaDeAlta
    public class UsuariosNegocio
    {
        // LOGIN determinista: compara Email y Contraseña en texto claro y (opcional) Activo = 1
        public bool Login(dominio.Usuarios usuario)
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));

            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Consulta con nombres de columnas reales (incluye acento en Contraseña)
                datos.SetearConsulta(
                    "SELECT [IDUsuario], [Nombre], [Apellido], [Email], [IDPerfil], [Activo] " +
                    "FROM [Usuarios] " +
                    "WHERE [Email] = @email AND [Contraseña] = @pass AND [Activo] = 1"
                );
                datos.SetearParametro("@email", usuario.Email ?? string.Empty);
                datos.SetearParametro("@pass", usuario.Contrasena ?? string.Empty);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    // Mapear campos a la entidad existente
                    usuario.IDUsuario = datos.Lector["IDUsuario"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDUsuario"]);
                    usuario.Nombre = datos.Lector["Nombre"] == DBNull.Value ? null : (string)datos.Lector["Nombre"];
                    if (HasColumn(datos, "Apellido") && datos.Lector["Apellido"] != DBNull.Value)
                        usuario.Apellido = (string)datos.Lector["Apellido"];

                    usuario.Email = datos.Lector["Email"] == DBNull.Value ? null : (string)datos.Lector["Email"];
                    usuario.Perfil = new dominio.Perfil
                    {
                        IDPerfil = datos.Lector["IDPerfil"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDPerfil"])
                    };

                    if (HasColumn(datos, "Activo") && datos.Lector["Activo"] != DBNull.Value)
                        usuario.Activo = Convert.ToInt32(datos.Lector["Activo"]) == 1;

                    return true;
                }

                return false;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // Agregar usuario según tu esquema (Contraseña en texto claro)
        public void agregar(dominio.Usuarios usuario)
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));

            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(
                    "INSERT INTO [Usuarios] ([Nombre], [Apellido], [Email], [Contraseña], [IDPerfil], [Activo], [FechaDeAlta]) " +
                    "VALUES (@nombre, @apellido, @email, @pass, @perfil, @activo, @fecha)"
                );
                datos.SetearParametro("@nombre", usuario.Nombre ?? string.Empty);
                datos.SetearParametro("@apellido", usuario.Apellido ?? string.Empty);
                datos.SetearParametro("@email", usuario.Email ?? string.Empty);
                datos.SetearParametro("@pass", usuario.Contrasena ?? string.Empty);
                datos.SetearParametro("@perfil", usuario.Perfil?.IDPerfil ?? 1);
                datos.SetearParametro("@activo", usuario.Activo ? 1 : 1); // por defecto activo
                datos.SetearParametro("@fecha", DateTime.Now);
                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
        public void Agregar(dominio.Usuarios usuario) => agregar(usuario);

        // Listado de usuarios (sin contraseña)
        public List<dominio.Usuarios> listar()
        {
            var lista = new List<dominio.Usuarios>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT [IDUsuario], [Nombre], [Apellido], [Email], [IDPerfil], [Activo] FROM [Usuarios]");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    var u = new dominio.Usuarios
                    {
                        IDUsuario = datos.Lector["IDUsuario"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDUsuario"]),
                        Nombre = datos.Lector["Nombre"] == DBNull.Value ? null : (string)datos.Lector["Nombre"],
                        Email = datos.Lector["Email"] == DBNull.Value ? null : (string)datos.Lector["Email"],
                        Perfil = new dominio.Perfil
                        {
                            IDPerfil = datos.Lector["IDPerfil"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDPerfil"])
                        }
                    };

                    if (HasColumn(datos, "Apellido") && datos.Lector["Apellido"] != DBNull.Value)
                        u.Apellido = (string)datos.Lector["Apellido"];

                    if (HasColumn(datos, "Activo") && datos.Lector["Activo"] != DBNull.Value)
                        u.Activo = Convert.ToInt32(datos.Lector["Activo"]) == 1;

                    lista.Add(u);
                }

                return lista;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
        public List<dominio.Usuarios> Listar() => listar();
        public List<dominio.Usuarios> ListarTodos() => listar();

        public void eliminar(int idUsuario)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("DELETE FROM [Usuarios] WHERE [IDUsuario] = @id");
                datos.SetearParametro("@id", idUsuario);
                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
        public void Eliminar(int idUsuario) => eliminar(idUsuario);

        public dominio.Usuarios ObtenerPorId(int idUsuario)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT [IDUsuario], [Nombre], [Apellido], [Email], [IDPerfil], [Activo] FROM [Usuarios] WHERE [IDUsuario] = @id");
                datos.SetearParametro("@id", idUsuario);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    var u = new dominio.Usuarios
                    {
                        IDUsuario = datos.Lector["IDUsuario"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDUsuario"]),
                        Nombre = datos.Lector["Nombre"] == DBNull.Value ? null : (string)datos.Lector["Nombre"],
                        Email = datos.Lector["Email"] == DBNull.Value ? null : (string)datos.Lector["Email"],
                        Perfil = new dominio.Perfil
                        {
                            IDPerfil = datos.Lector["IDPerfil"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["IDPerfil"])
                        }
                    };

                    if (HasColumn(datos, "Apellido") && datos.Lector["Apellido"] != DBNull.Value)
                        u.Apellido = (string)datos.Lector["Apellido"];

                    if (HasColumn(datos, "Activo") && datos.Lector["Activo"] != DBNull.Value)
                        u.Activo = Convert.ToInt32(datos.Lector["Activo"]) == 1;

                    return u;
                }

                return null;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // Helper: verifica si el lector tiene una columna por nombre (evita errores si el esquema cambia)
        private bool HasColumn(AccesoDatos datos, string columnName)
        {
            try
            {
                for (int i = 0; i < datos.Lector.FieldCount; i++)
                {
                    if (string.Equals(datos.Lector.GetName(i), columnName, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            catch { }
            return false;
        }
    }
}