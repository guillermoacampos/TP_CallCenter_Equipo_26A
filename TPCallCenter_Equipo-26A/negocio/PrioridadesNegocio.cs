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
                datos.SetearConsulta("SELECT IDPrioridades, Nombre, Nivel, Descripcion FROM Prioridades");
                datos.EjecutarLectura();

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
                datos.CerrarConexion();
            }
        }

        public void agregar(Prioridades nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("INSERT INTO Prioridades (Nombre, Nivel, Descripcion) VALUES (@Nombre, @Nivel, @Descripcion)");
                datos.SetearParametro("@Nombre", nuevo.Nombre);
                datos.SetearParametro("@Nivel", nuevo.Nivel);
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

        public void modificar(Prioridades prioridad)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("UPDATE Prioridades SET Nombre = @Nombre, Nivel = @Nivel, Descripcion = @Descripcion WHERE IDPrioridades = @IDPrioridades");
                datos.SetearParametro("@Nombre", prioridad.Nombre);
                datos.SetearParametro("@Nivel", prioridad.Nivel);
                datos.SetearParametro("@Descripcion", prioridad.Descripcion);
                datos.SetearParametro("@IDPrioridades", prioridad.IDPrioridades);
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
                datos.SetearConsulta("DELETE FROM Prioridades WHERE IDPrioridades = @IDPrioridades");
                datos.SetearParametro("@IDPrioridades", id);
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