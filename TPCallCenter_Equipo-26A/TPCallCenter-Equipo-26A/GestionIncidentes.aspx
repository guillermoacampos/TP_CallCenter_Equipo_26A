<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionIncidentes.aspx.cs" Inherits="TPCallCenter_Equipo_26A.GestionIncidentes" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .toolbar { margin-bottom: 15px; }
        .detalle-box { background: #fff; border: 1px solid #ddd; padding: 16px; margin-bottom: 20px; border-radius:6px; }
        .reasign-box, .edit-box, .accion-box { background:#f8f9fa; border:1px solid #e2e6ea; padding:12px; border-radius:6px; margin-top:12px; }
        .alert-inline { margin-top:8px; }
        .grid-actions .btn { margin-right: 4px; margin-bottom: 4px; }
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

        <!-- Panel de detalle -->
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

                <dt class="col-sm-3">Comentario Resolución</dt>
                <dd class="col-sm-9"><asp:Label ID="lblDetalleComentarioResolucion" runat="server" /></dd>

                <dt class="col-sm-3">Comentario Cierre</dt>
                <dd class="col-sm-9"><asp:Label ID="lblDetalleComentarioCierre" runat="server" /></dd>

                <dt class="col-sm-3">Creador</dt>
                <dd class="col-sm-9"><asp:Label ID="lblDetalleCreador" runat="server" /></dd>

                <dt class="col-sm-3">Asignado a</dt>
                <dd class="col-sm-9"><asp:Label ID="lblDetalleAsignado" runat="server" /></dd>
            </dl>

            <!-- Modificar descripción y pasar a 'En análisis' -->
            <asp:Panel ID="pnlEditar" runat="server" Visible="false" CssClass="edit-box">
                <h5 class="mb-2">Modificar descripción</h5>
                <asp:HiddenField ID="hfEditIncidenciaId" runat="server" />
                <div class="form-group">
                    <label for="txtNuevaDescripcion" class="form-label">Nueva descripción</label>
                    <asp:TextBox ID="txtNuevaDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                </div>
                <asp:Button ID="btnGuardarEdicion" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar y pasar a 'En análisis'" OnClick="btnGuardarEdicion_Click" />
                <asp:Label ID="lblEditarOk" runat="server" CssClass="alert alert-success alert-inline" Visible="false" />
                <asp:Label ID="lblEditarError" runat="server" CssClass="alert alert-danger alert-inline" Visible="false" />
            </asp:Panel>

            <!-- Resolver: requiere comentario -->
            <asp:Panel ID="pnlResolver" runat="server" Visible="false" CssClass="accion-box">
                <h5 class="mb-2">Resolver incidencia</h5>
                <asp:HiddenField ID="hfResolverIncidenciaId" runat="server" />
                <div class="form-group">
                    <label for="txtComentarioResolucion" class="form-label">Comentario de resolución (obligatorio)</label>
                    <asp:TextBox ID="txtComentarioResolucion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                </div>
                <asp:Button ID="btnConfirmarResolucion" runat="server" CssClass="btn btn-warning btn-sm" Text="Confirmar resolución" OnClick="btnConfirmarResolucion_Click" />
                <asp:Label ID="lblResolverOk" runat="server" CssClass="alert alert-success alert-inline" Visible="false" />
                <asp:Label ID="lblResolverError" runat="server" CssClass="alert alert-danger alert-inline" Visible="false" />
            </asp:Panel>

            <!-- Cerrar: requiere comentario -->
            <asp:Panel ID="pnlCerrar" runat="server" Visible="false" CssClass="accion-box">
                <h5 class="mb-2">Cerrar incidencia</h5>
                <asp:HiddenField ID="hfCerrarIncidenciaId" runat="server" />
                <div class="form-group">
                    <label for="txtComentarioCierre" class="form-label">Comentario de cierre (obligatorio)</label>
                    <asp:TextBox ID="txtComentarioCierre" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                </div>
                <asp:Button ID="btnConfirmarCierre" runat="server" CssClass="btn btn-danger btn-sm" Text="Confirmar cierre" OnClick="btnConfirmarCierre_Click" />
                <asp:Label ID="lblCerrarOk" runat="server" CssClass="alert alert-success alert-inline" Visible="false" />
                <asp:Label ID="lblCerrarError" runat="server" CssClass="alert alert-danger alert-inline" Visible="false" />
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
                        <div class="grid-actions">
                            <asp:Button ID="btnVer" runat="server" CssClass="btn btn-info btn-sm" Text="Ver" CommandName="Ver" CommandArgument='<%# Container.DataItemIndex %>' />
                            <asp:Button ID="btnModificar" runat="server" CssClass="btn btn-secondary btn-sm" Text="Modificar" CommandName="Modificar" CommandArgument='<%# Container.DataItemIndex %>' />
                            <asp:Button ID="btnResolver" runat="server" CssClass="btn btn-warning btn-sm" Text="Resolver" CommandName="Resolver" CommandArgument='<%# Container.DataItemIndex %>' />
                            <asp:Button ID="btnCerrar" runat="server" CssClass="btn btn-danger btn-sm" Text="Cerrar" CommandName="Cerrar" CommandArgument='<%# Container.DataItemIndex %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>