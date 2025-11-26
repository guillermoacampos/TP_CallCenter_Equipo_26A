using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using dominio;
using negocio;

namespace TPCallCenter_Equipo_26A
{
    public partial class GestionIncidentes : System.Web.UI.Page
    {
        private IncidenciasNegocio negocio = new IncidenciasNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            // verificar usuario loggeado 
            if (Session["Usuario"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                CargarEstados();
                CargarIncidencias();
            }
        }

        private void CargarEstados()
        {
            EstadosNegocio estadosNegocio = new EstadosNegocio();
            ddlEstado.DataSource = estadosNegocio.listar();
            ddlEstado.DataTextField = "Descripcion";
            ddlEstado.DataValueField = "IDEstado";
            ddlEstado.DataBind();
            ddlEstado.Items.Insert(0, new ListItem("Todos", "0"));
        }

        private void CargarIncidencias()
        {
            List<dominio.Incidencias> lista = negocio.ObtenerTodas();
            gvIncidencias.DataSource = lista;
            gvIncidencias.DataBind();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            int idEstado = int.Parse(ddlEstado.SelectedValue);
            List<dominio.Incidencias> lista = idEstado == 0 ? negocio.ObtenerTodas() : negocio.ObtenerPorEstado(idEstado);
            gvIncidencias.DataSource = lista;
            gvIncidencias.DataBind();
        }

        protected void gvIncidencias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvIncidencias.Rows[index];
            int idIncidencia = Convert.ToInt32(row.Cells[0].Text);

            if (e.CommandName == "Resolver")
            {
                negocio.ResolverIncidencia(idIncidencia, "Resolución automática.");
                CargarIncidencias();
            }
            else if (e.CommandName == "Cerrar")
            {
                negocio.CerrarIncidencia(idIncidencia, "Cierre automático.");
                CargarIncidencias();
            }
        }

        protected void btnEnviarCorreo_Click(object sender, EventArgs e)
        {
            try
            {
                EmailService emailService = new EmailService();
                string emailDestino = "nicole.burgos@alumnos.utn.frgp.edu.ar";
                string asunto = "Correo de Prueba";
                string cuerpo = "Este es un correo de prueba enviado desde la página de gestión de incidentes.";

                emailService.armarCorreo(emailDestino, asunto, cuerpo);
                emailService.enviarEmail();

                // Mostrar mensaje de éxito
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Correo enviado exitosamente.');", true);
            }
            catch (Exception ex)
            {
                // Escapar caracteres especiales en el mensaje de error
                string mensajeError = ex.Message.Replace("'", "\'").Replace("\n", "\\n").Replace("\r", "\\r");
                string detallesError = ex.StackTrace.Replace("'", "\'").Replace("\n", "\\n").Replace("\r", "\\r");

                // Mostrar mensaje de error con detalles escapados
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Error al enviar el correo: {mensajeError}\\nDetalles: {detallesError}');", true);
            }
        }
    }
}