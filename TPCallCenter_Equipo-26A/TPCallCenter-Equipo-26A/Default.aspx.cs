using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPCallCenter_Equipo_26A
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Cargar información inicial
                lblFecha.Text = "Última actualización: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }

        protected void btnReportes_Click(object sender, EventArgs e)
        {
            // Simular carga de reportes
            lblInfo.Text = "Cargando reportes del sistema... (Funcionalidad pendiente)";
            lblInfo.CssClass = "alert alert-warning";
        }

        protected void btnConfig_Click(object sender, EventArgs e)
        {
            // Simular configuración
            lblInfo.Text = "Accediendo a configuración del sistema... (Funcionalidad pendiente)";
            lblInfo.CssClass = "alert alert-info";
        }
    }
}