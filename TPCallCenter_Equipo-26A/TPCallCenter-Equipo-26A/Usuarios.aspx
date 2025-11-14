<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="TPCallCenter_Equipo_26A.Usuarios" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gestión de Usuarios</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Gestión de Usuarios</h1>
            <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="False" OnRowCommand="GvUsuarios_RowCommand">
                <Columns>
                    <asp:BoundField DataField="IDUsuario" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="Activo" HeaderText="Activo" />
                    <asp:ButtonField ButtonType="Button" CommandName="Editar" Text="Editar" />
                    <asp:ButtonField ButtonType="Button" CommandName="Eliminar" Text="Eliminar" />
                </Columns>
            </asp:GridView>

            <asp:Button ID="btnNuevoUsuario" runat="server" Text="Nuevo Usuario" OnClick="BtnNuevoUsuario_Click" />
        </div>
    </form>
</body>
</html>