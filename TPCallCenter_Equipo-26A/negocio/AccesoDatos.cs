using System;
using System.Data.SqlClient;

namespace negocio
{
    public class AccesoDatos : IDisposable
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;
        private bool disposed = false;

        public SqlDataReader Lector
        {
            get { return lector; }
        }

        public AccesoDatos()
        {
            //db niki: 
            conexion = new SqlConnection("server=(localdb)\\MSSQLLocalDB; database=CallCenter; integrated security=true");

            //db guillermo: 
            //conexion = new SqlConnection("server=localhost,1433; database=CallCenter; user=sa;PASSWORD=Doc39805119");

            comando = new SqlCommand { Connection = conexion };


        }

        public void SetearConsulta(string consulta)
        {
            if (comando == null)
                comando = new SqlCommand();

            // limpiar parámetros previos
            comando.Parameters.Clear();
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;

            // asegurar que el comando tenga la conexión asignada
            if (comando.Connection == null)
                comando.Connection = conexion;
        }

        public void EjecutarLectura()
        {
            try
            {
                if (conexion == null)
                    throw new InvalidOperationException("La conexión no fue inicializada.");

                if (comando.Connection == null)
                    comando.Connection = conexion;

                if (conexion.State != System.Data.ConnectionState.Open)
                    conexion.Open();

                lector = comando.ExecuteReader();
            }
            catch (Exception)
            {
                // relanzar preservando la pila de llamadas
                throw;
            }
        }

        public void EjecutarAccion()
        {
            try
            {
                if (conexion == null)
                    throw new InvalidOperationException("La conexión no fue inicializada.");

                if (comando.Connection == null)
                    comando.Connection = conexion;

                if (conexion.State != System.Data.ConnectionState.Open)
                    conexion.Open();

                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int EjecutarScalar()
        {
            try
            {
                if (conexion == null)
                    throw new InvalidOperationException("La conexión no fue inicializada.");

                if (comando.Connection == null)
                    comando.Connection = conexion;

                if (conexion.State != System.Data.ConnectionState.Open)
                    conexion.Open();

                object result = comando.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                    return 0;

                return Convert.ToInt32(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SetearParametro(string nombre, object valor)
        {
            if (comando == null)
                comando = new SqlCommand();

            // si valor es null pasamos DBNull.Value
            if (valor == null)
                comando.Parameters.AddWithValue(nombre, DBNull.Value);
            else
                comando.Parameters.AddWithValue(nombre, valor);
        }

        public void CerrarConexion()
        {
            // cerrar lector si existe
            try
            {
                if (lector != null)
                {
                    if (!lector.IsClosed)
                        lector.Close();
                    lector = null;
                }
            }
            catch
            {
                // ignorar errores al cerrar lector
            }

            // cerrar y liberar conexion
            try
            {
                if (conexion != null && conexion.State != System.Data.ConnectionState.Closed)
                    conexion.Close();
            }
            catch
            {
                // ignorar errores al cerrar conexion
            }
        }

        // IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    try { if (lector != null && !lector.IsClosed) lector.Close(); } catch { }
                    lector = null;

                    try { if (comando != null) { comando.Dispose(); comando = null; } } catch { }

                    try { if (conexion != null) { if (conexion.State != System.Data.ConnectionState.Closed) conexion.Close(); conexion.Dispose(); conexion = null; } } catch { }
                }

                disposed = true;
            }
        }

        ~AccesoDatos()
        {
            Dispose(false);
        }
    }
}