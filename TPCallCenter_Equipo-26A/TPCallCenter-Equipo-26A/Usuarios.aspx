<%@ Page Title="Gestión de Usuarios" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="TPCallCenter_Equipo_26A.Usuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <h1 class="text-center">Gestión de Usuarios</h1>
        <asp:GridView ID="gvUsuarios" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" OnRowCommand="GvUsuarios_RowCommand">
            <Columns>
                <asp:BoundField DataField="IDUsuario" HeaderText="ID" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="Activo" HeaderText="Activo" />
                <asp:ButtonField ButtonType="Button" CommandName="Eliminar" Text="Eliminar" />
            </Columns>
        </asp:GridView>

        <div class="mt-4 text-center">
            <asp:Button ID="btnNuevoUsuario" runat="server" CssClass="btn btn-primary" Text="Agregar Nuevo Usuario" OnClick="BtnNuevoUsuario_Click" />
        </div>
    </div>
</asp:Content>