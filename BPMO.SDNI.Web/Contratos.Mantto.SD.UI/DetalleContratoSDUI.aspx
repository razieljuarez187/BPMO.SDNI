<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="DetalleContratoSDUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.Mantto.SD.UI.DetalleContratoSDUI" %>
 <%--Satisface al caso de uso CU029 - Consultar Contratos de mantenimiento --%>
<%@ Register TagPrefix="uc" TagName="ucCatalogoDocumentosUI" Src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" %>
<%@ Register TagPrefix="uc" TagName="ucContrato" Src="~/Contratos.Mantto.UI/ucContratoManttoUI.ascx" %>
<%@ Register tagprefix="uc" TagName="ucHerramientasSDUI" Src="ucHerramientasSDUI.ascx" %>
<%--Satisface al caso de uso CU029 - Consultar Contratos de mantenimiento --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloContratoFSL.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .ContenedorMensajes { margin-bottom: 10px !important; margin-top: -10px !important; }        
        .GroupBody{ margin-right: 25px !important; }
        
        #BarraHerramientas { width: 832px !important;}
    </style>
    
    <script type="text/javascript">
            initChild = function () {
            $("span:contains('*')").hide();
                <%= ucContratoUI.ClientID %>_Inicializar();
                <%= ucHerramientas.ClientID %>_Inicializar();
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
                    $("#<%= txtObservacionesEliminacion.ClientID %>").val("");
                },
                close: function () { $(this).dialog("destroy"); },
                buttons:
                {
                    Aceptar: function () {
                        var text = $("#<%= txtObservacionesEliminacion.ClientID %>").val();
                        if (text.trim() == "") {
                            $("span:contains('*')").show();
                        }
                        else {
                            __doPostBack("<%= btnEliminarContrato.UniqueID %>", "");
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
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - DETALLE CONTRATO DE SERVICIO DEDICADO</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultar" runat="server" NavigateUrl="~/Contratos.Mantto.SD.UI/ConsultarContratoSDUI.aspx">
                        CONSULTAR S.D.
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="hlkRegistro" runat="server" NavigateUrl="~/Contratos.Mantto.SD.UI/RegistrarContratoSDUI.aspx">
                        REGISTRAR RENTA S.D.
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
            <uc:ucHerramientasSDUI ID="ucHerramientas" runat="server" />
        </div>

        <div id="divInformacionGeneral" class="GroupBody">
            <div id="divInformacionGeneralHeader" class="GroupHeader">
                <span>CONTRATO DE SERVICIO DEDICADO</span>
                <div class="GroupHeaderOpciones Ancho1Opciones">
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" cssClass="btnWizardRegresar" OnClick="btnRegresar_Click"/>
                </div>
            </div>
            <div id="divInformacionGeneralControles">
                <uc:ucContrato ID="ucContratoUI" runat="server" />
                <fieldset id="fsDocumentosAdjuntos">
                    <legend>Documentos Adjuntos al Contrato</legend>
                    <uc:ucCatalogoDocumentosUI ID="ucCatalogoDocumentos" runat="server" />
                </fieldset>
            </div>
        </div>
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

    <div title="Confirmación" style="display: none" id="dialogObservacion">
        ¿Desea Eliminar el Borrador del Contrato?
        <br />
        <br />
        <span>*</span>Observaciones:
        <asp:TextBox ID="txtObservacionesEliminacion" runat="server" TextMode="MultiLine" Style="max-height: 90px;
            height: 90px; width: 420px; max-width: 420px; margin: auto;"></asp:TextBox>
    </div>
    <asp:Button runat="server" ID="btnEliminarContrato" OnClick="btnEliminarContrato_Click" Style="display: none;" />
</asp:Content>
