using System;
using negocio;

namespace TPCallCenter_Equipo_26A
{
    public partial class NuevoCliente : System.Web.UI.Page
    {
        private readonly ClientesNegocio negocio = new ClientesNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Request.QueryString["id"] != null)
                    {
                        if (int.TryParse(Request.QueryString["id"], out int id))
                        {
                            CargarCliente(id);
                        }
                        else
                        {
                            Response.Redirect("Clientes.aspx", false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Session.Add("error", ex.Message);
                    Response.Redirect("Error.aspx", false);
                }
            }
        }

        private void CargarCliente(int id)
        {
            try
            {
                var cliente = negocio.obtenerPorId(id);
                if (cliente != null)
                {
                    txtNombre.Text = cliente.Nombre;
                    txtApellido.Text = cliente.Apellido;
                    txtDocumento.Text = cliente.Documento;
                    txtEmail.Text = cliente.Email;
                    txtTelefono.Text = cliente.Telefono;
                    txtDireccion.Text = cliente.Direccion;
                    ViewState["IDCliente"] = cliente.IDCliente;
                }
                else
                {
                    Response.Redirect("Clientes.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                {
                    Response.Write("<script>alert('Por favor, complete todos los campos requeridos.');</script>");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDocumento.Text))
                {
                    Response.Write("<script>alert('El campo Documento es obligatorio.');</script>");
                    return;
                }

                if (txtDocumento.Text.Length > 8)
                {
                    Response.Write("<script>alert('El campo Documento no puede tener más de 8 caracteres.');</script>");
                    return;
                }

                dominio.Clientes cliente = new dominio.Clientes
                {
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Documento = txtDocumento.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    Direccion = txtDireccion.Text.Trim(),
                    Activo = true,
                    fechaAlta = DateTime.Now
                };

                if (ViewState["IDCliente"] != null)
                {
                    cliente.IDCliente = Convert.ToInt32(ViewState["IDCliente"]);
                    negocio.modificar(cliente);
                }
                else
                {
                    negocio.agregar(cliente);
                }

                Response.Redirect("Clientes.aspx", false);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Clientes.aspx", false);
        }
    }
}