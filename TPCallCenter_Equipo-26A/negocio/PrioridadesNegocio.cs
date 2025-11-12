using System;
using System.Collections.Generic;
using dominio;

namespace negocio
{
    public class PrioridadesNegocio
    {
        public List<Prioridades> listar()
        {
            List<Prioridades> lista = new List<Prioridades>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT IDPrioridades, Nombre, Nivel, Descripcion FROM Prioridades");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Prioridades prioridad = new Prioridades
                    {
                        IDPrioridades = (int)datos.Lector["IDPrioridades"],
                        Nombre = (string)datos.Lector["Nombre"],
                        Nivel = (int)datos.Lector["Nivel"],
                        Descripcion = (string)datos.Lector["Descripcion"]
                    };

                    lista.Add(prioridad);
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

        public void agregar(Prioridades nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO Prioridades (Nombre, Nivel, Descripcion) VALUES (@Nombre, @Nivel, @Descripcion)");
                datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Nivel", nuevo.Nivel);
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

        public void modificar(Prioridades prioridad)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE Prioridades SET Nombre = @Nombre, Nivel = @Nivel, Descripcion = @Descripcion WHERE IDPrioridades = @IDPrioridades");
                datos.setearParametro("@Nombre", prioridad.Nombre);
                datos.setearParametro("@Nivel", prioridad.Nivel);
                datos.setearParametro("@Descripcion", prioridad.Descripcion);
                datos.setearParametro("@IDPrioridades", prioridad.IDPrioridades);
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
                datos.setearConsulta("DELETE FROM Prioridades WHERE IDPrioridades = @IDPrioridades");
                datos.setearParametro("@IDPrioridades", id);
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