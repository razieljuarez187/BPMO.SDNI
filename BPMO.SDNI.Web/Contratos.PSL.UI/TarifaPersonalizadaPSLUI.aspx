<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TarifaPersonalizadaPSLUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.TarifaPersonalizadaPSLUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%-- 
     Satisface al caso de uso CU015 - Registrar Contrato FULL SERVICE LEASING 
     Satisface al Caso de uso CU022 - Consultar Contrato Full Service Leasing
--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <title>Personalizar Tarifa</title>
    <!-- hojas de estilo-->
    <asp:Literal ID="ltEstilo" runat="server" Text="<link href='../Contenido/Estilos/EstiloDesarrollo.css' rel='Stylesheet' type='text/css'/>"></asp:Literal>
    <link href="<%= Page.ResolveUrl("~/Contenido/Estilos/Tema.JqueryUI/jquery.ui.all.css") %>" rel="stylesheet" type="text/css" />
	<link href="<%= Page.ResolveUrl("~/Contenido/Estilos/Tema.JqueryUI/jquery-ui-1.9.1.custom.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%= Page.ResolveUrl("~/Contenido/Estilos/EstiloContratoFSL.css")%>" rel="stylesheet" type="text/css" />
    <!--Funcionalidad javascript-->
    <script src="<%= Page.ResolveUrl("~/Contenido/Scripts/jquery-1.8.2.js") %>" type="text/javascript"></script>
	<script src="<%= Page.ResolveUrl("~/Contenido/Scripts/jquery-ui-1.9.1.custom.min.js") %>" type="text/javascript"></script>
	<script src="<%= Page.ResolveUrl("~/Contenido/Scripts/jquery.format.1.05.js") %>" type="text/javascript"></script>
    <!-- JS para activar el mensajes GrowUI y Bloqueo de la UI-->
    <script src="<%= Page.ResolveUrl("~/Contenido/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%= Page.ResolveUrl("~/Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("~/Contenido/Scripts/ObtenerFormatoImporte.js") %>" type="text/javascript"></script>  
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            initClient();

            Contador($("#<%=hdnContadorSession.ClientID %>").selector, $("#<%=hdnIniContFinSession.ClientID %>").selector);

            $('ul.MenuEstiloEstatico > li').click(function (event) {
                var evento = event.target;
                var texto = $(this).find('a.MenuEstiloElementoEstatico');
                if (texto.attr('href') != '#') {
                    window.location = texto.attr('href') + '?MenuSeleccionado=' + texto.text();
                    return false;
                }
                if (this != evento) {
                    if ($(evento).is('a')) {
                        window.location = $(evento).attr('href') + '?MenuSeleccionado=' + texto.text();
                    }
                    return false;
                }
            });

            $('.CampoNumero').format({ precision: 2, allow_negative: false, autofix: true });
            $('.CampoMoneda').format({ precision: 4, allow_negative: false, autofix: true });
            $('.CampoNumeroEntero').format({ precision: 0, allow_negative: false, autofix: true });
        });

        //Invoca el cierre del Dialog Contenedor que se encuentra en la página padre
        function closeParentUI(ejecutarFunc) {
            if (ejecutarFunc != undefined && ejecutarFunc != "") {
                window.parent.ejecutarAccion = true;
            }
            $('#closeDialog', parent.document).click();
        }
    </script>
    <script language="javascript" type="text/javascript">
        function initClient() {
            if ($("#<%=hdnTipoMensaje.ClientID %>").val() == "1") {
                mostrarMensaje("1");
            }
            if ($("#<%=hdnTipoMensaje.ClientID %>").val() == "2") {
                mostrarMensaje("2");
            }
            if ($("#<%=hdnTipoMensaje.ClientID %>").val() == "3") {
                MensajeError($("#<%=hdnMensaje.ClientID %>").val(), $("#<%=hdnDetalle.ClientID %>").val());
            }
            if ($("#<%=hdnTipoMensaje.ClientID %>").val() == "4") {
                mostrarMensaje("4");
            }
            
        }
    </script>
    <script language="javascript" type="text/javascript">
        //Mensaje GrowUI
        function mostrarMensaje(tipo) {
            var message = $("#<%=hdnMensaje.ClientID %>").val();
            MensajeGrowUI(message, tipo);
        }

        //Limpiar Hidden de Mensajes
        function LimpiarHdn() {
            $("#<%= hdnTipoMensaje.ClientID %>").val("0");
            $("#<%= hdnMensaje.ClientID %>").val("");
            $("#<%= hdnDetalle.ClientID %>").val("");
        }

        function clickNotificacion(btnId) {
            $('#' + btnId + '').click();
        }

        //Validar campos requeridos
        function ValidatePage() {
            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate();
            }
            if (!Page_IsValid) {
                MensajeGrowUI("Falta información necesaria para aplicar las tarifas del equipo aliado", "4");
                return;
            }
        }
    </script>
    <style type="text/css">  
        
    #divDatosTarifa fieldset { display: inherit; width: 95%; margin-top: 10px; margin-bottom: 13px; margin-left: 2%; margin-right: 2%; }
    #divDatosTarifa .trAlinearDerecha { margin: 5px auto 0px auto; width: 98%; }
    #divDatosTarifa .dvIzquierda { float: left; width: 49%; }
    #divDatosTarifa .dvIzquierda .trAlinearDerecha { margin: 0px 0px 0px auto; }
    #divDatosTarifa .dvDerecha { float: right; width: 49%; }
    #divDatosTarifa .dvDerecha .trAlinearDerecha { margin: 0px auto 0px 0px; }   
          
    .lblDescuentoDerecha { width: 40%; text-align:left; padding-left:10px; vertical-align:text-bottom; font-size:9px; color:Red; }
    .ContenedorMensajes
        {
            margin-bottom: 10px !important;
            margin-top: -10px !important;
        }
        .style2
        {
            width: 335px;
        }
        
        .GroupBody{ margin: 20px !important; }
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="smScriptManejador" EnableScriptGlobalization="true"
        AsyncPostBackTimeout="1080" AsyncPostBackErrorMessage="Se perdió la conexión con el servidor.">
    </asp:ScriptManager>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contenido/Scripts/jIdealease.Base.js") %>"></script>

