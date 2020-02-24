<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="DetalleMantenimientoUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.DetalleMantenimientoUI" %>

<%@ Register Src="~/Mantenimiento.UI/ucDatosOrdenServicioUI.ascx" TagName="ucDatosOrdenServicioUI" TagPrefix="ucPagina1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloIngresarUnidad.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/MantenimientoResponsive.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/bootstrap.1.8.2.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .GroupSection { max-width: 650px; min-width:100px; margin: 0px auto; }
        .GroupContentCollapsable table { margin: 20px auto; width: 506px; }
        .GroupContentCollapsable .btnComando { margin: 20px auto 0px auto; }
        .Grid { border: none;}
        .ChildGrid { margin: 0px; padding: 0px; border: none;}
        .btnRegistrarIngreso { background: #DE0814 !important; border: none !important; height: 30px; line-height:30px; color: White; font-weight: bold;text-decoration: none; text-transform: uppercase; width: 185px; cursor: pointer; }
        .Grid th, .ChildGrid th { font-size : 12px !important; text-align: center; }
        .Grid td, .ChildGrid td { font-size : 11px !important; text-align: left; vertical-align: middle; }
        .td-finder { float: left; white-space: nowrap;}
        .ChildGrid td { border: solid 1px #cccccc; border-top: none; }
        .WizardSteps{ width:auto;}
    </style>
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/bootstrap-1.8.2.js") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/mantenimiento-responsive.js") %>" type="text/javascript"></script>
     <script type="text/javascript">
        $(document).ready(function () {
            initScript();
        });

        function initScript() {
            cloneMenu();
            loadMenuPrincipalSelected();
            listenClickMenuResponsive();
        }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
			<asp:Label ID="lblEncabezadoLeyenda" runat="server">ORDEN DE SERVICIO IDEALEASE</asp:Label>
		</div>
        <!--Navegación secundaria-->
		<div style="height: 65px;">
			<!-- Menú secundario -->
			<span id="ContenedorMenuSecundario">
            </span>
            <!-- Barra de herramientas -->
			<div id="BarraHerramientas" style="width:100%">
				<div class="WizardSteps">
                </div>
            </div>
            <div class="BarraNavegacionExtra">
            </div>
        </div>
        <!-- Cuerpo -->
        <asp:MultiView ID="MultiView2" runat="server" ActiveViewIndex="0">
		    <asp:View ID="View1" runat="server">
                <asp:Panel ID="pnlOrdenServicio" runat="server" Visible="true">
                    <div id="divOrdenServicio" class="GroupBody" style="display:inline !important; margin-top:10px !important;">
                        <div id="divOrdenServicioHeader" class="GroupHeader">
                            <span>DATOS GENERALES</span>
                            <div class="GroupHeaderOpciones Ancho1Opciones" style="margin-top: 0px !important; z-index:0;">
					            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnCancelar_Click" />
			                </div>
                        </div>
                        <div id="divInformacionOrdenServicio" style="padding:0px 10px;">
                            <ucPagina1:ucDatosOrdenServicioUI ID="ucDatosOrdenServicioUI" runat="server" />
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlEquiposAliados" runat="server" Visible="true">
                    <div id="divEquipoAliado" class="GroupBody" style="display:inline !important; margin-top:10px !important;">
                        <div id="divEquipoAliadoHeader" class="GroupHeader">
                            <span>EQUIPOS ALIADOS</span>
                        </div>
                        <div id="divEquiposAliados" style="border: 2px; width:100% !important; overflow: auto; padding:5px;">
                                <asp:GridView ID="GridEquipoAliado" runat="server" AutoGenerateColumns="False" Width="100%"
                                    EnableViewState="false" CssClass="Grid" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="10" 
                                    OnPageIndexChanging="GridEquipoAliado_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="150px" HeaderText="No. Econ&oacute;mico">
                                            <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"NumeroEconomico") %>' Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left"/>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="150px" HeaderText="Modelo">
                                            <ItemTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Modelo") %>' Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left"/>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="150px" HeaderText="Cliente">
                                            <ItemTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Cliente") %>' Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left"/>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="150px" HeaderText="Kilometraje">
                                            <ItemTemplate>
                                                    <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Kilometraje") %>' Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left"/>
                                            <ItemStyle CssClass="tdGridRightAlign" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="150px" HeaderText="Hor&oacute;metro">
                                            <ItemTemplate>
                                                    <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Horometro") %>' Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left"/>
                                            <ItemStyle CssClass="tdGridRightAlign" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="150px" HeaderText="Tipo Servicio">
                                            <ItemTemplate>
                                                    <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"TipoMantenimiento") %>' Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left"/>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>No se encontraron equipos aliados</EmptyDataTemplate>
                                    <RowStyle CssClass="GridRow" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <FooterStyle CssClass="GridFooter" />
                                    <PagerStyle CssClass="GridPager" />
                                    <SelectedRowStyle CssClass="GridSelectedRow" />
                                    <AlternatingRowStyle CssClass="GridAlternatingRow" />
                                </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlDatosOpcionales" runat="server" Visible="true">
                    <div id="divDatosOpciones" class="GroupBody" style="display:inline !important; margin-top:10px !important;">
                        <div id="divDatosOpcionalesHeader" class="GroupHeader">
                            <span>DATOS OPCIONALES</span>
                        </div>
                        <div id="divDatosOpcionalesControles" style="padding:0px 10px;">
                            <table class="trAlinearDerecha form-two-columns-responsive" style="margin: 0px auto 10px auto; width: auto; display: inherit; border: 1px solid transparent;">
                                <tr>
                                    <td class="tdCentradoVertical input-label-responsive">Inventario:</td>
                                    <td class="input-space-responsive">&nbsp;</td>
                                    <td class="tdCentradoVertical input-group-responsive" colspan="5">
                                        <asp:TextBox ID="txtFInventario" runat="server" ReadOnly="true" Enabled="false" TextMode="MultiLine" Rows="2" Columns="3" CssClass="input-text-responsive full-width"></asp:TextBox>
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical input-label-responsive">Descripción de Falla (IL700):</td>
                                    <td class="input-space-responsive">&nbsp;</td>
                                    <td class="tdCentradoVertical input-group-responsive" colspan="5">
                                        <asp:TextBox ID="txtFDescripcionFalla" runat="server" ReadOnly="true" Enabled="false" TextMode="MultiLine" Rows="2" Columns="3" CssClass="input-text-responsive full-width"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical input-label-responsive">Códigos de Falla:</td>
                                    <td class="input-space-responsive">&nbsp;</td>
                                    <td class="tdCentradoVertical input-group-responsive" colspan="5">
                                        <asp:TextBox ID="txtFCodigosFalla" runat="server" ReadOnly="true" Enabled="false" TextMode="MultiLine" Rows="2" Columns="3" CssClass="input-text-responsive full-width"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical input-label-responsive">Observaciones:</td>
                                    <td class="input-space-responsive">&nbsp;</td>
                                    <td class="tdCentradoVertical input-group-responsive" colspan="5">
                                        <asp:TextBox ID="txtFObservaciones" runat="server" ReadOnly="true" Enabled="false" TextMode="MultiLine" Rows="2" Columns="3" CssClass="input-text-responsive full-width"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical input-label-responsive">Fecha de Apertura:</td>
                                    <td class="input-space-responsive">&nbsp;</td>
                                    <td class="tdCentradoVertical input-group-responsive">
                                        <asp:TextBox ID="txtFFechaA" runat="server" ReadOnly="true" Enabled="false" CssClass="input-text-responsive"></asp:TextBox>
                                    </td>
                                    <td class="input-space-responsive">&nbsp;</td>
                                    <td class="tdCentradoVertical input-label-responsive">Fecha de Cierre:</td>
                                    <td class="input-space-responsive">&nbsp;</td>
                                    <td class="tdCentradoVertical input-group-responsive">
                                        <asp:TextBox ID="txtFFechaC" runat="server" ReadOnly="true" Enabled="false" CssClass="input-text-responsive"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </asp:Panel>
            </asp:View>
        </asp:MultiView>
        <asp:HiddenField ID="hdnLibroActivos" runat="server" />
        </div>
</asp:Content>
