<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarSeguroUI.aspx.cs" Inherits="BPMO.SDNI.Tramites.UI.RegistrarSeguroUI" %>
<%@ Register Src="~/Tramites.UI/ucDeducibleSeguroUI.ascx" TagPrefix="uc" TagName="ucDeducibleSeguroUI" %>
<%@ Register Src="~/Tramites.UI/ucSeguroUI.ascx" TagPrefix="uc" TagName="ucSeguroUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloSeguroUI.css" rel="stylesheet" type="text/css" />
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%=Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#tabs").tabs();
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(onRequestStart);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onRequestEnd)
        });

        function onRequestStart() {
        }

        function onRequestEnd() {
            $("#tabs").tabs();
            $('.CampoFecha').attr('readonly', true);
        }  
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">UNIDADES - REGISTRAR SEGURO</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
			<ul id="MenuSecundario">
				<li>
					<asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Tramites.UI/ConsultarSeguroUI.aspx">
						CONSULTAR
						<img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
					</asp:HyperLink>
				</li>
                <li class="MenuSecundarioSeleccionado">
                    <a href="#">
                        REGISTRAR
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </a>
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
                <span>SEGUROS</span>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ToolTip="Guardar Seguro" CssClass="btnWizardGuardar GroupHeaderOpcionesMargin" ValidationGroup="Obligatorios" onclick="btnGuardar_Click"/>
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" ToolTip="Cancelar" CssClass="btnWizardCancelar GroupHeaderOpcionesMargin" ValidationGroup="Obligatorios"  onclick="btnCancelar_Click"/>                
                </div>
            </div>
            <div id="ControlesDatos">
                <uc:ucSeguroUI runat="server" ID="ucucSeguroUI" />
            </div>                
        </div>   
    </div>
    <div class="ContenedorMensajes">
        <span class="Requeridos"></span>
        <br />
        <span class="FormatoIncorrecto"></span>
    </div>
    <asp:HiddenField ID="hdnTipoMensaje" runat="server" />
    <asp:HiddenField ID="hdnMensaje" runat="server" />
    <asp:HiddenField ID="hdnVIN" runat="server" />    
</asp:Content>
