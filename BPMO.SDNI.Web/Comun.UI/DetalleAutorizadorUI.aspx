<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="DetalleAutorizadorUI.aspx.cs" Inherits="BPMO.SDNI.Comun.UI.DetalleAutorizadorUI" %>
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
	<script type="text/javascript">
        initChild = function () {
            ConfiguracionBarraHerramientas();
            $("span:contains('*')").css({ 'display': 'none' });
        };
        $(document).ready(initChild);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="PaginaContenido">
		<!-- Barra de localización -->
		<div id="BarraUbicacion">
			<asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - CONSULTAR DETALLES DE AUTORIZADOR</asp:Label>
		</div>
		<!-- Menú secundario -->
		<div style="height: 80px;">
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
						<img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
					</asp:HyperLink>
				</li>
			</ul>
			<!-- Barra de herramientas -->
            <div id="BarraHerramientas">
				<asp:Menu runat="server" ID="mAutorizador" IncludeStyleBlock="False" Orientation="Horizontal"
					CssClass="MenuPrimario" OnMenuItemClick="mAutorizador_MenuItemClick">
					<Items>
					    <asp:MenuItem Text="Autorizador" Value="AutorizadorID" Enabled="False" Selectable="false">
						</asp:MenuItem>
						<asp:MenuItem Text="Editar" Value="Editar"></asp:MenuItem>
                        <asp:MenuItem Text="Estatus" Value="Estatus" Enabled="False"></asp:MenuItem>
					</Items>
					<StaticItemTemplate>
						<asp:Label runat="server" ID="lblOpcion" CssClass='<%# (string) Eval("Value") == "AutorizadorID" || (string) Eval("Value") == "Estatus" ? "Informacion" : string.Empty %>' Text='<%# Eval("Text") %>'></asp:Label>
						<asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "AutorizadorID" || (string) Eval("Value") == "Estatus" %>' Style="width: 100px" CssClass="textBoxDisabled" Enabled="false"></asp:TextBox>
					</StaticItemTemplate>
					<LevelSubMenuStyles>
						<asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" />
					</LevelSubMenuStyles>
				</asp:Menu>
				<div class="Ayuda" style="float: right">
					<input id="Button1" type="button" class="btnAyuda" onclick="ShowHelp();" />
				</div>
			</div>
			<div class="BarraNavegacionExtra">
				<input id="btnNuevoConsulta" type="button" value="Nueva Consulta" onclick="window.location='<%= Page.ResolveUrl("~/Comun.UI/ConsultarAutorizadorUI.aspx") %>'" />
			</div>
		</div>

		<div id="DatosCatalogo" class="GroupBody">
			<div id="EncabezadoDatosCatalogo" class="GroupHeader">
				<asp:Label ID="lblTituloPaso" runat="server" Text="DETALLES DEL AUTORIZADOR"></asp:Label>
				<div class="GroupHeaderOpciones Ancho2Opciones">
					<asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btnWizardEditar" OnClick="btnEditar_Click" />
                    <asp:Button ID="btnRegresar" runat="server" Text="REGRESAR" CssClass="btnWizardRegresar" onclick="btnRegresar_Click" />
				</div>
			</div>
            <div id="ControlesDatos">
			    <ucAutorizador:ucAutorizadorUI ID="ucAutorizador" runat="server" />
            </div>
            <br />
		</div>
	</div>
	<asp:HiddenField ID="hdnTipoMensaje" runat="server" />
	<asp:HiddenField ID="hdnMensaje" runat="server" />
</asp:Content>
