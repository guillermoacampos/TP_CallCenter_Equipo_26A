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
        <asp:Button ID="btnNuevoIncidente" runat="server" Text="Nuevo Incidente" CssClass="btn btn-primary" PostBackUrl="~/NuevoIncidente.aspx" />
    </div>
</asp:Content>