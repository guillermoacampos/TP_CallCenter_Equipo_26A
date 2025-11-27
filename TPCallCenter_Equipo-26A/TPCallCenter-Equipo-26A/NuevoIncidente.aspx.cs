using System;
using System.Web;
using System.Web.UI;
using dominio;
using negocio;
using System.Diagnostics;

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
            var ds = clientesNegocio.listar();
            ddlCliente.DataSource = ds;
            // Si tu listar devuelve DataTable con Nombre y Apellido separados, ajustar aquí para mostrar Nombre + Apellido
            // Ejemplo: si el DataTable tiene Nombre y Apellido, crear campo NombreCompleto o usar DataTextFormatString en GridView
            ddlCliente.DataTextField = "Nombre";
            ddlCliente.DataValueField = "IDCliente";
            ddlCliente.DataBind();
            ddlCliente.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Seleccione --", "0"));
        }

        private void CargarTiposDeIncidencia()
        {
            var ds = tiposNegocio.listar();
            ddlTipoIncidencia.DataSource = ds;
            ddlTipoIncidencia.DataTextField = "Nombre";
            ddlTipoIncidencia.DataValueField = "IDTipoIncidencia";
            ddlTipoIncidencia.DataBind();
            ddlTipoIncidencia.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Seleccione --", "0"));
        }

        private void CargarPrioridades()
        {
            var ds = prioridadesNegocio.listar();
            ddlPrioridad.DataSource = ds;
            ddlPrioridad.DataTextField = "Nombre";
            ddlPrioridad.DataValueField = "IDPrioridad";
            ddlPrioridad.DataBind();
            ddlPrioridad.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Seleccione --", "0"));
        }

        protected void btnCrear_Click(object sender, EventArgs e)
        {
            // Limpio mensajes anteriores
            lblMensaje.Visible = false;
            lblError.Visible = false;
            lblMensaje.Text = string.Empty;
            lblError.Text = string.Empty;

            try
            {
                // Primer chequeo: sesión
                dominio.Usuarios usuarioLogueado = Session["Usuario"] as dominio.Usuarios;
                if (usuarioLogueado == null)
                {
                    lblError.Visible = true;
                    lblError.Text = "No hay un usuario logueado. Por favor inicie sesión.";
                    return;
                }

                // Validaciones de campos seleccionados
                if (ddlCliente.SelectedValue == "0" || ddlTipoIncidencia.SelectedValue == "0" || ddlPrioridad.SelectedValue == "0")
                {
                    lblError.Visible = true;
                    lblError.Text = "Complete todos los campos obligatorios.";
                    return;
                }

                // Construir incidencia
                dominio.Incidencias nuevaIncidencia = new dominio.Incidencias
                {
                    Cliente = new dominio.Clientes { IDCliente = int.Parse(ddlCliente.SelectedValue) },
                    TipoIncidencia = new dominio.TiposDeIncidencia { IDTipoIncidencia = int.Parse(ddlTipoIncidencia.SelectedValue) },
                    Prioridad = new dominio.Prioridades { IDPrioridad = int.Parse(ddlPrioridad.SelectedValue) },
                    Descripcion = txtDescripcion.Text?.Trim(),
                    FechaAlta = DateTime.Now
                    // no seteamos CreadorUsuario aquí porque el método CrearIncidencia recibe el ID del creador
                };

                // Obtener id del estado "Abierto" a través del negocio si quieres setearlo antes
                // int idEstadoAbierto = incidenciasNegocio.ObtenerIdEstadoPorNombre_Public("Abierto");
                // nuevaIncidencia.IDEstado = idEstadoAbierto; // si tu clase Incidencias tiene la propiedad IDEstado

                // Llamar al negocio para crear la incidencia. Ahora devuelve el NumeroReclamo (identity)
                int numeroReclamo = incidenciasNegocio.CrearIncidencia(nuevaIncidencia, usuarioLogueado.IDUsuario);

                lblMensaje.Visible = true;
                lblMensaje.Text = "Incidencia creada correctamente. Nº reclamo: " + numeroReclamo;

                // Redirigir a la gestión de incidencias para mostrar el listado actualizado
                Response.Redirect("GestionIncidentes.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error en btnCrear_Click: " + ex.ToString());
                lblError.Visible = true;
                lblError.Text = "Error al crear la incidencia: " + HttpUtility.HtmlEncode(ex.Message);
            }
        }
    }
}