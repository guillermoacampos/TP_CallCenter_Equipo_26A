<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="TPCallCenter_Equipo_26A.Error" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container d-flex justify-content-center align-items-center" style="height: 100vh;">
            <div class="card p-4" style="max-width: 500px; width: 100%;">
                <h1 class="text-danger text-center">Ocurrió un error</h1>
                <p class="text-center">
                    <asp:Label ID="lblError" runat="server" CssClass="text-danger"></asp:Label>
                </p>
                <div class="text-center">
                    <a href="Clientes.aspx" class="btn btn-primary">Volver</a>
                </div>
            </div>
        </div>
    </form>
</body>
</html>