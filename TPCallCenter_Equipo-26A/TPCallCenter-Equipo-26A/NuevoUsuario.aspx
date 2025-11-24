<%@ Page Title="Nuevo Usuario" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="NuevoUsuario.aspx.cs" Inherits="TPCallCenter_Equipo_26A.NuevoUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <h1 class="text-center">Nuevo Usuario</h1>
        <div class="mb-3">
            <label for="txtNombre" class="form-label">Nombre</label>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ingrese el nombre"></asp:TextBox>
        </div>
        <div class="mb-3">
            <label for="txtApellido" class="form-label">Apellido</label>
            <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" placeholder="Ingrese el apellido"></asp:TextBox>
        </div>
        <div class="mb-3">
            <label for="txtEmail" class="form-label">Email</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Ingrese el email"></asp:TextBox>
        </div>
        <div class="mb-3">
            <label for="txtContrasena" class="form-label">Contraseña</label>
            <asp:TextBox ID="txtContrasena" runat="server" CssClass="form-control" TextMode="Password" placeholder="Ingrese la contraseña"></asp:TextBox>
        </div>
        <div class="mb-3">
            <label for="ddlPerfil" class="form-label">Perfil</label>
            <asp:DropDownList ID="ddlPerfil" runat="server" CssClass="form-select">
                <asp:ListItem Value="1">Telefonista</asp:ListItem>
                <asp:ListItem Value="2">Administrador</asp:ListItem>
                <asp:ListItem Value="3">Supervisor</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="text-center">
            <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-success" Text="Guardar" OnClick="BtnGuardar_Click" />
            <a href="Usuarios.aspx" class="btn btn-secondary">Cancelar</a>
        </div>
    </div>
</asp:Content>