<div id="Div1" class="GroupBody">
    <div id="div2" class="GroupHeader">
        <asp:Label runat="server" ID="lblTitulo" Text="Personalizar Tarifa"></asp:Label>
        
    </div>
    <div id="divDatosTarifa">
        <fieldset>
		<legend>Tarifas</legend>
		<div class="dvIzquierda">
			<table class="trAlinearDerecha" style="width:350px">
				<tr>
					<td class="tdCentradoVertical">
						<span>*</span>Tarifa
					</td>
					<td style="width: 20px;">
						&nbsp;
					</td>
					<td class="tdCentradoVertical">
						<asp:TextBox runat="server" ID="txtTarifaPersonalizadaTarifa" MaxLength="15" AutoPostBack="true"
							Width="100px" CssClass="CampoNumero" ontextchanged="txtTarifaPersonalizadaTarifa_TextChanged" ></asp:TextBox>
					</td>
				</tr>
                <tr>
					<td class="tdCentradoVertical">
						<span>*</span>Porcentaje de descuento a aplicar
					</td>
					<td style="width: 20px;">
						&nbsp;
					</td>
					<td class="tdCentradoVertical">
						<asp:TextBox runat="server" ID="txtTarifaPersonalizadaPorcentajeDescuento" MaxLength="6" AutoPostBack="true"
							Width="100px" CssClass="CampoNumero" OnTextChanged="txtTarifaPersonalizadaPorcentajeDescuento_OnTextChanged" onkeypress="return descuentoMaximo(event, this);">0.00</asp:TextBox>
					</td>
				</tr>	
                <tr>
					<td class="tdCentradoVertical">
						<span>*</span>Tarifa con descuento
					</td>
					<td style="width: 20px;">
						&nbsp;
					</td>
					<td class="tdCentradoVertical">
						    <asp:TextBox runat="server" ID="txtTarifaPersonalizadaTarifaConDescuento" MaxLength="15"
							    Width="100px" CssClass="CampoNumero" Enabled="false" ></asp:TextBox>
                           
					</td>
				</tr>				
				<tr>
					<td class="tdCentradoVertical">
						C&oacute;digo de Autorizaci&oacute;n</td>
					<td style="width: 20px;">
						&nbsp;</td>
					<td class="tdCentradoVertical">
						<asp:TextBox runat="server" ID="txtTarifaPersonalizadaCodigoAutorizacion" MaxLength="15"
							Width="100px" Enabled="False"></asp:TextBox>
					</td>
				</tr>               
			</table>
            <asp:Label runat="server" ID="lblTarifaPersonalizadaEtiqueta" CssClass="lblDescuentoDerecha" />    
		</div>
		<div class="dvDerecha">
			<table class="trAlinearDerecha" style="width: 350px">
                <tr>
					<td class="tdCentradoVertical">
						<span>*</span>Tipo de Tarifa
					</td>
					<td style="width: 20px;">
						&nbsp;
					</td>
					<td class="tdCentradoVertical">
						<asp:TextBox runat="server" ID="txtTarifaPersonalizadaTipoTarifa" MaxLength="5" Width="150px"
							 Enabled="false" ></asp:TextBox>
					</td>
				</tr>	
                <tr>
					<td class="tdCentradoVertical">
						<span>*</span>Turno</td>
					<td style="width: 20px;">
						&nbsp;
					</td>
					<td class="tdCentradoVertical">
						<asp:TextBox runat="server" ID="txtTarifaPersonalizadaTurno" MaxLength="5" Width="150px"
							 Enabled="false" ></asp:TextBox>
					</td>
				</tr>						
				<tr>
					<td class="tdCentradoVertical">
						<span>*</span>Tarifa Hora Adicional
					</td>
					<td style="width: 20px;">
						&nbsp;
					</td>
					<td class="tdCentradoVertical">
						<asp:TextBox runat="server" ID="txtTarifaPersonalizadaTarifaHrAdicional" MaxLength="15"
							Width="100px" CssClass="CampoNumero" Enabled="false" ></asp:TextBox>
					</td>
				</tr>
                <tr>
					<td class="tdCentradoVertical">
						Porcentaje Seguro
					</td>
					<td style="width: 20px;">
						&nbsp;
					</td>
					<td class="tdCentradoVertical">
						<asp:TextBox runat="server" ID="txtTarifaPersonalizadaPorcentajeSeguro" MaxLength="15"
							Width="100px" CssClass="CampoNumero" Enabled="false" ></asp:TextBox>
					</td>
				</tr>
                <tr>
					<td class="tdCentradoVertical">
						Seguro
					</td>
					<td style="width: 20px;">
						&nbsp;
					</td>
					<td class="tdCentradoVertical">
						<asp:TextBox runat="server" ID="txtTarifaPersonalizadaSeguro" MaxLength="15"
							Width="100px" CssClass="CampoNumero" Enabled="false" ></asp:TextBox>
					</td>
				</tr>
			</table>
			<br />
            
            <asp:Button runat="server" Text="Solicitar" ID="btnSolicitarAutorizacion" CssClass="btnWizardContinuar"
                onclick="btnSolicitarAutorizacion_Click" />
            
			<asp:button runat="server" ID="btnValidarAutorizacion" Text="Validar"
				onclick="btnValidarAutorizacion_Click" CssClass="btnWizardGuardar" /> 
                
            <asp:Button runat="server" ID="btnCancelarCambioTarifa" Text="Cancelar"
                CssClass="btnWizardCancelar" OnClientClick="closeParentUI('');"/>
                    
		</div>
	</fieldset>
    <asp:HiddenField ID="hdnCodigoAutorizacion" runat="server" />
    <asp:HiddenField ID="hdnEstatusAutorizacion" runat="server" />
    <asp:HiddenField ID="hdnTarifaPersonalizaDescuentoMax" runat="server" />
    <asp:HiddenField ID="hdnUnidadOperativaID" runat="server" />
    <asp:HiddenField ID="hdnModeloID" runat="server" />
    <asp:HiddenField ID="hdnSucursalID" runat="server" />
    <asp:HiddenField ID="hdnModuloID" runat="server" />
    <asp:HiddenField ID="hdnUsuarioID" runat="server" />
    <asp:HiddenField ID="hdnCuentaClienteID" runat="server" />
    <asp:HiddenField ID="hdnTarifaAlza" runat="server" />
    <asp:HiddenField ID="hdnTarifaBase" runat="server" />
    <asp:HiddenField ID="hdnDescuentoBase" runat="server" />
    </div>
    <br />
</div>
    
    <!-- Campos Ocultos -->
    <asp:UpdatePanel ID="updMensaje" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnDetalle" />
            <asp:HiddenField runat="server" ID="hdnMensaje" />
            <asp:HiddenField runat="server" ID="hdnTipoMensaje" Value="0"/>  
            <asp:HiddenField runat="server" ID="hdnMostrarMenu" Value="True" />
            <asp:HiddenField runat="server" ID="hdnContadorSession" Value="" />
            <asp:HiddenField runat="server" ID="hdnIniContFinSession" Value="" />
            <asp:HiddenField runat="server" ID="hdnSessionKey" Value="" />
            <asp:HiddenField runat="server" ID="hdnMenuSeleccionado" Value="Inicio" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Ventana de Diálogo para mostrar los mensajes de: Información, Advertencia y Éxito -->
    <div id="dialog-confirm" title="Confirmación" style="display: none;">
        <div id="dMensajeConfirmacion">
        </div>
    </div>
    </form>
</body>
</html>
