<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionIncidentes.aspx.cs" Inherits="TPCallCenter_Equipo_26A.GestionIncidentes" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .toolbar { margin-bottom: 15px; }
        .detalle-box { background: #fff; border: 1px solid #ddd; padding: 16px; margin-bottom: 20px; border-radius:6px; }
        .reasign-box { background:#f8f9fa; border:1px solid #e2e6ea; padding:12px; border-radius:6px; margin-top:12px; }
        .alert-inline { margin-top:8px; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h2>Gestión de Incidentes</h2>

        <asp:Label ID="lblMensajeGestion" runat="server" CssClass="alert alert-success" Visible="false" />
        <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger" Visible="false" />

        <div class="row toolbar">
            <div class="col-md-4">
                <asp:DropDownList ID="ddlFiltroEstado" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-2">
                <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="Filtrar" OnClick="btnFiltrar_Click" />
            </div>
            <div class="col-md-6 text-right">
                <asp:Button ID="btnNuevo" runat="server" CssClass="btn btn-success" Text="Nuevo Incidente" PostBackUrl="~/NuevoIncidente.aspx" />
            </div>
        </div>

        <asp:Panel ID="pnlDetalle" runat="server" Visible="false" CssClass="detalle-box">
            <asp:Label ID="lblDetalleError" runat="server" CssClass="alert alert-danger" Visible="false" />

            <dl class="row">
                <dt class="col-sm-3">Nº Reclamo</dt>
                <dd class="col-sm-9"><asp:Label ID="lblDetalleNumero" runat="server" /></dd>

                <dt class="col-sm-3">Cliente</dt>
                <dd class="col-sm-9"><asp:Label ID="lblDetalleCliente" runat="server" /></dd>

                <dt class="col-sm-3">Tipo</dt>
                <dd class="col-sm-9"><asp:Label ID="lblDetalleTipo" runat="server" /></dd>

                <dt class="col-sm-3">Prioridad</dt>
                <dd class="col-sm-9"><asp:Label ID="lblDetallePrioridad" runat="server" /></dd>

                <dt class="col-sm-3">Estado</dt>
                <dd class="col-sm-9"><asp:Label ID="lblDetalleEstado" runat="server" /></dd>

                <dt class="col-sm-3">Fecha Alta</dt>
                <dd class="col-sm-9"><asp:Label ID="lblDetalleFechaAlta" runat="server" /></dd>

                <dt class="col-sm-3">Descripción</dt>
                <dd class="col-sm-9"><asp:Label ID="lblDetalleDescripcion" runat="server" /></dd>

                <dt class="col-sm-3">Creador</dt>
                <dd class="col-sm-9"><asp:Label ID="lblDetalleCreador" runat="server" /></dd>

                <dt class="col-sm-3">Asignado a</dt>
                <dd class="col-sm-9"><asp:Label ID="lblDetalleAsignado" runat="server" /></dd>
            </dl>

            <!-- Panel de reasignación (solo supervisor) -->
            <asp:Panel ID="pnlReasignar" runat="server" Visible="false" CssClass="reasign-box">
                <h5 class="mb-2">Reasignar incidencia</h5>
                <asp:HiddenField ID="hfIncidenciaId" runat="server" />
                <div class="form-group">
                    <label for="ddlUsuarios" class="form-label">Nuevo usuario asignado</label>
                    <asp:DropDownList ID="ddlUsuarios" runat="server" CssClass="form-control" />
                </div>
                <asp:Button ID="btnReasignar" runat="server" CssClass="btn btn-warning btn-sm" Text="Guardar reasignación" OnClick="btnReasignar_Click" />
                <asp:Label ID="lblReasignarOk" runat="server" CssClass="alert alert-success alert-inline" Visible="false" />
                <asp:Label ID="lblReasignarError" runat="server" CssClass="alert alert-danger alert-inline" Visible="false" />
            </asp:Panel>

            <div class="mt-3">
                <asp:Button ID="btnVolverDetalle" runat="server" Text="Volver" CssClass="btn btn-secondary" OnClick="btnVolverDetalle_Click" />
            </div>
        </asp:Panel>

        <asp:GridView ID="gvIncidencias" runat="server"
                      AutoGenerateColumns="false"
                      CssClass="table table-striped table-sm"
                      OnRowCommand="gvIncidencias_RowCommand"
                      OnRowDataBound="gvIncidencias_RowDataBound"
                      DataKeyNames="IDIncidencia">
            <Columns>
                <asp:BoundField DataField="IDIncidencia" HeaderText="ID" Visible="false" />
                <asp:BoundField DataField="NumeroReclamo" HeaderText="Nº Reclamo" />
                <asp:BoundField DataField="ClienteNombre" HeaderText="Cliente" />
                <asp:BoundField DataField="TipoNombre" HeaderText="Tipo" />
                <asp:BoundField DataField="PrioridadNombre" HeaderText="Prioridad" />
                <asp:BoundField DataField="EstadoDescripcion" HeaderText="Estado" />
                <asp:BoundField DataField="FechaAlta" HeaderText="Fecha Alta" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:Button ID="btnVer" runat="server" CssClass="btn btn-info btn-sm" Text="Ver" CommandName="Ver" CommandArgument='<%# Container.DataItemIndex %>' />
                        <asp:Button ID="btnResolver" runat="server" CssClass="btn btn-warning btn-sm" Text="Resolver" CommandName="Resolver" CommandArgument='<%# Container.DataItemIndex %>' />
                        <asp:Button ID="btnCerrar" runat="server" CssClass="btn btn-danger btn-sm" Text="Cerrar" CommandName="Cerrar" CommandArgument='<%# Container.DataItemIndex %>' />
                        <asp:Button ID="btnReasignarRow" runat="server" CssClass="btn btn-secondary btn-sm" Text="Reasignar" CommandName="Reasignar" CommandArgument='<%# Container.DataItemIndex %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>