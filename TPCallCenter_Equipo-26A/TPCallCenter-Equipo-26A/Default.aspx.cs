using System;
using System.Web.UI;

namespace TPCallCenter_Equipo_26A
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Obtener el nombre del usuario desde la sesión
                var usuario = Session["usuario"] as dominio.Usuarios;
                if (usuario != null)
                {
                    lblUsuario.Text = usuario.Nombre;
                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                }
            }
        }
    }
}