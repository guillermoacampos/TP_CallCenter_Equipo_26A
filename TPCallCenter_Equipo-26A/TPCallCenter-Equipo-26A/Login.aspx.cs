using System;
using System.Web.UI;
using dominio;
using negocio;

namespace TPCallCenter_Equipo_26A
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Evitar que los usuarios logueados vuelvan al login
            if (Session["usuario"] != null)
            {
                Response.Redirect("Default.aspx", false);
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            dominio.Usuarios usuario = new dominio.Usuarios();
            UsuariosNegocio negocio = new UsuariosNegocio();
            try
            {
                usuario.Email = txtEmail.Text;
                usuario.Contrasena = txtPassword.Text;

                if (negocio.Login(usuario)) // Método para validar credenciales
                {
                    Session.Add("usuario", usuario);
                    Response.Redirect("Default.aspx", false);
                }
                else
                {
                    Session.Add("error", "Usuario o contraseña incorrectos");
                    Response.Redirect("Error.aspx");
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx");
            }
        }
    }
}