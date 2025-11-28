<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionIncidentes.aspx.cs" Inherits="TPCallCenter_Equipo_26A.GestionIncidentes" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="HeadIncidencias" ContentPlaceHolderID="head" runat="server">
    <style>
        .inc-card {
            background:#ffffff;
            border:1px solid #d8dde2;
            border-radius:14px;
            box-shadow:0 12px 28px rgba(0,0,0,0.20);
            overflow:hidden;
            margin-bottom:40px;
        }
        .inc-card-header {
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
        .inc-filters {
            display:flex;
            flex-wrap:wrap;
            gap:12px;
            padding:16px 24px 8px;
            background:#f9fbfc;
            border-bottom:1px solid #e2e6ea;
        }
        .inc-filters .form-control,
        .inc-filters .btn { border-radius:8px; }

        .inc-table-wrapper { padding:20px 24px 10px; }

        .inc-table {
            width:100%;
            border-collapse:separate;
            border-spacing:0;
            font-size:.9rem;
        }
        .inc-table thead th {
            background:#eef2f5;
            color:#3a4a52;
            font-weight:600;
            padding:10px 12px;
            border-top:1px solid #d8dde2;
            border-bottom:1px solid #d8dde2;
            white-space:nowrap;
        }
        .inc-table tbody td {
            background:#ffffff;
            padding:9px 12px;
            border-bottom:1px solid #ecf0f2;
            vertical-align:middle;
        }
        .inc-table tbody tr:nth-child(even) td { background:#f6f8f9; }
        .inc-table tbody tr:hover td { background:#e9f3ff; }
        .inc-table th:first-child,
        .inc-table td:first-child { border-left:1px solid #d8dde2; }
        .inc-table th:last-child,
        .inc-table td:last-child { border-right:1px solid #d8dde2; }
        .inc-table thead th:first-child { border-top-left-radius:6px; }
        .inc-table thead th:last-child { border-top-right-radius:6px; }

        .grid-actions .btn {
            margin:2px 4px 2px 0;
            padding:6px 10px;
            font-size:.70rem;
            font-weight:600;
            border-radius:6px;
            letter-spacing:.4px;
        }

        .detalle-box {
            background:#fff;
            border:1px solid #d8dde2;
            padding:20px 22px;
            border-radius:12px;
            margin-bottom:24px;
            box-shadow:0 6px 18px rgba(0,0,0,0.12);
        }
        .edit-box, .accion-box {
            background:#f8f9fa;
            border:1px solid #e2e6ea;
            padding:14px 16px;
            border-radius:10px;
            margin-top:14px;
        }
        .alert-inline { margin-top:8px; }

        /* Badges estado */
        .estado-badge {
            display:inline-block;
            padding:4px 10px;
            font-size:.63rem;
            font-weight:700;
            letter-spacing:.5px;
            border-radius:16px;
            text-transform:uppercase;
        }
        .estado-abierto { background:#007bff1a; color:#005bb5; }
        .estado-analisis { background:#ffc10733; color:#8a6d00; }
        .estado-asignado { background:#17a2b81a; color:#0d7d8f; }
        .estado-resuelto { background:#28a7451a; color:#1b6e30; }
        .estado-cerrado { background:#6c757d1a; color:#495057; }
        .estado-reabierto { background:#dc35451a; color:#b21f2d; }

        /* Responsive */
        @media (max-width: 991.98px) {
            .inc-table thead { display:none; }
            .inc-table tbody tr {
                display:block;
                margin-bottom:14px;
                border:1px solid #d8dde2;
                border-radius:10px;
                overflow:hidden;
                box-shadow:0 4px 14px rgba(0,0,0,0.10);
            }
            .inc-table tbody td {
                display:block;
                border-bottom:1px solid #e5eaee;
                background:#fff !important;
            }
            .inc-table tbody td:last-child { border-bottom:none; }
            .inc-table tbody td[data-label]:before {
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

<asp:Content ID="BodyIncidencias" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="inc-card">
        <div class="inc-card-header">
            <span>Gestión de Incidencias</span>
            <asp:Button ID="btnNuevo" runat="server" CssClass="btn btn-success" Text="Nuevo Incidente" PostBackUrl="~/NuevoIncidente.aspx" />
        </div>

        <div class="inc-filters">
            <asp:DropDownList ID="ddlFiltroEstado" runat="server" CssClass="form-control" />
            <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="Filtrar" OnClick="btnFiltrar_Click" />
            <asp:Label ID="lblMensajeGestion" runat="server" CssClass="alert alert-success mb-0" Visible="false" />
            <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger mb-0" Visible="false" />
        </div>

        <div class="inc-table-wrapper">
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
                          CssClass="inc-table"
                          AllowPaging="true"
                          PageSize="10"
                          OnPageIndexChanging="gvIncidencias_PageIndexChanging"
                          OnRowCommand="gvIncidencias_RowCommand"
                          OnRowDataBound="gvIncidencias_RowDataBound"
                          DataKeyNames="IDIncidencia">
                <PagerStyle CssClass="pagination" />
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

            <asp:Label ID="lblTotalIncidencias" runat="server" CssClass="text-muted mt-2 d-block" />
        </div>
    </div>
</asp:Content>