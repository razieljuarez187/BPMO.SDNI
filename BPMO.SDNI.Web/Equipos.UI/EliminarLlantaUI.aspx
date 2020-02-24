<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EliminarLlantaUI.aspx.cs"
    Inherits="BPMO.SDNI.Equipos.UI.EliminarLlantaUI" %>

<%@ Register Src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" TagName="ucCatalogoDocumentosUI"
    TagPrefix="uc" %>
<!-- Satisface al Caso de uso CU089 - Bitacora de Llantas -->
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <title>Sistema de Negocio de Idealease</title>
    <!-- hojas de estilo-->
    <asp:Literal ID="ltEstilo" runat="server" Text="<link href='../Contenido/Estilos/EstiloDesarrollo.css' rel='Stylesheet' type='text/css' />"></asp:Literal>
    <link href="../Contenido/Estilos/Tema.JqueryUI/jquery.ui.all.css" rel="stylesheet"
        type="text/css" />
    <link href="../Contenido/Estilos/Tema.JqueryUI/jquery-ui-1.9.1.custom.css" rel="stylesheet"
        type="text/css" />
    <!--Funcionalidad javascript-->
    
    <script src="../Contenido/Scripts/jquery-1.8.2.js" type="text/javascript"></script>
    <script src="../Contenido/Scripts/jquery-ui-1.9.1.custom.min.js" type="text/javascript"></script>
    <!-- JS para activar el mensajes GrowUI y Bloqueo de la UI-->
    <script src="../Contenido/Scripts/jquery.blockUI.js" type="text/javascript"></script>
	<!--Funcionalidad Deshabilitar Enter en cajas de texto-->
	<script src="<%= 
Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            initClient();
            window.returnValue = 0;
        });

        function checkText(campo, maxlength) {
            if (campo.value.length >= maxlength) {
                campo.value = campo.value.substring(0, maxlength);
                return false;
            }
        }

        function Cerrar(confirmado) {
            //confirmado = 1 - Si elimino
            //confirmado = 0 - No elimino
            window.returnValue = confirmado;
            window.close();
            return false;
        }

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
    <style type="text/css">
        #divContenidoPrincipal
        {
            width: 940px;
            margin: 0px auto;
        }
        #ControlesDatos .trAlinearDerecha { width: 95%; margin: 0px auto; display: inherit; }
    </style>
</head>
<body>
    <div id="divContenidoPrincipal">
        <form id="form1" runat="server">
            <asp:ScriptManager runat="server" ID="smScriptManejador" EnableScriptGlobalization="true"
                AsyncPostBackTimeout="1080" AsyncPostBackErrorMessage="Se perdió la conexión con el servidor.">
            </asp:ScriptManager>
            <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contenido/Scripts/jIdealease.Base.js") %>"></script>
            <div id="DatosCatalogo" class="GroupBody" style="display: inherit; margin-top: 35px;">
                <div id="divInformacionGeneral" class="GroupHeader">
                    <span>UNIDADES - ELIMINAR LLANTA</span>
                    <div class="GroupHeaderOpciones Ancho2Opciones">
                        <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar" CssClass="btnWizardGuardar" OnClick="btnConfirmar_Click" />
                        <input id="btnCancelar" type="submit" value="Cancelar" onclick=" return Cerrar(0);" class="btnWizardCancelar" />
                    </div>
                </div>
                <div id="ControlesDatos" style="display: inherit; margin: 10px auto;">
                    <uc:ucCatalogoDocumentosUI ID="DocumentosEliminar" runat="server" />
                </div>
            </div>
            <!-- Campos Ocultos -->
            <asp:UpdatePanel ID="updMensaje" runat="server">
                <ContentTemplate>
                    <asp:HiddenField runat="server" ID="hdnDetalle" />
                    <asp:HiddenField runat="server" ID="hdnMensaje" />
                    <asp:HiddenField runat="server" ID="hdnTipoMensaje" Value="0" />
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
    </div>
</body>
</html>
