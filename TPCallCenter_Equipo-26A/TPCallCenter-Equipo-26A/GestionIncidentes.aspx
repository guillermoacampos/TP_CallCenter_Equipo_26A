<%@ Page Title="Gestión de Incidentes" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="GestionIncidentes.aspx.cs" Inherits="TPCallCenter_Equipo_26A.GestionIncidentes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .page-title {
            font-size: 2rem;
            color: #343a40;
            margin-bottom: 20px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <h1 class="page-title">Gestión de Incidentes</h1>

        <div class="row mb-3">
            <div class="col-md-6">
                <label for="ddlEstado" class="form-label">Filtrar por Estado:</label>
                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
            <div class="col-md-6 d-flex align-items-end">
                <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-primary" OnClick="btnFiltrar_Click" />
            </div>
        </div>

        <asp:Button ID="btnNuevoIncidente" runat="server" Text="Nuevo Incidente" CssClass="btn btn-success mb-3" PostBackUrl="~/NuevoIncidente.aspx" />

        <asp:GridView ID="gvIncidencias" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" OnRowCommand="gvIncidencias_RowCommand">
            <Columns>
                <asp:BoundField DataField="NumReclamo" HeaderText="N° Reclamo" />
                <asp:BoundField DataField="Cliente.Nombre" HeaderText="Cliente" />
                <asp:BoundField DataField="TipoIncidencia.Nombre" HeaderText="Tipo" />
                <asp:BoundField DataField="Prioridad.Nombre" HeaderText="Prioridad" />
                <asp:BoundField DataField="Estado.Descripcion" HeaderText="Estado" />
                <asp:BoundField DataField="FechaAlta" HeaderText="Fecha Alta" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:ButtonField CommandName="Resolver" Text="Resolver" ButtonType="Button" ItemStyle-CssClass="btn btn-success" />
                <asp:ButtonField CommandName="Cerrar" Text="Cerrar" ButtonType="Button" ItemStyle-CssClass="btn btn-danger" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>