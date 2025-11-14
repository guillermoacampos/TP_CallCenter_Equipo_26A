using System;
using negocio;

namespace TPCallCenter_Equipo_26A
{
    public partial class Clientes : System.Web.UI.Page
    {
        private ClientesNegocio negocio = new ClientesNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            var lista = negocio.listar();
            gvClientes.DataSource = lista;
            gvClientes.DataBind();

            if (lblContador != null)
                lblContador.Text = $"Total de clientes: {(lista != null ? lista.Count : 0)}";
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("NuevoCliente.aspx");
        }

        protected void gvClientes_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvClientes.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvClientes_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"NuevoCliente.aspx?id={id}");
            }
            else if (e.CommandName == "Eliminar")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                try
                {
                    negocio.eliminar(id); // baja lógica
                    BindGrid();
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error al eliminar: " + ex.Message.Replace("'", "") + "');</script>");
                }
            }
        }
    }
}