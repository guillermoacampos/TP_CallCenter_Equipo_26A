using System;
using System.Web.UI;
using dominio;
using negocio;

namespace TPCallCenter_Equipo_26A
{
    public partial class NuevoIncidente : System.Web.UI.Page
    {
        private ClientesNegocio clientesNegocio = new ClientesNegocio();
        private TiposDeIncidenciaNegocio tiposNegocio = new TiposDeIncidenciaNegocio();
        private PrioridadesNegocio prioridadesNegocio = new PrioridadesNegocio();
        private IncidenciasNegocio incidenciasNegocio = new IncidenciasNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarClientes();
                CargarTiposDeIncidencia();
                CargarPrioridades();
            }
        }

        private void CargarClientes()
        {
            ddlCliente.DataSource = clientesNegocio.listar();
            ddlCliente.DataTextField = "Nombre";
            ddlCliente.DataValueField = "IDCliente";
            ddlCliente.DataBind();
        }

        private void CargarTiposDeIncidencia()
        {
            ddlTipoIncidencia.DataSource = tiposNegocio.listar();
            ddlTipoIncidencia.DataTextField = "Nombre";
            ddlTipoIncidencia.DataValueField = "IDTipoIncidencia";
            ddlTipoIncidencia.DataBind();
        }

        private void CargarPrioridades()
        {
            ddlPrioridad.DataSource = prioridadesNegocio.listar();
            ddlPrioridad.DataTextField = "Nombre";
            ddlPrioridad.DataValueField = "IDPrioridad";
            ddlPrioridad.DataBind();
        }

        protected void btnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                dominio.Usuarios usuarioLogueado = (dominio.Usuarios)Session["Usuario"];
                if (usuarioLogueado == null)
                {
                    throw new InvalidOperationException("No hay un usuario logueado en la sesión.");
                }

                dominio.Incidencias nuevaIncidencia = new dominio.Incidencias
                {
                    Cliente = new dominio.Clientes { IDCliente = int.Parse(ddlCliente.SelectedValue) },
                    TipoIncidencia = new dominio.TiposDeIncidencia { IDTipoIncidencia = int.Parse(ddlTipoIncidencia.SelectedValue) },
                    Prioridad = new dominio.Prioridades { IDPrioridad = int.Parse(ddlPrioridad.SelectedValue) },
                    Descripcion = txtDescripcion.Text,
                    FechaAlta = DateTime.Now,
                    CreadorUsuario = usuarioLogueado
                };

                incidenciasNegocio.CrearIncidencia(nuevaIncidencia, usuarioLogueado.IDUsuario);
                Response.Redirect("GestionIncidentes.aspx");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error al crear la incidencia: " + ex.Message + "');</script>");
            }
        }
    }
}