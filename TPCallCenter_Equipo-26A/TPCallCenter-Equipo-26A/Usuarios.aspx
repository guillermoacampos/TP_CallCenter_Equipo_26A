<%@ Page Title="Gestión de Usuarios" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="TPCallCenter_Equipo_26A.Usuarios" %>

<asp:Content ID="HeadUsuarios" ContentPlaceHolderID="head" runat="server">
    <style>
        .users-card {
            background:#ffffff;
            border:1px solid #d8dde2;
            border-radius:14px;
            box-shadow:0 12px 28px rgba(0,0,0,0.20);
            overflow:hidden;
            margin-bottom:40px;
        }
        .users-card-header {
            background:#2f3e46;
            color:#fff;
            padding:18px 24px;
            font-weight:700;
            font-size:1.15rem;
            display:flex;
            align-items:center;
            justify-content:space-between;
            letter-spacing:.4px;
        }
        .users-toolbar {
            display:flex;
            flex-wrap:wrap;
            gap:12px;
            padding:16px 24px 8px;
            background:#f9fbfc;
            border-bottom:1px solid #e2e6ea;
        }
        .users-toolbar .form-control,
        .users-toolbar .btn {
            border-radius:8px;
        }
        .users-table-wrapper {
            padding:20px 24px 10px;
        }

        /* Tabla sólida */
        .users-table {
            width:100%;
            border-collapse:separate;
            border-spacing:0;
            font-size:.9rem;
        }
        .users-table thead th {
            background:#eef2f5;
            color:#3a4a52;
            font-weight:600;
            padding:10px 12px;
            border-top:1px solid #d8dde2;
            border-bottom:1px solid #d8dde2;
            white-space:nowrap;
        }
        .users-table tbody td {
            background:#ffffff;
            padding:9px 12px;
            border-bottom:1px solid #ecf0f2;
            vertical-align:middle;
        }
        .users-table tbody tr:nth-child(even) td {
            background:#f6f8f9;
        }
        .users-table tbody tr:hover td {
            background:#e9f3ff;
        }
        .users-table th:first-child,
        .users-table td:first-child { border-left:1px solid #d8dde2; }
        .users-table th:last-child,
        .users-table td:last-child { border-right:1px solid #d8dde2; }
        .users-table thead th:first-child { border-top-left-radius:6px; }
        .users-table thead th:last-child { border-top-right-radius:6px; }

        /* Badges */
        .badge-estado {
            display:inline-block;
            padding:4px 10px;
            font-size:.65rem;
            font-weight:700;
            letter-spacing:.5px;
            border-radius:16px;
            text-transform:uppercase;
        }
        .badge-activo { background:#28a7451a; color:#1b6e30; }
        .badge-inactivo { background:#dc35451a; color:#b21f2d; }

        .grid-actions .btn {
            margin:2px 4px 2px 0;
            padding:6px 10px;
            font-size:.70rem;
            font-weight:600;
            border-radius:6px;
            letter-spacing:.4px;
        }

        .users-footer {
            padding:10px 24px 24px;
            font-size:.75rem;
            color:#5d6b73;
            display:flex;
            justify-content:space-between;
            align-items:center;
            flex-wrap:wrap;
            gap:10px;
        }

        /* Paginación GridView */
        .users-table-wrapper .pagination {
            list-style:none;
            padding:0;
            margin:10px 0;
            display:flex;
            gap:4px;
        }
        .users-table-wrapper .pagination li a,
        .users-table-wrapper .pagination li span {
            display:block;
            padding:6px 10px;
            background:#eef2f5;
            border:1px solid #d8dde2;
            border-radius:6px;
            font-size:.75rem;
            font-weight:600;
            color:#33464e;
            text-decoration:none;
        }
        .users-table-wrapper .pagination li a:hover {
            background:#d7e4ed;
        }
        .users-table-wrapper .pagination .active span {
            background:#0d6efd;
            color:#fff;
            border-color:#0d6efd;
        }

        /* Responsive */
        @media (max-width: 991.98px) {
            .users-table thead { display:none; }
            .users-table tbody tr {
                display:block;
                margin-bottom:14px;
                border:1px solid #d8dde2;
                border-radius:10px;
                overflow:hidden;
                box-shadow:0 4px 14px rgba(0,0,0,0.10);
            }
            .users-table tbody td {
                display:block;
                border-bottom:1px solid #e5eaee;
                background:#fff !important;
            }
            .users-table tbody td:last-child { border-bottom:none; }
            .users-table tbody td[data-label]:before {
                content:attr(data-label);
                font-weight:600;
                display:block;
                margin-bottom:2px;
                color:#3a4a52;
                font-size:.68rem;
                text-transform:uppercase;
                letter-spacing:.5px;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="BodyUsuarios" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="users-card">
        <div class="users-card-header">
            <span>Gestión de Usuarios</span>
            <asp:Button ID="btnNuevoUsuario" runat="server" CssClass="btn btn-primary" Text="Nuevo Usuario" PostBackUrl="~/NuevoUsuario.aspx" />
        </div>

        <div class="users-toolbar">
            <asp:Label ID="lblErrorUsuarios" runat="server" CssClass="alert alert-danger mb-0" Visible="false" />
            <asp:Label ID="lblOkUsuarios" runat="server" CssClass="alert alert-success mb-0" Visible="false" />
        </div>

        <div class="users-table-wrapper">
            <asp:GridView ID="gvUsuarios" runat="server"
                          AutoGenerateColumns="false"
                          CssClass="users-table"
                          AllowPaging="true"
                          PageSize="10"
                          OnPageIndexChanging="gvUsuarios_PageIndexChanging"
                          OnRowDataBound="gvUsuarios_RowDataBound"
                          DataKeyNames="IDUsuario">
                <PagerStyle CssClass="pagination" />
                <Columns>
                    <asp:BoundField DataField="IDUsuario" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="PerfilDescripcion" HeaderText="Perfil" />
                    <asp:BoundField DataField="FechaAlta" HeaderText="Fecha Alta" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="Activo" HeaderText="Estado" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <div class="grid-actions">
                                <asp:Button ID="btnEditar" runat="server" CssClass="btn btn-warning btn-sm" Text="Editar" CommandName="Editar" CommandArgument='<%# Container.DataItemIndex %>' />
                                <asp:Button ID="btnEliminar" runat="server" CssClass="btn btn-danger btn-sm" Text="Eliminar" CommandName="Eliminar" CommandArgument='<%# Container.DataItemIndex %>' />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <div class="users-footer">
                <span>Total usuarios: <asp:Label ID="lblTotalUsuarios" runat="server" /></span>
            </div>
        </div>
    </div>
</asp:Content>