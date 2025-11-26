using System;
using System.Collections.Generic;
using dominio;

namespace negocio
{
    public class IncidenciasNegocio
    {
        public List<int> ObtenerCantidadPorTipo()
        {
            List<int> cantidades = new List<int>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT IDTipoIncidencia, COUNT(*) AS Cantidad FROM Incidencias GROUP BY IDTipoIncidencia ORDER BY IDTipoIncidencia");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    cantidades.Add((int)datos.Lector["Cantidad"]);
                }

                return cantidades;
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