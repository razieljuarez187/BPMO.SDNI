<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
	AutoEventWireup="true" CodeBehind="DetalleLlantaUI.aspx.cs" Inherits="BPMO.SDNI.Equipos.UI.DetalleLlantaUI" %>

<%--Satisface al CU089 - Bitácora de Llantas--%>
<%@ Register Src="~/Equipos.UI/ucLlantaUI.ascx" TagName="ucLlantaUI" TagPrefix="ucLlanta" %>
<%@ Register Src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" TagName="ucCatalogoDocumentosUI" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #MenuSecundario { float: left; height: 64px; }
        #BarraHerramientas { width: 835px; float: right; }
        .GroupBody { margin: 10px auto; width: 761px; }
        .GroupHeader { width: 100%; }
        #ControlesDatos { min-height: 120px; margin-top: 10px; position: inherit; border: 1px solid transparent; }
    </style>
    <script language="javascript" type="text/javascript">
        function mostrarDialogoEliminar() {
            showDialogModal('EliminarLlantaUI.aspx', 'Eliminar llanta', "955px", "450px", undefined);
        }

        initChild = function () {
            ConfiguracionBarraHerramientas();
            $("span:contains('*')").css({ 'display': 'none' });
        }
        $(document).ready(function () {
            initChild();
        });
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
		<!-- Barra de localización -->
		<div id="BarraUbicacion">
			<asp:Label ID="lblEncabezadoLeyenda" runat="server">UNIDADES - CONSULTAR DETALLES DE LLANTA</asp:Label>
		</div>
		<!-- Menú secundario -->
		<div style="height: 80px;">
			<ul id="MenuSecundario">
				<li class="MenuSecundarioSeleccionado">
					<asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Equipos.UI/ConsultarLlantaUI.aspx">
						CONSULTAR
						<img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
					</asp:HyperLink>
				</li>
				<li>
					<asp:HyperLink ID="hlRegistroOrden" runat="server" NavigateUrl="~/Equipos.UI/RegistrarLlantaUI.aspx">
						REGISTRAR
						<img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
					</asp:HyperLink>
				</li>
			</ul>
			<!-- Barra de herramientas -->
			<div id="BarraHerramientas">
				<asp:Menu runat="server" ID="mBajaLlanta" IncludeStyleBlock="False" Orientation="Horizontal"
					CssClass="MenuPrimario" OnMenuItemClick="mBajaLlanta_MenuItemClick">
					<Items>
						<asp:MenuItem Text="Llanta" Value="LlantaID" Enabled="False" Selectable="false">
						</asp:MenuItem>
						<asp:MenuItem Text="Editar" Value="Editar"></asp:MenuItem>
						<asp:MenuItem Text="Eliminar" Value="EliminarLlanta"></asp:MenuItem>
					</Items>
					<StaticItemTemplate>
						<asp:Label runat="server" ID="lblOpcion" CssClass='<%# (string) Eval("Value") == "LlantaID" ? "Informacion" : string.Empty %>' Text='<%# Eval("Text") %>'></asp:Label>
						<asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "LlantaID" %>' Style="width: 100px" CssClass="textBoxDisabled" Enabled="false"></asp:TextBox>
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
				<input id="btnNuevoConsulta" type="button" value="Nueva Consulta" onclick="window.location='<%= Page.ResolveUrl("~/Equipos.UI/ConsultarLlantaUI.aspx") %>'" />
			</div>
		</div>

		<div id="DatosCatalogo" class="GroupBody">
			<div id="EncabezadoDatosCatalogo" class="GroupHeader">
				<asp:Label ID="lblTituloPaso" runat="server" Text="DETALLES DE LLANTA"></asp:Label>
				<div class="GroupHeaderOpciones Ancho1Opciones">
					<asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btnWizardEditar" OnClick="btnEditar_Click" />
				</div>
			</div>
            <div id="ControlesDatos">
			    <ucLlanta:ucLlantaUI ID="ucLlanta" runat="server" />
            </div>
            <br />
		</div>
	</div>
	<asp:HiddenField ID="hdnTipoMensaje" runat="server" />
	<asp:HiddenField ID="hdnMensaje" runat="server" />
    <asp:HiddenField ID="hdnShowDialogModal" runat="server" value="0"/>
</asp:Content>