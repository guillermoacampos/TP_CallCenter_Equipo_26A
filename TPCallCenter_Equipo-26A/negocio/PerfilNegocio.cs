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
                datos.SetearConsulta("SELECT IDPerfil, Nombre, Descripcion FROM Perfiles");
                datos.EjecutarLectura();

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
                datos.CerrarConexion();
            }
        }

        public void agregar(Perfil nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("INSERT INTO Perfiles (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)");
                datos.SetearParametro("@Nombre", nuevo.Nombre);
                datos.SetearParametro("@Descripcion", nuevo.Descripcion);
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

        public void modificar(Perfil perfil)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("UPDATE Perfiles SET Nombre = @Nombre, Descripcion = @Descripcion WHERE IDPerfil = @IDPerfil");
                datos.SetearParametro("@Nombre", perfil.Nombre);
                datos.SetearParametro("@Descripcion", perfil.Descripcion);
                datos.SetearParametro("@IDPerfil", perfil.IDPerfil);
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
                datos.SetearConsulta("DELETE FROM Perfiles WHERE IDPerfil = @IDPerfil");
                datos.SetearParametro("@IDPerfil", id);
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
    }
}