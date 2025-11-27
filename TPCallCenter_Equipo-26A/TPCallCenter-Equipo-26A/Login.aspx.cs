using System;
using System.Web;
using negocio;

namespace TPCallCenter_Equipo_26A
{
    public partial class Login : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            dominio.Usuarios usuario = new dominio.Usuarios();
            UsuariosNegocio negocio = new UsuariosNegocio();

            try
            {
                usuario.Email = txtEmail.Text?.Trim();
                usuario.Contrasena = txtPassword.Text?.Trim();

                bool ok = negocio.Login(usuario);

                if (ok)
                {
                    // Guardar usuario con la misma clave que usan otras páginas ("Usuario")
                    Session["Usuario"] = usuario;

                    // Redirigir a la página principal
                    Response.Redirect("Default.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
                else
                {
                    // Credenciales inválidas: redirigir a la página de error o mostrar mensaje.
                    Session["error"] = "Usuario o contraseña incorrectos";
                    Response.Redirect("Error.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
            }
            catch (Exception ex)
            {
                // Guardar error y redirigir a Error.aspx
                Session["error"] = ex.Message;
                Response.Redirect("Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }
    }
}