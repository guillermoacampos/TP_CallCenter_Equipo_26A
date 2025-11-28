<%@ Page Title="Reportes" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="TPCallCenter_Equipo_26A.Reportes" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <style>
        .report-section { background:#fff; border:1px solid #e2e5e8; border-radius:10px; padding:18px; margin-bottom:22px; }
        .section-title { font-weight:700; font-size:1.05rem; color:#0f323c; margin-bottom:12px; display:flex; align-items:center; justify-content:space-between; }
        .filters-card { background:#fff; border:1px solid #e2e5e8; border-radius:10px; padding:16px; margin-bottom:24px; }
        .table thead th { background:#f2f5f7; border-bottom:1px solid #d9dee2; }
        .table-sm td, .table-sm th { padding:.45rem .6rem; }
        .state-badge { padding:4px 10px; border-radius:20px; font-size:.70rem; font-weight:600; letter-spacing:.5px; }
        .badge-abierto { background:#007bff1a; color:#0062cc; }
        .badge-asignado { background:#17a2b81a; color:#138496; }
        .badge-analisis { background:#ffca2c33; color:#856404; }
        .badge-resuelto { background:#28a7451a; color:#1e7e34; }
        .badge-cerrado { background:#6c757d1a; color:#495057; }
        .badge-reabierto { background:#dc35451a; color:#c82333; }
        .count-badge { background:#eef2f5; border-radius:5px; padding:4px 8px; font-size:.75rem; font-weight:600; color:#33464e; }
        .gv-empty { text-align:center; padding:18px !important; font-style:italic; color:#5d6b73; }
        @media (max-width: 991.98px) {
            .filters-card .form-group { margin-bottom:8px; }
        }
    </style>
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-3 mb-5">
        <h2 class="mb-3">Reportes de Incidencias</h2>
        <asp:Label ID="lblErrorReportes" runat="server" CssClass="alert alert-danger" Visible="false" />

        <!-- Filtros (sin fechas) -->
        <div class="filters-card">
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Prioridad</label>
                        <asp:DropDownList ID="ddlPrioridad" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Tipo</label>
                        <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Asignado</label>
                        <asp:DropDownList ID="ddlAsignado" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Solo estado</label>
                        <asp:DropDownList ID="ddlSoloEstado" runat="server" CssClass="form-control" />
                    </div>
                </div>
            </div>
            <div class="text-right">
                <asp:Button ID="btnAplicarFiltros" runat="server" CssClass="btn btn-primary" Text="Aplicar filtros" OnClick="btnAplicarFiltros_Click" />
            </div>
        </div>

        <!-- Abierto -->
        <asp:Panel ID="pnlAbierto" runat="server" CssClass="report-section">
            <div class="section-title">
                <span>Estado: Abierto <span class="state-badge badge-abierto">ABIERTO</span></span>
                <asp:Label ID="lblCountAbierto" runat="server" CssClass="count-badge" />
            </div>
            <asp:GridView ID="gvAbierto" runat="server" AutoGenerateColumns="false" CssClass="table table-sm table-hover"
                          EmptyDataText="Sin incidencias en este momento" EmptyDataRowStyle-CssClass="gv-empty">
                <Columns>
                    <asp:BoundField DataField="NumeroReclamo" HeaderText="Nº Reclamo" />
                    <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                    <asp:BoundField DataField="Prioridad" HeaderText="Prioridad" />
                    <asp:BoundField DataField="FechaAlta" HeaderText="Fecha Alta" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    <asp:BoundField DataField="Asignado" HeaderText="Asignado" />
                </Columns>
            </asp:GridView>
        </asp:Panel>

        <!-- Asignado -->
        <asp:Panel ID="pnlAsignado" runat="server" CssClass="report-section">
            <div class="section-title">
                <span>Estado: Asignado <span class="state-badge badge-asignado">ASIGNADO</span></span>
                <asp:Label ID="lblCountAsignado" runat="server" CssClass="count-badge" />
            </div>
            <asp:GridView ID="gvAsignado" runat="server" AutoGenerateColumns="false" CssClass="table table-sm table-hover"
                          EmptyDataText="Sin incidencias en este momento" EmptyDataRowStyle-CssClass="gv-empty">
                <Columns>
                    <asp:BoundField DataField="NumeroReclamo" HeaderText="Nº Reclamo" />
                    <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                    <asp:BoundField DataField="Prioridad" HeaderText="Prioridad" />
                    <asp:BoundField DataField="FechaAlta" HeaderText="Fecha Alta" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    <asp:BoundField DataField="Asignado" HeaderText="Asignado" />
                </Columns>
            </asp:GridView>
        </asp:Panel>

        <!-- En Análisis -->
        <asp:Panel ID="pnlAnalisis" runat="server" CssClass="report-section">
            <div class="section-title">
                <span>Estado: En Análisis <span class="state-badge badge-analisis">ANÁLISIS</span></span>
                <asp:Label ID="lblCountAnalisis" runat="server" CssClass="count-badge" />
            </div>
            <asp:GridView ID="gvAnalisis" runat="server" AutoGenerateColumns="false" CssClass="table table-sm table-hover"
                          EmptyDataText="Sin incidencias en este momento" EmptyDataRowStyle-CssClass="gv-empty">
                <Columns>
                    <asp:BoundField DataField="NumeroReclamo" HeaderText="Nº Reclamo" />
                    <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                    <asp:BoundField DataField="Prioridad" HeaderText="Prioridad" />
                    <asp:BoundField DataField="FechaAlta" HeaderText="Fecha Alta" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    <asp:BoundField DataField="Asignado" HeaderText="Asignado" />
                </Columns>
            </asp:GridView>
        </asp:Panel>

        <!-- Resuelto -->
        <asp:Panel ID="pnlResuelto" runat="server" CssClass="report-section">
            <div class="section-title">
                <span>Estado: Resuelto <span class="state-badge badge-resuelto">RESUELTO</span></span>
                <asp:Label ID="lblCountResuelto" runat="server" CssClass="count-badge" />
            </div>
            <asp:GridView ID="gvResuelto" runat="server" AutoGenerateColumns="false" CssClass="table table-sm table-hover"
                          EmptyDataText="Sin incidencias en este momento" EmptyDataRowStyle-CssClass="gv-empty">
                <Columns>
                    <asp:BoundField DataField="NumeroReclamo" HeaderText="Nº Reclamo" />
                    <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                    <asp:BoundField DataField="Prioridad" HeaderText="Prioridad" />
                    <asp:BoundField DataField="FechaAlta" HeaderText="Fecha Alta" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    <asp:BoundField DataField="Asignado" HeaderText="Asignado" />
                </Columns>
            </asp:GridView>
        </asp:Panel>

        <!-- Cerrado -->
        <asp:Panel ID="pnlCerrado" runat="server" CssClass="report-section">
            <div class="section-title">
                <span>Estado: Cerrado <span class="state-badge badge-cerrado">CERRADO</span></span>
                <asp:Label ID="lblCountCerrado" runat="server" CssClass="count-badge" />
            </div>
            <asp:GridView ID="gvCerrado" runat="server" AutoGenerateColumns="false" CssClass="table table-sm table-hover"
                          EmptyDataText="Sin incidencias en este momento" EmptyDataRowStyle-CssClass="gv-empty">
                <Columns>
                    <asp:BoundField DataField="NumeroReclamo" HeaderText="Nº Reclamo" />
                    <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                    <asp:BoundField DataField="Prioridad" HeaderText="Prioridad" />
                    <asp:BoundField DataField="FechaAlta" HeaderText="Fecha Alta" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    <asp:BoundField DataField="Asignado" HeaderText="Asignado" />
                </Columns>
            </asp:GridView>
        </asp:Panel>

        <!-- Reabierto -->
        <asp:Panel ID="pnlReabierto" runat="server" CssClass="report-section">
            <div class="section-title">
                <span>Estado: Reabierto <span class="state-badge badge-reabierto">REABIERTO</span></span>
                <asp:Label ID="lblCountReabierto" runat="server" CssClass="count-badge" />
            </div>
            <asp:GridView ID="gvReabierto" runat="server" AutoGenerateColumns="false" CssClass="table table-sm table-hover"
                          EmptyDataText="Sin incidencias en este momento" EmptyDataRowStyle-CssClass="gv-empty">
                <Columns>
                    <asp:BoundField DataField="NumeroReclamo" HeaderText="Nº Reclamo" />
                    <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                    <asp:BoundField DataField="Prioridad" HeaderText="Prioridad" />
                    <asp:BoundField DataField="FechaAlta" HeaderText="Fecha Alta" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    <asp:BoundField DataField="Asignado" HeaderText="Asignado" />
                </Columns>
            </asp:GridView>
        </asp:Panel>
    </div>
</asp:Content>