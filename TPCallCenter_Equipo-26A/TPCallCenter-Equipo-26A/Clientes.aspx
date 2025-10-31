<%@ Page Title="Gestión de Clientes" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Clientes.aspx.cs" Inherits="TPCallCenter_Equipo_26A.Clientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .btn-action {
            margin: 2px;
        }
        .alert {
            margin-bottom: 20px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <h3><i class="fas fa-users"></i> Gestión de Clientes - Web Forms</h3>
                        <asp:Button ID="btnNuevoCliente" runat="server" 
                                    Text="Nuevo Cliente" 
                                    CssClass="btn btn-primary"
                                    OnClick="btnNuevoCliente_Click" />
                    </div>
                    <div class="card-body">
                        <!-- Mensaje de estado -->
                        <asp:Label ID="lblMensaje" runat="server" 
                                   CssClass="alert alert-info d-block" 
                                   Text="Sistema de gestión de clientes usando ASP.NET Web Forms (.aspx)">
                        </asp:Label>
                        
                        <!-- Botones de acción -->
                        <div class="mb-3">
                            <asp:Button ID="btnCargarClientes" runat="server" 
                                        Text="Cargar Clientes" 
                                        CssClass="btn btn-success"
                                        OnClick="btnCargarClientes_Click" />
                            <asp:Button ID="btnLimpiar" runat="server" 
                                        Text="Limpiar Lista" 
                                        CssClass="btn btn-secondary"
                                        OnClick="btnLimpiar_Click" />
                            <asp:Button ID="btnExportar" runat="server" 
                                        Text="Exportar" 
                                        CssClass="btn btn-info"
                                        OnClick="btnExportar_Click" />
                        </div>

                        <!-- GridView de clientes -->
                        <asp:GridView ID="gvClientes" runat="server" 
                                      CssClass="table table-striped table-bordered"
                                      AutoGenerateColumns="false"
                                      EmptyDataText="No hay clientes para mostrar"
                                      HeaderStyle-CssClass="table-dark">
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
                                                   Text='<%# (bool)Eval("Activo") ? "Activo" : "Inactivo" %>'
                                                   CssClass='<%# (bool)Eval("Activo") ? "badge bg-success" : "badge bg-danger" %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:Button ID="btnVer" runat="server" 
                                                    Text="Ver" 
                                                    CssClass="btn btn-sm btn-info btn-action"
                                                    CommandArgument='<%# Eval("IDCliente") %>'
                                                    OnClick="btnVer_Click" />
                                        <asp:Button ID="btnEditar" runat="server" 
                                                    Text="Editar" 
                                                    CssClass="btn btn-sm btn-warning btn-action"
                                                    CommandArgument='<%# Eval("IDCliente") %>'
                                                    OnClick="btnEditar_Click" />
                                        <asp:Button ID="btnEliminar" runat="server" 
                                                    Text="Eliminar" 
                                                    CssClass="btn btn-sm btn-danger btn-action"
                                                    CommandArgument='<%# Eval("IDCliente") %>'
                                                    OnClick="btnEliminar_Click"
                                                    OnClientClick="return confirm('¿Está seguro de eliminar este cliente?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        
                        <!-- Contador de registros -->
                        <div class="mt-3">
                            <asp:Label ID="lblContador" runat="server" 
                                       CssClass="text-muted">
                            </asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
