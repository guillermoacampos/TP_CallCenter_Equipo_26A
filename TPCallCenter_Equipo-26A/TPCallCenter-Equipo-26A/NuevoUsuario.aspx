<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NuevoUsuario.aspx.cs" Inherits="TPCallCenter_Equipo_26A.NuevoUsuario" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Nuevo Usuario</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Nuevo Usuario</h1>
            <asp:Label ID="lblNombre" runat="server" Text="Nombre:"></asp:Label>
            <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lblApellido" runat="server" Text="Apellido:"></asp:Label>
            <asp:TextBox ID="txtApellido" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lblContrasena" runat="server" Text="Contraseña:"></asp:Label>
            <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password"></asp:TextBox>
            <br />
            <asp:Label ID="lblPerfil" runat="server" Text="Perfil:"></asp:Label>
            <asp:DropDownList ID="ddlPerfil" runat="server"></asp:DropDownList>
            <br />
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="BtnGuardar_Click" />
        </div>
    </form>
</body>
</html>