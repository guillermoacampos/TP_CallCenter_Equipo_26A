using System;
using System.Collections.Generic;
using dominio;

namespace negocio
{
    public class EstadosNegocio
    {
        public List<Estados> listar()
        {
            List<Estados> lista = new List<Estados>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT IDEstado, Descripcion FROM Estados");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Estados estado = new Estados
                    {
                        IDEstado = (int)datos.Lector["IDEstado"],
                        Descripcion = (string)datos.Lector["Descripcion"]
                    };

                    lista.Add(estado);
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

        public void agregar(Estados nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO Estados (Descripcion) VALUES (@Descripcion)");
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

        public void modificar(Estados estado)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE Estados SET Descripcion = @Descripcion WHERE IDEstado = @IDEstado");
                datos.setearParametro("@Descripcion", estado.Descripcion);
                datos.setearParametro("@IDEstado", estado.IDEstado);
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
                datos.setearConsulta("DELETE FROM Estados WHERE IDEstado = @IDEstado");
                datos.setearParametro("@IDEstado", id);
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