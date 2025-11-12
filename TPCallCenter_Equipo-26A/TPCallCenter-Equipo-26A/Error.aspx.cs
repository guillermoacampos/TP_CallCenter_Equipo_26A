using System;
using System.Web.UI;

namespace TPCallCenter_Equipo_26A
{
    public partial class Error : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["error"] != null)
            {
                lblError.Text = Session["error"].ToString();
            }
            else
            {
                lblError.Text = "Ha ocurrido un error inesperado.";
            }
        }
    }
}