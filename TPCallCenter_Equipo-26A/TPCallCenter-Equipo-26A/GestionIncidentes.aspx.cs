using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using dominio;
using negocio;

namespace TPCallCenter_Equipo_26A
{
    public partial class GestionIncidentes : System.Web.UI.Page
    {
        private IncidenciasNegocio negocio = new IncidenciasNegocio();
        private EstadosNegocio estadosNeg = new EstadosNegocio();
        private UsuariosNegocio usuariosNeg = new UsuariosNegocio();

        private const int PERFIL_TELEFONISTA = 1;
        private const int PERFIL_ADMIN = 2;
        private const int PERFIL_SUPERVISOR = 3;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var usuario = Session["Usuario"] as dominio.Usuarios;
                if (usuario == null)
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                CargarFiltroEstados();
                BindGrid();

                if (!string.IsNullOrEmpty(Request.QueryString["reclamo"]))
                {
                    lblMensajeGestion.Visible = true;
                    lblMensajeGestion.Text = "Incidencia creada correctamente. Nº reclamo: " + Server.HtmlEncode(Request.QueryString["reclamo"]);
                }
            }
        }

        private void CargarFiltroEstados()
        {
            try
            {
                var dt = estadosNeg.listar();
                ddlFiltroEstado.DataSource = dt;
                ddlFiltroEstado.DataTextField = "Descripcion";
                ddlFiltroEstado.DataValueField = "IDEstado";
                ddlFiltroEstado.DataBind();
                ddlFiltroEstado.Items.Insert(0, new ListItem("Todos", "0"));
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error al cargar estados: " + ex.Message;
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            try
            {
                var usuario = Session["Usuario"] as dominio.Usuarios;
                if (usuario == null) { Response.Redirect("~/Login.aspx"); return; }

                List<Incidencias> lista = negocio.ObtenerTodas();

                int perfil = usuario.Perfil?.IDPerfil ?? -1;
                if (perfil == PERFIL_TELEFONISTA)
                {
                    // Telefonista ve solo las incidencias asignadas a él
                    lista = lista.Where(i => i.AsignadoUsuario != null && i.AsignadoUsuario.IDUsuario == usuario.IDUsuario).ToList();
                }
                // Admin y Supervisor ven todo

                if (ddlFiltroEstado.SelectedValue != "0")
                {
                    int idEstado = Convert.ToInt32(ddlFiltroEstado.SelectedValue);
                    lista = lista.Where(i => i.Estado != null && i.Estado.IDEstado == idEstado).ToList();
                }

                var data = lista.Select(i => new
                {
                    i.IDIncidencia,
                    NumeroReclamo = i.NumeroReclamo,
                    ClienteNombre = i.Cliente?.Nombre ?? "",
                    TipoNombre = i.TipoIncidencia?.Nombre ?? "",
                    PrioridadNombre = i.Prioridad?.Nombre ?? "",
                    EstadoDescripcion = i.Estado?.Descripcion ?? "",
                    FechaAlta = i.FechaAlta,
                    Descripcion = i.Descripcion ?? ""
                }).ToList();

                gvIncidencias.DataSource = data;
                gvIncidencias.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error al cargar incidencias: " + ex.Message;
            }
        }

        protected void gvIncidencias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvIncidencias.Rows[index];

            int idIncidencia;
            if (gvIncidencias.DataKeys != null && gvIncidencias.DataKeys.Count > index && gvIncidencias.DataKeys[index] != null)
                idIncidencia = Convert.ToInt32(gvIncidencias.DataKeys[index].Value);
            else
                idIncidencia = Convert.ToInt32(row.Cells[0].Text);

            var usuarioLogueado = Session["Usuario"] as dominio.Usuarios;
            if (usuarioLogueado == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            try
            {
                switch (e.CommandName)
                {
                    case "Resolver":
                        negocio.ResolverIncidencia(idIncidencia, "Resolución automática.", usuarioLogueado);
                        BindGrid();
                        break;
                    case "Cerrar":
                        negocio.CerrarIncidencia(idIncidencia, "Cierre automático.", usuarioLogueado);
                        BindGrid();
                        break;
                    case "Ver":
                        MostrarDetallePanel(idIncidencia);
                        break;
                    case "Reasignar":
                        // Solo supervisor puede abrir la reasignación
                        if (usuarioLogueado.Perfil?.IDPerfil == PERFIL_SUPERVISOR)
                        {
                            MostrarDetallePanel(idIncidencia, cargarUsuarios: true, habilitarReasignar: true);
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No tienes permiso para reasignar incidencias.";
                        }
                        break;
                }
            }
            catch (UnauthorizedAccessException uex)
            {
                lblError.Visible = true;
                lblError.Text = uex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error al procesar la acción: " + ex.Message;
            }
        }

        protected void gvIncidencias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            var usuario = Session["Usuario"] as dominio.Usuarios;
            int perfil = usuario?.Perfil?.IDPerfil ?? -1;

            var btnResolver = e.Row.FindControl("btnResolver") as Button;
            var btnCerrar = e.Row.FindControl("btnCerrar") as Button;
            var btnReasignar = e.Row.FindControl("btnReasignarRow") as Button;

            // Telefonista: no ve resolver/cerrar ni reasignar
            if (perfil == PERFIL_TELEFONISTA)
            {
                if (btnResolver != null) btnResolver.Visible = false;
                if (btnCerrar != null) btnCerrar.Visible = false;
                if (btnReasignar != null) btnReasignar.Visible = false;
            }
            else
            {
                // Admin: ve resolver/cerrar, NO ve reasignar
                if (perfil == PERFIL_ADMIN)
                {
                    if (btnResolver != null) btnResolver.Visible = true;
                    if (btnCerrar != null) btnCerrar.Visible = true;
                    if (btnReasignar != null) btnReasignar.Visible = false;
                }
                // Supervisor: ve todo incluyendo reasignar
                if (perfil == PERFIL_SUPERVISOR)
                {
                    if (btnResolver != null) btnResolver.Visible = true;
                    if (btnCerrar != null) btnCerrar.Visible = true;
                    if (btnReasignar != null) btnReasignar.Visible = true;
                }
            }
        }

        private void MostrarDetallePanel(int idIncidencia, bool cargarUsuarios = false, bool habilitarReasignar = false)
        {
            try
            {
                var usuario = Session["Usuario"] as dominio.Usuarios;
                var inc = negocio.ObtenerIncidenciaPorId(idIncidencia);
                if (inc == null)
                {
                    pnlDetalle.Visible = true;
                    lblDetalleError.Visible = true;
                    lblDetalleError.Text = "Incidencia no encontrada.";
                    return;
                }

                int perfil = usuario?.Perfil?.IDPerfil ?? -1;
                if (perfil == PERFIL_TELEFONISTA &&
                    (inc.AsignadoUsuario == null || inc.AsignadoUsuario.IDUsuario != usuario.IDUsuario))
                {
                    pnlDetalle.Visible = false;
                    lblError.Visible = true;
                    lblError.Text = "No tienes permiso para ver esta incidencia.";
                    return;
                }

                pnlDetalle.Visible = true;
                lblDetalleError.Visible = false;

                lblDetalleNumero.Text = inc.NumeroReclamo.ToString();
                lblDetalleCliente.Text = inc.Cliente?.Nombre ?? "";
                lblDetalleTipo.Text = inc.TipoIncidencia?.Nombre ?? "";
                lblDetallePrioridad.Text = inc.Prioridad?.Nombre ?? "";
                lblDetalleEstado.Text = inc.Estado?.Descripcion ?? "";
                lblDetalleFechaAlta.Text = inc.FechaAlta == DateTime.MinValue ? "" : inc.FechaAlta.ToString("yyyy-MM-dd");
                lblDetalleDescripcion.Text = inc.Descripcion ?? "";
                lblDetalleCreador.Text = inc.CreadorUsuario?.Nombre ?? "";
                lblDetalleAsignado.Text = inc.AsignadoUsuario?.Nombre ?? "";

                pnlReasignar.Visible = (habilitarReasignar && perfil == PERFIL_SUPERVISOR);

                if (pnlReasignar.Visible && cargarUsuarios)
                {
                    hfIncidenciaId.Value = inc.IDIncidencia.ToString();
                    CargarUsuariosAsignables(inc.AsignadoUsuario?.IDUsuario);
                }
            }
            catch (Exception ex)
            {
                pnlDetalle.Visible = true;
                lblDetalleError.Visible = true;
                lblDetalleError.Text = "Error al cargar detalle: " + ex.Message;
            }
        }

        private void CargarUsuariosAsignables(int? idSeleccionado)
        {
            try
            {
                var listaUsuarios = usuariosNeg.listar(); // Ajustar si hay método específico
                ddlUsuarios.DataSource = listaUsuarios;
                ddlUsuarios.DataTextField = "Nombre";
                ddlUsuarios.DataValueField = "IDUsuario";
                ddlUsuarios.DataBind();

                if (idSeleccionado.HasValue)
                {
                    var item = ddlUsuarios.Items.FindByValue(idSeleccionado.Value.ToString());
                    if (item != null) ddlUsuarios.SelectedValue = item.Value;
                }
            }
            catch (Exception ex)
            {
                lblReasignarError.Visible = true;
                lblReasignarError.Text = "Error al cargar usuarios: " + ex.Message;
            }
        }

        protected void btnReasignar_Click(object sender, EventArgs e)
        {
            var usuario = Session["Usuario"] as dominio.Usuarios;
            if (usuario?.Perfil?.IDPerfil != PERFIL_SUPERVISOR)
            {
                lblReasignarError.Visible = true;
                lblReasignarError.Text = "No tienes permiso para reasignar.";
                return;
            }

            try
            {
                int idIncidencia = int.Parse(hfIncidenciaId.Value);
                int nuevoUsuarioId = int.Parse(ddlUsuarios.SelectedValue);

                negocio.ReasignarIncidencia(idIncidencia, nuevoUsuarioId, usuario); // Implementar este método

                lblReasignarOk.Visible = true;
                lblReasignarOk.Text = "Incidencia reasignada correctamente.";
                lblReasignarError.Visible = false;

                // Refrescar detalle para mostrar nuevo asignado
                MostrarDetallePanel(idIncidencia);
                BindGrid();
            }
            catch (Exception ex)
            {
                lblReasignarOk.Visible = false;
                lblReasignarError.Visible = true;
                lblReasignarError.Text = "Error al reasignar: " + ex.Message;
            }
        }

        protected void btnVolverDetalle_Click(object sender, EventArgs e)
        {
            pnlDetalle.Visible = false;
            pnlReasignar.Visible = false;
            BindGrid();
        }
    }
}