using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using dominio;          // Asegúrate que apunta a tu proyecto 'dominio'
using negocio;

namespace TPCallCenter_Equipo_26A
{
    public partial class Reportes : Page
    {
        private IncidenciasNegocio incNeg = new IncidenciasNegocio();
        private PrioridadesNegocio prioridadesNeg = new PrioridadesNegocio();
        private TiposDeIncidenciaNegocio tiposNeg = new TiposDeIncidenciaNegocio();
        private UsuariosNegocio usuariosNeg = new UsuariosNegocio();

        // Estados
        private const int ESTADO_ABIERTO = 1;
        private const int ESTADO_EN_ANALISIS = 2;
        private const int ESTADO_CERRADO = 3;
        private const int ESTADO_REABIERTO = 4;
        private const int ESTADO_ASIGNADO = 5;
        private const int ESTADO_RESUELTO = 6;

        // Perfiles (ajusta si tus IDs son otros)
        private const int PERFIL_ADMIN = 2;
        private const int PERFIL_SUPERVISOR = 3;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dominio.Usuarios usuario = Session["Usuario"] as dominio.Usuarios;
                if (usuario == null)
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                // DIAGNÓSTICO: tipo real y si detectamos una clase sin Perfil
                System.Diagnostics.Debug.WriteLine("Tipo de usuario (FullName): " + usuario.GetType().FullName);

                int perfilId = GetPerfilIdSeguro(usuario);

                if (perfilId == -9999)
                {
                    lblErrorReportes.Visible = true;
                    lblErrorReportes.Text = "Error: la clase Usuarios usada aquí no expone la propiedad Perfil. Revisa duplicados.";
                    return;
                }

                if (perfilId != PERFIL_ADMIN && perfilId != PERFIL_SUPERVISOR)
                {
                    lblErrorReportes.Visible = true;
                    lblErrorReportes.Text = "No tienes permisos para ver los reportes.";
                    return;
                }

