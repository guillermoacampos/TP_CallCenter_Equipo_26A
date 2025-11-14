using System;
using System.Web.UI;

namespace TPCallCenter_Equipo_26A
{
    public partial class NuevoCliente : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // carga de pagina ? 
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Por ahora solo deje el mensaje
                Response.Write("<script>alert('Cliente guardado exitosamente ');</script>");
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error: {ex.Message}');</script>");
            }
        }
    }
}