<%@ Page Title="Nuevo Usuario" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="NuevoUsuario.aspx.cs" Inherits="TPCallCenter_Equipo_26A.NuevoUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <h1 class="text-center">Nuevo Usuario</h1>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="text-danger" HeaderText="Por favor corrija los siguientes errores:" />
        <div class="mb-3">
            <label for="txtNombre" class="form-label">Nombre</label>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ingrese el nombre" MaxLength="50"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio." CssClass="text-danger" />
        </div>
        <div class="mb-3">
            <label for="txtApellido" class="form-label">Apellido</label>
            <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" placeholder="Ingrese el apellido" MaxLength="50"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvApellido" runat="server" ControlToValidate="txtApellido" ErrorMessage="El apellido es obligatorio." CssClass="text-danger" />
        </div>
        <div class="mb-3">
            <label for="txtEmail" class="form-label">Email</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Ingrese el email" MaxLength="100"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="El email es obligatorio." CssClass="text-danger" />
            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="El formato del email es inválido." CssClass="text-danger" ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" />
        </div>
        <div class="mb-3">
            <label for="txtContrasena" class="form-label">Contraseña</label>
            <asp:TextBox ID="txtContrasena" runat="server" CssClass="form-control" TextMode="Password" placeholder="Ingrese la contraseña" MaxLength="6"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvContrasena" runat="server" ControlToValidate="txtContrasena" ErrorMessage="La contraseña es obligatoria." CssClass="text-danger" />
            <asp:RegularExpressionValidator ID="revContrasena" runat="server" ControlToValidate="txtContrasena" ErrorMessage="La contraseña debe tener un máximo de 6 caracteres." CssClass="text-danger" ValidationExpression="^.{1,6}$" />
        </div>
        <div class="mb-3">
            <label for="ddlPerfil" class="form-label">Perfil</label>
            <asp:DropDownList ID="ddlPerfil" runat="server" CssClass="form-select">
                <asp:ListItem Value="" Text="Seleccione un perfil" />
                <asp:ListItem Value="1">Telefonista</asp:ListItem>
                <asp:ListItem Value="2">Administrador</asp:ListItem>
                <asp:ListItem Value="3">Supervisor</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvPerfil" runat="server" ControlToValidate="ddlPerfil" InitialValue="" ErrorMessage="Debe seleccionar un perfil." CssClass="text-danger" />
        </div>
        <div class="text-center">
            <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-success" Text="Guardar" OnClick="BtnGuardar_Click" />
            <a href="Usuarios.aspx" class="btn btn-secondary">Cancelar</a>
        </div>
    </div>
</asp:Content>