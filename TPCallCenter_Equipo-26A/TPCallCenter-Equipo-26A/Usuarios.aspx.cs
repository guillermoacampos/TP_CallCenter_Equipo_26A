using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPCallCenter_Equipo_26A
{
    public partial class Usuarios : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // aca carga de usuarios
        }

        protected void GvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // aca van los comandos de la grilla
        }

        protected void BtnNuevoUsuario_Click(object sender, EventArgs e)
        {
            Response.Redirect("NuevoUsuario.aspx");
        }
    }
}