                CargarFiltros();
                CargarTablas();
            }
        }

        // Intenta obtener IDPerfil. Si la propiedad no existe en el tipo que ve el compilador, esta parte dará error.
        // Para comprobar conflicto, hemos encapsulado la lógica.
        private int GetPerfilIdSeguro(dominio.Usuarios usuario)
        {
            // Si la propiedad Perfil existe en esta definición, compila.
            // Si el compilador sigue diciendo que no existe, verás CS1061 aquí.
            try
            {
                // Si realmente existe la propiedad Perfil e IDPerfil en tu clase dominio.Perfil:
                int id = (usuario.Perfil != null) ? usuario.Perfil.IDPerfil : -1;
                return id;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Excepción intentando acceder a Perfil: " + ex);
                // Devuelvo código especial para mostrar mensaje
                return -9999;
            }
        }

        private void CargarFiltros()
        {
            try
            {
                var prioridades = prioridadesNeg.listar();
                ddlPrioridad.DataSource = prioridades;
                ddlPrioridad.DataTextField = "Nombre";
                ddlPrioridad.DataValueField = "IDPrioridad";
                ddlPrioridad.DataBind();
                ddlPrioridad.Items.Insert(0, new ListItem("Todas", "0"));

                var tipos = tiposNeg.listar();
                ddlTipo.DataSource = tipos;
                ddlTipo.DataTextField = "Nombre";
                ddlTipo.DataValueField = "IDTipoIncidencia";
                ddlTipo.DataBind();
                ddlTipo.Items.Insert(0, new ListItem("Todos", "0"));

                var usuarios = usuariosNeg.listar();
                ddlAsignado.DataSource = usuarios;
                ddlAsignado.DataTextField = "Nombre";
                ddlAsignado.DataValueField = "IDUsuario";
                ddlAsignado.DataBind();
                ddlAsignado.Items.Insert(0, new ListItem("Todos", "0"));

                ddlSoloEstado.Items.Clear();
                ddlSoloEstado.Items.Add(new ListItem("Todos los estados", "0"));
                ddlSoloEstado.Items.Add(new ListItem("Abierto", ESTADO_ABIERTO.ToString()));
                ddlSoloEstado.Items.Add(new ListItem("Asignado", ESTADO_ASIGNADO.ToString()));
                ddlSoloEstado.Items.Add(new ListItem("En Análisis", ESTADO_EN_ANALISIS.ToString()));
                ddlSoloEstado.Items.Add(new ListItem("Resuelto", ESTADO_RESUELTO.ToString()));
                ddlSoloEstado.Items.Add(new ListItem("Cerrado", ESTADO_CERRADO.ToString()));
                ddlSoloEstado.Items.Add(new ListItem("Reabierto", ESTADO_REABIERTO.ToString()));
            }
            catch (Exception ex)
            {
                lblErrorReportes.Visible = true;
                lblErrorReportes.Text = "Error al cargar filtros: " + ex.Message;
            }
        }

        protected void btnAplicarFiltros_Click(object sender, EventArgs e)
        {
            CargarTablas();
        }

        private void CargarTablas()
        {
            try
            {
                var todas = incNeg.ObtenerTodas();

                if (ddlPrioridad.SelectedValue != "0")
                {
                    int idPrioridad = int.Parse(ddlPrioridad.SelectedValue);
                    todas = todas.Where(i => i.Prioridad?.IDPrioridad == idPrioridad).ToList();
                }
                if (ddlTipo.SelectedValue != "0")
                {
                    int idTipo = int.Parse(ddlTipo.SelectedValue);
                    todas = todas.Where(i => i.TipoIncidencia?.IDTipoIncidencia == idTipo).ToList();
                }
                if (ddlAsignado.SelectedValue != "0")
                {
                    int idAsignado = int.Parse(ddlAsignado.SelectedValue);
                    todas = todas.Where(i => i.AsignadoUsuario?.IDUsuario == idAsignado).ToList();
                }

                bool filtroUnEstado = ddlSoloEstado.SelectedValue != "0";
                int estadoSeleccionado = filtroUnEstado ? int.Parse(ddlSoloEstado.SelectedValue) : -1;

                CargarTablaEstado(todas, ESTADO_ABIERTO, pnlAbierto, gvAbierto, lblCountAbierto, filtroUnEstado, estadoSeleccionado);
                CargarTablaEstado(todas, ESTADO_ASIGNADO, pnlAsignado, gvAsignado, lblCountAsignado, filtroUnEstado, estadoSeleccionado);
                CargarTablaEstado(todas, ESTADO_EN_ANALISIS, pnlAnalisis, gvAnalisis, lblCountAnalisis, filtroUnEstado, estadoSeleccionado);
                CargarTablaEstado(todas, ESTADO_RESUELTO, pnlResuelto, gvResuelto, lblCountResuelto, filtroUnEstado, estadoSeleccionado);
                CargarTablaEstado(todas, ESTADO_CERRADO, pnlCerrado, gvCerrado, lblCountCerrado, filtroUnEstado, estadoSeleccionado);
                CargarTablaEstado(todas, ESTADO_REABIERTO, pnlReabierto, gvReabierto, lblCountReabierto, filtroUnEstado, estadoSeleccionado);
            }
            catch (Exception ex)
            {
                lblErrorReportes.Visible = true;
                lblErrorReportes.Text = "Error al cargar reportes: " + ex.Message;
            }
        }

        private void CargarTablaEstado(
            List<Incidencias> origen,
            int idEstado,
            Panel panel,
            GridView grid,
            Label lblCount,
            bool filtroUnEstado,
            int estadoSeleccionado)
        {
            panel.Visible = !filtroUnEstado || estadoSeleccionado == idEstado;
            if (!panel.Visible) return;

            var lista = origen
                .Where(i => i.Estado?.IDEstado == idEstado)
                .OrderByDescending(i => i.FechaAlta)
                .Select(i => new
                {
                    i.NumeroReclamo,
                    Cliente = i.Cliente?.Nombre ?? "",
                    Prioridad = i.Prioridad?.Nombre ?? "",
                    FechaAlta = i.FechaAlta,
                    Descripcion = i.Descripcion ?? "",
                    Asignado = i.AsignadoUsuario?.Nombre ?? ""
                }).ToList();

            grid.DataSource = lista;
            grid.DataBind();
            lblCount.Text = lista.Count.ToString();
        }
    }
}