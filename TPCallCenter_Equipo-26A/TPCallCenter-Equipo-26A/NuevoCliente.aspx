<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NuevoCliente.aspx.cs" Inherits="TPCallCenter_Equipo_26A.NuevoCliente" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Nuevo Cliente</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Nuevo Cliente</h1>
            <asp:Label ID="lblNombre" runat="server" Text="Nombre:"></asp:Label>
            <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lblApellido" runat="server" Text="Apellido:"></asp:Label>
            <asp:TextBox ID="txtApellido" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lblTelefono" runat="server" Text="Teléfono:"></asp:Label>
            <asp:TextBox ID="txtTelefono" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lblDireccion" runat="server" Text="Dirección:"></asp:Label>
            <asp:TextBox ID="txtDireccion" runat="server"></asp:TextBox>
            <br />
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="BtnGuardar_Click" />
        </div>
    </form>
    <a href="Login.aspx">Login.aspx</a>
</body>
</html>