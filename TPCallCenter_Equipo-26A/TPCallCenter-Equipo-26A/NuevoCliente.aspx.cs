using System;
using negocio;

namespace TPCallCenter_Equipo_26A
{
    public partial class NuevoCliente : System.Web.UI.Page
    {
        // DECLARACIONES MANUALES DE CONTROLES (evitan crear .designer.cs)
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl titulo;
        protected global::System.Web.UI.WebControls.ValidationSummary ValidationSummary1;
        protected global::System.Web.UI.WebControls.Label lblNombre;
        protected global::System.Web.UI.WebControls.TextBox txtNombre;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator reqNombre;
        protected global::System.Web.UI.WebControls.Label lblApellido;
        protected global::System.Web.UI.WebControls.TextBox txtApellido;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator reqApellido;
        protected global::System.Web.UI.WebControls.Label lblDocumento;
        protected global::System.Web.UI.WebControls.TextBox txtDocumento;
        protected global::System.Web.UI.WebControls.Label lblEmail;
        protected global::System.Web.UI.WebControls.TextBox txtEmail;
        protected global::System.Web.UI.WebControls.Label lblTelefono;
        protected global::System.Web.UI.WebControls.TextBox txtTelefono;
        protected global::System.Web.UI.WebControls.Label lblDireccion;
        protected global::System.Web.UI.WebControls.TextBox txtDireccion;
        protected global::System.Web.UI.WebControls.Button btnGuardar;
        protected global::System.Web.UI.WebControls.Button btnCancelar;

        private ClientesNegocio negocio = new ClientesNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int id;
                    if (int.TryParse(Request.QueryString["id"], out id))
                    {
                        CargarCliente(id);
                        if (titulo != null) titulo.InnerText = "Editar Cliente";
                    }
                    else
                    {
                        Response.Redirect("Clientes.aspx");
                    }
                }
            }
        }

        private void CargarCliente(int id)
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
                Response.Redirect("Clientes.aspx");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                dominio.Clientes cliente = new dominio.Clientes();
                cliente.Nombre = txtNombre.Text.Trim();
                cliente.Apellido = txtApellido.Text.Trim();
                cliente.Documento = txtDocumento.Text.Trim();
                cliente.Email = txtEmail.Text.Trim();
                cliente.Telefono = txtTelefono.Text.Trim();
                cliente.Direccion = txtDireccion.Text.Trim();
                cliente.Activo = true;
                cliente.fechaAlta = DateTime.Now;

                if (ViewState["IDCliente"] != null)
                {
                    cliente.IDCliente = Convert.ToInt32(ViewState["IDCliente"]);
                    negocio.modificar(cliente);
                }
                else
                {
                    negocio.agregar(cliente);
                }

                Response.Redirect("Clientes.aspx");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error al guardar: " + ex.Message.Replace("'", "") + "');</script>");
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Clientes.aspx");
        }
    }
}