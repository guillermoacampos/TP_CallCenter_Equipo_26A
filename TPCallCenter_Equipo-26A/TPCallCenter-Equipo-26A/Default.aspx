<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TPCallCenter_Equipo_26A.Default" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <style>
        .welcome-title {
            font-size:2rem;
            font-weight:800;
            margin:0 0 4px;
            color:#fff;
            text-shadow:0 2px 4px rgba(0,0,0,0.35);
        }
        .welcome-sub {
            color:#e2ecef;
            font-size:.95rem;
            margin-bottom:18px;
            text-shadow:0 1px 3px rgba(0,0,0,0.4);
        }
        .card-soft.inverted {
            background:rgba(255,255,255,0.10);
            border:1px solid rgba(255,255,255,0.25);
            color:#fff;
        }
        .card-soft.inverted h3 { color:#fff; }
        .activity-list { list-style:none; margin:0; padding:0; }
        .activity-list li {
            padding:10px 12px;
            border:1px solid #e2e6ea;
            background:#fff;
            border-radius:10px;
            margin-bottom:10px;
            font-size:.9rem;
            display:flex; justify-content:space-between; align-items:center;
        }
        .activity-list li .small { color:#5d6b73; }
    </style>
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div class="card-soft inverted">
            <h1 class="welcome-title">Bienvenido: <asp:Label ID="lblNombreUsuario" runat="server" /></h1>
            <p class="welcome-sub">Panel de inicio rápido del sistema de gestión de incidencias.</p>

            <div class="quick-actions">
                <asp:Button ID="btnNuevoIncidente" runat="server" CssClass="btn btn-primary" Text="Nuevo incidente" OnClick="btnNuevoIncidente_Click" />
                <asp:Button ID="btnVerMisIncidencias" runat="server" CssClass="btn btn-outline-light" Text="Mis incidencias" OnClick="btnVerMisIncidencias_Click" />
                <asp:Button ID="btnIrClientes" runat="server" CssClass="btn btn-outline-light" Text="Clientes" OnClick="btnIrClientes_Click" />
                <asp:PlaceHolder ID="phAdminAcciones" runat="server">
                    <asp:Button ID="btnIrUsuarios" runat="server" CssClass="btn btn-outline-light" Text="Usuarios" OnClick="btnIrUsuarios_Click" />
                    <asp:Button ID="btnIrReportes" runat="server" CssClass="btn btn-outline-light" Text="Reportes" OnClick="btnIrReportes_Click" />
                </asp:PlaceHolder>
            </div>
        </div>

        <div class="card-soft" style="margin-top:28px;">
            <div class="section-title">Indicadores rápidos</div>
            <div class="kpi-grid">
                <div class="kpi-box">
                    <div class="kpi-label">Clientes</div>
                    <div class="kpi-value"><asp:Label ID="lblClientes" runat="server" /></div>
                </div>
                <div class="kpi-box">
                    <div class="kpi-label">Incidencias abiertas</div>
                    <div class="kpi-value"><asp:Label ID="lblIncAbiertas" runat="server" /></div>
                </div>
                <div class="kpi-box">
                    <div class="kpi-label">En análisis</div>
                    <div class="kpi-value"><asp:Label ID="lblIncAnalisis" runat="server" /></div>
                </div>
                <div class="kpi-box">
                    <div class="kpi-label">Asignadas</div>
                    <div class="kpi-value"><asp:Label ID="lblIncAsignadas" runat="server" /></div>
                </div>
                <div class="kpi-box">
                    <div class="kpi-label">Resueltas sin cerrar</div>
                    <div class="kpi-value"><asp:Label ID="lblIncResueltas" runat="server" /></div>
                </div>
                <div class="kpi-box">
                    <div class="kpi-label">Usuarios activos</div>
                    <div class="kpi-value"><asp:Label ID="lblUsuariosActivos" runat="server" /></div>
                </div>
            </div>
        </div>

        <div class="card-soft" style="margin-top:28px;">
            <div class="section-title">Actividad reciente</div>
            <asp:Repeater ID="rptActividad" runat="server">
                <ItemTemplate>
                    <li>
                        <span><strong>#<%# Eval("NumeroReclamo") %></strong> <%# Eval("Descripcion") %></span>
                        <span class="small"><%# Eval("FechaAlta","{0:yyyy-MM-dd}") %></span>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    <% if (rptActividad.Items.Count == 0) { %>
                        <div class="text-muted">Sin actividad reciente.</div>
                    <% } %>
                </FooterTemplate>
                <HeaderTemplate><ul class="activity-list"></HeaderTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>