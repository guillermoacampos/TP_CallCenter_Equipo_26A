using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using negocio;
using dominio;

namespace TPCallCenter_Equipo_26A
{
    public partial class Usuarios : Page
    {
        private UsuariosNegocio negocio = new UsuariosNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarUsuarios();
            }
        }

        private void CargarUsuarios()
        {
            gvUsuarios.DataSource = negocio.listar();
            gvUsuarios.DataBind();
        }

        protected void GvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int idUsuario = Convert.ToInt32(e.CommandArgument);
                negocio.eliminar(idUsuario);
                CargarUsuarios();
            }
        }

        protected void BtnNuevoUsuario_Click(object sender, EventArgs e)
        {
            Response.Redirect("NuevoUsuario.aspx");
        }
    }
}