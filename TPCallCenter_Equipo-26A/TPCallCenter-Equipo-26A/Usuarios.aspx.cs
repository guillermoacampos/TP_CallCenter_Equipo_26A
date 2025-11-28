using System;
using System.Linq;
using System.Collections.Generic;
using dominio;
using negocio;
using UsuarioDominio = dominio.Usuarios;

namespace TPCallCenter_Equipo_26A
{
    public partial class Usuarios : System.Web.UI.Page
    {
        private UsuariosNegocio usuNeg = new UsuariosNegocio();
        private List<UsuarioDominio> _cache;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarUsuarios();
            }
        }

        private void CargarUsuarios()
        {
            try
            {
                _cache = usuNeg.listar();
                gvUsuarios.DataSource = _cache.Select(u => new
                {
                    u.IDUsuario,
                    u.Nombre,
                    u.Apellido,
                    u.Email,
                    PerfilDescripcion = u.Perfil?.Descripcion ?? "",
                    FechaAlta = u.FechaAlta,
                    Activo = u.Activo ? "Activo" : "Inactivo"
                }).ToList();

                gvUsuarios.DataBind();
                lblTotalUsuarios.Text = _cache.Count.ToString();
            }
            catch (Exception ex)
            {
                lblErrorUsuarios.Visible = true;
                lblErrorUsuarios.Text = "Error al cargar usuarios: " + ex.Message;
            }
        }

        protected void gvUsuarios_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvUsuarios.PageIndex = e.NewPageIndex;
            CargarUsuarios();
        }

        protected void gvUsuarios_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType != System.Web.UI.WebControls.DataControlRowType.DataRow) return;

            // Estado (columna index según orden: ID(0), Nombre(1), Apellido(2), Email(3), Perfil(4), Fecha(5), Estado(6))
            int estadoIndex = 6;
            string estadoTxt = e.Row.Cells[estadoIndex].Text.Trim();
            string css = "badge-estado " + (estadoTxt.Equals("Activo", StringComparison.OrdinalIgnoreCase) ? "badge-activo" : "badge-inactivo");
            e.Row.Cells[estadoIndex].Text = $"<span class='{css}'>{estadoTxt}</span>";
        }

        // Manejo de acciones (Editar / Eliminar) si ya lo tenías:
        protected void gvUsuarios_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            // Implementar según tu lógica
            if (e.CommandName == "Eliminar")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int id = Convert.ToInt32(gvUsuarios.DataKeys[index].Value);
                try
                {
                    usuNeg.eliminar(id, bajaLogica: true);
                    CargarUsuarios();
                    lblOkUsuarios.Visible = true;
                    lblOkUsuarios.Text = "Usuario eliminado (baja lógica).";
                }
                catch (Exception ex)
                {
                    lblErrorUsuarios.Visible = true;
                    lblErrorUsuarios.Text = "Error al eliminar: " + ex.Message;
                }
            }
            else if (e.CommandName == "Editar")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int id = Convert.ToInt32(gvUsuarios.DataKeys[index].Value);
                Response.Redirect("NuevoUsuario.aspx?id=" + id);
            }
        }
    }
}