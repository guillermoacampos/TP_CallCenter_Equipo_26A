using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using negocio;
using dominio;

namespace TPCallCenter_Equipo_26A
{
    public partial class Clientes : System.Web.UI.Page
    {
        private ClientesNegocio clientesNegocio = new ClientesNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Cargar clientes al inicio
                CargarClientes();
            }
        }

        protected void btnCargarClientes_Click(object sender, EventArgs e)
        {
            CargarClientes();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            gvClientes.DataSource = null;
            gvClientes.DataBind();
            lblMensaje.Text = "Lista de clientes limpiada";
            lblMensaje.CssClass = "alert alert-warning d-block";
            lblContador.Text = "";
        }

        protected void btnNuevoCliente_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = "Funcionalidad 'Nuevo Cliente' pendiente de implementación";
            lblMensaje.CssClass = "alert alert-info d-block";
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = "Exportando datos... (Funcionalidad pendiente)";
            lblMensaje.CssClass = "alert alert-info d-block";
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string idCliente = btn.CommandArgument;
            lblMensaje.Text = $"Mostrando detalles del cliente ID: {idCliente}";
            lblMensaje.CssClass = "alert alert-info d-block";
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string idCliente = btn.CommandArgument;
            lblMensaje.Text = $"Editando cliente ID: {idCliente} (funcionalidad pendiente)";
            lblMensaje.CssClass = "alert alert-info d-block";
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string idCliente = btn.CommandArgument;
            lblMensaje.Text = $"Cliente ID: {idCliente} eliminado (simulado)";
            lblMensaje.CssClass = "alert alert-danger d-block";
            
            // Recargar la lista
            CargarClientes();
        }

        private void CargarClientes()
        {
            try
            {
                // Usar el namespace completo para evitar conflictos
                List<dominio.Clientes> clientes = clientesNegocio.listar();

                gvClientes.DataSource = clientes;
                gvClientes.DataBind();
                
                // Actualizar mensaje y contador
                lblMensaje.Text = $"Clientes cargados correctamente desde la base de datos - {DateTime.Now:HH:mm:ss}";
                lblMensaje.CssClass = "alert alert-success d-block";
                lblContador.Text = $"Total de clientes: {clientes.Count}";
            }
            catch (Exception ex)
            {
                // Si hay error, mostrar mensaje y cargar datos de ejemplo
                lblMensaje.Text = $"Error al cargar desde BD: {ex.Message}. Mostrando datos de ejemplo.";
                lblMensaje.CssClass = "alert alert-warning d-block";
                
                CargarDatosEjemplo();
            }
        }

        private void CargarDatosEjemplo()
        {
            // Datos de ejemplo si no funciona la BD
            var clientesEjemplo = new List<object>
            {
                new {
                    IDCliente = 1,
                    Nombre = "Juan",
                    Apellido = "Pérez",
                    Documento = "12345678",
                    Email = "juan@email.com",
                    Telefono = "1234567890",
                    Direccion = "Av. Siempre Viva 123",
                    fechaAlta = DateTime.Now.AddDays(-30),
                    Activo = true
                },
                new {
                    IDCliente = 2,
                    Nombre = "María",
                    Apellido = "González",
                    Documento = "87654321",
                    Email = "maria@email.com",
                    Telefono = "0987654321",
                    Direccion = "Calle Falsa 456",
                    fechaAlta = DateTime.Now.AddDays(-15),
                    Activo = true
                }
            };

            gvClientes.DataSource = clientesEjemplo;
            gvClientes.DataBind();
            lblContador.Text = $"Total de clientes (ejemplo): {clientesEjemplo.Count}";
        }
    }
}