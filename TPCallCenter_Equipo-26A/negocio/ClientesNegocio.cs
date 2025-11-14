using System;
using System.Collections.Generic;
using dominio;

namespace negocio
{
    public class ClientesNegocio
    {
        public List<Clientes> listar()
        {
            List<Clientes> lista = new List<Clientes>();

            using (AccesoDatos datos = new AccesoDatos())
            {
                datos.SetearConsulta("SELECT IDCliente, Nombre, Apellido, Documento, Email, Telefono, Direccion, Activo, fechaAlta FROM Clientes WHERE Activo = 1");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Clientes aux = new Clientes();

                    aux.IDCliente = datos.Lector["IDCliente"] != DBNull.Value ? Convert.ToInt32(datos.Lector["IDCliente"]) : 0;
                    aux.Nombre = datos.Lector["Nombre"] != DBNull.Value ? datos.Lector["Nombre"].ToString() : null;
                    aux.Apellido = datos.Lector["Apellido"] != DBNull.Value ? datos.Lector["Apellido"].ToString() : null;
                    aux.Documento = datos.Lector["Documento"] != DBNull.Value ? datos.Lector["Documento"].ToString() : null;
                    aux.Email = datos.Lector["Email"] != DBNull.Value ? datos.Lector["Email"].ToString() : null;
                    aux.Telefono = datos.Lector["Telefono"] != DBNull.Value ? datos.Lector["Telefono"].ToString() : null;
                    aux.Direccion = datos.Lector["Direccion"] != DBNull.Value ? datos.Lector["Direccion"].ToString() : null;
                    aux.Activo = datos.Lector["Activo"] != DBNull.Value ? Convert.ToBoolean(datos.Lector["Activo"]) : false;
                    aux.fechaAlta = datos.Lector["fechaAlta"] != DBNull.Value ? Convert.ToDateTime(datos.Lector["fechaAlta"]) : DateTime.MinValue;

                    lista.Add(aux);
                }

                datos.CerrarConexion();
            }

            return lista;
        }

        public Clientes obtenerPorId(int id)
        {
            Clientes cliente = null;

            using (AccesoDatos datos = new AccesoDatos())
            {
                datos.SetearConsulta("SELECT IDCliente, Nombre, Apellido, Documento, Email, Telefono, Direccion, Activo, fechaAlta FROM Clientes WHERE IDCliente = @id AND Activo = 1");
                datos.SetearParametro("@id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    cliente = new Clientes();
                    cliente.IDCliente = datos.Lector["IDCliente"] != DBNull.Value ? Convert.ToInt32(datos.Lector["IDCliente"]) : 0;
                    cliente.Nombre = datos.Lector["Nombre"] != DBNull.Value ? datos.Lector["Nombre"].ToString() : null;
                    cliente.Apellido = datos.Lector["Apellido"] != DBNull.Value ? datos.Lector["Apellido"].ToString() : null;
                    cliente.Documento = datos.Lector["Documento"] != DBNull.Value ? datos.Lector["Documento"].ToString() : null;
                    cliente.Email = datos.Lector["Email"] != DBNull.Value ? datos.Lector["Email"].ToString() : null;
                    cliente.Telefono = datos.Lector["Telefono"] != DBNull.Value ? datos.Lector["Telefono"].ToString() : null;
                    cliente.Direccion = datos.Lector["Direccion"] != DBNull.Value ? datos.Lector["Direccion"].ToString() : null;
                    cliente.Activo = datos.Lector["Activo"] != DBNull.Value ? Convert.ToBoolean(datos.Lector["Activo"]) : false;
                    cliente.fechaAlta = datos.Lector["fechaAlta"] != DBNull.Value ? Convert.ToDateTime(datos.Lector["fechaAlta"]) : DateTime.MinValue;
                }

                datos.CerrarConexion();
            }

            return cliente;
        }

        public void agregar(Clientes nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("INSERT INTO Clientes (Nombre, Apellido, Documento, Email, Telefono, Direccion, Activo, FechaAlta) VALUES (@Nombre, @Apellido, @Documento, @Email, @Telefono, @Direccion, @Activo, @FechaAlta)");
                datos.SetearParametro("@Nombre", nuevo.Nombre);
                datos.SetearParametro("@Apellido", nuevo.Apellido);
                datos.SetearParametro("@Documento", nuevo.Documento);
                datos.SetearParametro("@Email", nuevo.Email);
                datos.SetearParametro("@Telefono", nuevo.Telefono);
                datos.SetearParametro("@Direccion", nuevo.Direccion);
                datos.SetearParametro("@Activo", nuevo.Activo);
                datos.SetearParametro("@FechaAlta", nuevo.fechaAlta);
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

        public void modificar(Clientes cliente)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("UPDATE Clientes SET Nombre = @Nombre, Apellido = @Apellido, Documento = @Documento, Email = @Email, Telefono = @Telefono, Direccion = @Direccion, Activo = @Activo WHERE IDCliente = @IDCliente");
                datos.SetearParametro("@Nombre", cliente.Nombre);
                datos.SetearParametro("@Apellido", cliente.Apellido);
                datos.SetearParametro("@Documento", cliente.Documento);
                datos.SetearParametro("@Email", cliente.Email);
                datos.SetearParametro("@Telefono", cliente.Telefono);
                datos.SetearParametro("@Direccion", cliente.Direccion);
                datos.SetearParametro("@Activo", cliente.Activo);
                datos.SetearParametro("@IDCliente", cliente.IDCliente);
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
                datos.SetearConsulta("UPDATE Clientes SET Activo = 0 WHERE IDCliente = @IDCliente");
                datos.SetearParametro("@IDCliente", id);
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