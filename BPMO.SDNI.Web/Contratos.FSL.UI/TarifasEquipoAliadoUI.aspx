<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TarifasEquipoAliadoUI.aspx.cs"
    Inherits="BPMO.SDNI.Contratos.FSL.UI.TarifasEquipoAliadoUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/Contratos.FSL.UI/ucTarifasFSLUI.ascx" TagName="ucTarifasFSLUI"
    TagPrefix="uc" %>
<%-- 
     Satisface al caso de uso CU015 - Registrar Contrato FULL SERVICE LEASING 
     Satisface al Caso de uso CU022 - Consultar Contrato Full Service Leasing
--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <title>Sistema de Negocio de Idealease</title>
    <!-- hojas de estilo-->
    <asp:Literal ID="ltEstilo" runat="server" Text="<link href='../Contenido/Estilos/EstiloDesarrollo.css' rel='Stylesheet' type='text/css'/>"></asp:Literal>
    <link href="<%= Page.ResolveUrl("~/Contenido/Estilos/Tema.JqueryUI/jquery.ui.all.css") %>" rel="stylesheet" type="text/css" />
	<link href="<%= Page.ResolveUrl("~/Contenido/Estilos/Tema.JqueryUI/jquery-ui-1.9.1.custom.css")%>" rel="stylesheet" type="text/css" />
    <!--Funcionalidad javascript-->
    <script src="<%= Page.ResolveUrl("~/Contenido/Scripts/jquery-1.8.2.js") %>" type="text/javascript"></script>
	<script src="<%= Page.ResolveUrl("~/Contenido/Scripts/jquery-ui-1.9.1.custom.min.js") %>" type="text/javascript"></script>
	<script src="<%= Page.ResolveUrl("~/Contenido/Scripts/jquery.format.1.05.js") %>" type="text/javascript"></script>
    <!-- JS para activar el mensajes GrowUI y Bloqueo de la UI-->
    <script src="<%= Page.ResolveUrl("~/Contenido/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%= Page.ResolveUrl("~/Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
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

            $('.CampoNumero').format({ precision: 4, allow_negative: false, autofix: true });
            $('.CampoMoneda').format({ precision: 4, allow_negative: false, autofix: true });
            $('.CampoNumeroEntero').format({ precision: 0, allow_negative: false, autofix: true });


            window.returnValue = 0;
        });

        //Invoca el cierre del Dialog Contenedor que se encuentra en la página padre
        function closeParentUI(retornaValor) {
            if (retornaValor != undefined && retornaValor != "") {
                window.parent.ejecutarAccion = true;
                window.parent.valorRetornado = retornaValor;
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

            <%= ucTarifasEquipoAdicional.ClientID %>_Inicializar();
        }

        function AplicarTarifas(aplicar) {
            if (aplicar) {
                $('input:text').attr('disabled', false);
                $('input:text').each(function () { $(this).val(''); });
                $('select').each(function () { $(this).children().first().attr('selected', true); });
                $('select').attr('disabled', false);
            } else {
                $('input:text').attr('disabled', true);
                $('input:text').each(function () { $(this).val('0'); });
                $('select').each(function () { $(this).children().last().attr('selected', true); });
                $('select').attr('disabled', true);
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
        .GroupBody
        {
            max-width: 900px !important;
            margin: 20px auto;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="smScriptManejador" EnableScriptGlobalization="true"
        AsyncPostBackTimeout="1080" AsyncPostBackErrorMessage="Se perdió la conexión con el servidor.">
    </asp:ScriptManager>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contenido/Scripts/jIdealease.Base.js") %>"></script>
    <div id="DatosOrden" class="GroupBody">
        <div id="divInformacionGeneral" class="GroupHeader">
            <span>TARIFAS DE EQUIPO ALIADO</span>
            <div class="GroupHeaderOpciones Ancho2Opciones">
                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btnWizardGuardar" ValidationGroup="Requeridos"
                    OnClick="btnAgregar_Click" />
                <input id="btnCancelar" type="submit" value="Cancelar" onclick=" return closeParentUI('');" class="btnWizardCancelar" />
            </div>
        </div>
        <div id="DatosControles">
            <table class="trAlinearDerecha" style="margin: 0px 20px; display: inherit; border: 1px solid transparent;">
                <tr>
                    <td class="tdCentradoVerical">Modelo</td>
                    <td style="width: 20px;">&nbsp;</td>
                    <td class="tdCentradoVerical">
                        <asp:Label ID="lblNombreModelo" runat="server" CssClass="tdValorizado">#Modelo</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVerical">#VIN</td>
                    <td style="width: 20px;">&nbsp;</td>
                    <td class="tdCentradoVerical">
                        <asp:Label ID="lblVIN" runat="server" CssClass="tdValorizado">#Numero de Serie</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>No aplicar Tarifas Adicionales</td>
                    <td style="width: 20px;">&nbsp;</td>
                    <td class="tdCentradoVerical">
                        <asp:CheckBox runat="server" ID="cbSinTarifas" AutoPostBack="True" 
                            oncheckedchanged="cbSinTarifas_CheckedChanged" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="margin-bottom: 5px; margin-left: 5px; margin-right: 5px;">
            <uc:ucTarifasFSLUI ID="ucTarifasEquipoAdicional" runat="server" />
            <asp:UpdatePanel ID="updFake" runat="server">
                <ContentTemplate>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAgregar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
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
