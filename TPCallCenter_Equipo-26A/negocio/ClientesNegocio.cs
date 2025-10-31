using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class ClientesNegocio
    {
        public List<Clientes> listar()
        {
            List<Clientes> lista = new List<Clientes>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT IDCliente, Nombre, Apellido, Documento, Email, Telefono, Direccion, Activo, fechaAlta FROM Clientes WHERE Activo = 1");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Clientes aux = new Clientes();
                    aux.IDCliente = (int)datos.Lector["IDCliente"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Documento = (string)datos.Lector["Documento"];
                    aux.Email = (string)datos.Lector["Email"];
                    aux.Telefono = (string)datos.Lector["Telefono"];
                    aux.Direccion = (string)datos.Lector["Direccion"];
                    aux.Activo = (bool)datos.Lector["Activo"];
                    aux.fechaAlta = (DateTime)datos.Lector["fechaAlta"];

                    lista.Add(aux);
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

        public Clientes obtenerPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            Clientes cliente = null;

            try
            {
                datos.setearConsulta("SELECT IDCliente, Nombre, Apellido, Documento, Email, Telefono, Direccion, Activo, fechaAlta FROM Clientes WHERE IDCliente = @id AND Activo = 1");
                datos.setearParametro("@id", id);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    cliente = new Clientes();
                    cliente.IDCliente = (int)datos.Lector["IDCliente"];
                    cliente.Nombre = (string)datos.Lector["Nombre"];
                    cliente.Apellido = (string)datos.Lector["Apellido"];
                    cliente.Documento = (string)datos.Lector["Documento"];
                    cliente.Email = (string)datos.Lector["Email"];
                    cliente.Telefono = (string)datos.Lector["Telefono"];
                    cliente.Direccion = (string)datos.Lector["Direccion"];
                    cliente.Activo = (bool)datos.Lector["Activo"];
                    cliente.fechaAlta = (DateTime)datos.Lector["fechaAlta"];
                }

                return cliente;
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