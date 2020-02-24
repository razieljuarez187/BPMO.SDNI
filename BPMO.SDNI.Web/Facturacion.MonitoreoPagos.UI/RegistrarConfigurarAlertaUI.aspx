<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarConfigurarAlertaUI.aspx.cs" Inherits="BPMO.SDNI.Facturacion.MonitoreoPagos.UI.RegistrarConfigurarAlertaUI" %>
<%@ Register src="ucConfigurarAlertaUI.ascx" tagname="ucConfigurarAlertaUI" tagprefix="uc1" %>

<%--Satisface el caso de uso CU009 – Configuración Notificación de facturación--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #MenuSecundario { float: left; height: 64px; }
        #BarraHerramientas { width: 835px; float: right; }
        .GroupBody { margin: 10px auto; width: 761px; }
        .GroupHeader { width: 100%; }
        #ControlesDatos { min-height: 120px; margin-top: 10px; position: inherit; border: 1px solid transparent; }
    </style>

    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
      
        function BtnBuscar(guid, xml, senderId) {
            if (!senderId)
                throw new Error("No se ha definido el objeto postback para gestionar el resultado de búsqueda");

            var width = ObtenerAnchoBuscador(xml);
            var height = "320px";

            var sender = $("#" + senderId);

            $.BuscadorWeb({
                xml: xml,
                guid: guid,
                btnSender: sender,
                features: {
                    dialogWidth: width,
                    dialogHeight: height,
                    center: 'yes',
                    maximize: '0',
                    minimize: 'no'
                }
            });
        }        
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">CONFIGURACIÓN DE NOTIFICACIÓN DE ALERTA - REGISTRAR EMPLEADO PARA RECIBIR NOTIFICACIÓN</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
        <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li id="ConsultarCatalogo">
                    <asp:HyperLink ID="hlkConsultar" runat="server" NavigateUrl="~/Facturacion.MonitoreoPagos.UI/ConsultarConfigurarAlertaUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkRegistro" runat="server" NavigateUrl="~/Facturacion.MonitoreoPagos.UI/RegistrarConfigurarAlertaUI.aspx">
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
        </div>
        <div id="DatosCatalogo" class="GroupBody">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <asp:Label ID="lblTituloPaso" runat="server" Text="DATOS GENERALES"></asp:Label>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btnWizardGuardar" ValidationGroup="ActualizarRegistro" onclick="btnGuardar_Click"/>                    
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" CausesValidation="false" UseSubmitBehavior="false" onclick="btnCancelar_Click"  />
                </div>
            </div>
            <div id="ControlesDatos">
                <uc1:ucConfigurarAlertaUI ID="ucConfigurarAlerta" runat="server" />
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
