using System;
using System.Web.UI;
using negocio;
using dominio;

namespace TPCallCenter_Equipo_26A
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Usuarios usuario = new Usuarios();
            UsuariosNegocio negocio = new UsuariosNegocio();
            try
            {
                usuario.Email = txtEmail.Text;
                usuario.Contraseña = txtPassword.Text;

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