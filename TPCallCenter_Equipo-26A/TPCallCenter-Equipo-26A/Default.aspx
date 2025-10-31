<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TPCallCenter_Equipo_26A.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <h3><i class="fas fa-tachometer-alt"></i> Dashboard - ASP.NET Web Forms</h3>
                    </div>
                    <div class="card-body">
                        <div class="alert alert-info">
                            <strong>¡Bienvenido!</strong> Este es un sistema de Call Center desarrollado con ASP.NET Web Forms (.aspx)
                        </div>
                        
                        <div class="row">
                            <div class="col-md-4">
                                <div class="card bg-primary text-white">
                                    <div class="card-body">
                                        <h5 class="card-title">
                                            <i class="fas fa-users"></i> Clientes
                                        </h5>
                                        <p class="card-text">Gestión de clientes del call center</p>
                                        <asp:LinkButton ID="lnkClientes" runat="server" 
                                                        CssClass="btn btn-light" 
                                                        PostBackUrl="~/Clientes.aspx">
                                            Ver Clientes
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="col-md-4">
                                <div class="card bg-success text-white">
                                    <div class="card-body">
                                        <h5 class="card-title">
                                            <i class="fas fa-chart-bar"></i> Reportes
                                        </h5>
                                        <p class="card-text">Estadísticas y reportes del sistema</p>
                                        <asp:Button ID="btnReportes" runat="server" 
                                                    Text="Ver Reportes" 
                                                    CssClass="btn btn-light"
                                                    OnClick="btnReportes_Click" />
                                    </div>
                                </div>
                            </div>
                            
                            <div class="col-md-4">
                                <div class="card bg-warning text-dark">
                                    <div class="card-body">
                                        <h5 class="card-title">
                                            <i class="fas fa-cog"></i> Configuración
                                        </h5>
                                        <p class="card-text">Configuraciones del sistema</p>
                                        <asp:Button ID="btnConfig" runat="server" 
                                                    Text="Configurar" 
                                                    CssClass="btn btn-dark"
                                                    OnClick="btnConfig_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <hr />
                        
                        <div class="row mt-4">
                            <div class="col-12">
                                <h4>Información del Sistema</h4>
                                <asp:Label ID="lblInfo" runat="server" CssClass="text-muted">
                                    Sistema desarrollado con ASP.NET Web Forms (.NET Framework 4.8)
                                </asp:Label>
                                <br />
                                <asp:Label ID="lblFecha" runat="server" CssClass="text-muted">
                                </asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
