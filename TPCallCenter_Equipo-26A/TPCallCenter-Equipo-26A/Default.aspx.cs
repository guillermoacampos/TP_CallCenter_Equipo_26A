using System;
using System.Linq;
using System.Collections.Generic;
using dominio;
using negocio;
using UsuarioDominio = dominio.Usuarios;   // Alias para evitar conflicto con la página Usuarios

namespace TPCallCenter_Equipo_26A
{
    public partial class Default : System.Web.UI.Page
    {
        private IncidenciasNegocio incNeg = new IncidenciasNegocio();
        private ClientesNegocio cliNeg = new ClientesNegocio();
        private UsuariosNegocio usuNeg = new UsuariosNegocio();

        // IDs de estado (ajusta según tu tabla Estados)
        private const int ESTADO_ABIERTO = 1;
        private const int ESTADO_EN_ANALISIS = 2;
        private const int ESTADO_ASIGNADO = 5;
        private const int ESTADO_RESUELTO = 6;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // FORZAR USO DE LA ENTIDAD
                UsuarioDominio usuario = Session["Usuario"] as UsuarioDominio;
                if (usuario == null)
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                lblNombreUsuario.Text = usuario.Nombre;

                int perfil = (usuario.Perfil != null) ? usuario.Perfil.IDPerfil : -1;
                phAdminAcciones.Visible = (perfil == 2 || perfil == 3);

                CargarKPIs(perfil, usuario);
                CargarActividadReciente(perfil, usuario);
            }
        }

        private void CargarKPIs(int perfil, UsuarioDominio usuario)
        {
            try
            {
                var incidencias = incNeg.ObtenerTodas();   // Ajustar si tu método se llama diferente
                var clientes = cliNeg.listar();            // Ajustar al nombre real en tu negocio
                var usuarios = usuNeg.listar();            // Devuelve List<Usuarios> (entidad dominio)

                // Telefonista (ejemplo perfil == 1) ve solo sus incidencias
                if (perfil == 1)
                {
                    incidencias = incidencias
                        .Where(i => i.AsignadoUsuario != null && i.AsignadoUsuario.IDUsuario == usuario.IDUsuario)
                        .ToList();
                }

                lblClientes.Text = clientes.Count.ToString();
                lblIncAbiertas.Text = incidencias.Count(i => i.Estado?.IDEstado == ESTADO_ABIERTO).ToString();
                lblIncAnalisis.Text = incidencias.Count(i => i.Estado?.IDEstado == ESTADO_EN_ANALISIS).ToString();
                lblIncAsignadas.Text = incidencias.Count(i => i.Estado?.IDEstado == ESTADO_ASIGNADO).ToString();
                lblIncResueltas.Text = incidencias.Count(i => i.Estado?.IDEstado == ESTADO_RESUELTO).ToString();
                lblUsuariosActivos.Text = usuarios.Count(u => u.Activo).ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en CargarKPIs: " + ex.Message);
            }
        }

        private void CargarActividadReciente(int perfil, UsuarioDominio usuario)
        {
            try
            {
                var incidencias = incNeg.ObtenerTodas();

                if (perfil == 1)
                {
                    incidencias = incidencias
                        .Where(i => i.AsignadoUsuario != null && i.AsignadoUsuario.IDUsuario == usuario.IDUsuario)
                        .ToList();
                }

                var ultimas = incidencias
                    .OrderByDescending(i => i.FechaAlta)
                    .Take(5)
                    .Select(i => new
                    {
                        i.NumeroReclamo,
                        Descripcion = TrimMax(i.Descripcion, 60),
                        i.FechaAlta
                    }).ToList();

                rptActividad.DataSource = ultimas;
                rptActividad.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en CargarActividadReciente: " + ex.Message);
            }
        }

        private string TrimMax(string texto, int max)
        {
            if (string.IsNullOrWhiteSpace(texto)) return "";
            texto = texto.Trim();
            if (texto.Length <= max) return texto;
            return texto.Substring(0, max - 3) + "...";
        }

        // Acciones rápidas
        protected void btnNuevoIncidente_Click(object sender, EventArgs e) => Response.Redirect("NuevoIncidente.aspx");
        protected void btnVerMisIncidencias_Click(object sender, EventArgs e) => Response.Redirect("GestionIncidentes.aspx?mis=1");
        protected void btnIrClientes_Click(object sender, EventArgs e) => Response.Redirect("Clientes.aspx");
        protected void btnIrUsuarios_Click(object sender, EventArgs e) => Response.Redirect("Usuarios.aspx");
        protected void btnIrReportes_Click(object sender, EventArgs e) => Response.Redirect("Reportes.aspx");
    }
}