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
                datos.SetearConsulta("SELECT IDEstado, Descripcion FROM Estados");
                datos.EjecutarLectura();

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
                datos.CerrarConexion();
            }
        }

        public void agregar(Estados nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("INSERT INTO Estados (Descripcion) VALUES (@Descripcion)");
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

        public void modificar(Estados estado)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("UPDATE Estados SET Descripcion = @Descripcion WHERE IDEstado = @IDEstado");
                datos.SetearParametro("@Descripcion", estado.Descripcion);
                datos.SetearParametro("@IDEstado", estado.IDEstado);
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
                datos.SetearConsulta("DELETE FROM Estados WHERE IDEstado = @IDEstado");
                datos.SetearParametro("@IDEstado", id);
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