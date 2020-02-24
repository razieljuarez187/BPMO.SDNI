<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="AgregarDocumentosContratoUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.FSL.UI.AgregarDocumentosContratoUI" %>
    <%-- Satisface al Caso de Uso CU023 - Editar Contrato Full Service Leasing--%>
<%@ Register Src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" TagName="ucCatalogoDocumentosUI" TagPrefix="uc" %>
<%@ Register Src="ucHerramientasFSLUI.ascx" TagName="HerramientasFSLUI" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloContratoFSL.css" rel="stylesheet" type="text/css" />
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
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

            initChild = function() {
                <%= ucHerramientas.ClientID %>_Inicializar();
            };
            $(document).ready(initChild);
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <div style="display: block;">
            <!-- Barra de localización -->
            <div id="BarraUbicacion">
                <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - AGREGAR DOCUMENTOS CONTRATO FULL SERVICE LEASING</asp:Label>
            </div>
            <!--Navegación secundaria-->
            <div style="height: 80px;">
                <!-- Menú secundario -->
                <ul id="MenuSecundario" style="float: left; height: 64px;">
                    <li class="MenuSecundarioSeleccionado">
                        <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Contratos.FSL.UI/ConsultarContratosFSLUI.aspx">
                            CONSULTAR F.S.L.
                            <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                        </asp:HyperLink>
                    </li>
                    <li>
                        <asp:HyperLink ID="hlRegistroOrden" runat="server" NavigateUrl="~/Contratos.FSL.UI/RegistrarContratoFSLUI.aspx">
                            REGISTRAR RENTA F.S.L.
                            <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                        </asp:HyperLink>
                    </li>
                </ul>
                <!-- Barra de herramientas -->
                <uc:herramientasfslui ID="ucHerramientas" runat="server"  />        
            </div>
            <asp:MultiView ID="mvCU023" runat="server" ActiveViewIndex="0">
                <asp:View ID="vwContrato" runat="server">
                    <div id="divHeader" class="GroupBody">
                        <div id="divHeaderBar" class="GroupHeader">
                            <span>Agregar Documentos a Contrato</span>
                            <div class="GroupHeaderOpciones Ancho2Opciones">
                                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btnWizardGuardar"
                                    OnClick="btnGuardar_Click" />
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar"
                                    OnClick="btnCancelar_Click" />
                            </div>                            
                        </div>
                        <div id="divDatosFinales">
                                <fieldset id="fsDocumentosAdjuntos">
                                    <legend>Documentos Adjuntos al Contrato</legend>
                                    <uc:ucCatalogoDocumentosUI ID="ucCatalogoDocumentos" runat="server" />
                                </fieldset>
                            </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </div>

        <div class="ContenedorMensajes" style="display: inline; width: 100%; margin-bottom: 5px; text-align: right; text-transform: uppercase; position:relative; float:right;">
            <span class="Requeridos RequeridosFSL"></span>
            <br />
            <span class="FormatoIncorrecto FormatoIncorrectoFSL"></span>
        </div>
    </div>

    <asp:HiddenField runat="server" ID="hdnContratoID"/>
    <asp:HiddenField runat="server" ID="hdnEstatusContrato"/>
    <asp:HiddenField runat="server" ID="hdnCodigoUltimoContrato"/>
    <asp:HiddenField runat="server" ID="hdnUnidadOperativaContratoID"/>
</asp:Content>