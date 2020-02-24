<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="DetalleContratoROUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.DetalleContratoROUI" %>

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
                <%=ucInformacionGeneralPSL.ClientID %>_Inicializar(false);
            };
            $(document).ready(initChild);
    </script>
    <script type="text/javascript">
        function abrirDialogoEliminar() {
            $("#dialogObservacion").dialog({
                autoOpen: true,
                closeOnEscape: true,
                modal: true,
                minWidth: 460,
                minHeight: 250,
                open: function () {
                    $("#<%= txtboxObser.ClientID %>").val("");
                },
                close: function () { $(this).dialog("destroy"); },
                buttons:
                {
                    Aceptar: function () {
                        var text = $("#<%= txtboxObser.ClientID %>").val();
                        if (text.trim() == "") {
                            $("span:contains('*')").show();
                        }
                        else {
                            __doPostBack("<%= btnEliminarContratoBorrador.UniqueID %>", "");
                            $(this).dialog("close");
                        }
                    },
                    Cancelar: function () {
                        $(this).dialog("close");
                    }
                }
            });
            $("#dialogObservacion").parent().appendTo("form:first");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - DETALLE CONTRATO DE RENTA ORDINARIA</asp:Label>
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
                        <div class="GroupHeaderOpciones Ancho2Opciones">
                            <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btnWizardEditar" onclick="btnEditar_Click" />
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
    <%-- Campos Ocultos--%>
    <asp:HiddenField runat="server" ID="hdnLineaContratoID" />
    <asp:HiddenField runat="server" ID="hdnTipoListadoVerificacionPSL" Value="0" />
    <asp:Button runat="server" ID="btnEliminarContratoBorrador" OnClick="btnEliminarContratoBorrador_Click" style="display:none;" />
    <div title="Confirmación" style="display:none" id="dialogObservacion">
        ¿Desea Eliminar el Borrador del Contrato?
        <br />
        <br />
        <span>*</span>Observaciones:
        <asp:TextBox ID="txtboxObser" runat="server" TextMode="MultiLine"
            Style="max-height:90px;height:90px;width:420px;max-width:420px;margin:auto;"></asp:TextBox>
    </div>
</asp:Content>
