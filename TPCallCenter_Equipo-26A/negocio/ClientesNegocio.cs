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
                datos.setearConsulta("SELECT IDCliente, Nombre, Apellido, Documento, Email, Telefono, Direccion, Activo, fechaAlta FROM Clientes WHERE Activo = 1");
                datos.ejecutarLectura();

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

                datos.cerrarConexion();
            }

            return lista;
        }

        public Clientes obtenerPorId(int id)
        {
            Clientes cliente = null;

            using (AccesoDatos datos = new AccesoDatos())
            {
                datos.setearConsulta("SELECT IDCliente, Nombre, Apellido, Documento, Email, Telefono, Direccion, Activo, fechaAlta FROM Clientes WHERE IDCliente = @id AND Activo = 1");
                datos.setearParametro("@id", id);
                datos.ejecutarLectura();

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

                datos.cerrarConexion();
            }

            return cliente;
        }

        public void agregar(Clientes nuevo)
        {
            using (AccesoDatos datos = new AccesoDatos())
            {
                datos.setearConsulta("INSERT INTO Clientes (Nombre, Apellido, Documento, Email, Telefono, Direccion, Activo, fechaAlta) VALUES (@Nombre, @Apellido, @Documento, @Email, @Telefono, @Direccion, @Activo, @fechaAlta)");
                datos.setearParametro("@Nombre", string.IsNullOrWhiteSpace(nuevo.Nombre) ? null : nuevo.Nombre);
                datos.setearParametro("@Apellido", string.IsNullOrWhiteSpace(nuevo.Apellido) ? null : nuevo.Apellido);
                datos.setearParametro("@Documento", string.IsNullOrWhiteSpace(nuevo.Documento) ? null : nuevo.Documento);
                datos.setearParametro("@Email", string.IsNullOrWhiteSpace(nuevo.Email) ? null : nuevo.Email);
                datos.setearParametro("@Telefono", string.IsNullOrWhiteSpace(nuevo.Telefono) ? null : nuevo.Telefono);
                datos.setearParametro("@Direccion", string.IsNullOrWhiteSpace(nuevo.Direccion) ? null : nuevo.Direccion);
                datos.setearParametro("@Activo", nuevo.Activo);
                datos.setearParametro("@fechaAlta", nuevo.fechaAlta == DateTime.MinValue ? DateTime.Now : nuevo.fechaAlta);

                datos.ejecutarAccion();
                datos.cerrarConexion();
            }
        }

        public void modificar(Clientes cliente)
        {
            using (AccesoDatos datos = new AccesoDatos())
            {
                datos.setearConsulta("UPDATE Clientes SET Nombre = @Nombre, Apellido = @Apellido, Documento = @Documento, Email = @Email, Telefono = @Telefono, Direccion = @Direccion, Activo = @Activo WHERE IDCliente = @IDCliente");
                datos.setearParametro("@Nombre", string.IsNullOrWhiteSpace(cliente.Nombre) ? null : cliente.Nombre);
                datos.setearParametro("@Apellido", string.IsNullOrWhiteSpace(cliente.Apellido) ? null : cliente.Apellido);
                datos.setearParametro("@Documento", string.IsNullOrWhiteSpace(cliente.Documento) ? null : cliente.Documento);
                datos.setearParametro("@Email", string.IsNullOrWhiteSpace(cliente.Email) ? null : cliente.Email);
                datos.setearParametro("@Telefono", string.IsNullOrWhiteSpace(cliente.Telefono) ? null : cliente.Telefono);
                datos.setearParametro("@Direccion", string.IsNullOrWhiteSpace(cliente.Direccion) ? null : cliente.Direccion);
                datos.setearParametro("@Activo", cliente.Activo);
                datos.setearParametro("@IDCliente", cliente.IDCliente);

                datos.ejecutarAccion();
                datos.cerrarConexion();
            }
        }

        // Baja lógica (coherente con listar() que filtra Activo = 1)
        public void eliminar(int id)
        {
            using (AccesoDatos datos = new AccesoDatos())
            {
                datos.setearConsulta("UPDATE Clientes SET Activo = 0 WHERE IDCliente = @IDCliente");
                datos.setearParametro("@IDCliente", id);
                datos.ejecutarAccion();
                datos.cerrarConexion();
            }
        }
    }
}