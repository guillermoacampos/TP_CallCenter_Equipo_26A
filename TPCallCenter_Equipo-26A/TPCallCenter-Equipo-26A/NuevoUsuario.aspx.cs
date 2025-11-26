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
                // 
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
                    // Imprimir el error en la consola del servidor
                    System.Diagnostics.Debug.WriteLine("Error al guardar el usuario: " + ex);
                    throw; 
                }
            }
        }
    }
}