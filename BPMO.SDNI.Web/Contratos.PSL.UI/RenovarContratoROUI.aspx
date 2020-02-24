<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="RenovarContratoROUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.RenovarContratoROUI" %>

<%@ Register Src="~/Contratos.PSL.UI/ucContratoPSLUI.ascx" TagPrefix="uc" TagName="ucInformacionGeneralPSLUI" %>
<%@ Register Src="~/Contratos.PSL.UI/ucLineaContratoPSLUI.ascx" TagPrefix="uc" TagName="ucLineaContratoPSLUI" %>
<%@ Register TagPrefix="uc" TagName="ucCatalogoDocumentosUI" Src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" %>
<%@ Register TagPrefix="uc" TagName="HerramientasPSLUI" Src="~/Contratos.PSL.UI/ucHerramientasPSLUI.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloContratoFSL.css" rel="stylesheet" type="text/css" />
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <style type="text/css">
        .ContenedorMensajes
        {
            margin-bottom: 10px !important;
            margin-top: -10px !important;
        }
        .style2
        {
            width: 335px;
        }
        
        .GroupBody{ margin-right: 25px !important; }
    </style>
    <script type="text/javascript">
        function confirmarRentaUnidadReservada(origen) {
            var $div = $('<div title="Confirmación"></div>');
            $div.append('La unidad que desea rentar tiene una o más reservaciones para otro cliente que causan conflicto con las fechas del contrato.<br />Si continúa se quitará la unidad de las reservaciones.<br />¿Desea rentar la unidad?');
            $("#dialog:ui-dialog").dialog("destroy");
            $($div).dialog({
                closeOnEscape: true,
                modal: true,
                minWidth: 460,
                close: function () { $(this).dialog("destroy"); },
                buttons: {
                    Aceptar: function () {
                        $(this).dialog("close");
                        if (origen == 'btnTerminoPrevio') {
                            __doPostBack("<%= btnRenovar.UniqueID %>", "");
                        }                        
                    },
                    Cancelar: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
    </script>
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

            initChild = function () {
                <%=ucInformacionGeneralPSL.ClientID %>_Inicializar(true, true);
            };
            $(document).ready(initChild);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - RENOVAR CONTRATO DE RENTA ORDINARIA</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li>
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Contratos.PSL.UI/ConsultarContratoROUI.aspx">
                        CONSULTAR R.O.
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlRegistroOrden" runat="server" NavigateUrl="~/Contratos.PSL.UI/RegistrarContratoROUI.aspx">
                        REGISTRAR RENTA R.O.
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <div id="BarraHerramientas">
                <div class="Ayuda" style="top: 0px;">
                    <input id="btnAyuda" type="button" onclick="ShowHelp();" class="btnAyuda" />
                </div>
                <uc:HerramientasPSLUI ID="ucHerramientas" runat="server" Visible="True"/>
            </div>
        </div>
         <asp:MultiView ID="mvCU015" runat="server" ActiveViewIndex="0">
            <asp:View ID="vwContrato" runat="server">
                <div id="divInformacionGeneral" class="GroupBody">
                    <div id="divInformacionGeneralHeader" class="GroupHeader">
                        <span>Contrato DE RENTA ORDINARIA</span>
                        <div class="GroupHeaderOpciones Ancho3Opciones">
                            <asp:Button ID="btnRenovar" runat="server" Text="Renovar" CssClass="btnWizardTerminar" OnClick="btnRenovar_Click" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" OnClick="btnCancelar_Click" />
                        </div>
                    </div>
                    <div id="divInformacionGeneralControles">
                        <uc:ucInformacionGeneralPSLUI runat="server" ID="ucInformacionGeneralPSL" />
                    </div>
                    <div id="divDatosFinales" class="GroupBody">                    
                        <fieldset id="fsDocumentosAdjuntos">
                            <legend>Documentos Adjuntos al Contrato</legend>
                            <uc:ucCatalogoDocumentosUI ID="ucCatalogoDocumentos" runat="server" />
                        </fieldset>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="vwLineaContrato" runat="server">
                <uc:ucLineaContratoPSLUI runat="server" ID="ucLineaContratoPSLUI"/>
            </asp:View>
        </asp:MultiView>
    </div>
     <div class="ContenedorMensajes">
        <span class="Requeridos RequeridosFSL"></span>
        <br />
        <span class="FormatoIncorrecto FormatoIncorrectoFSL"></span>
    </div>
    <asp:HiddenField ID="hdnUC" runat="server" />
    <asp:HiddenField ID="hdnFC" runat="server" />
    <asp:HiddenField ID="hdnUUA" runat="server" />
    <asp:HiddenField ID="hdnFUA" runat="server" />

</asp:Content>
