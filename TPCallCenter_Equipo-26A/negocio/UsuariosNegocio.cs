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
                datos.setearConsulta("SELECT IDUsuario, Nombre, Apellido, Email, Contraseña, IDPerfil, Activo, FechaDeAlta FROM Usuarios WHERE Activo = 1");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Usuarios aux = new Usuarios();
                    aux.IDUsuario = (int)datos.Lector["IDUsuario"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Email = (string)datos.Lector["Email"];
                    aux.Contraseña = (string)datos.Lector["Contraseña"];
                    aux.Perfil = new Perfil
                    {
                        IDPerfil = (int)datos.Lector["IDPerfil"]
                    };
                    aux.Activo = (bool)datos.Lector["Activo"];
                    aux.FechaDeAlta = (DateTime)datos.Lector["FechaDeAlta"];

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
                datos.cerrarConexion();
            }
        }

        public Usuarios obtenerPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            Usuarios usuario = null;

            try
            {
                datos.setearConsulta("SELECT IDUsuario, Nombre, Apellido, Email, Contraseña, IDPerfil, Activo, FechaDeAlta FROM Usuarios WHERE IDUsuario = @id AND Activo = 1");
                datos.setearParametro("@id", id);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    usuario = new Usuarios();
                    usuario.IDUsuario = (int)datos.Lector["IDUsuario"];
                    usuario.Nombre = (string)datos.Lector["Nombre"];
                    usuario.Apellido = (string)datos.Lector["Apellido"];
                    usuario.Email = (string)datos.Lector["Email"];
                    usuario.Contraseña = (string)datos.Lector["Contraseña"];
                    usuario.Perfil = new Perfil
                    {
                        IDPerfil = (int)datos.Lector["IDPerfil"]
                    };
                    usuario.Activo = (bool)datos.Lector["Activo"];
                    usuario.FechaDeAlta = (DateTime)datos.Lector["FechaDeAlta"];
                }

                return usuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public bool Login(Usuarios usuario)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT IDUsuario, Nombre, Apellido, Email, Contraseña, IDPerfil, Activo, FechaDeAlta FROM Usuarios WHERE Email = @Email AND Contraseña = @Contraseña AND Activo = 1");
                datos.setearParametro("@Email", usuario.Email);
                datos.setearParametro("@Contraseña", usuario.Contraseña);
                datos.ejecutarLectura();

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
                    usuario.FechaDeAlta = (DateTime)datos.Lector["FechaDeAlta"];

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
                datos.cerrarConexion();
            }
        }
    }
}