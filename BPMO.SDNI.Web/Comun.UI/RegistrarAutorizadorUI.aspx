<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarAutorizadorUI.aspx.cs" Inherits="BPMO.SDNI.Comun.UI.RegistrarAutorizadorUI" %>
<%-- 
    Satisface al caso de uso CU098 - Configurar Autorizadores para los Contratos
--%>
<%@ Register Src="~/Comun.UI/ucAutorizadorUI.ascx" TagName="ucAutorizadorUI" TagPrefix="ucAutorizador" %>
<%@ Register TagPrefix="uc" Namespace="BPMO.SDNI.Comun.UI" Assembly="BPMO.SDNI.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
    #MenuSecundario { float: left; height: 64px; }
        #BarraHerramientas { width: 835px; float: right; }
        .GroupBody { margin: 10px auto; width: 761px; }
        .GroupHeader { width: 100%; }
        #ControlesDatos { min-height: 120px; margin-top: 10px; position: inherit; border: 1px solid transparent; }
</style>
<!--Funcionalidad Deshabilitar Enter en cajas de texto-->
<script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () { initChild(); });

    function initChild() {
        inicializeHorizontalPanels();
    }
    function inicializeHorizontalPanels() {
        $(".GroupHeaderCollapsable").click(function () {
            $(this).next(".GroupContentCollapsable").slideToggle(500);
            if ($(this).find(".imgMenu").attr("src") == "../Contenido/Imagenes/FlechaArriba.png")
                $(this).find(".imgMenu").attr("src", "../Contenido/Imagenes/FlechaAbajo.png");
            else
                $(this).find(".imgMenu").attr("src", "../Contenido/Imagenes/FlechaArriba.png");
            return false;
        });
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - CONFIGURAR AUTORIZADORES</asp:Label>
        </div>
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li>
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Comun.UI/ConsultarAutorizadorUI.aspx">
                        CONSULTAR 
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlRegistroOrden" runat="server" NavigateUrl="~/Comun.UI/RegistrarAutorizadorUI.aspx">
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
                <asp:Label ID="lblTituloPaso" runat="server" Text="REGISTRAR AUTORIZADOR"></asp:Label>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnRegistrar" runat="server" Text="Registrar" CssClass="btnWizardGuardar" OnClick="btnRegistrar_Click"/>
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" OnClick="btnCancelar_Click"/>
                </div>
            </div>
            <div id="ControlesDatos">
                <ucAutorizador:ucAutorizadorUI ID="ucAutorizador" runat="server" />
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
