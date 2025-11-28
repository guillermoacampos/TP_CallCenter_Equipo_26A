<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TPCallCenter_Equipo_26A.Login" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Ingresar - TP Call Center</title>

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css" crossorigin="anonymous" />

    <style>
        :root {
            --brand-bg: #0f323c;
            --brand-accent: #1d4e58;
            --primary: #0d6efd;
            --danger: #dc3545;
        }
        html, body { height: 100%; }
        body {
            margin: 0;
            font-family: "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif;
            color: #0f323c;
            background-color: #0b1f24;
        }
        /* Fondo con imagen y overlay: separar propiedades para evitar warnings del parser */
        .login-bg {
            position: fixed; inset: 0;
            background-image: url('https://images.unsplash.com/photo-1525182008055-f88b95ff7980?q=80&w=1600&auto=format&fit=crop');
            background-position: center;
            background-repeat: no-repeat;
            background-size: cover;
            filter: blur(2px);
            transform: scale(1.03);
        }
        .login-overlay {
            position: fixed; inset: 0;
            background: linear-gradient(120deg, rgba(15,50,60,0.75), rgba(15,50,60,0.55));
        }
        .login-wrapper {
            position: relative; z-index: 1;
            min-height: 100%;
            display: grid;
            place-items: center;
            padding: 24px;
        }
        .login-card {
            width: 100%;
            max-width: 400px;
            background: #ffffff;
            border: 1px solid #e5eaee;
            border-radius: 14px;
            box-shadow: 0 14px 34px rgba(0,0,0,0.25);
            overflow: hidden;
        }
        .login-header {
            background: linear-gradient(120deg, var(--brand-bg), var(--brand-accent));
            color: #fff;
            padding: 20px 22px;
        }
        .login-title { margin: 0; font-weight: 800; letter-spacing: .4px; }
        .login-subtitle { margin: 4px 0 0; opacity: .9; }
        .login-body { padding: 18px 18px 6px; }
        .form-label { font-weight: 600; color: #24414a; }
        .form-control { border-radius: 8px; border-color: #d9dee2; }
        .btn-login { width: 100%; border-radius: 8px; padding: 10px 14px; font-weight: 700; }
        .login-footer { padding: 10px 18px 18px; color: #5d6b73; font-size: .9rem; }
        .alert-inline { border-radius: 8px; margin-bottom: 12px; padding: 10px 12px; }
        .brand-mini { display: inline-flex; align-items: center; gap: 8px; font-weight: 700; color: #0f323c; }
        .brand-dot { width: 8px; height: 8px; background: var(--primary); border-radius: 50%; display: inline-block; }
        @media (max-width: 480px) { .login-card { max-width: 92vw; } }
    </style>
</head>
<body>
    <div class="login-bg"></div>
    <div class="login-overlay"></div>

    <form id="form1" runat="server">
        <div class="login-wrapper">
            <div class="login-card">
                <div class="login-header">
                    <h1 class="login-title">Call Center</h1>
                    <p class="login-subtitle">Ingreso al sistema</p>
                </div>
                <div class="login-body">
                    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger alert-inline" Visible="false" />

                    <div class="form-group">
                        <label class="form-label" for="txtEmail">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Placeholder="usuario@empresa.local" />
                    </div>

                    <div class="form-group">
                        <label class="form-label" for="txtPassword">Contraseña</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" Placeholder="••••••••" />
                    </div>

                    <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-primary btn-login" Text="Ingresar" OnClick="btnLogin_Click" />
                </div>
                <div class="login-footer">
                    <span class="brand-mini"><span class="brand-dot"></span> TP Call Center</span>
                </div>
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
</body>
</html>