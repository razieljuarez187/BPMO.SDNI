<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
	AutoEventWireup="true" CodeBehind="CerrarContratoFSLUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.FSL.UI.CerrarContratoFSLUI" %>

<%-- Satisface al caso de uso CU026 - Registrar Terminación de Contrato Full Service Leasing --%>
<%@ Register TagPrefix="uc" TagName="herramientasfslui" Src="~/Contratos.FSL.UI/ucHerramientasFSLUI.ascx" %>
<%@ Register TagPrefix="uc" TagName="ucInformacionGeneralUI" Src="~/Contratos.FSL.UI/ucInformacionGeneralUI.ascx" %>
<%@ Register TagPrefix="uc" TagName="ucClienteContratoUI" Src="~/Contratos.FSL.UI/ucClienteContratoUI.ascx" %>
<%@ Register TagPrefix="uc" TagName="ucDatosRentaUI" Src="~/Contratos.FSL.UI/ucDatosRentaUI.ascx" %>
<%@ Register TagPrefix="uc" TagName="ucInformacionPagoUI" Src="~/Contratos.FSL.UI/ucInformacionPagoUI.ascx" %>
<%@ Register TagPrefix="uc" TagName="ucLineaContratoFSLUI" Src="~/Contratos.FSL.UI/ucLineaContratoFSLUI.ascx" %>
<%@ Register TagPrefix="uc" TagName="ucFinalizacionContratoFSLUI" Src="~/Contratos.FSL.UI/ucFinalizacionContratoFSLUI.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href="../Contenido/Estilos/EstiloContratoFSL.css" rel="stylesheet" type="text/css" />
	<!--Funcionalidad Deshabilitar Enter en cajas de texto-->
	<script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>"
		type="text/javascript"></script>
	<script type="text/javascript">
            //Validar campos requeridos
            function ValidatePage(Texto) {
                if (typeof (Page_ClientValidate) == 'function') {
                    Page_ClientValidate();
                }
                if (!Page_IsValid) {
                    MensajeGrowUI("Falta información necesaria para " + Texto, "4");
                    return;
                }
            }

            initChild = function() {
	             $("span:contains('*')").hide();
	             $("#divDatosCierre span:contains('*')").show();
                <%= ucFinalizacionContratoFSL.ClientID %>_Inicializar();
                <%= ucHerramientas.ClientID %>_Inicializar();
            };
            $(document).ready(initChild);
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="PaginaContenido">
		<div style="display: block;">
			<!-- Barra de localización -->
			<div id="BarraUbicacion">
				<asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - EDITAR CONTRATO FULL SERVICE LEASING</asp:Label>
			</div>
			<!--Navegación secundaria-->
			<div style="height: 80px;">
				<!-- Menú secundario -->
				<ul id="MenuSecundario" style="float: left; height: 64px;">
					<li class="MenuSecundarioSeleccionado">
						<asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Contratos.FSL.UI/ConsultarContratosFSLUI.aspx">
                        CONSULTAR F.S.L.
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
						</asp:HyperLink>
					</li>
					<li>
						<asp:HyperLink ID="hlRegistroOrden" runat="server" NavigateUrl="~/Contratos.FSL.UI/RegistrarContratoFSLUI.aspx">
                        REGISTRAR RENTA F.S.L.
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
						</asp:HyperLink>
					</li>
				</ul>
				<!-- Barra de herramientas -->
				<uc:herramientasfslui ID="ucHerramientas" runat="server" />
			</div>
			<asp:MultiView ID="mvCU026" runat="server" ActiveViewIndex="0">
				<asp:View ID="vwContrato" runat="server">
					<div id="divInformacionGeneral" class="GroupBody">
						<div id="divInformacionGeneralHeader" class="GroupHeader">
							<span>Contrato DE Renta FULL SERVICE</span>
							<div class="GroupHeaderOpciones Ancho2Opciones">
								<asp:Button ID="btnGuardar" runat="server" Text="Cerrar" CssClass="btnWizardGuardar"
									OnClick="btnGuardar_Click" />
								<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar"
									OnClick="btnCancelar_Click" />
							</div>
						</div>
						<div id="divInformacionGeneralControles">
							<uc:ucInformacionGeneralUI runat="server" ID="ucInformacionGeneral" />
						</div>
					</div>
					<div id="divDatosCliente" class="GroupBody">
						<div id="divDatosClienteHeader" class="GroupHeader">
							<span>DATOS DE CLIENTE</span>
						</div>
						<div id="divDatosClienteControles">
							<uc:ucClienteContratoUI runat="server" ID="ucClienteContrato" />
						</div>
					</div>
					
						
					
					<uc:ucDatosRentaUI runat="server" ID="ucDatosRenta" />
					<div id="divDatosFinales" class="GroupBody">
						<div id="divDatosFinalesHeader" class="GroupHeader">
							<span>DATOS DE PAGO</span>
						</div>
						<uc:ucInformacionPagoUI runat="server" ID="ucInformacionPago" />
					</div>
					<div id="divDatosCierre" class="GroupBody">
						<div id="divDatosCierreHeader" class="GroupHeader">
							<span>DATOS DE CIERRE</span>
						</div>
					<uc:ucFinalizacionContratoFSLUI runat="server" ID="ucFinalizacionContratoFSL" />
					</div>

				</asp:View>
				<asp:View ID="vwLineaContrato" runat="server">
					<uc:ucLineaContratoFSLUI ID="ucLineaContrato" runat="server" />
				</asp:View>
			</asp:MultiView>
		</div>
		<div class="ContenedorMensajes" style="display: inline; width: 100%; margin-bottom: 5px;
			text-align: right; text-transform: uppercase; position: relative; float: right;">
			<span class="Requeridos RequeridosFSL"></span>
			<br />
			<span class="FormatoIncorrecto FormatoIncorrectoFSL"></span>
		</div>
	</div>
	<asp:HiddenField runat="server" ID="hdnContratoID" />
	<asp:HiddenField runat="server" ID="hdnEstatusContrato" />
	<asp:HiddenField runat="server" ID="hdnCodigoUltimoContrato" />
	<asp:HiddenField runat="server" ID="hdnUnidadOperativaContratoID" />
	<asp:HiddenField runat="server" ID="hdnFechaCierre" />
	<asp:HiddenField runat="server" ID="hdnUsuarioCierre" />
	<asp:HiddenField runat="server" ID="hdnObservacionesCierre" />
</asp:Content>
