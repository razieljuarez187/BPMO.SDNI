<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="DetalleActaNacimientoUI.aspx.cs" Inherits="BPMO.SDNI.Equipos.UI.DetalleActaNacimientoUI" %>
<%-- 
	Satisface al CU079 - Consultar Acta de Nacimiento de Unidad
    Satisface la solicitud de cambio SC0006
--%>
<%@ Register Src="~/Equipos.UI/ucDatosGeneralesUI.ascx" TagName="ucDatosGeneralesUI" TagPrefix="ucPagina1" %>
<%@ Register Src="~/Equipos.UI/ucDatosTecnicosUI.ascx" TagName="ucDatosTecnicosUI" TagPrefix="ucPagina2" %>
<%@ Register Src="~/Equipos.UI/ucNumerosSerieUI.ascx" TagName="ucNumerosSerieUI" TagPrefix="ucPagina3" %>
<%@ Register Src="~/Equipos.UI/ucAsignacionLlantasUI.ascx" TagName="ucAsignacionLlantasUI" TagPrefix="ucPagina4" %>
<%@ Register Src="~/Equipos.UI/ucAsignacionEquiposAliadosUI.ascx" TagName="ucAsignacionEquiposAliadosUI" TagPrefix="ucPagina5" %>
<%@ Register Src="~/Tramites.UI/ucTramitesActivosUI.ascx" TagName="ucTramitesActivosUI" TagPrefix="ucPagina6" %>
<%@ Register Src="~/Equipos.UI/ucResumenActaNacimientoUI.ascx" TagName="ucResumenActaNacimientoUI" TagPrefix="ucPagina7" %>
<%@ Register Src="~/Equipos.UI/ucActaNacimientoUI.ascx" TagName="ucActaNacimientoUI" TagPrefix="ucActaOriginal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href="../Contenido/Estilos/EstiloActaNacimiento.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript" src="../Contenido/Scripts/ObtenerFormatoImporte.js"></script>
	<script language="javascript" type="text/javascript">
	    $(document).ready(function () {
	        initChild();
	    });
	    function initChild() {
	        setWizardStep($("#<%=hdnPaginaActual.ClientID %>").val());

	        $("#dialogActaNacimientoOriginal").dialog({
	            autoOpen: false, width: 900, height: 500, modal: true, resizable: false, draggable: true
	        });

	        $("span:contains('*')").css({ 'display': 'none' });

	        $('.CampoFecha').datepicker({ yearRange: '-100:+10',
	            changeYear: true,
	            changeMonth: true,
	            dateFormat: "dd/mm/yy",
	            buttonImage: '../Contenido/Imagenes/calendar.gif',
	            buttonImageOnly: true,
	            toolTipText: "Fecha de compra",
	            showOn: 'button'
	        });
	        $('.CampoFecha').attr('readonly', true);
	    }
	    function HabilitaFechasDesflote(Habilitar) {
	        $('.CampoFechas').datepicker({ yearRange: '-100:+10',
	            changeYear: true,
	            changeMonth: true,
	            dateFormat: "dd/mm/yy",
	            buttonImage: '../Contenido/Imagenes/calendar.gif',
	            buttonImageOnly: true,
	            toolTipText: "Fecha de compra",
	            showOn: 'button',
	            disabled: Habilitar
	        });
	        $('.CampoFechas').attr('readonly', true);
	    }
	    function openDialog(nombre) {
	        $("#" + nombre).parent().appendTo($("form:first"));
	        $("#" + nombre).dialog('open');
	    }
	</script>
	<script language="javascript" type="text/javascript">
    function InicializarControlesEmpresas(valoresTabs, valoresEtiquetasPrincipales) 
       {
           /// <summary>
           /// Método que inicializa las configuraciones por empresa
           /// </summary>
           /// <param name="valoresTabs">Valores de las pestañas a ocultar</param>
           /// <param name="valoresEtiquetasPrincipales">Valores de las etiquetas a renombrar</param>
           if (valoresTabs.length != 0) {
               var lstElementosTabs = valoresTabs.split(',');
               for (var i = 0; i < lstElementosTabs.length; i++) {
                   document.getElementById("tabsContent").children[lstElementosTabs[i]].style.display = "none";
               }
           }

           if (valoresEtiquetasPrincipales.length != 0) {
               var lstElementosEtiquetasPrincipales = valoresEtiquetasPrincipales.split(',');

               document.getElementById("RE01").innerHTML = lstElementosEtiquetasPrincipales[0];
               document.getElementById("RE02").innerHTML = lstElementosEtiquetasPrincipales[1];
               document.getElementById("RE03").innerHTML = lstElementosEtiquetasPrincipales[2];
               document.getElementById("RE04").innerHTML = lstElementosEtiquetasPrincipales[3];
               document.getElementById("RE05").innerHTML = lstElementosEtiquetasPrincipales[4];
               document.getElementById("RE07").innerHTML = lstElementosEtiquetasPrincipales[5];
               document.getElementById("RE08").innerHTML = lstElementosEtiquetasPrincipales[6];
               document.getElementById("RE09").innerHTML = lstElementosEtiquetasPrincipales[7];
               document.getElementById("RE12").innerHTML = lstElementosEtiquetasPrincipales[8];
           }

       }



	    function setWizardStep(stepNumber) {
	        $(".WizardSteps ul li").removeClass("ActualStep");
	        $(".WizardSteps ul li").removeClass("PastStep");

	        if (stepNumber != "" && $("#wizard-step-" + stepNumber) != null) {
	            $("#wizard-step-" + stepNumber).addClass("ActualStep");
	        }

	        $(".BarraEstaticaWizard").show();

	        for (var i = stepNumber - 1; i > 0; i--) {
	            $("#wizard-step-" + i).addClass("PastStep");
	        }

	        $("#<%=lblTituloPaso.ClientID %>").text("Detalle del Acta de Nacimiento");
	        if (stepNumber == 0) $("#<%=lblTituloPaso.ClientID %>").text("Selección de la Unidad");
	        if (stepNumber == 1) $("#<%=lblTituloPaso.ClientID %>").text("Datos Generales");
	        if (stepNumber == 2) $("#<%=lblTituloPaso.ClientID %>").text("Datos Técnicos");
	        if (stepNumber == 3) $("#<%=lblTituloPaso.ClientID %>").text("Números de Serie");
	        if (stepNumber == 4) $("#<%=lblTituloPaso.ClientID %>").text("Llantas");
	        if (stepNumber == 5) $("#<%=lblTituloPaso.ClientID %>").text("Equipos Aliados");
	        if (stepNumber == 6) $("#<%=lblTituloPaso.ClientID %>").text("Trámites");
	        if (stepNumber == 7) $("#<%=lblTituloPaso.ClientID %>").text("Resumen");

	        $(".WizardSteps ul li").bind('click', function () { jumpToWizardStep(this); });
	    }

	    function jumpToWizardStep(liControl) {
	        var stepNumber = $(liControl).attr('id').replace("wizard-step-", "");
	        $("#<%=hdnPaginaBrinco.ClientID %>").val(stepNumber);
	        $("#<%=btnBrincarPagina.ClientID %>").click();
	    }
    </script>
	<script type="text/javascript">
	    function BtnVisualizar(guid, xml) {
	        var width = ObtenerAnchoBuscador(xml);            

	        $.BuscadorWeb({
	            url: '../Buscador.UI/VisorUI.aspx',
	            xml: xml,
	            guid: guid,
	            btnSender: null,
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="PaginaContenido">
		<!-- Barra de localización -->
		<div id="BarraUbicacion">
			<asp:Label ID="lblEncabezadoLeyenda" runat="server">UNIDADES - CONSULTAR DETALLES DE ACTA DE NACIMIENTO</asp:Label>
		</div>
		<!--Navegación secundaria-->
		<div style="height: 65px;">
			<!-- Menú secundario -->
			<span id="ContenedorMenuSecundario">
				<ul id="MenuSecundario">
					<li id="ConsultarCatalogo" class="MenuSecundarioSeleccionado">
						<asp:HyperLink ID="hlkConsultarActaNacimiento" runat="server" NavigateUrl="~/Equipos.UI/ConsultarActaNacimientoUI.aspx">
							CONSULTAR
							<img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
						</asp:HyperLink>
					</li>
					<li id="RegistrarCatalogo">
						<asp:HyperLink ID="hlkRegistroActaNacimiento" runat="server" NavigateUrl="~/Equipos.UI/RegistrarActaNacimientoUI.aspx">
							REGISTRAR
							<img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
						</asp:HyperLink>
					</li>
				</ul>
			</span>
			<!-- Barra de herramientas -->
		    <!-- RQM 14150, se agrega un identificador a la lista para poder verificar los tabs que se ocultan en generación y construcción -->
			<div id="BarraHerramientas">
				<div class="WizardSteps">
					<ul id="tabsContent">
						<li id="wizard-step-1">1.DATOS GENERALES</li>
						<li id="wizard-step-2">2.DATOS TÉCNICOS</li>
						<li id="wizard-step-3">3.NÚMEROS DE SERIE</li>
						<li id="wizard-step-4">4.LLANTAS</li>
						<li id="wizard-step-5">5.EQUIPOS ALIADOS</li>
						<li id="wizard-step-6">6.TRÁMITES</li>
						<li id="wizard-step-7">7.RESUMEN</li>
					</ul>
				</div>
				<div class="Ayuda" style="top: 0px;">                    
					<input id="btnAyuda" type="button" onclick="ShowHelp();" class="btnAyuda" />
				</div>
			</div>
			<div class="BarraNavegacionExtra">
				<input id="btnNuevoRegistro" type="button" value="Nueva Consulta"
					onclick="window.location='<%= Page.ResolveUrl("~/Equipos.UI/ConsultarActaNacimientoUI.aspx") %>'" />
				<input id="btnActaOriginal" runat="server" type="button" value="Acta Nacimiento Original" onclick="javascript: openDialog('dialogActaNacimientoOriginal'); return false;" />
			</div>
		</div>
	    <!-- RQM 14150, se agrega un identificador a los Span para cambiar los textos de las etiquetas -->
		<div class="BarraEstaticaWizard">
			<span>Datos de Unidad</span>
			<br /><span id="RE01">VIN</span>
			<br /><asp:TextBox ID="txtEstaticoNumSerie" runat="server" Enabled="false"></asp:TextBox>
			<br /><span id="RE02">Clave Activo Oracle</span>
			<br /><asp:TextBox ID="txtEstaticoClaveOracle" runat="server" Enabled="false"></asp:TextBox>
			<br /><span id="RE03">ID Leader</span>
			<br /><asp:TextBox ID="txtEstaticoIDLeader" runat="server" Enabled="false"></asp:TextBox>
			<br /><span id="RE04"># Económico</span>
			<br /><asp:TextBox ID="txtEstaticoNumEconomico" runat="server" Enabled="false"></asp:TextBox>
			<br /><span id="RE05">Tipo Unidad</span>
			<br /><asp:TextBox ID="txtEstaticoTipoUnidad" runat="server" Enabled="false"></asp:TextBox>
			<br /><span id="RE07">Modelo</span>
			<br /><asp:TextBox ID="txtEstaticoModelo" runat="server" Enabled="false"></asp:TextBox>
			<br /><span id="RE08">Año</span>
			<br /><asp:TextBox ID="txtEstaticoAnio" runat="server" Enabled="false"></asp:TextBox>
			<br /><span id="RE09">Fecha Compra</span>
			<br /><asp:TextBox ID="txtEstaticoFechaCompra" runat="server" Enabled="false"></asp:TextBox>
			<br /><span id="RE12">Monto Factura</span>
			<br /><asp:TextBox ID="txtEstaticoMontoFactura" runat="server" Enabled="false"></asp:TextBox>
		    <br />
		    <br /><asp:Button ID="btnActualizarOracle" runat="server" Text="" 
                CssClass="btnActualizar" onclick="btnActualizarOracle_Click"  />
		</div>

		<div id="DatosCatalogo" class="GroupBody">
			<div id="EncabezadoDatosCatalogo" class="GroupHeader">
				<asp:Label ID="lblTituloPaso" runat="server" Text="DATOS GENERALES"></asp:Label>
				<div class="GroupHeaderOpciones Ancho4Opciones">
					<asp:Button ID="btnContinuar" runat="server" Text="Continuar" CssClass="btnWizardContinuar" onclick="btnContinuar_Click" />
					<asp:Button ID="btnHistorial" runat="server" Text="Historial" CssClass="btnWizardHistorial" onclick="btnHistorial_Click" />
					<asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btnWizardEditar" onclick="btnEditar_Click" />
					<asp:Button ID="btnAnterior" runat="server" Text="Anterior" CssClass="btnWizardAtras" onclick="btnAnterior_Click" />
				</div>
			</div>
			<div id="ControlesDatos" style="padding-bottom: 20px;">
				<asp:MultiView ID="mvCU079" runat="server" ActiveViewIndex="0">
					<asp:View ID="vwPagina1" runat="server">
						<ucPagina1:ucDatosGeneralesUI ID="ucDatosGeneralesUI" runat="server" />
					</asp:View>
					<asp:View ID="vwPagina2" runat="server">
						<ucPagina2:ucDatosTecnicosUI ID="ucDatosTecnicosUI" runat="server" />
					</asp:View>
					<asp:View ID="vwPagina3" runat="server">
						<ucPagina3:ucNumerosSerieUI ID="ucNumerosSerieUI" runat="server" />
					</asp:View>
					<asp:View ID="vwPagina4" runat="server">
						<ucPagina4:ucAsignacionLlantasUI ID="ucAsignacionLlantasUI" runat="server" />
					</asp:View>
					<asp:View ID="vwPagina5" runat="server">
						<ucPagina5:ucAsignacionEquiposAliadosUI ID="ucAsignacionEquiposAliadosUI" runat="server" />
					</asp:View>
					<asp:View ID="vwPagina6" runat="server">
						<ucPagina6:ucTramitesActivosUI ID="ucTramitesActivosUI" runat="server" />
					</asp:View>
					<asp:View ID="vwPagina7" runat="server">
                        <%--SC0006 - Los cambios de la solictud de cambio se reflejan en la actualización del control ucResumenActaNacimientoUI--%>
						<ucPagina7:ucResumenActaNacimientoUI ID="ucResumenActaNacimientoUI" runat="server" />
					</asp:View>
				</asp:MultiView>
			</div>
		</div>
	</div>
	
	<asp:HiddenField ID="hdnEquipoId" runat="server" />
	<asp:HiddenField ID="hdnUnidadId" runat="server" />
	<asp:HiddenField ID="hdnMarcaID" runat="server" />
	<asp:HiddenField ID="hdnTipoUnidadID" runat="server" />
	<asp:HiddenField ID="hdnModeloID" runat="server" />

	<asp:HiddenField ID="hdnEstatusUnidad" runat="server" />
	<asp:HiddenField ID="hdnPaginaActual" runat="server" />
	<asp:HiddenField ID="hdnPaginaBrinco" runat="server" />
	<asp:HiddenField ID="hdnTipoMensaje" runat="server" />
	<asp:HiddenField ID="hdnMensaje" runat="server" />
	<asp:HiddenField ID="hdnEmpresaConPermiso" runat="server" />
    <asp:HiddenField ID="hdnValoresTab" runat="server" />

	<asp:Button ID="btnBrincarPagina" runat="server" Text="" onclick="btnBrincarPagina_Click" style="display: none;" />

	<!-- Ventanas de Diálogo -->
	<div id="dialogActaNacimientoOriginal" title="Acta de Nacimiento Original" style="display: none;">
		<ucActaOriginal:ucActaNacimientoUI ID="ucActaNacimientoUI" runat="server" />
	</div>

</asp:Content>
