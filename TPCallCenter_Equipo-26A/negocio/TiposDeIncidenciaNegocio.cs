using System;
using System.Collections.Generic;
using dominio;

namespace negocio
{
    public class TiposDeIncidenciaNegocio
    {
        public List<TiposDeIncidencia> listar()
        {
            List<TiposDeIncidencia> lista = new List<TiposDeIncidencia>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT IDTipoIncidencia, Nombre, Descripcion FROM TiposDeIncidencia");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    TiposDeIncidencia tipo = new TiposDeIncidencia
                    {
                        IDTipoIncidencia = (int)datos.Lector["IDTipoIncidencia"],
                        Nombre = (string)datos.Lector["Nombre"],
                        Descripcion = (string)datos.Lector["Descripcion"]
                    };

                    lista.Add(tipo);
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

        public void agregar(TiposDeIncidencia nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO TiposDeIncidencia (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)");
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

        public void modificar(TiposDeIncidencia tipo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE TiposDeIncidencia SET Nombre = @Nombre, Descripcion = @Descripcion WHERE IDTipoIncidencia = @IDTipoIncidencia");
                datos.setearParametro("@Nombre", tipo.Nombre);
                datos.setearParametro("@Descripcion", tipo.Descripcion);
                datos.setearParametro("@IDTipoIncidencia", tipo.IDTipoIncidencia);
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
                datos.setearConsulta("DELETE FROM TiposDeIncidencia WHERE IDTipoIncidencia = @IDTipoIncidencia");
                datos.setearParametro("@IDTipoIncidencia", id);
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