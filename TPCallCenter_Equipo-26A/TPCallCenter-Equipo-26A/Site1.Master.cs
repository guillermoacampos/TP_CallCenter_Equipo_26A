using System;
using System.Web;
using System.Web.UI;

namespace TPCallCenter_Equipo_26A
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Evitar redirección infinita para la página de login
            if (!Page.AppRelativeVirtualPath.Contains("Login.aspx") && Session["usuario"] == null)
            {
                Response.Redirect("Login.aspx", false);
            }

            // Mostrar el sidebar solo si el usuario está logueado
            sidebarWrapper.Visible = Session["usuario"] != null;

            if (!IsPostBack)
            {
                // Verificar si el usuario tiene permisos de administrador
                var usuario = Session["usuario"] as dominio.Usuarios;
                if (usuario != null && usuario.Perfil.IDPerfil == 2) // IDPerfil 2 = Administrador
                {
                    phGestionUsuarios.Visible = true;
                }
                else
                {
                    phGestionUsuarios.Visible = false;
                }
            }
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx", false);
        }
    }
}