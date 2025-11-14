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
                datos.SetearConsulta("SELECT IDTipoIncidencia, Nombre, Descripcion FROM TiposDeIncidencia");
                datos.EjecutarLectura();

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
                datos.CerrarConexion();
            }
        }

        public void agregar(TiposDeIncidencia nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("INSERT INTO TiposDeIncidencia (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)");
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

        public void modificar(TiposDeIncidencia tipo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("UPDATE TiposDeIncidencia SET Nombre = @Nombre, Descripcion = @Descripcion WHERE IDTipoIncidencia = @IDTipoIncidencia");
                datos.SetearParametro("@Nombre", tipo.Nombre);
                datos.SetearParametro("@Descripcion", tipo.Descripcion);
                datos.SetearParametro("@IDTipoIncidencia", tipo.IDTipoIncidencia);
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
                datos.SetearConsulta("DELETE FROM TiposDeIncidencia WHERE IDTipoIncidencia = @IDTipoIncidencia");
                datos.SetearParametro("@IDTipoIncidencia", id);
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