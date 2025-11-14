<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="TPCallCenter_Equipo_26A.Error" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Ocurrió un error</h1>
            <p><asp:Label ID="lblError" runat="server" CssClass="text-danger"></asp:Label></p>
            <a href="Clientes.aspx">Volver</a>
        </div>
    </form>
</body>
</html>