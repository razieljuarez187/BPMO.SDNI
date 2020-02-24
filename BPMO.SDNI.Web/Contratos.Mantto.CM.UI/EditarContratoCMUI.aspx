<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="EditarContratoCMUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.Mantto.CM.UI.EditarContratoCMUI" %>
<%@ Register TagPrefix="uc" TagName="ucContratoManttoUI" Src="~/Contratos.Mantto.UI/ucContratoManttoUI.ascx" %>
<%@ Register TagPrefix="uc" TagName="ucCatalogoDocumentosUI" Src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" %>
<%@ Register TagPrefix="uc" TagName="ucHerramientasCMUI" Src="~/Contratos.Mantto.CM.UI/ucHerramientasCMUI.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloContratoFSL.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .ContenedorMensajes { margin-bottom: 10px !important; margin-top: -10px !important; }        
        .GroupBody{ margin-right: 25px !important; }
    </style>
    
    <script type="text/javascript">
            initChild = function () {
                <%= ucucContratoManttoUI.ClientID %>_Inicializar();
                <%= ucHerramientas.ClientID %>_Inicializar();
            };
            $(document).ready(initChild);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">    
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - EDITAR CONTRATO DE MANTENIMIENTO</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
               <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Contratos.Mantto.CM.UI/ConsultarContratoCMUI.aspx">
                        CONSULTAR C.M.
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="hlRegistroOrden" runat="server" NavigateUrl="~/Contratos.Mantto.CM.UI/RegistrarContratoCMUI.aspx">
                        REGISTRAR RENTA C.M.
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <uc:ucHerramientasCMUI ID="ucHerramientas" runat="server" />
        </div>
        <div id="divInformacionGeneral" class="GroupBody">
            <div id="divInformacionGeneralHeader" class="GroupHeader">
                <span>CONTRATO DE MANTENIMIENTO</span>
                <div class="GroupHeaderOpciones Ancho3Opciones">
                    <asp:Button ID="btnTerminoPrevio" runat="server" Text="Terminar" 
                        CssClass="btnWizardTerminar" onclick="btnTerminoPrevio_Click" />
                    <asp:Button ID="btnGuardarPrevio" runat="server" Text="Borrador" 
                        CssClass="btnWizardBorrador" onclick="btnGuardarPrevio_Click"/>
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
                        CssClass="btnWizardCancelar" onclick="btnCancelar_Click"/>
                </div>
            </div>
            <div id="divInformacionGeneralControles">
                <uc:ucContratoManttoUI runat="server" ID="ucucContratoManttoUI" />
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
