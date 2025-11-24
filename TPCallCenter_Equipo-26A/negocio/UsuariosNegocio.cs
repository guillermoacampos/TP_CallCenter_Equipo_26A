using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class UsuariosNegocio
    {
        public List<Usuarios> listar()
        {
            List<Usuarios> lista = new List<Usuarios>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT IDUsuario, Nombre, Apellido, Email, Contraseña, IDPerfil, Activo, FechaDeAlta FROM Usuarios WHERE Activo = 1");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Usuarios aux = new Usuarios();
                    aux.IDUsuario = (int)datos.Lector["IDUsuario"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Email = (string)datos.Lector["Email"];
                    aux.Contrasena = (string)datos.Lector["Contraseña"];
                    aux.Perfil = new Perfil
                    {
                        IDPerfil = (int)datos.Lector["IDPerfil"]
                    };
                    aux.Activo = (bool)datos.Lector["Activo"];
                    aux.FechaAlta = (DateTime)datos.Lector["FechaDeAlta"];

                    lista.Add(aux);
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

        public Usuarios obtenerPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            Usuarios usuario = null;

            try
            {
                datos.SetearConsulta("SELECT IDUsuario, Nombre, Apellido, Email, Contraseña, IDPerfil, Activo, FechaDeAlta FROM Usuarios WHERE IDUsuario = @id AND Activo = 1");
                datos.SetearParametro("@id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    usuario = new Usuarios();
                    usuario.IDUsuario = (int)datos.Lector["IDUsuario"];
                    usuario.Nombre = (string)datos.Lector["Nombre"];
                    usuario.Apellido = (string)datos.Lector["Apellido"];
                    usuario.Email = (string)datos.Lector["Email"];
                    usuario.Contrasena = (string)datos.Lector["Contraseña"];
                    usuario.Perfil = new Perfil
                    {
                        IDPerfil = (int)datos.Lector["IDPerfil"]
                    };
                    usuario.Activo = (bool)datos.Lector["Activo"];
                    usuario.FechaAlta = (DateTime)datos.Lector["FechaDeAlta"];
                }

                return usuario;
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

        public void agregar(Usuarios nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("INSERT INTO Usuarios (Nombre, Apellido, Email, Contraseña, IDPerfil, Activo, FechaDeAlta) VALUES (@Nombre, @Apellido, @Email, @Contrasena, @IDPerfil, @Activo, @FechaAlta)");
                datos.SetearParametro("@Nombre", nuevo.Nombre);
                datos.SetearParametro("@Apellido", nuevo.Apellido);
                datos.SetearParametro("@Email", nuevo.Email);
                datos.SetearParametro("@Contrasena", nuevo.Contrasena);
                datos.SetearParametro("@IDPerfil", nuevo.Perfil.IDPerfil);
                datos.SetearParametro("@Activo", nuevo.Activo);
                datos.SetearParametro("@FechaAlta", nuevo.FechaAlta);
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

        public void modificar(Usuarios usuario)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("UPDATE Usuarios SET Nombre = @Nombre, Apellido = @Apellido, Email = @Email, Contraseña = @Contrasena, IDPerfil = @IDPerfil, Activo = @Activo WHERE IDUsuario = @IDUsuario");
                datos.SetearParametro("@Nombre", usuario.Nombre);
                datos.SetearParametro("@Apellido", usuario.Apellido);
                datos.SetearParametro("@Email", usuario.Email);
                datos.SetearParametro("@Contrasena", usuario.Contrasena);
                datos.SetearParametro("@IDPerfil", usuario.Perfil.IDPerfil);
                datos.SetearParametro("@Activo", usuario.Activo);
                datos.SetearParametro("@IDUsuario", usuario.IDUsuario);
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

        public void eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("DELETE FROM Usuarios WHERE IDUsuario = @IDUsuario");
                datos.SetearParametro("@IDUsuario", id);
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

        public bool Login(Usuarios usuario)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT IDUsuario, Nombre, Apellido, Email, Contraseña, IDPerfil, Activo FROM Usuarios WHERE Email = @Email AND Contraseña = @Contrasena AND Activo = 1");
                datos.SetearParametro("@Email", usuario.Email);
                datos.SetearParametro("@Contrasena", usuario.Contrasena);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    usuario.IDUsuario = (int)datos.Lector["IDUsuario"];
                    usuario.Nombre = (string)datos.Lector["Nombre"];
                    usuario.Apellido = (string)datos.Lector["Apellido"];
                    usuario.Perfil = new Perfil
                    {
                        IDPerfil = (int)datos.Lector["IDPerfil"]
                    };
                    usuario.Activo = (bool)datos.Lector["Activo"];

                    return true;
                }

                return false;
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