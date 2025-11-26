<%@ Page Title="Nuevo Incidente" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="NuevoIncidente.aspx.cs" Inherits="TPCallCenter_Equipo_26A.NuevoIncidente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-title {
            font-size: 2rem;
            color: #343a40;
            margin-bottom: 20px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h1 class="form-title">Crear Nueva Incidencia</h1>

        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" HeaderText="Por favor corrija los siguientes errores:" />

        <div class="mb-3">
            <label for="ddlCliente" class="form-label">Cliente:</label>
            <asp:DropDownList ID="ddlCliente" runat="server" CssClass="form-select" />
        </div>

        <div class="mb-3">
            <label for="ddlTipoIncidencia" class="form-label">Tipo de Incidencia:</label>
            <asp:DropDownList ID="ddlTipoIncidencia" runat="server" CssClass="form-select" />
        </div>

        <div class="mb-3">
            <label for="ddlPrioridad" class="form-label">Prioridad:</label>
            <asp:DropDownList ID="ddlPrioridad" runat="server" CssClass="form-select" />
        </div>

        <div class="mb-3">
            <label for="txtDescripcion" class="form-label">Descripción:</label>
            <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
        </div>

        <asp:Button ID="btnCrear" runat="server" Text="Crear Incidencia" CssClass="btn btn-primary" OnClick="btnCrear_Click" />
    </div>
</asp:Content>