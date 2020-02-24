<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="EditarAutorizadorUI.aspx.cs" Inherits="BPMO.SDNI.Comun.UI.EditarAutorizadorUI" %>
<%-- 
    Satisface al caso de uso CU098 - Configurar Autorizadores para los Contratos
--%>
<%@ Register Src="~/Comun.UI/ucAutorizadorUI.ascx" TagName="ucAutorizadorUI" TagPrefix="ucAutorizador" %>
<%@ Register TagPrefix="uc" Namespace="BPMO.SDNI.Comun.UI" Assembly="BPMO.SDNI.Web" %>
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
<script language="javascript" type="text/javascript" id="JQuerySection">
    $(document).ready(function () {
        initChild();
    });
    </script>
    <script language="javascript" type="text/javascript" id="JavaScriptFunctions">
        initChild = function () {
            ConfiguracionBarraHerramientas();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - EDITAR AUTORIZADOR</asp:Label>
        </div>
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultar" runat="server" NavigateUrl="~/Comun.UI/ConsultarAutorizadorUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="hlkRegistro" runat="server" NavigateUrl="~/Comun.UI/RegistrarAutorizadorUI.aspx">
                        REGISTRAR 
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <div id="BarraHerramientas">
                <asp:Menu runat="server" ID="mEditarAutorizador" IncludeStyleBlock="False" Orientation="Horizontal"
                    CssClass="MenuPrimario">
                    <Items>
                        <asp:MenuItem Text="Autorizador" Value="AutorizadorID" Enabled="False" Selectable="false">
						</asp:MenuItem>
                        <asp:MenuItem Text="Editar" Value="EditarAutorizador" Selected="true" NavigateUrl="#">
                        </asp:MenuItem>
                        <asp:MenuItem Text="Estatus" Value="Estatus" Enabled="False"></asp:MenuItem>
                    </Items>
                    <StaticItemTemplate>
                        <asp:Label runat="server" ID="lblOpcion" CssClass='<%# (string) Eval("Value") == "AutorizadorID" || (string) Eval("Value") == "Estatus" ? "Informacion" : string.Empty %>' Text='<%# Eval("Text") %>'></asp:Label>
                        <asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "AutorizadorID" || (string) Eval("Value") == "Estatus" %>' Style="width: 100px" CssClass="textBoxDisabled" Enabled="false"></asp:TextBox>
                    </StaticItemTemplate>
                    <LevelSubMenuStyles>
                        <asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" />
                    </LevelSubMenuStyles>
                    <DynamicHoverStyle CssClass="itemSeleccionado" />
                    <DynamicSelectedStyle CssClass="itemSeleccionado"></DynamicSelectedStyle>
                    <StaticSelectedStyle CssClass="itemSeleccionado"></StaticSelectedStyle>
                </asp:Menu>
                <div class="Ayuda" style="float: right">
                    <input id="Button1" type="button" class="btnAyuda" onclick="ShowHelp();" />
                </div>
            </div>
        </div>

        <div id="DatosCatalogo" class="GroupBody">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <asp:Label ID="lblTituloPaso" runat="server" Text="EDITAR AUTORIZADOR"></asp:Label>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnEditar" runat="server" Text="Guardar" CssClass="btnWizardGuardar" OnClick="btnEditar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" OnClick="btnCancelar_Click" />
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
