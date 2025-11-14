<%@ Page Title="Gestión de Clientes" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Clientes.aspx.cs" Inherits="TPCallCenter_Equipo_26A.Clientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        html, body, .container-fluid {
            height: 100%;
            margin: 0;
            padding: 0;
        }
        .btn-action {
            margin: 2px;
            border-radius: 5px;
        }
        .alert {
            margin-bottom: 20px;
            border-radius: 5px;
        }
        .table {
            border-radius: 5px;
            overflow: hidden;
            border: 1px solid #dee2e6;
        }
        .card {
            border: none;
            border-radius: 10px;
            background-color: #f8f9fa;
            height: 100%;
        }
        .card-header {
            background-color: #343a40;
            color: white;
            font-size: 1.25rem;
            border-bottom: none;
        }
        .card-body {
            background-color: #ffffff;
            height: calc(100% - 56px);
            display: flex;
            flex-direction: column;
        }
        .badge {
            font-size: 0.85rem;
            padding: 0.4em 0.6em;
        }
        .btn {
            border-radius: 5px;
            font-size: 0.9rem;
        }
        .btn-primary {
            background-color: #007bff;
            border-color: #007bff;
        }
        .btn-primary:hover {
            background-color: #0056b3;
            border-color: #004085;
        }
        .btn-secondary {
            background-color: #6c757d;
            border-color: #6c757d;
        }
        .btn-secondary:hover {
            background-color: #5a6268;
            border-color: #545b62;
        }
        .btn-info {
            background-color: #17a2b8;
            border-color: #17a2b8;
        }
        .btn-info:hover {
            background-color: #117a8b;
            border-color: #0f6674;
        }
        .btn-success {
            background-color: #28a745;
            border-color: #28a745;
        }
        .btn-success:hover {
            background-color: #218838;
            border-color: #1e7e34;
        }
        .btn-warning {
            background-color: #ffc107;
            border-color: #ffc107;
        }
        .btn-warning:hover {
            background-color: #e0a800;
            border-color: #d39e00;
        }
        .btn-danger {
            background-color: #dc3545;
            border-color: #dc3545;
        }
        .btn-danger:hover {
            background-color: #c82333;
            border-color: #bd2130;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <h3><i class="fas fa-users"></i> Gestión de Clientes - Call Center</h3>
                        <div>
                            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo Cliente" CssClass="btn btn-primary btn-action" OnClick="btnNuevo_Click" />
                        </div>
                    </div>
                    <div class="card-body">
                        <!-- GridView de clientes -->
                        <asp:GridView ID="gvClientes" runat="server" 
                                      CssClass="table table-striped table-bordered"
                                      AutoGenerateColumns="false"
                                      EmptyDataText="No hay clientes para mostrar"
                                      HeaderStyle-CssClass="table-dark"
                                      AllowPaging="True" PageSize="10"
                                      OnPageIndexChanging="gvClientes_PageIndexChanging"
                                      OnRowCommand="gvClientes_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="IDCliente" HeaderText="ID" 
                                                ItemStyle-Width="80px" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                                <asp:BoundField DataField="Email" HeaderText="Email" />
                                <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                                <asp:BoundField DataField="fechaAlta" HeaderText="Fecha Alta" 
                                                DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:TemplateField HeaderText="Estado">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEstado" runat="server"
                                                   Text='<%# Eval("Activo") != DBNull.Value && (bool)Eval("Activo") ? "Activo" : "Inactivo" %>'
                                                   CssClass='<%# Eval("Activo") != DBNull.Value && (bool)Eval("Activo") ? "badge bg-success" : "badge bg-danger" %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%-- Acciones (Editar / Eliminar) --%>
                                <asp:TemplateField HeaderText="Acciones">
                                    <ItemTemplate>
                                        <asp:Button ID="btnEditar" runat="server" Text="Editar" CommandName="Editar" CommandArgument='<%# Eval("IDCliente") %>' CssClass="btn btn-sm btn-warning btn-action" />
                                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CommandName="Eliminar" CommandArgument='<%# Eval("IDCliente") %>' CssClass="btn btn-sm btn-danger btn-action" OnClientClick="return confirm('¿Eliminar cliente?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>

                        <!-- Contador de registros -->
                        <div class="mt-3">
                            <asp:Label ID="lblContador" runat="server" CssClass="text-muted"></asp:Label>
                        </div>
                       
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>