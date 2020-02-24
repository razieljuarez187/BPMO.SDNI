<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="ConfigurarFacturacionPSLUI.aspx.cs" Inherits="BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI.ConfigurarFacturacionPSLUI" ValidateRequest="false" %>

<%@ Register Src="~/Facturacion.AplicacionesFacturacion.UI/ucHerramientasPrefacturaUI.ascx"
    TagPrefix="ucHerramientas" TagName="ucHerramientasPrefacturaUI" %>
<%@ Register Src="~/Facturacion.AplicacionesFacturacion.UI/ucInformacionGeneralPSLUI.ascx"
    TagPrefix="ucPagina1" TagName="ucInformacionGeneralPSLUI" %>
<%@ Register Src="~/Facturacion.AplicacionesFacturacion.UI/ucLineasFacturaContratoPSLUI.ascx"
    TagPrefix="ucPagina2" TagName="ucLineasFacturaContratoPSLUI" %>
<%@ Register Src="~/Facturacion.AplicacionesFacturacion.UI/ucCostosAdicionalesFacturaContratoUI.ascx"
    TagPrefix="ucPagina3" TagName="ucCostosAdicionalesFacturaContratoUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloContratoFSL.css" rel="stylesheet" type="text/css" />
    <style>
    #BarraHerramientas {
        width: 832px !important;
        padding-top: .5em;
    }
    
    .GroupBody {
        margin-left: 1.8em;
        width: 780px;
    }
