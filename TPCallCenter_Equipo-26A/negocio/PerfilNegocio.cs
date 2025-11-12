using System;
using System.Collections.Generic;
using dominio;

namespace negocio
{
    public class PerfilNegocio
    {
        public List<Perfil> listar()
        {
            List<Perfil> lista = new List<Perfil>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT IDPerfil, Nombre, Descripcion FROM Perfiles");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Perfil perfil = new Perfil
                    {
                        IDPerfil = (int)datos.Lector["IDPerfil"],
                        Nombre = (string)datos.Lector["Nombre"],
                        Descripcion = (string)datos.Lector["Descripcion"]
                    };

                    lista.Add(perfil);
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

        public void agregar(Perfil nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO Perfiles (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)");
                datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Descripcion", nuevo.Descripcion);
                datos.ejecutarAccion();
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

        public void modificar(Perfil perfil)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE Perfiles SET Nombre = @Nombre, Descripcion = @Descripcion WHERE IDPerfil = @IDPerfil");
                datos.setearParametro("@Nombre", perfil.Nombre);
                datos.setearParametro("@Descripcion", perfil.Descripcion);
                datos.setearParametro("@IDPerfil", perfil.IDPerfil);
                datos.ejecutarAccion();
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

        public void eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("DELETE FROM Perfiles WHERE IDPerfil = @IDPerfil");
                datos.setearParametro("@IDPerfil", id);
                datos.ejecutarAccion();
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