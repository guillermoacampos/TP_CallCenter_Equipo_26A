using System;
using System.Linq;
using System.Collections.Generic;
using dominio;
using negocio;
using UsuarioDominio = dominio.Usuarios;

namespace TPCallCenter_Equipo_26A
{
    public partial class GestionIncidentes : System.Web.UI.Page
    {
        private IncidenciasNegocio incNeg = new IncidenciasNegocio();

        // Estados (ajusta si difieren)
        private const int ESTADO_ABIERTO = 1;
        private const int ESTADO_EN_ANALISIS = 2;
        private const int ESTADO_CERRADO = 3;
        private const int ESTADO_REABIERTO = 4;
        private const int ESTADO_ASIGNADO = 5;
        private const int ESTADO_RESUELTO = 6;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var usuario = UsuarioActual();
                if (usuario == null)
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }
                CargarEstados();
                CargarIncidencias();
            }
        }

        private UsuarioDominio UsuarioActual() => Session["Usuario"] as UsuarioDominio;
        private int PerfilActual() => UsuarioActual()?.Perfil?.IDPerfil ?? -1;

        private bool MostrarSoloMisIncidencias()
        {
            // mis=1 en la query string (botón "Mis incidencias")
            string qs = Request.QueryString["mis"];
            return qs == "1";
        }

        private void CargarEstados()
        {
            ddlFiltroEstado.Items.Clear();
            ddlFiltroEstado.Items.Add(new System.Web.UI.WebControls.ListItem("Todos", "0"));
            ddlFiltroEstado.Items.Add(new System.Web.UI.WebControls.ListItem("Abierto", ESTADO_ABIERTO.ToString()));
            ddlFiltroEstado.Items.Add(new System.Web.UI.WebControls.ListItem("En análisis", ESTADO_EN_ANALISIS.ToString()));
            ddlFiltroEstado.Items.Add(new System.Web.UI.WebControls.ListItem("Asignado", ESTADO_ASIGNADO.ToString()));
            ddlFiltroEstado.Items.Add(new System.Web.UI.WebControls.ListItem("Resuelto", ESTADO_RESUELTO.ToString()));
            ddlFiltroEstado.Items.Add(new System.Web.UI.WebControls.ListItem("Cerrado", ESTADO_CERRADO.ToString()));
            ddlFiltroEstado.Items.Add(new System.Web.UI.WebControls.ListItem("Reabierto", ESTADO_REABIERTO.ToString()));
        }

        private void CargarIncidencias()
        {
            lblError.Visible = false;
            lblMensajeGestion.Visible = false;

            try
            {
                var usuario = UsuarioActual();
                if (usuario == null)
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                var lista = incNeg.ObtenerTodas();

                // Si el perfil es Telefonista (ejemplo perfil 1) siempre ver solo asignadas a él
                if (PerfilActual() == 1)
                {
                    lista = lista.Where(i => i.AsignadoUsuario != null && i.AsignadoUsuario.IDUsuario == usuario.IDUsuario).ToList();
                }
                else if (MostrarSoloMisIncidencias())
                {
                    // Para Supervisor/Admin cuando pulsa "Mis incidencias"
                    lista = lista.Where(i => i.AsignadoUsuario != null && i.AsignadoUsuario.IDUsuario == usuario.IDUsuario).ToList();
                }

                if (ddlFiltroEstado.SelectedValue != "0")
                {
                    int idEstado = int.Parse(ddlFiltroEstado.SelectedValue);
                    lista = lista.Where(i => i.Estado?.IDEstado == idEstado).ToList();
                }

                gvIncidencias.DataSource = lista.Select(i => new
                {
                    i.IDIncidencia,
                    i.NumeroReclamo,
                    ClienteNombre = i.Cliente?.Nombre ?? "",
                    TipoNombre = i.TipoIncidencia?.Nombre ?? "",
                    PrioridadNombre = i.Prioridad?.Nombre ?? "",
                    EstadoDescripcion = i.Estado?.Descripcion ?? "",
                    FechaAlta = i.FechaAlta,
                    Descripcion = i.Descripcion ?? ""
                }).ToList();

                gvIncidencias.DataBind();
                lblTotalIncidencias.Text = "Total incidencias: " + lista.Count;
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar incidencias: " + ex.Message);
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            CargarIncidencias();
        }

        protected void gvIncidencias_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvIncidencias.PageIndex = e.NewPageIndex;
            CargarIncidencias();
        }

        protected void gvIncidencias_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType != System.Web.UI.WebControls.DataControlRowType.DataRow) return;
            int estadoIndex = 5;
            string estado = e.Row.Cells[estadoIndex].Text.Trim().ToLower();
            string baseCss = "estado-badge ";
            switch (estado)
            {
                case "abierto": baseCss += "estado-abierto"; break;
                case "en análisis": baseCss += "estado-analisis"; break;
                case "asignado": baseCss += "estado-asignado"; break;
                case "resuelto": baseCss += "estado-resuelto"; break;
                case "cerrado": baseCss += "estado-cerrado"; break;
                case "reabierto": baseCss += "estado-reabierto"; break;
            }
            e.Row.Cells[estadoIndex].Text = $"<span class='{baseCss}'>{e.Row.Cells[estadoIndex].Text}</span>";

            // Ocultar botones no permitidos al telefonista
            if (PerfilActual() == 1)
            {
                var btnMod = e.Row.FindControl("btnModificar") as System.Web.UI.WebControls.Button;
                var btnRes = e.Row.FindControl("btnResolver") as System.Web.UI.WebControls.Button;
                var btnCer = e.Row.FindControl("btnCerrar") as System.Web.UI.WebControls.Button;
                if (btnMod != null) btnMod.Visible = false;
                if (btnRes != null) btnRes.Visible = false;
                if (btnCer != null) btnCer.Visible = false;
            }
        }

        protected void gvIncidencias_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Ver" && e.CommandName != "Modificar" &&
                e.CommandName != "Resolver" && e.CommandName != "Cerrar")
                return;

            if (!int.TryParse(e.CommandArgument.ToString(), out int index) ||
                index < 0 || index >= gvIncidencias.Rows.Count)
            {
                MostrarError("Índice inválido.");
                return;
            }

            int id = Convert.ToInt32(gvIncidencias.DataKeys[index].Value);

            var incidencia = incNeg.ObtenerIncidenciaPorId(id);
            if (incidencia == null)
            {
                MostrarError("Incidencia no encontrada.");
                return;
            }

            // Seguridad adicional: telefonista solo ve las propias
            var usuario = UsuarioActual();
            if (PerfilActual() == 1 &&
                !(incidencia.AsignadoUsuario != null && incidencia.AsignadoUsuario.IDUsuario == usuario.IDUsuario))
            {
                MostrarError("No tienes permiso para ver esta incidencia.");
                return;
            }

            if (e.CommandName == "Ver")
                MostrarDetalle(incidencia, "ver");
            else if (e.CommandName == "Modificar")
                MostrarDetalle(incidencia, "editar");
            else if (e.CommandName == "Resolver")
                MostrarDetalle(incidencia, "resolver");
            else if (e.CommandName == "Cerrar")
                MostrarDetalle(incidencia, "cerrar");
        }

        private void MostrarDetalle(Incidencias inc, string modo)
        {
            pnlDetalle.Visible = true;
            lblDetalleNumero.Text = inc.NumeroReclamo.ToString();
            lblDetalleCliente.Text = inc.Cliente?.Nombre;
            lblDetalleTipo.Text = inc.TipoIncidencia?.Nombre;
            lblDetallePrioridad.Text = inc.Prioridad?.Nombre;
            lblDetalleEstado.Text = inc.Estado?.Descripcion;
            lblDetalleFechaAlta.Text = inc.FechaAlta.ToString("yyyy-MM-dd");
            lblDetalleDescripcion.Text = inc.Descripcion;
            lblDetalleComentarioResolucion.Text = inc.ComentarioResolucion;
            lblDetalleComentarioCierre.Text = inc.ComentarioCierre;
            lblDetalleCreador.Text = inc.CreadorUsuario?.Nombre;
            lblDetalleAsignado.Text = inc.AsignadoUsuario?.Nombre;

            pnlEditar.Visible = (modo == "editar");
            pnlResolver.Visible = (modo == "resolver");
            pnlCerrar.Visible = (modo == "cerrar");

            hfEditIncidenciaId.Value =
                hfResolverIncidenciaId.Value =
                hfCerrarIncidenciaId.Value = inc.IDIncidencia.ToString();
        }

        protected void btnGuardarEdicion_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(hfEditIncidenciaId.Value);
                var inc = incNeg.ObtenerIncidenciaPorId(id);
                if (inc == null) throw new Exception("Incidencia no encontrada");
                inc.Descripcion = txtNuevaDescripcion.Text.Trim();
                inc.Estado.IDEstado = ESTADO_EN_ANALISIS;
                incNeg.Actualizar(inc);
                lblEditarOk.Visible = true;
                lblEditarOk.Text = "Actualizada y pasada a 'En análisis'.";
                CargarIncidencias();
            }
            catch (Exception ex)
            {
                lblEditarError.Visible = true;
                lblEditarError.Text = "Error: " + ex.Message;
            }
        }

        protected void btnConfirmarResolucion_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(hfResolverIncidenciaId.Value);
                var inc = incNeg.ObtenerIncidenciaPorId(id);
                if (inc == null) throw new Exception("Incidencia no encontrada");
                if (string.IsNullOrWhiteSpace(txtComentarioResolucion.Text))
                    throw new Exception("Comentario obligatorio.");

                inc.ComentarioResolucion = txtComentarioResolucion.Text.Trim();
                inc.Estado.IDEstado = ESTADO_RESUELTO;
                incNeg.Actualizar(inc);
                lblResolverOk.Visible = true;
                lblResolverOk.Text = "Marcada como Resuelta.";
                CargarIncidencias();
            }
            catch (Exception ex)
            {
                lblResolverError.Visible = true;
                lblResolverError.Text = "Error: " + ex.Message;
            }
        }

        protected void btnConfirmarCierre_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(hfCerrarIncidenciaId.Value);
                var inc = incNeg.ObtenerIncidenciaPorId(id);
                if (inc == null) throw new Exception("Incidencia no encontrada");
                if (string.IsNullOrWhiteSpace(txtComentarioCierre.Text))
                    throw new Exception("Comentario obligatorio.");

                inc.ComentarioCierre = txtComentarioCierre.Text.Trim();
                inc.Estado.IDEstado = ESTADO_CERRADO;
                incNeg.Actualizar(inc);
                lblCerrarOk.Visible = true;
                lblCerrarOk.Text = "Incidencia cerrada.";
                CargarIncidencias();
            }
            catch (Exception ex)
            {
                lblCerrarError.Visible = true;
                lblCerrarError.Text = "Error: " + ex.Message;
            }
        }

        protected void btnVolverDetalle_Click(object sender, EventArgs e)
        {
            pnlDetalle.Visible = false;
            pnlEditar.Visible = false;
            pnlResolver.Visible = false;
            pnlCerrar.Visible = false;
        }

        private void MostrarError(string mensaje)
        {
            lblError.Visible = true;
            lblError.Text = mensaje;
        }
    }
}