</style>

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            if (document.referrer != null && document.referrer != "") {
                var value = $('#<%= this.hdfPaginaCancelacion.ClientID %>').val();
                if ((value == null || value == "")) {
                    // RI0008
                    $('#<%= this.hdfPaginaCancelacion.ClientID %>').val(document.referrer);
                }

                value = $('#<%= this.hdfPaginaConsultarNuevo.ClientID %>').val();
                if ((value == null || value == "")) {
                    // RI0008
                    $('#<%= this.hdfPaginaConsultarNuevo.ClientID %>').val(document.referrer);
                }
            }

            initChild();
        });

        function initChild() {
            setWizardStep($("#<%=hdnPaginaActual.ClientID %>").val());
        }

        function setWizardStep(stepNumber) {
            $("#<%=lblTituloPaso.ClientID %>").text("Registrar Acta de Nacimiento");

            if (stepNumber == 1) $("#<%=lblTituloPaso.ClientID %>").text("Información Cabecera");
            if (stepNumber == 2) $("#<%=lblTituloPaso.ClientID %>").text("Líneas de Factura");
            if (stepNumber == 3) $("#<%=lblTituloPaso.ClientID %>").text("Observaciones");
            if (stepNumber == 4) $("#<%=lblTituloPaso.ClientID %>").text("Información Cabecera");
            if (stepNumber == 5) $("#<%=lblTituloPaso.ClientID %>").text("Líneas de Factura");
        }

        function abrirConfirmacion(msg, botonID) {
            var $div = $('<div title="Confirmación"></div>');
            $div.append(msg);
            $("#dialog:ui-dialog").dialog("destroy");
            $($div).dialog({
                closeOnEscape: true,
                modal: true,
                minWidth: 460,
                close: function() { $(this).dialog("destroy"); },
                buttons: {
                    Aceptar: function() {
                        $(this).dialog("close");
                        __doPostBack(botonID, "");
                    },
                    Cancelar: function() {
                        $(this).dialog("close");
                    }
                }});
        }

        function BtnBuscarCostos(guid, xml) {
            $('#<%= this.hdnGUID.ClientID %>').val(guid);
            var width = ObtenerAnchoBuscador(xml);

            $.BuscadorWeb({
                xml: xml,
                guid: guid,
                btnSender: $("#<%=btnResultCostos.ClientID %>"),
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
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">FACTURACIÓN - CONFIGURAR FACTURACIÓN</asp:Label>
        </div>
        <!--Navegación secundaria-->

            <!-- Barra de herramientas -->
            <ucHerramientas:ucHerramientasPrefacturaUI id="ucHerramientas" runat="server" />

        <div class="BarraEstaticaWizard">
            <span>Datos de Cliente</span>
            <br />
            <span>Cliente</span>
            <br />
            <asp:TextBox ID="txtNombreCliente" runat="server" Enabled="false" TextMode="MultiLine" style="resize: none;"
                Rows="3"></asp:TextBox>
            <br />
            <span>Fecha De Inicio</span>
            <br />
            <asp:TextBox ID="txtFechaInicio" runat="server" Enabled="false"></asp:TextBox>
            <br />
            <span>Cuenta Cliente Oracle</span>
            <br />
            <asp:TextBox ID="txtCuentaCliente" runat="server" Enabled="false"></asp:TextBox>
            <br />
            <span>RFC</span>
            <br />
            <asp:TextBox ID="txtRFC" runat="server" Enabled="false"></asp:TextBox>
            <br />
            <span>Direcci&oacute;n Cliente</span>
            <br />
            <asp:TextBox ID="txtDireccionCliente" runat="server" Enabled="false" TextMode="MultiLine" style="resize: none;"
                Rows="3"></asp:TextBox>
            <br />
            <br />
            <span class="titulo">Datos de Generaci&oacute;n</span>
            <br />
            <span>Usuario Generador</span>
            <br />
            <asp:TextBox ID="txtUsuarioGenerador" runat="server" Enabled="false"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="btnConsultarOtra" runat="server" UseSubmitBehavior="false" CausesValidation="false" CssClass="btnConsultarOtra" OnClick="btnConsultarOtra_Click" />
        </div>
        <div id="DatosCatalogo" class="GroupBody" style="margin-right: 1.5em; margin-top: 40px; display: inline-table">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader" runat="server">
                <asp:Label ID="lblTituloPaso" runat="server" Text="INFORMACIÓN CABECERA"></asp:Label>
                <div class="GroupHeaderOpciones Ancho3Opciones">
                    <asp:Button ID="btnContinuar" runat="server" Text="Continuar" CausesValidation="false" UseSubmitBehavior="false" CssClass="btnWizardContinuar"
                        OnClick="btnContinuar_Click" ValidationGroup="Obligatorios" />
                    <asp:Button ID="btnTerminar" runat="server" Text="Terminar" CausesValidation="false" UseSubmitBehavior="false" CssClass="btnWizardTerminar"
                        OnClick="btnTerminar_Click" ValidationGroup="Obligatorios" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false" UseSubmitBehavior="false" CssClass="btnWizardCancelar"
                        OnClick="btnCancelar_Click" />
                    <asp:Button ID="btnAnterior" runat="server" Text="Anterior" CausesValidation="false" UseSubmitBehavior="false" CssClass="btnWizardAtras"
                        OnClick="btnAnterior_Click" ValidationGroup="Obligatorios" />
                </div>
            </div>
            <div id="ControlesDatos">
                <asp:MultiView ID="mvCU005" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vwPagina1" runat="server">
                        <ucPagina1:ucInformacionGeneralPSLUI runat="server" ID="ucInformacionGeneralPSLUI" OnFormaPagoChanged="ucInformacionGeneralPSLUI_FormaPagoChanged" />
                    </asp:View>
                    <asp:View ID="vwPagina2" runat="server">
                        <ucPagina2:ucLineasFacturaContratoPSLUI runat="server" ID="ucLineasFacturaContratoPSLUI" />
                    </asp:View>
                    <asp:View ID="vwPagina3" runat="server">
                        <ucPagina3:ucCostosAdicionalesFacturaContratoUI runat="server" ID="ucCostosAdicionalesFacturaContratoUI" OnMonedaChanged="ucCostosAdicionalesUI_MonedaChanged" />
                    </asp:View>                
                </asp:MultiView>
                <br />
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </div>
        <asp:Button runat="server" ID="btnConfirmarEnviarNoFacturado" style="display: none;" OnClick="btnConfirmarEnviarNoFacturado_OnClick"/>
    </div>
    <asp:HiddenField ID="hdnPaginaActual" Value="1" runat="server" />
    <asp:HiddenField ID="hdfPaginaConsultarNuevo" Value="" runat="server" />
    <asp:HiddenField ID="hdfPaginaCancelacion" Value="" runat="server" />
    <asp:HiddenField ID="hdnTipoMensaje" runat="server" />
    <asp:HiddenField ID="hdnMensaje" runat="server" />
    <asp:HiddenField ID="hdnGUID" runat="server" />    
    <asp:HiddenField ID="hdnEnvioFactura" runat="server" Value="NO"/>        
    <asp:Button ID="btnResultCostos" runat="server" Text="Button" OnClick="btnResultCostos_Click" Style="display: none;" />
</asp:Content>
