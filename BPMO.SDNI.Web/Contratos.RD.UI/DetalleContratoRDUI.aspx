<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="DetalleContratoRDUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.RD.UI.DetalleContratoRDUI" %>

<%-- Satisface al Caso de Uso CU003 - Consultar Contrato Renta Diaria--%>
<%@ Register Src="ucResumenContratoRDUI.ascx" TagName="ucResuContratoRDUI" TagPrefix="uc" %>
<%@ Register Src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" TagName="ucCatalogoDocumentosUI" TagPrefix="uc" %>
<%@ Register Src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" TagName="ucCatalogoDocumentosEntregaUI" TagPrefix="uc" %>
<%@ Register Src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" TagName="ucCatalogoDocumentosRecepcionUI" TagPrefix="uc" %>
<%@ Register Src="~/Flota.UI/ucEquiposAliadosUnidadUI.ascx" TagPrefix="uc" TagName="ucEquiposAliadosUnidadUI" %>
<%@ Register Src="~/Flota.UI/ucDatosGeneralesElementoUI.ascx" TagPrefix="uc" TagName="ucDatosGeneralesElementoUI" %>
<%@ Register Src="ucHerramientasRDUI.ascx" TagName="HerramientasRDUI" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #BarraHerramientas
        {
            width: 832px !important;
        }
    </style>
    <link href="../Contenido/Estilos/EstiloContratoFSL.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        initChild = function () {
            $("span:contains('*')").hide();
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
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - CONSULTAR DETALLES DE CONTRATO RENTA DIARIA</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 64px;">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Contratos.RD.UI/ConsultarContratoRDUI.aspx">
                        CONSULTAR R.D.
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="hlRegistroOrden" runat="server" NavigateUrl="~/Contratos.RD.UI/RegistrarContratoRDUI.aspx">
                        REGISTRAR RENTA R.D. 
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <uc:HerramientasRDUI ID="ucHerramientas" runat="server" />
        </div>
        <!--espacio uc informacion contrato -->
        <div class="GroupBody">
            <div class="GroupHeader">
                <asp:Label ID="lblTituloContrato" runat="server" Text="Informaci&oacute;n General"></asp:Label>
                <%-- SC0020--%>
                <div class="GroupHeaderOpciones Ancho1Opciones" id = "dvHdrButton" runat="server">
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" cssClass="btnWizardRegresar" OnClick="btnRegresarConsultaF_Click"/>
                </div>
                <%-- SC0020--%>
            </div>
            <div id="divInformacionGeneralControles">
                <uc:ucResuContratoRDUI ID="ucResuContratoRD" runat="server" />
            </div>
        </div>
        <div class="GroupBody">
            <div class="GroupHeader">
                <asp:Label ID="lblTituloUnidad" runat="server" Text="INFORMACI&Oacute;N DE LA UNIDAD"></asp:Label>
            </div>
            <uc:ucDatosGeneralesElementoUI ID="ucDatosGeneralesElementoUI" runat="server" />
        </div>
        <div class="GroupBody">
            <div class="GroupHeader">
                <asp:Label ID="lblTituloEquipoAliado" runat="server" Text="INFORMACI&Oacute;N DE EQUIPOS ALIADOS"></asp:Label>
            </div>
            <uc:ucEquiposAliadosUnidadUI ID="ucEquiposAliadosUnidadUI" runat="server" />
        </div>
        <div id="divDatosFinales" class="GroupBody">
            <fieldset id="fsDocumentosAdjuntos">
                <legend>Documentos Adjuntos al Contrato</legend>
                <uc:ucCatalogoDocumentosUI ID="ucCatalogoDocumentos" runat="server" />
            </fieldset>
            <fieldset id="fsCheckListEntrega">
                <legend>Documentos del ChekList de Entrega</legend>
                <uc:ucCatalogoDocumentosEntregaUI ID="ucCatalogoDocumentosEntrega" runat="server" />
            </fieldset>
            <fieldset id="fsCheckListRecepcion">
                <legend>Documentos del ChekList de Recepci&oacute;n</legend>
                <uc:ucCatalogoDocumentosRecepcionUI ID="ucCatalogoDocumentosRecepcion" runat="server" />
            </fieldset>
        </div>
    </div>
    <asp:Button runat="server" ID="btnEliminarContratoBorrador" OnClick="btnEliminarContratoBorrador_Click" Style="display: none;" />
    <asp:HiddenField runat="server" ID="hdnContratoID" />
    <asp:HiddenField runat="server" ID="hdnEstatusID" />
    <asp:HiddenField runat="server" ID="hdnFUA" />
    <asp:HiddenField runat="server" ID="hdnUUA" />
    <asp:HiddenField runat="server" ID="hdnCancelable" />
    <asp:HiddenField runat="server" ID="hdnCerrable" />
    <div title="Confirmación" style="display: none" id="dialogObservacion">
        ¿Desea Eliminar el Borrador del Contrato?
        <br />
        <br />
        <span>*</span>Observaciones:
        <asp:TextBox ID="txtboxObser" runat="server" TextMode="MultiLine" Style="max-height: 90px;
            height: 90px; width: 420px; max-width: 420px; margin: auto;"></asp:TextBox>
    </div>
</asp:Content>
