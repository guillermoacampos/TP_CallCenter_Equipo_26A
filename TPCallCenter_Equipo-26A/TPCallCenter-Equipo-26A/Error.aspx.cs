using System;

namespace TPCallCenter_Equipo_26A
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["error"] != null)
            {
                lblError.Text = Session["error"].ToString();
            }
        }
    }
}