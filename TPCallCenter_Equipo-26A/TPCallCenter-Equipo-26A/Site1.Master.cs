using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace TPCallCenter_Equipo_26A
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        private const int PERFIL_TELEFONISTA = 1;
        private const int PERFIL_ADMIN = 2;
        private const int PERFIL_SUPERVISOR = 3;

        protected void Page_Load(object sender, EventArgs e)
        {
            var sessionUser = Session["Usuario"] ?? Session["usuario"];
            bool isLoginPage = Page.AppRelativeVirtualPath != null &&
                               Page.AppRelativeVirtualPath.IndexOf("Login.aspx", StringComparison.OrdinalIgnoreCase) >= 0;

            if (!isLoginPage && sessionUser == null)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            var usuario = sessionUser as dominio.Usuarios;

            // Fallback: si Perfil es null pero existe propiedad IDPerfil en el objeto (dependiendo de tu clase dominio.Usuarios)
            int perfil =
                (usuario?.Perfil?.IDPerfil) ??
                (int?)GetPropertyValue(usuario, "IDPerfil") ??
                -1;

            var sidebar = FindControl("sidebarWrapper") as HtmlGenericControl;
            var phGestion = FindControl("phGestionUsuarios") as PlaceHolder;

            if (sidebar != null)
                sidebar.Visible = usuario != null;

            bool puedeGestionUsuarios = (perfil == PERFIL_ADMIN || perfil == PERFIL_SUPERVISOR);

            if (phGestion != null)
                phGestion.Visible = puedeGestionUsuarios;

            // (Opcional) diagnóstico rápido: descomenta para ver el perfil real
            // var lblDiag = FindControl("lblPerfilDiag") as Label;
            // if (lblDiag != null) lblDiag.Text = "Perfil=" + perfil;
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        // Helper genérico para tratar de leer una propiedad por reflexión si existe (ej. IDPerfil directo)
        private object GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null) return null;
            var prop = obj.GetType().GetProperty(propertyName);
            if (prop == null) return null;
            return prop.GetValue(obj);
        }
    }
}