<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
	AutoEventWireup="true" CodeBehind="DetalleCuentaClienteUI.aspx.cs" Inherits="BPMO.SDNI.Comun.UI.DetalleCuentaClienteUI" %>

<%@ Register Src="~/Comun.UI/ucDatosRepresentanteLegalUI.ascx" TagName="ucDatosRepresentanteLegalUI"
	TagPrefix="ucDRL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!--Funcionalidad Deshabilitar Enter en cajas de texto-->
<script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
	<link href="../Contenido/Estilos/EstiloCatCliente.css" rel="stylesheet" type="text/css" />
	<script language="javascript" type="text/javascript" id="JQuerySection">
		$(document).ready(function () {
			initChild();

		});
	</script>
	<script language="javascript" type="text/javascript" id="JavaScriptFunctions">
		initChild = function () {
			ConfiguracionBarraHerramientas();
		};

		function Dialog() {
			$("#dialog").dialog({
				modal: true,
				width: 1100,
				height: 400,
				resizable: false,
				buttons: {
					"Aceptar": function () {
						$(this).dialog("close");
					}
				}
			});
			$("#dialog").parent().appendTo("form:first");
		}

		function DialogoDetalleObligado() {

			$("#DialogRepresentantesObligados").dialog({
				modal: true,
				width: 900,
				height: 400,
				resizable: false,
				buttons: {
					"Aceptar": function () {
						$(this).dialog("close");

					}
				}
			});
			$("#DialogRepresentantesObligados").parent().appendTo("form:first");
        }
        //Método para cambiar etiquetas de formulario, según a la unidad operativa a la que pertenezca.
		function InicializarControlesEmpresas(json) {
		    var obj = JSON.parse(json);
		    document.getElementById("lblDiasUnidad").innerHTML = obj.CLI01;
		    document.getElementById("lblTipoCuenta").innerHTML = obj.CLI02;
		    document.getElementById("lblHorasUnidad").innerHTML = obj.CLI03;
		} 
	</script>
	<style>
		.etiquetaControl
		{
			max-width: 200px;
			min-width: 200px;
			width: 200px;
			text-align: right;
		}
		.control
		{
			max-width: 200px;
			min-width: 200px;
			width: 200px;
		}
		.espacio
		{
			max-width: 20px;
			min-width: 20px;
			width: 20px;
		}
		 .Correo { text-transform: none !important; }
		 
		.InputReset,
        select
        {
	        height: 25px;
	        width: 82%;
	        font-family : Century Gothic, Arial, Verdana, Serif; 
	
	        border-color:#d7d7d7; 
	        border-style:solid; 
	        border-width:1px;
	        border-radius:5px; -moz-border-radius:5px; -webkit-border-radius:5px; -khtml-border-radius:5px;
        }
        .InputReset
        {
	        padding-right:5px; padding-left:5px;
        }
        .InputReset,
        select[disabled]
        {
	        border-color:#d2d2d2 !important; 
	        border-style:solid; 
	        border-width:1px;
        }
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="PaginaContenido">
		<!--Barra de Localización-->
		<div id="BarraUbicacion">
			<asp:Label ID="lblEncabezadoLeyenda" runat="server">CAT&Aacute;LOGOS - CONSULTAR DETALLES DE CLIENTE</asp:Label>
		</div>
		<!--Menú secundario-->
		<div style="height: 80px;">
			<!-- Menú secundario -->
			<ul id="MenuSecundario">
				<li class="MenuSecundarioSeleccionado">
					<asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Comun.UI/ConsultarCuentaClienteUI.aspx"> CONSULTAR <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /></asp:HyperLink>
				</li>
				<li>
					<asp:HyperLink ID="hlRegistroOrden" runat="server" NavigateUrl="~/Comun.UI/RegistrarCuentaClienteUI.aspx"> REGISTRAR <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /></asp:HyperLink>
				</li>
			</ul>
			<div id="BarraHerramientas" style="float: right;">
				<asp:Menu runat="server" ID="mCuentaCliente" IncludeStyleBlock="False" Orientation="Horizontal"
					CssClass="MenuPrimario" OnMenuItemClick="mCuentaCliente_MenuItemClick">
					<Items>
						<asp:MenuItem Text="Cliente" Value="ClienteID" Enabled="False" Selectable="false">
						</asp:MenuItem>
						<asp:MenuItem Text="Editar" Value="Editar"></asp:MenuItem>
					</Items>
					<StaticItemTemplate>
						<asp:Label runat="server" ID="lblOpcion" CssClass='<%# (string) Eval("Value") == "ClienteID" ? "Informacion" : string.Empty %>'
							Text='<%# Eval("Text") %>'></asp:Label>
						<asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "ClienteID" %>'
							Style="width: 100px" CssClass="textBoxDisabled" ReadOnly="true"></asp:TextBox>
					</StaticItemTemplate>
					<LevelSubMenuStyles>
						<asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" />
					</LevelSubMenuStyles>
				</asp:Menu>
				<div class="Ayuda" style="float: right">
					<input id="btnAyuda" type="button" class="btnAyuda" onclick="ShowHelp();" />
				</div>
			</div>
			<div class="BarraNavegacionExtra">
				<input id="btnNuevoConsulta" type="button" value="Nueva Consulta" onclick="window.location='<%= Page.ResolveUrl("~/Comun.UI/ConsultarCuentaClienteUI.aspx") %>'" />
			</div>
		</div>
		<!--Detalle del Cliente-->
		<asp:MultiView ID="mvCU068" runat="server" ActiveViewIndex="0">
			<asp:View ID="vwCliente" runat="server">
				<div id="divInformacionGeneral" class="GroupBody">
					<div id="divInformacionHeader" class="GroupHeader">
						<span>Cliente</span>
                        <div class="GroupHeaderOpciones Ancho1Opciones">
                            <asp:Button ID="btnActualizar" runat="server" 
                                CssClass="btnActualizar" onclick="btnActualizar_Click"/>
                        </div>
						<div class="GroupHeaderOpciones Ancho1Opciones">
							<asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btnWizardEditar"
								OnClick="btnEditar_Click" />
						</div>
					</div>
					<div id="divInformacionClienteControles">
						<table class="trAlinearDerecha">
							<tr>
								<td class="tdCentradoVertical" style="width: 200px;"><label>Nombre</label></td>
								<td style="width: 20px;">&nbsp;</td>
								<td colspan="4" class="tdCentradoVertical">
									<asp:TextBox ID="txtNombre" runat="server" Enabled="false" Width="301px"></asp:TextBox>
								</td>
							</tr>
							<tr>
								<td class="tdCentradoVertical" style="width: 200px;"><label id="lblTipoCuenta">Tipo Contribuyente</label></td>
								<td style="width: 20px;">&nbsp;</td>
								<td class="tdCentradoVertical" style="width: 230px;">
									<asp:TextBox ID="txtTipoContribuyente" runat="server" Enabled="false" Columns="10"></asp:TextBox>
								</td>
								<td class="tdCentradoVertical" style="width: 200px; text-align: right;"><label>RFC</label></td>
								<td style="width: 20px;">&nbsp;</td>
								<td class="tdCentradoVertical">
									<asp:TextBox ID="txtRFC" runat="server" Enabled="false" Columns="16"></asp:TextBox>
								</td>
							</tr>
                            <tr>
                                <td class="tdCentradoVertical" style="width: 200px;"><label>Cuenta Oracle</label></td>
								<td style="width: 20px;">&nbsp;</td>
								<td class="tdCentradoVertical" style="width: 230px;"><asp:TextBox runat="server" ID="txtNumeroCuentaOracle" Enabled="False" Width="95%"></asp:TextBox></td>
                                <td class="tdCentradoVertical" style="width: 200px; text-align: right;"><label id="lblTipoCuenta" runat="server">Tipo de Cuenta</label></td>
								<td style="width: 20px;">&nbsp;</td>
								<td class="tdCentradoVertical">
								    <asp:DropDownList runat="server" ID="ddlTipoCuenta" Width="185px">
										<asp:ListItem Text="[Seleccion una opción]" Value=""></asp:ListItem>
									</asp:DropDownList>
								</td>
                            </tr>
							<tr>
								<td class="tdCentradoVertical" style="width: 200px;"><label>CURP</label></td>
								<td style="width: 20px;">&nbsp;</td>
								<td class="tdCentradoVertical" style="width: 230px;">
									<asp:TextBox runat="server" ID="txtCURP" Columns="25"></asp:TextBox>
								</td>
								<td class="tdCentradoVertical" style="width: 200px; text-align: right;"><span id="opcional"></span><label>CORREO</label></td>
								<td style="width: 20px;">&nbsp;</td>
								<td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" type="email" name="email" ID="txtCorreo" Columns="25" CssClass="Correo InputReset" 
                                        MaxLength="100" Width="178px" Enabled="False"></asp:TextBox></td>
							</tr>

                              <tr id="trDiasUso" runat="server">
                                <td class="tdCentradoVertical" style="width: 200px;">
                                    <span id="Span1"></span><label id="lblDiasUnidad">DIAS USO UNIDAD(MES)</label>
                                </td>
                                <td style="width: 20px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical" style="width: 230px;">
                                    <asp:TextBox runat="server" ID="txtDiasUsoUnidad" Columns="25" 
                                        MaxLength="18" Width="31px" Enabled="False" 
                                        CssClass="InputReset CampoNumeroEntero"></asp:TextBox>
                                </td>
                                <td class="tdCentradoVertical" style="width: 200px; text-align: right;">
                                    <span id="Span2"></span><label id="lblHorasUnidad" runat="server">HORAS USO UNIDAD(DIA)</label>
                                </td>
                                <td style="width: 20px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" ID="txtHorasUsoUnidad" Columns="25" CssClass="InputReset CampoNumeroEntero" 
                                        MaxLength="18" Width="30px" Enabled="False"></asp:TextBox>                                    
                                </td>
                            </tr>
                            <tr id="trSector" runat="server">
                                <td ><span id="Span3"></span><Label ID="lblSector" runat="server">SECTOR</Label></td>
                                <td></td>
                                <td>
                                     <%--ComboBox para el campo Sector--%>
                                    <asp:DropDownList runat="server" ID="ddlSector" Columns="26" 
                                        Enabled="true" Visible="false" OnSelectedIndexChanged="ddlSector_SelectedIndexChanged" >                                       
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 200px; text-align: right;">
                                <span id="Span4"></span><label>TELÉFONO</label>
                                </td>
                                <td></td>                                
                                <td>
                                    <asp:TextBox runat="server" ID="txtTelefonos" CssClass="InputReset CampoNumeroEntero" 
                                    Columns="26" Enabled="False" Visible="False" MaxLength="12"></asp:TextBox>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>
                                <asp:Button ID="btnMas"  CssClass="btnAgregarATabla" runat="server" 
                                            Style="float: left; margin: 0 30px 10px 0" Visible="False" 
                                        onclick="btnMas_Click" Text="Agregar a tabla"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td >
                                <asp:GridView ID="grdvTelefonos" runat="server" AutoGenerateColumns="False"  Style="float: left;"
                                        AllowPaging="True" PageSize="5" Width="166px" 
                                        onrowdeleting="grdvTelefonos_OnRowDeleting" AllowSorting="True" 
                                        CssClass="Grid" GridLines="None" 
                                        onpageindexchanging="grdvTelefonos_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField HeaderText="Teléfonos" DataField="Telefono">
                                            </asp:BoundField>
                                            <asp:CommandField ShowDeleteButton="True" ButtonType="Image" 
                                                DeleteImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png" DeleteText="" /> 
                                        </Columns>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <EditRowStyle CssClass="GridAlternatingRow" />
                                        <PagerStyle CssClass="GridPager" />
                                        <RowStyle CssClass="GridRow" />
                                        <FooterStyle CssClass="GridFooter" />
                                        <SelectedRowStyle CssClass="GridSelectedRow" />
                                        <AlternatingRowStyle CssClass="GridAlternatingRow" />
                                    </asp:GridView>
                                </td>
                            </tr>
						</table>
					</div>
				</div>
				<asp:Panel ID="pnlActaConstitutiva" runat="server" Visible="true">
					<div id="divActaConstitutiva" class="GroupBody">
						<div id="divActaConstitutivaHeader" class="GroupHeader">
							<span>Acta Constitutiva</span>
						</div>
						<div id="divInformacionActaControles">
							<table class="trAlinearDerecha">
								<tr>
								<td class="tdCentradoVertical" style="width: 200px;">
										# Escritura
									</td>
									<td style="width: 20px;">
										&nbsp;
									</td>
									<td class="tdCentradoVertical" style="width: 230px;">
										<asp:TextBox ID="txtNumeroEscritura" runat="server" Width="135px"></asp:TextBox>
									</td>
									<td class="tdCentradoVertical" style="width: 200px; text-align: right;">
										Fecha Escritura
									</td>
									<td style="width: 20px;">
										&nbsp;
									</td>
									<td class="tdCentradoVertical">
										<asp:TextBox ID="txtFechaEscritura" runat="server" CssClass="CampoFecha"></asp:TextBox>
									</td>
								</tr>
								<tr id="trSeccion1" runat="server">
									<td class="tdCentradoVertical" style="width: 200px;">
										Nombre del Notario
									</td>
									<td style="width: 20px;">
										&nbsp;
									</td>
									<td class="tdCentradoVertical" style="width: 230px;">
										<asp:TextBox ID="txtNombreNotario" runat="server" Width="200px"></asp:TextBox>
									</td>
									<td class="tdCentradoVertical" style="width: 200px; text-align: right;">
										# Notar&iacute;a
									</td>
									<td style="width: 20px;">
										&nbsp;
									</td>
									<td>
										<asp:TextBox ID="txtNumeroNotaria" runat="server" Width="135px"></asp:TextBox>
									</td>
								</tr>
								<tr id="trSeccion2" runat="server">
									<td class="tdCentradoVertical" style="width: 200px;">
										Localidad de la Notaría
									</td>
									<td style="width: 20px;">
										&nbsp;
									</td>
									<td class="tdCentradoVertical" style="width: 230px;">
										<asp:TextBox ID="txtLocalidadNotaria" runat="server"></asp:TextBox>
										<div class="indicacion">
											(Formato: Ciudad,Estado)</div>
									</td>
									<td class="tdCentradoVertical" style="width: 200px; text-align: right;">
										# Folio de Inscripción
									</td>
									<td style="width: 20px;">
										&nbsp;
									</td>
									<td class="tdCentradoVertical">
										<asp:TextBox ID="txtNumeroFolio" runat="server" Width="135px"></asp:TextBox>
									</td>
								</tr>
								<tr id="trSeccion3" runat="server">
									<td class="tdCentradoVertical" style="width: 200px;">
										Fecha RPPC
									</td>
									<td style="width: 20px;">
										&nbsp;
									</td>
									<td class="tdCentradoVertical" style="width: 230px;">
										<asp:TextBox ID="txtFechaRPPC" runat="server" CssClass="CampoFecha"></asp:TextBox>
									</td>
									<td class="tdCentradoVertical" style="width: 200px; text-align: right;">
										Localidad RPPC
									</td>
									<td style="width: 20px;">
										&nbsp;
									</td>
									<td class="tdCentradoVertical">
										<asp:TextBox ID="txtLocalidadRPPC" runat="server"></asp:TextBox>
										<div class="indicacion">(Formato: Ciudad,Estado)</div>
									</td>
								</tr>
							</table>
						</div>
					</div>
				</asp:Panel>
				<asp:Panel ID="pnlRegistroHacienda" runat="server" Visible="false">
					<div id="divRegistroHacienda" class="GroupBody">
						<div id="divRegistroHaciendaHeader" class="GroupHeader">
							<span>Registro de Hacienda</span>
						</div>
						<div id="divInformacionHaciendaControles">
							<table class="trAlinearDerecha">
								<tr>
									<td class="tdCentradoVertical" style="width: 200px;">
										Fecha de Registro
									</td>
									<td style="width: 20px;">
										&nbsp;
									</td>
									<td class="tdCentradoVertical" style="width: 230px;">
										<asp:TextBox ID="txtFechaRegistro" runat="server" CssClass="CampoFecha"></asp:TextBox>
									</td>
									<td class="tdCentradoVertical" align="right">
										Giro de la Empresa
									</td>
									<td style="width: 20px;">
										&nbsp;
									</td>
									<td class="tdCentradoVertical">
										<asp:TextBox ID="txtGiroEmpresa" runat="server"></asp:TextBox>
									</td>
								</tr>
							</table>
						</div>
					</div>
				</asp:Panel>
                <asp:Panel ID="pnlCreditoCliente" runat="server">
                    <div id="divCreditoCliente" class="GroupBody">
                        <div id="divCreditoClienteHeader" class="GroupHeader">
                            <span>CR&Eacute;DITO DEL CLIENTE</span>
                        </div>
                        <div id="divInformacionCreditoCliente">
                            <asp:Repeater ID="rptCreditoCliente" runat="server"
                                onitemdatabound="rptCreditoCliente_ItemDataBound">
                                <ItemTemplate>
                                    <fieldset style="width: 70%;margin: 0 auto;">
                                        <legend><asp:Label ID="lblNombreMoneda" runat="server" Text="CRÉDITO EN [MONEDA]"> </asp:Label></legend>
                                        <table style="width:100%; margin: 0 auto;">
                                            <tr>
                                                <td class="trAlineaDerechaCliente">
                                                    <label>MONEDA</label>
                                                </td>
                                                <td class="trAlineaCentroCliente">
                                                    &nbsp;
                                                </td>
                                                <td  class="trAlineaIzquierdaCliente">
                                                    <asp:TextBox ID="txtMoneda" runat="server" Width="90%"></asp:TextBox>
                                                </td>
                                                 <td class="trAlineaDerechaCliente">
                                                    <label>L&Iacute;MITE</label>
                                                </td>
                                                <td class="trAlineaCentroCliente">
                                                    &nbsp;
                                                </td>
                                                <td  class="trAlineaIzquierdaCliente">
                                                    <asp:TextBox ID="txtLimiteCredito" runat="server" Width="90%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="trAlineaDerechaCliente">
                                                    <label>D&Iacute;AS FACTURA</label>
                                                </td>
                                                <td class="trAlineaCentroCliente">
                                                    &nbsp;
                                                </td>
                                                <td  class="trAlineaIzquierdaCliente">
                                                    <asp:TextBox ID="txtDiasFactura" runat="server" Width="90%"></asp:TextBox>
                                                </td>
                                                 <td class="trAlineaDerechaCliente">
                                                    <label>DISPONIBLE</label>
                                                </td>
                                                <td class="trAlineaCentroCliente">
                                                    &nbsp;
                                                </td>
                                                <td  class="trAlineaIzquierdaCliente">
                                                    <asp:TextBox ID="txtCreditoDisponible" runat="server" Width="90%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="trAlineaDerechaCliente">
                                                    <label>D&Iacute;AS CR&Eacute;DITO</label>
                                                </td>
                                                <td class="trAlineaCentroCliente">
                                                    &nbsp;
                                                </td>
                                                <td  class="trAlineaIzquierdaCliente">
                                                    <asp:TextBox ID="txtDiasCredito" runat="server" Width="90%"></asp:TextBox>
                                                </td>
                                                 <td class="trAlineaDerechaCliente">
                                                    &nbsp;
                                                </td>
                                                <td class="trAlineaCentroCliente">
                                                    &nbsp;
                                                </td>
                                                <td  class="trAlineaIzquierdaCliente">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                       </table>
                                    </fieldset>                                   
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Label ID="lblNoHayCredito" runat="server" Text="NO SE ENCONTRÓ NINGÚN CRÉDITO DISPONIBLE" Visible="false" style="padding-left:5%;"></asp:Label>
                        </div>
                    </div>
                </asp:Panel>
				<div id="divRepresentantesLegales" class="GroupBody" runat="server">
					<div id="divRepresentantesLegalesHeader" class="GroupHeader">
						<span>Representantes Legales</span>
					</div>
					<div id="divRepresentantesLegalesControles">
						<asp:GridView ID="grdRepresentantesLegales" runat="server" AutoGenerateColumns="false"
							CellPadding="4" GridLines="None" CssClass="Grid" PageSize="5" AllowPaging="True"
							AllowSorting="True" OnRowCommand="grdRepresentantesLegales_RowCommand" OnPageIndexChanging="grdRepresentantesLegales_PageIndexChanging"
							Width="95%">
							<Columns>
								<asp:BoundField HeaderText="Nombre" DataField="Nombre"></asp:BoundField>
								<asp:TemplateField HeaderText="Dirección" ItemStyle-HorizontalAlign="Justify"><ItemTemplate><asp:Label runat="server" ID="lblDireccion" Text='<%# ((BPMO.SDNI.Comun.BO.RepresentanteLegalBO)Container.DataItem).DireccionPersona.Calle %>'></asp:Label></ItemTemplate><ItemStyle Width="300px" /></asp:TemplateField>
								<asp:TemplateField HeaderText="¿Depositario?" ItemStyle-HorizontalAlign="Justify">
                                    <ItemTemplate>
                                            <asp:Label runat="server" ID="lblEsDepositario" Text='<%# ((BPMO.SDNI.Comun.BO.RepresentanteLegalBO)Container.DataItem).EsDepositario.ToString().ToUpper().Replace("TRUE","SI").Replace("FALSE","NO") %>'></asp:Label>                                        
                                    </ItemTemplate><ItemStyle Width="110px" />
                                </asp:TemplateField>
								<asp:TemplateField HeaderText="Folio RPPC" ItemStyle-HorizontalAlign="Justify"><ItemTemplate><asp:Label runat="server" ID="lblFolioRPPC" Text='<%# ((BPMO.SDNI.Comun.BO.RepresentanteLegalBO)Container.DataItem).ActaConstitutiva.NumeroRPPC %>'></asp:Label></ItemTemplate><ItemStyle Width="150px" /></asp:TemplateField>
								<asp:TemplateField><ItemTemplate><asp:ImageButton runat="server" ID="ibtDetalle" ImageUrl="~/Contenido/Imagenes/VER.png" ToolTip="Ver Detalles"
											CommandName="CMDDETALLE" CommandArgument='<%#Container.DataItemIndex%>' /></ItemTemplate><ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" /></asp:TemplateField>
							</Columns>
							<HeaderStyle CssClass="GridHeader" />
							<EditRowStyle CssClass="GridAlternatingRow" />
							<PagerStyle CssClass="GridPager" />
							<RowStyle CssClass="GridRow" />
							<FooterStyle CssClass="GridFooter" />
							<SelectedRowStyle CssClass="GridSelectedRow" />
							<AlternatingRowStyle CssClass="GridAlternatingRow" />
						</asp:GridView>
					</div>
				</div>
				<div id="divObligadosSolidarios" class="GroupBody">
					<div id="divObligadosSolidariosHeader" class="GroupHeader">
						<span>Obligados Solidarios</span>
					</div>
					<div id="divObligadosSolidariosControles">
						<asp:GridView ID="grdObligadosSolidarios" runat="server" AutoGenerateColumns="false"
							CellPadding="4" GridLines="None" CssClass="Grid" PageSize="5" AllowPaging="True"
							AllowSorting="True" OnPageIndexChanging="grdObligadosSolidarios_PageIndexChanging"
							Width="95%" OnRowCommand="grdObligadosSolidarios_RowCommand" OnRowDataBound="grdObligadosSolidarios_RowDataBound">
							<Columns>
								<asp:BoundField HeaderText="Nombre" DataField="Nombre"></asp:BoundField>
								<asp:TemplateField HeaderText="Dirección" ItemStyle-HorizontalAlign="Justify"><ItemTemplate><asp:Label runat="server" ID="lblDireccion" Text='<%# ((BPMO.SDNI.Comun.BO.ObligadoSolidarioBO)Container.DataItem).DireccionPersona.Calle %>'></asp:Label></ItemTemplate><ItemStyle Width="300px" /></asp:TemplateField>
								<asp:BoundField HeaderText="Teléfono" DataField="Telefono"><ItemStyle Width="140px" /></asp:BoundField>
								<asp:TemplateField HeaderText="Tipo Obligado Solidario" ItemStyle-HorizontalAlign="Justify">
                                    <ItemTemplate>
                                        
                                        <asp:Label runat="server" ID="lblTipoObligado" Text='<%# ((BPMO.SDNI.Comun.BO.ObligadoSolidarioBO)Container.DataItem).TipoObligado %>'></asp:Label>
                                    </ItemTemplate><ItemStyle Width="110px" />
                                </asp:TemplateField>
								<asp:TemplateField><ItemTemplate><asp:ImageButton runat="server" ID="ibtDetalle" ImageUrl="~/Contenido/Imagenes/VER.png" ToolTip="Ver Detalles"
											CommandName="CMDDETALLE" CommandArgument='<%#Container.DataItemIndex%>' /></ItemTemplate><ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" /></asp:TemplateField>
							</Columns>
							<HeaderStyle CssClass="GridHeader" />
							<EditRowStyle CssClass="GridAlternatingRow" />
							<PagerStyle CssClass="GridPager" />
							<RowStyle CssClass="GridRow" />
							<FooterStyle CssClass="GridFooter" />
							<SelectedRowStyle CssClass="GridSelectedRow" />
							<AlternatingRowStyle CssClass="GridAlternatingRow" />
						</asp:GridView>
					</div>
				</div>
			<div id="divObservaciones" runat="server" class="GroupBody">
			    <div id="div6" class="GroupHeader">
			        <span>Observaciones</span>
			    </div>
			    <div id="div7">
			        <table class="trAlinearDerecha">
			            <tr>
			                <td class="espacio">
			                    &nbsp;
			                </td>
			                <td class="tdCentradoVertical control">
			                    <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine" 
			                        Width="890px" Height="122px" MaxLength="2000"></asp:TextBox>
			                    &nbsp;
			                </td>
			                <td class="espacio">
			                    &nbsp;
			                </td>
			            </tr>
			        </table>
			    </div>
			</div>
			</asp:View>
		</asp:MultiView>
	</div>
	<div id="dialog" style="display: none;">
		<div id="divContenedorDatos" class="GroupBody">
			<div class="GroupHeader">
				<span>DATOS DEL REPRESENTANTE LEGAL</span>
			</div>
			<ucDRL:ucDatosRepresentanteLegalUI runat="server" ID="ucDatosRepresentanteLegal" />
		</div>
	</div>
	<div id="DialogRepresentantesObligados" style="display: none;" title="REPRESENTANTES LEGALES DEL OBLIGADO SOLIDARIO">
		<asp:GridView ID="grdRepresentantesObligados" runat="server" AutoGenerateColumns="false"
			CellPadding="4" GridLines="None" CssClass="Grid" PageSize="5" AllowPaging="True"
			AllowSorting="True" Width="95%" OnPageIndexChanging="grdRepresentantesObligados_PageIndexChanging">
			<Columns>
				<asp:BoundField HeaderText="Nombre" DataField="Nombre"></asp:BoundField>
				<asp:TemplateField HeaderText="Dirección" ItemStyle-HorizontalAlign="Justify">
					<ItemTemplate>
						<asp:Label runat="server" ID="lblDireccion" Text='<%# ((BPMO.SDNI.Comun.BO.RepresentanteLegalBO)Container.DataItem).DireccionPersona.Calle %>'></asp:Label></ItemTemplate>
					<ItemStyle Width="300px" />
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Folio RPPC" ItemStyle-HorizontalAlign="Justify">
					<ItemTemplate>
						<asp:Label runat="server" ID="lblFolioRPPC" Text='<%# ((BPMO.SDNI.Comun.BO.RepresentanteLegalBO)Container.DataItem).ActaConstitutiva.NumeroRPPC %>'></asp:Label></ItemTemplate>
					<ItemStyle Width="150px" />
				</asp:TemplateField>
			</Columns>
			<HeaderStyle CssClass="GridHeader" />
			<EditRowStyle CssClass="GridAlternatingRow" />
			<PagerStyle CssClass="GridPager" />
			<RowStyle CssClass="GridRow" />
			<FooterStyle CssClass="GridFooter" />
			<SelectedRowStyle CssClass="GridSelectedRow" />
			<AlternatingRowStyle CssClass="GridAlternatingRow" />
		</asp:GridView>
	</div>
    <asp:HiddenField runat="server" ID="hdnBtnActualizar" Value="false" />
</asp:Content>