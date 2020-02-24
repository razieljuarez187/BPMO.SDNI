<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarContratoSDUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.Mantto.SD.UI.RegistrarContratoSDUI" %>
<%@ Register TagPrefix="uc" TagName="ucCatalogoDocumentosUI" Src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" %>
<%@ Register TagPrefix="uc" TagName="ucContrato" Src="~/Contratos.Mantto.UI/ucContratoManttoUI.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloContratoFSL.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .ContenedorMensajes { margin-bottom: 10px !important; margin-top: -10px !important; }        
        .GroupBody{ margin-right: 25px !important; }
    </style>
    
    <script type="text/javascript">
            initChild = function () {
                <%= ucContratoUI.ClientID %>_Inicializar();
            };
            $(document).ready(initChild);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - REGISTRAR CONTRATO DE SERVICIO DEDICADO</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li>
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Contratos.Mantto.SD.UI/ConsultarContratoSDUI.aspx">
                        CONSULTAR S.D.
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlRegistroOrden" runat="server" NavigateUrl="~/Contratos.Mantto.SD.UI/RegistrarContratoSDUI.aspx">
                        REGISTRAR RENTA S.D.
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <div id="BarraHerramientas">
                <div class="Ayuda" style="top: 0px;">
                    <input id="btnAyuda" type="button" onclick="ShowHelp();" class="btnAyuda" />
                </div>
            </div>
        </div>

        <div id="divInformacionGeneral" class="GroupBody">
            <div id="divInformacionGeneralHeader" class="GroupHeader">
                <span>Contrato DE SERVICIO DEDICADO</span>
                <div class="GroupHeaderOpciones Ancho3Opciones">
                    <asp:Button ID="btnTermino" runat="server" Text="Terminar" CssClass="btnWizardTerminar" OnClick="btnTermino_Click" />
                    <asp:Button ID="btnGuardar" runat="server" Text="Borrador" CssClass="btnWizardBorrador" OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" OnClick="btnCancelar_Click" />
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
</asp:Content>
