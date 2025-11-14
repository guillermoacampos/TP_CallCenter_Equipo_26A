<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NuevoCliente.aspx.cs" Inherits="TPCallCenter_Equipo_26A.NuevoCliente" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-group { margin-bottom: 10px; }
        .form-actions { margin-top: 12px; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-8 offset-2">
                <div class="card">
                    <div class="card-header">
                        <h3 id="titulo">Nuevo Cliente</h3>
                    </div>
                    <div class="card-body">
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="text-danger" />

                        <div class="form-group">
                            <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqNombre" runat="server" ControlToValidate="txtNombre" ErrorMessage="Nombre requerido" CssClass="text-danger" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblApellido" runat="server" Text="Apellido"></asp:Label>
                            <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqApellido" runat="server" ControlToValidate="txtApellido" ErrorMessage="Apellido requerido" CssClass="text-danger" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblDocumento" runat="server" Text="Documento"></asp:Label>
                            <asp:TextBox ID="txtDocumento" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqDocumento" runat="server" ControlToValidate="txtDocumento" ErrorMessage="Documento requerido" CssClass="text-danger" />
                            <asp:RegularExpressionValidator ID="regexDocumento" runat="server" ControlToValidate="txtDocumento" ErrorMessage="El Documento no puede exceder los 8 caracteres." ValidationExpression="^.{1,8}$" CssClass="text-danger" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblTelefono" runat="server" Text="Teléfono"></asp:Label>
                            <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblDireccion" runat="server" Text="Dirección"></asp:Label>
                            <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="form-actions">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success" OnClick="BtnGuardar_Click" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="BtnCancelar_Click" />
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>