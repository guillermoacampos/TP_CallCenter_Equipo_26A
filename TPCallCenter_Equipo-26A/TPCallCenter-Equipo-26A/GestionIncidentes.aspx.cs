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

        // Perfiles
        private const int PERFIL_TELEFONISTA = 1;
        private const int PERFIL_ADMIN = 2;
        private const int PERFIL_SUPERVISOR = 3;

        // Estados (según tu dominio)
        private const int ESTADO_EN_ANALISIS = 2;
        private const int ESTADO_RESUELTO = 6;
        private const int ESTADO_CERRADO = 3;

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
                if (usuario == null)
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                List<Incidencias> lista = negocio.ObtenerTodas();

                int perfil = usuario.Perfil?.IDPerfil ?? -1;
                if (perfil == PERFIL_TELEFONISTA)
                {
                    // Telefonista solo ve (y por ende solo podrá modificar) sus incidencias asignadas
                    lista = lista.Where(i => i.AsignadoUsuario != null && i.AsignadoUsuario.IDUsuario == usuario.IDUsuario).ToList();
                }

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
                    case "Ver":
                        MostrarDetallePanel(idIncidencia);
                        break;

                    case "Modificar":
                        // Ahora TODOS los perfiles pueden modificar (telefonista solo ve las suyas)
                        if (!PuedeModificar(idIncidencia, usuarioLogueado))
                        {
                            lblError.Visible = true;
                            lblError.Text = "No tienes permiso para modificar esta incidencia.";
                            return;
                        }
                        MostrarDetallePanel(idIncidencia);
                        pnlEditar.Visible = true;
                        hfEditIncidenciaId.Value = idIncidencia.ToString();
                        txtNuevaDescripcion.Text = lblDetalleDescripcion.Text;
                        break;

                    case "Resolver":
                        if (!PuedeAccionarSobre(idIncidencia, usuarioLogueado))
                        {
                            lblError.Visible = true;
                            lblError.Text = "No tienes permiso para resolver esta incidencia.";
                            return;
                        }
                        MostrarDetallePanel(idIncidencia);
                        pnlResolver.Visible = true;
                        hfResolverIncidenciaId.Value = idIncidencia.ToString();
                        lblResolverOk.Visible = false;
                        lblResolverError.Visible = false;
                        break;

                    case "Cerrar":
                        if (!PuedeAccionarSobre(idIncidencia, usuarioLogueado))
                        {
                            lblError.Visible = true;
                            lblError.Text = "No tienes permiso para cerrar esta incidencia.";
                            return;
                        }
                        MostrarDetallePanel(idIncidencia);
                        pnlCerrar.Visible = true;
                        hfCerrarIncidenciaId.Value = idIncidencia.ToString();
                        lblCerrarOk.Visible = false;
                        lblCerrarError.Visible = false;
                        break;
                }
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

            var btnModificar = e.Row.FindControl("btnModificar") as Button;
            var btnResolver = e.Row.FindControl("btnResolver") as Button;
            var btnCerrar = e.Row.FindControl("btnCerrar") as Button;

            // Nueva lógica: TODOS ven Modificar (telefonista ya está filtrado a sus incidencias)
            if (btnModificar != null) btnModificar.Visible = true;

            if (perfil == PERFIL_TELEFONISTA)
            {
                if (btnResolver != null) btnResolver.Visible = true;
                if (btnCerrar != null) btnCerrar.Visible = true;
            }
            else
            {
                if (btnResolver != null) btnResolver.Visible = true;
                if (btnCerrar != null) btnCerrar.Visible = true;
            }
        }

        private void MostrarDetallePanel(int idIncidencia)
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
                lblDetalleComentarioResolucion.Text = inc.ComentarioResolucion ?? "";
                lblDetalleComentarioCierre.Text = inc.ComentarioCierre ?? "";
                lblDetalleCreador.Text = inc.CreadorUsuario?.Nombre ?? "";
                lblDetalleAsignado.Text = inc.AsignadoUsuario?.Nombre ?? "";

                pnlEditar.Visible = false;
                pnlResolver.Visible = false;
                pnlCerrar.Visible = false;
            }
            catch (Exception ex)
            {
                pnlDetalle.Visible = true;
                lblDetalleError.Visible = true;
                lblDetalleError.Text = "Error al cargar detalle: " + ex.Message;
            }
        }

        protected void btnGuardarEdicion_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(hfEditIncidenciaId.Value);
                string nuevaDesc = (txtNuevaDescripcion.Text ?? "").Trim();

                if (string.IsNullOrEmpty(nuevaDesc))
                {
                    lblEditarError.Visible = true;
                    lblEditarError.Text = "La descripción no puede estar vacía.";
                    lblEditarOk.Visible = false;
                    return;
                }

                negocio.ModificarDescripcionYEstado(id, nuevaDesc, ESTADO_EN_ANALISIS);

                lblEditarOk.Visible = true;
                lblEditarOk.Text = "Descripción actualizada y estado cambiado a 'En análisis'.";
                lblEditarError.Visible = false;

                MostrarDetallePanel(id);
                BindGrid();
            }
            catch (Exception ex)
            {
                lblEditarOk.Visible = false;
                lblEditarError.Visible = true;
                lblEditarError.Text = "Error al guardar cambios: " + ex.Message;
            }
        }

        protected void btnConfirmarResolucion_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(hfResolverIncidenciaId.Value);
                string comentario = (txtComentarioResolucion.Text ?? "").Trim();

                if (string.IsNullOrEmpty(comentario))
                {
                    lblResolverError.Visible = true;
                    lblResolverError.Text = "Debes ingresar un comentario de resolución.";
                    lblResolverOk.Visible = false;
                    return;
                }

                var usuarioLogueado = Session["Usuario"] as dominio.Usuarios;
                negocio.ResolverIncidenciaConComentario(id, comentario, usuarioLogueado, ESTADO_RESUELTO);

                lblResolverOk.Visible = true;
                lblResolverOk.Text = "Incidencia resuelta.";
                lblResolverError.Visible = false;

                MostrarDetallePanel(id);
                BindGrid();
            }
            catch (Exception ex)
            {
                lblResolverOk.Visible = false;
                lblResolverError.Visible = true;
                lblResolverError.Text = "Error al resolver: " + ex.Message;
            }
        }

        protected void btnConfirmarCierre_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(hfCerrarIncidenciaId.Value);
                string comentario = (txtComentarioCierre.Text ?? "").Trim();

                if (string.IsNullOrEmpty(comentario))
                {
                    lblCerrarError.Visible = true;
                    lblCerrarError.Text = "Debes ingresar un comentario de cierre.";
                    lblCerrarOk.Visible = false;
                    return;
                }

                var usuarioLogueado = Session["Usuario"] as dominio.Usuarios;
                negocio.CerrarIncidenciaConComentario(id, comentario, usuarioLogueado, ESTADO_CERRADO);

                lblCerrarOk.Visible = true;
                lblCerrarOk.Text = "Incidencia cerrada.";
                lblCerrarError.Visible = false;

                MostrarDetallePanel(id);
                BindGrid();
            }
            catch (Exception ex)
            {
                lblCerrarOk.Visible = false;
                lblCerrarError.Visible = true;
                lblCerrarError.Text = "Error al cerrar: " + ex.Message;
            }
        }

        protected void btnVolverDetalle_Click(object sender, EventArgs e)
        {
            pnlDetalle.Visible = false;
            pnlEditar.Visible = false;
            pnlResolver.Visible = false;
            pnlCerrar.Visible = false;
            BindGrid();
        }

        // ---- Helpers de permisos ----

        // Ahora TODOS pueden modificar; se filtra por visibilidad de la incidencia (telefonista sólo las suyas).
        private bool PuedeModificar(int idIncidencia, dominio.Usuarios u)
        {
            if (u == null) return false;
            int perfil = u.Perfil?.IDPerfil ?? -1;
            if (perfil == PERFIL_ADMIN || perfil == PERFIL_SUPERVISOR) return true;
            if (perfil == PERFIL_TELEFONISTA)
            {
                var inc = negocio.ObtenerIncidenciaPorId(idIncidencia);
                return inc != null && inc.AsignadoUsuario != null && inc.AsignadoUsuario.IDUsuario == u.IDUsuario;
            }
            return false;
        }

        private bool PuedeAccionarSobre(int idIncidencia, dominio.Usuarios u)
        {
            if (u == null) return false;
            int perfil = u.Perfil?.IDPerfil ?? -1;
            if (perfil == PERFIL_ADMIN || perfil == PERFIL_SUPERVISOR) return true;
            if (perfil == PERFIL_TELEFONISTA)
            {
                var inc = negocio.ObtenerIncidenciaPorId(idIncidencia);
                return inc != null && inc.AsignadoUsuario != null && inc.AsignadoUsuario.IDUsuario == u.IDUsuario;
            }
            return false;
        }
    }
}