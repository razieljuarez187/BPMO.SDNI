<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="DetalleExpedienteUnidadUI.aspx.cs" Inherits="BPMO.SDNI.Flota.UI.DetalleExpedienteUnidadUI" %>

<%--Satisface al CU081 - Consultar Seguimiento Flota--%>
<%--Satisface al CU074 - Consultar Expediente de Unidades--%>
<%--Satisface la solicitud de cambio SC0006--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloActaNacimiento.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        /*Estilos para alinear verticalmente los elementos del menú con una imagen*/
        div.MenuPrimario ul:first-child li:nth-of-type(7) a input[type="text"] { margin-top: -4px !important; }
        div.MenuPrimario ul:first-child li:nth-of-type(4).static a,
        div.MenuPrimario ul:first-child li:nth-of-type(5).static a,
        div.MenuPrimario ul:first-child li:nth-of-type(6).static a
        {
            display: table !important;
        }
        div.MenuPrimario ul:first-child li:nth-of-type(4).static a span,
        div.MenuPrimario ul:first-child li:nth-of-type(5).static a span,
        div.MenuPrimario ul:first-child li:nth-of-type(6).static a span,
        div.MenuPrimario ul:first-child li:nth-of-type(4).static a img,
        div.MenuPrimario ul:first-child li:nth-of-type(5).static a img,
        div.MenuPrimario ul:first-child li:nth-of-type(6).static a img
        {
            display: table-cell !important;
            vertical-align: middle;
        }
        div.MenuPrimario ul:first-child li:nth-of-type(4).static a img,
        div.MenuPrimario ul:first-child li:nth-of-type(5).static a img,
        div.MenuPrimario ul:first-child li:nth-of-type(6).static a img
        {
            padding-left: 3px;
        }
        /*Estilo para hacer que el primer elemento del menú, al seleccionarse, se ve igual que al seleccionar otro*/
        div.MenuPrimario ul:first-child li:nth-of-type(1).Seleccionado a { color: White !important; }
        /*Estilo para sobre-escribir el display:none que hace el menú cuando se hace click sobre el elemento seleccionado 'Expediente'*/
        div.MenuPrimario ul:first-child li.static.has-popup ul.level2 a.dynamic { display: inline !important; } 
        /*Estilo para alargar la barra lateral del acta de nacimiento para soportar un campo más*/
        .BarraEstaticaWizard { height: 425px; }
        
        #ControlesDatos fieldset { width: 95%; margin-left: auto; margin-right: auto; margin-bottom: 15px; }
        #ControlesDatos .trAlinearDerecha { width: 95%; margin-left: auto; margin-right: auto; }
        
        #BarraNavegacionInferior { border: 0px solid #ffffff !important; width: 768px; margin-right: 36px; }
        #BarraNavegacionInferior input { border-radius: 0px !important; float: left; }
    </style>
    <script type="text/javascript">
        initChild = function () {
            ConfiguracionBarraHerramientas();
            $("span:contains('*')").css({ 'display': 'none' });

            $("#dialogLlantas").dialog({
                autoOpen: false,
                closeOnEscape: true,
                modal: true,
                minWidth: 460,
                minHeight: 200
            });
            $("#dialogEquiposAliados").dialog({
                autoOpen: false,
                closeOnEscape: true,
                modal: true,
                minWidth: 900,
                minHeight: 200
            });
        };
        $(document).ready(initChild);

        function mostrarLlantas() {
            $("#dialogLlantas").dialog("open");
        }
        function mostrarEquiposAliados() {
            $("#dialogEquiposAliados").dialog("open");
        }
    </script>
	<script type="text/javascript">
	    function BtnVisualizar(guid, xml) {
	        var width = ObtenerAnchoBuscador(xml);

	        $.BuscadorWeb({
	            url: '../Buscador.UI/VisorUI.aspx',
	            xml: xml,
	            guid: guid,
	            btnSender: $("#<%=btnResult.ClientID %>"),
	            features: {
	                dialogWidth: width,
	                dialogHeight: '320px',
	                center: 'yes',
	                maximize: '0',
	                minimize: 'no'
	            }
	        });
	    }
	</script>
    <script type="text/javascript">
        function mostrarReporte(ruta) {
            if (ruta !== null && ruta !== '') {
                window.open(ruta, '', 'width=1000,height=800,left=50,top=50,toolbar=yes,scrollbars=1');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">FLOTA - EXPEDIENTE DE LA UNIDAD</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 65px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li id="ConsultarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultar" runat="server" NavigateUrl="~/Flota.UI/ConsultarSeguimientoFlotaUI.aspx">
						CONSULTAR UNIDAD
						<img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
                <li id="Movimiento">
                    <asp:HyperLink ID="hlkMovimiento" runat="server" NavigateUrl="~/Flota.UI/CambiarSucursalEquipoAliadoUI.aspx">
						MOVER EQ. ALIADO
						<img id="img1" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
            </ul>
			<!-- Barra de herramientas -->
			<div id="BarraHerramientas">
				<asp:Menu runat="server" ID="mFlota" IncludeStyleBlock="False" Orientation="Horizontal"
					CssClass="MenuPrimario" OnMenuItemClick="mFlota_MenuItemClick">
					<Items>
						<asp:MenuItem Text="Expediente" Value="Expediente" Selected="true" NavigateUrl="#"></asp:MenuItem>

                        <%--SC0006 - Adición de lógica de validación de movimientos permitidos para el estatus de siniestro--%>
						<asp:MenuItem Text="Movimientos" Value="Movimientos" Selectable="false">             
                            <asp:MenuItem Text="Alta de Unidad" Value="Alta"></asp:MenuItem>           
                            <asp:MenuItem Text="Baja de Unidad" Value="Baja"></asp:MenuItem>           
                            <asp:MenuItem Text="Reactivar Unidad" Value="Reactivar"></asp:MenuItem>           
                            <asp:MenuItem Text="Cambiar Unidad de Sucursal" Value="CambiarSucursal"></asp:MenuItem>           
                            <asp:MenuItem Text="Cambiar Unidad de Departamento" Value="CambiarDepartamento"></asp:MenuItem>                    
                            <asp:MenuItem Text="Cambiar Asignación de Equipos Aliados" Value="CambiarEquiposAliados"></asp:MenuItem> 
                        </asp:MenuItem>
						<asp:MenuItem Text="Reporte" Value="Reportes" Selectable="False" Enabled="false">             
                        </asp:MenuItem>
						<asp:MenuItem Text="Disponible" Value="Disponible" Enabled="False" Selectable="false"></asp:MenuItem>
						<asp:MenuItem Text="Contrato" Value="Contrato" Enabled="False" Selectable="false"></asp:MenuItem>
						<asp:MenuItem Text="Eq. Aliado" Value="EquipoAliado" Enabled="False" Selectable="false"></asp:MenuItem>
						<asp:MenuItem Text="Placas" Value="Placas" Enabled="False" Selectable="false"></asp:MenuItem>
					</Items>
					<StaticItemTemplate>
						<asp:Label runat="server" ID="lblOpcion" CssClass='<%# (string) Eval("Value") == "Placas" || (string) Eval("Value") == "Disponible"|| (string) Eval("Value") == "Contrato"|| (string) Eval("Value") == "EquipoAliado" ? "Informacion" : string.Empty %>' Text='<%# Eval("Text") %>'></asp:Label>
						<asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "Placas" %>' Style="width: 100px" CssClass="textBoxDisabled" Enabled="false"></asp:TextBox>
                        <asp:Image ID="imgEstatus" ImageUrl="~/Contenido/Imagenes/ESTATUS-NO-ICO.png" Visible='<%# (string) Eval("Value") == "Disponible" || (string) Eval("Value") == "Contrato" || (string) Eval("Value") == "EquipoAliado" %>' runat="server" />
					</StaticItemTemplate>
					<LevelSubMenuStyles>
						<asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" />
					</LevelSubMenuStyles>
				</asp:Menu>
				<div class="Ayuda" style="float: right">
					<input id="Button1" type="button" class="btnAyuda" onclick="ShowHelp();" />
				</div>
			</div>
			<div class="BarraNavegacionExtra">
				<input id="btnNuevoConsulta" type="button" value="Nueva Consulta" onclick="window.location='<%= Page.ResolveUrl("~/Flota.UI/ConsultarSeguimientoFlotaUI.aspx") %>'" />
			</div>
        </div>
        
        <div class="BarraEstaticaWizard">
            <span>Datos de Unidad</span>
            <br /><span>VIN</span>
            <br /><asp:TextBox ID="txtEstaticoNumSerie" runat="server" Enabled="false"></asp:TextBox>
            <br /><span>Clave Activo Oracle</span>
            <br /><asp:TextBox ID="txtEstaticoClaveOracle" runat="server" Enabled="false"></asp:TextBox>
            <br /><span>ID Leader</span>
            <br /><asp:TextBox ID="txtEstaticoIDLeader" runat="server" Enabled="false"></asp:TextBox>
            <br /><span># Económico</span>
            <br /><asp:TextBox ID="txtEstaticoNumEconomico" runat="server" Enabled="false"></asp:TextBox>
            <br /><span>Tipo Unidad</span>
            <br /><asp:TextBox ID="txtEstaticoTipoUnidad" runat="server" Enabled="false"></asp:TextBox>
            <br /><span>Modelo</span>
            <br /><asp:TextBox ID="txtEstaticoModelo" runat="server" Enabled="false"></asp:TextBox>
            <br /><span>Año</span>
            <br /><asp:TextBox ID="txtEstaticoAnio" runat="server" Enabled="false"></asp:TextBox>
            <br /><span>Fecha Compra</span>
            <br /><asp:TextBox ID="txtEstaticoFechaCompra" runat="server" Enabled="false"></asp:TextBox>
            <br /><span>Monto Factura</span>
            <br /><asp:TextBox ID="txtEstaticoMontoFactura" runat="server" Enabled="false"></asp:TextBox>
            <br /><span>Folio Factura</span>
            <br /><asp:TextBox ID="txtFolioFacturaCompra" runat="server" Enabled="false"></asp:TextBox>
        </div>
        <div id="DatosCatalogo" class="GroupBody">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <asp:Label ID="lblTituloPaso" runat="server" Text="EXPEDIENTE DE LA UNIDAD"></asp:Label>
                <div class="GroupHeaderOpciones Ancho1Opciones">
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" cssClass="btnWizardRegresar" OnClick="btnRegresar_Click"/>
                </div>
            </div>
            <div id="ControlesDatos">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical" style="width: 190px;">&Aacute;rea / Departamento</td>
                        <td style="width: 5px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtAreaNombre" runat="server" Enabled="false" style="width:80px;"></asp:TextBox>
                        </td>
                        <td class="tdCentradoVertical" style="width: 150px; text-align: right;">Estatus</td>
                        <td style="width: 5px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtEstatusNombre" runat="server" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Sucursal</td>
                        <td style="width: 5px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtSucursalNombre" runat="server" Enabled="false"></asp:TextBox>
                        </td>
                        <td class="tdCentradoVertical" style="text-align: right;">Tipo Placas</td>
                        <td style="width: 5px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtTipoPlaca" runat="server" Enabled="false" style="width: 70px;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Valor en Libros</td>
                        <td style="width: 5px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtValorLibros" runat="server" Enabled="false" CssClass="CampoNumero"></asp:TextBox>
                        </td>
                        <td class="tdCentradoVertical" style="text-align: right;">Residual</td>
                        <td style="width: 5px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtResidualPorcentaje" runat="server" Enabled="false" CssClass="CampoNumero" style="width:25px;"></asp:TextBox> % 
                            <asp:TextBox ID="txtResidualMonto" runat="server" Enabled="false" CssClass="CampoNumero" style="width: 85px;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Depreciaci&oacute;n Mensual</td>
                        <td style="width: 5px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtDepreciacionMensualPorcentaje" runat="server" Enabled="false" CssClass="CampoNumero" style="width:25px;"></asp:TextBox> % 
                            <asp:TextBox ID="txtDepreciacionMensualMonto" runat="server" Enabled="false" CssClass="CampoNumero" style="width: 85px;"></asp:TextBox>
                        </td>
                        <td class="tdCentradoVertical" style="text-align: right;">Fecha Sustituci&oacute;n</td>
                        <td style="width: 5px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtFechaSustitucion" runat="server" Enabled="false" CssClass="CampoFecha"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Vida &Uacute;til</td>
                        <td style="width: 5px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtMesesVidaUtilTotal" runat="server" Enabled="false" CssClass="CampoNumero" style="width:60px;"></asp:TextBox> Meses
                        </td>
                        <td class="tdCentradoVertical" style="text-align: right;">Vida &Uacute;til Restante</td>
                        <td style="width: 5px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtMesesVidaUtilRestante" runat="server" Enabled="false" CssClass="CampoNumero" style="width:60px;"></asp:TextBox> Meses
                        </td>
                    </tr>
                </table>
                <fieldset>
                    <legend>Seguro</legend>
                    <table class="trAlinearDerecha">
                        <tr>
                            <td class="tdCentradoVertical" style="width: 150px;">P&oacute;liza</td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtNumeroPoliza" runat="server" Enabled="false"></asp:TextBox>
                            </td>
                            <td class="tdCentradoVertical" style="width: 150px; text-align: right;">Aseguradora</td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtAseguradora" runat="server" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdCentradoVertical">Vigencia Inicial</td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtFechaVigenciaSeguroInicial" runat="server" Enabled="false" CssClass="CampoFecha"></asp:TextBox>
                            </td>
                            <td class="tdCentradoVertical" style="text-align: right;">Vigencia Final</td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtFechaVigenciaSeguroFinal" runat="server" Enabled="false" CssClass="CampoFecha"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>Contrato</legend>
                    <table class="trAlinearDerecha">
                        <tr>
                            <td class="tdCentradoVertical" style="width: 55px;">#</td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical" style="width: 100px;">
                                <asp:TextBox ID="txtNumeroContrato" runat="server" Enabled="false" style="width: 95px;"></asp:TextBox>
                            </td>
                            <td class="tdCentradoVertical" style="text-align: right;">Inicio</td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical" style="width: 80px;">
                                <asp:TextBox ID="txtFechaInicioContrato" runat="server" Enabled="false" CssClass="CampoFecha" style="width: 75px;"></asp:TextBox>
                            </td>
                            <td class="tdCentradoVertical" style="text-align: right; width: 100px;">Vencimiento</td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical" style="width: 80px;">
                                <asp:TextBox ID="txtFechaVencimientoContrato" runat="server" Enabled="false" CssClass="CampoFecha" style="width: 75px;"></asp:TextBox>
                            </td>
                            <td class="tdCentradoVertical" style="text-align: right;">Faltan</td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtMesesFaltanteContrato" runat="server" Enabled="false" CssClass="CampoNumero" style="width: 30px;"></asp:TextBox> Meses
                            </td>
                        </tr>
                        <tr>
                            <td class="tdCentradoVertical">Cliente</td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical" colspan="10">
                                <asp:TextBox ID="txtCuentaClienteNombre" runat="server" Enabled="false" style="width: 97%;"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </div>
        <div id="BarraNavegacionInferior" class="GroupBody">
            <asp:Button ID="btnActa" runat="server" Text="" CssClass="btnBarraActaNacimiento" onclick="btnActa_Click" />
            <asp:Button ID="btnContrato" runat="server" Text="" CssClass="btnBarraContrato" onclick="btnContrato_Click" />
            <asp:Button ID="btnSeguro" runat="server" Text="" CssClass="btnBarraSeguro" onclick="btnSeguro_Click" />
            <asp:Button ID="btnHistorial" runat="server" Text="" CssClass="btnBarraHistorial" onclick="btnHistorial_Click" />
            <asp:Button ID="btnTramites" runat="server" Text="" CssClass="btnBarraTramites" onclick="btnTramites_Click" />
            <asp:Button ID="btnMantenimiento" runat="server" Text="" CssClass="btnBarraMantenimiento" Enabled="false" />
            <asp:Button ID="btnLlantas" runat="server" Text="" CssClass="btnBarraLlantas" OnClientClick="javascript: mostrarLlantas(); return false;" />
            <asp:Button ID="btnEquipoAliado" runat="server" Text="" CssClass="btnBarraEquipoAliado" OnClientClick="javascript: mostrarEquiposAliados(); return false;" />
        </div>
    </div>
    <asp:HiddenField ID="hdnUnidadID" runat="server" />
    <asp:HiddenField ID="hdnEquipoID" runat="server" />
    <asp:HiddenField ID="hdnEstatusUnidadID" runat="server" />
    <asp:HiddenField ID="hdnAreaID" runat="server" />
    <asp:HiddenField ID="hdnSeguroID" runat="server" />
    <asp:HiddenField ID="hdnContratoID" runat="server" />
    <asp:HiddenField ID="hdnTipoContratoID" runat="server" />
    <asp:HiddenField ID="hdnEstaDisponible" runat="server" />
    <asp:HiddenField ID="hdnEstaEnContrato" runat="server" />
    <asp:HiddenField ID="hdnTieneEquipoAliado" runat="server" />
    <asp:HiddenField ID="hdnTiempoUsoActivos" runat="server" />
    <div id="dialogLlantas" title="Llantas asignadas a la unidad" style="display: none;">
        <asp:GridView runat="server" ID="grdLlantas" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false" 
            EnableSortingAndPagingCallbacks="false" CssClass="Grid" style="width: 95%; margin-left: auto; margin-right: auto; margin-bottom: 10px;">
            <Columns>
                <asp:BoundField DataField="Codigo" HeaderText="Código">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Marca" HeaderText="Marca">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Modelo" HeaderText="Modelo">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Posicion" HeaderText="Posicion" NullDisplayText="Refacción">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
            </Columns>
            <RowStyle CssClass="GridRow" />
            <HeaderStyle CssClass="GridHeader" />
            <FooterStyle CssClass="GridFooter" />
            <PagerStyle CssClass="GridPager" />
            <SelectedRowStyle CssClass="GridSelectedRow" />
            <AlternatingRowStyle CssClass="GridAlternatingRow" />
        </asp:GridView>
    </div>
    <div id="dialogEquiposAliados" title="Equipos aliados asignados a la unidad" style="display: none;">
        <asp:GridView runat="server" ID="grdEquiposAliados" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false" 
            EnableSortingAndPagingCallbacks="false" CssClass="Grid" style="width: 100%; margin-left: auto; margin-right: auto;">
            <Columns>
                <asp:BoundField DataField="NumeroSerie" HeaderText="# Serie">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField>
                    <HeaderTemplate>Modelo</HeaderTemplate>
                    <ItemTemplate><%# DataBinder.Eval(Container.DataItem,"Modelo.Nombre") %></ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:BoundField DataField="Dimension" HeaderText="Dimensión">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="PBV" HeaderText="PBV" DataFormatString="{0:#,##0.00##}">
                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="PBC" HeaderText="PBC" DataFormatString="{0:#,##0.00##}">
                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="IDLider" HeaderText="ID Lider">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="ClaveActivoOracle" HeaderText="Clave Activo Oracle">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
            </Columns>
            <RowStyle CssClass="GridRow" />
            <HeaderStyle CssClass="GridHeader" />
            <FooterStyle CssClass="GridFooter" />
            <PagerStyle CssClass="GridPager" />
            <SelectedRowStyle CssClass="GridSelectedRow" />
            <AlternatingRowStyle CssClass="GridAlternatingRow" />
        </asp:GridView>
    </div>
    <%--Campos ocultos--%>
    <asp:Button ID="btnResult" runat="server" Text="Button" CausesValidation="false" UseSubmitBehavior="false" Style="display: none;" />
</asp:Content>
