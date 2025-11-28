using System;
using dominio;
using negocio;

namespace TPCallCenter_Equipo_26A
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["LastEmail"] != null)
                    txtEmail.Text = Session["LastEmail"].ToString();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string email = (txtEmail.Text ?? "").Trim();
                string pass = (txtPassword.Text ?? "").Trim();

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
                {
                    ShowError("Ingresá tu email y contraseña.");
                    return;
                }

                UsuariosNegocio usuarioNeg = new UsuariosNegocio();
                dominio.Usuarios user = usuarioNeg.Login(email, pass);

                if (user == null)
                {
                    ShowError("Email o contraseña incorrectos.");
                    return;
                }

                // Si el tipo es dominio.Usuarios, Activo existe; si aun marca error, revisa duplicidad de clase 'Usuarios'
                if (!user.Activo)
                {
                    ShowError("Tu usuario está inactivo. Contacta al administrador.");
                    return;
                }

                Session["Usuario"] = user;
                Session["LastEmail"] = email;

                Response.Redirect("~/Default.aspx");
            }
            catch (Exception ex)
            {
                ShowError("Error al iniciar sesión: " + ex.Message);
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }
    }
}