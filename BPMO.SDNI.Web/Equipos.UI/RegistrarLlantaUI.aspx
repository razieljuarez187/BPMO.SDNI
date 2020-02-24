<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="RegistrarLlantaUI.aspx.cs" Inherits="BPMO.SDNI.Equipos.UI.RegistrarLlantaUI" %>

<%--Satisface al CU089 - Bitácora de llantas--%>
<%@ Register Src="~/Equipos.UI/ucLlantaUI.ascx" TagName="ucLlantaUI" TagPrefix="ucLlanta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
     <script src="<%= 
Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" 
type="text/javascript"></script>
    <style type="text/css">
        #MenuSecundario { float: left; height: 64px; }
        #BarraHerramientas { width: 835px; float: right; }
        .GroupBody { margin: 10px auto; width: 761px; }
        .GroupHeader { width: 100%; }
        #ControlesDatos { min-height: 120px; margin-top: 10px; position: inherit; border: 1px solid transparent; }
    </style>
    <script language="javascript" type="text/javascript">
        function abrirConfirmacion(msg) {
            var $div = $('<div title="Confirmación"></div>');
            $div.append(msg);
            $("#dialog:ui-dialog").dialog("destroy");
            $($div).dialog({
                closeOnEscape: true,
                modal: true,
                minWidth: 460,
                close: function () { $(this).dialog("destroy"); },
                buttons: {
                    Aceptar: function () {
                        $(this).dialog("close");
                        __doPostBack("<%= btnRegistrar.UniqueID %>", "");
                    },
                    Cancelar: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
        function BtnBuscar(guid, xml, sender) {
            var width = ObtenerAnchoBuscador(xml);

            $.BuscadorWeb({
                xml: xml,
                guid: guid,
                btnSender: $("#" + sender),
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
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">UNIDADES - REGISTRAR LLANTA</asp:Label>
        </div>
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li>
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Equipos.UI/ConsultarLlantaUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlRegistroOrden" runat="server" NavigateUrl="~/Equipos.UI/RegistrarLlantaUI.aspx">
                        REGISTRAR 
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
            <div class="BarraNavegacionExtra">
                <asp:Button ID="btnNuevoRegistro" runat="server" Text="REINICIAR REGISTRO" OnClick="btnNuevoRegistro_Click" />
            </div>
        </div>

        <div id="DatosCatalogo" class="GroupBody">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <asp:Label ID="lblTituloPaso" runat="server" Text="REGISTRAR LLANTA"></asp:Label>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnRegistrar" runat="server" Text="Registrar" CssClass="btnWizardGuardar" OnClick="btnRegistrar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" OnClick="btnCancelar_Click" />
                </div>
            </div>
            <div id="ControlesDatos">
                <ucLlanta:ucLlantaUI ID="ucLlanta" runat="server" />
            </div>
            <div class="ContenedorMensajes">
                <span class="Requeridos"></span>
                <br />
                <span class="FormatoIncorrecto"></span>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnTipoMensaje" runat="server" />
    <asp:HiddenField ID="hdnMensaje" runat="server" />
</asp:Content>
