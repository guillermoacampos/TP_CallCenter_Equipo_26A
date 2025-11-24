using System;
using System.Web.UI;
using dominio;
using negocio;

namespace TPCallCenter_Equipo_26A
{
    public partial class NuevoUsuario : Page
    {
        private UsuariosNegocio negocio = new UsuariosNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Cargar datos iniciales si es necesario
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    dominio.Usuarios nuevoUsuario = new dominio.Usuarios
                    {
                        Nombre = txtNombre.Text,
                        Apellido = txtApellido.Text,
                        Email = txtEmail.Text,
                        Contrasena = txtContrasena.Text,
                        Perfil = new dominio.Perfil { IDPerfil = int.Parse(ddlPerfil.SelectedValue) },
                        Activo = true,
                        FechaAlta = DateTime.Now
                    };

                    negocio.agregar(nuevoUsuario);
                    Response.Redirect("Usuarios.aspx", false);
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error al guardar el usuario: " + ex.Message.Replace("'", "\\'") + "');</script>");
                }
            }
        }
    }
}