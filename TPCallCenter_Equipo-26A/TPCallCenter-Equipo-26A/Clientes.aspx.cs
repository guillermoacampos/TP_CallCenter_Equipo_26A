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
                List<dominio.Clientes> clientes = clientesNegocio?.listar();

                if (clientes == null || clientes.Count == 0)
                {
                    gvClientes.DataSource = null;
                    gvClientes.DataBind();
                    lblMensaje.Text = "No se encontraron clientes.";
                    lblMensaje.CssClass = "alert alert-warning d-block";
                    lblContador.Text = "";
                    return;
                }

                gvClientes.DataSource = clientes;
                gvClientes.DataBind();

                // Actualizar contador
                lblContador.Text = $"Total de clientes: {clientes.Count}";
            }
            catch (Exception ex)
            {
                // Manejo de errores
                lblMensaje.Text = $"Error al cargar clientes: {ex.Message}";
                lblMensaje.CssClass = "alert alert-danger d-block";
                gvClientes.DataSource = null;
                gvClientes.DataBind();
                lblContador.Text = "";
            }
        }
    }
}