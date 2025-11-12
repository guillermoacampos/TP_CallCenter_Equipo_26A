using System;
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
            if (Session["usuario"] == null)
            {
                sidebarWrapper.Visible = false;
            }
            else
            {
                sidebarWrapper.Visible = true;
            }
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx", false);
        }
    }
}