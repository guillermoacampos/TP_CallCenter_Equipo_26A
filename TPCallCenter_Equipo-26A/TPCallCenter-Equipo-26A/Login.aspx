<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TPCallCenter_Equipo_26A.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Centrado vertical dentro del contenedor del master */
        .login-wrapper {
            width:100%;
            min-height: calc(100vh - 48px);
            display:flex;
            align-items:center;
            justify-content:center;
        }
        .login-card {
            width:100%;
            max-width:380px;
            background:#fff;
            border:1px solid #e2e5e8;
            border-radius:12px;
            padding:24px 26px 28px;
            box-shadow:0 10px 24px rgba(15,50,60,.08);
        }
        .login-card h1 {
            font-size:2rem;
            font-weight:800;
            letter-spacing:.5px;
            color:#0f323c;
            margin:0 0 10px;
        }
        .login-card h5 {
            font-size:1rem;
            font-weight:600;
            margin-bottom:18px;
            color:#0f323c;
        }
        .login-card .form-group { margin-bottom:16px; }
        .login-card .form-label { font-weight:500; margin-bottom:6px; }
        .login-card .btn-primary {
            background:#0d6efd;
            border-color:#0d6efd;
            font-weight:600;
            padding:10px 16px;
            border-radius:6px;
        }
        .login-card .btn-primary:hover {
            background:#0b5ed7;
            border-color:#0b5ed7;
        }
        /* Evitar que el contenido principal del Master agregue demasiado ancho */
        .content-inner { padding:0 !important; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="login-wrapper">
        <div class="login-card">
            <h1>CALL CENTER</h1>
            <h5>Login</h5>

            <div class="form-group">
                <label for="txtEmail" class="form-label">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Ingresa tu email"></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="txtPassword" class="form-label">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Ingresa tu contraseña"></asp:TextBox>
            </div>

            <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-primary btn-block w-100" Text="Login" OnClick="btnLogin_Click" />
        </div>
    </div>
</asp:Content>