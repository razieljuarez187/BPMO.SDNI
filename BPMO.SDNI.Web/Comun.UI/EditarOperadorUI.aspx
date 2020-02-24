<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="EditarOperadorUI.aspx.cs" Inherits="BPMO.SDNI.Comun.UI.EditarOperadorUI" %>
<%@ Register Src="~/Comun.UI/ucOperadorUI.ascx" TagName="ucOperadorUI" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #MenuSecundario { float: left; height: 64px; }
        #BarraHerramientas { width: 835px; float: right; }
        .GroupBody { margin: 10px auto; width: 761px; }
        .GroupHeader { width: 100%; }
        #ControlesDatos { min-height: 120px; margin-top: 10px; position: inherit; border: 1px solid transparent; }
    </style>
	<script language="javascript" type="text/javascript" id="JQuerySection">
	    initChild = function () {
	        ConfiguracionBarraHerramientas();
            <%= ucDatosOperadorUI.ClientID %>_Inicializar();
	    };
		$(document).ready(function () {
			initChild();
		});
	</script>
	<script language="javascript" type="text/javascript" id="JavaScriptFunctions">
		function Dialog() {
			$("#dialog").dialog({
				modal: true,
				width: 1100,
				height: 400,
				resizable: false,
				buttons: {
					"Aceptar": function () {
						$(this).dialog("close");
					}
				}
			});
			$("#dialog").parent().appendTo("form:first");
		}

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="PaginaContenido">
		<!--Barra de Localización-->
		<div id="BarraUbicacion">
			<asp:Label ID="lblEncabezadoLeyenda" runat="server">CAT&Aacute;LOGOS - EDITAR OPERADOR</asp:Label>
		</div>
		<!--Menú secundario-->
		<div style="height: 65px;">
			<!-- Menú secundario -->
			<ul id="MenuSecundario">
				<li class="MenuSecundarioSeleccionado">
					<asp:HyperLink ID="hlkConsultar" runat="server" NavigateUrl="~/Comun.UI/ConsultarOperadorUI.aspx"> CONSULTAR <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /></asp:HyperLink>
				</li>
				<li>
					<asp:HyperLink ID="hlkRegistro" runat="server" NavigateUrl="~/Comun.UI/RegistrarOperadorUI.aspx"> REGISTRAR <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /></asp:HyperLink>
				</li>
			</ul>
			<div id="BarraHerramientas" style="float: right;">
                <asp:Menu runat="server" ID="mOperador" IncludeStyleBlock="False" Orientation="Horizontal" CssClass="MenuPrimario">
					<Items>
						<asp:MenuItem Text="Operador" Value="OperadorID" Enabled="False" Selectable="false">
						</asp:MenuItem>
						<asp:MenuItem Text="Editar" Value="Editar" Selected="true" NavigateUrl="#"></asp:MenuItem>
                        <asp:MenuItem Text="Estatus" Value="Estatus" Enabled="False" Selectable="false"></asp:MenuItem>
                        <asp:MenuItem Text="Desactivar" Value="Desactivar" Enabled="false"></asp:MenuItem>
					</Items>
					<StaticItemTemplate>
						<asp:Label runat="server" ID="lblOpcion" CssClass='<%# (string) Eval("Value") == "OperadorID" || (string) Eval("Value") == "Estatus" ? "Informacion" : string.Empty %>' Text='<%# Eval("Text") %>'></asp:Label>
						<asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "OperadorID" || (string) Eval("Value") == "Estatus" %>' Style="width: 100px" CssClass="textBoxDisabled" Enabled="false"></asp:TextBox>
					</StaticItemTemplate>
					<LevelSubMenuStyles>
						<asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" />
					</LevelSubMenuStyles>
                      <DynamicHoverStyle CssClass="itemSeleccionado" />
                    <DynamicSelectedStyle CssClass="itemSeleccionado"></DynamicSelectedStyle>
                    <StaticSelectedStyle CssClass="itemSeleccionado"></StaticSelectedStyle>
				</asp:Menu>
				<div class="Ayuda" style="float: right">
					<input id="btnAyuda" type="button" class="btnAyuda" onclick="ShowHelp();" />
				</div>
			</div>
			<div class="BarraNavegacionExtra">
				<input id="btnNuevoConsulta" type="button" value="Nueva Consulta" onclick="window.location='<%= Page.ResolveUrl("~/Comun.UI/ConsultarCuentaClienteUI.aspx") %>'" />
			</div>
		</div>
		<!--Detalle del Cliente-->
		<div id="DatosCatalogo" class="GroupBody">
			<div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <asp:Label ID="lblTituloPaso" runat="server" Text="DATOS GENERALES"></asp:Label>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btnWizardTerminar" onclick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnCancelar_Click" />
                </div>
            </div>
			<div id="ControlesDatos">
                <uc:ucOperadorUI ID="ucDatosOperadorUI" runat="server" />
			</div>
            <br />
		</div>
	</div>
    <asp:HiddenField ID="hdnFC" runat="server" />
    <asp:HiddenField ID="hdnUC" runat="server" />
    <asp:HiddenField ID="hdnFUA" runat="server" />
    <asp:HiddenField ID="hdnUUA" runat="server" />
    <asp:HiddenField ID="hdnFechaDesactivacion" runat="server" />
    <asp:HiddenField ID="hdnMotivoDesactivacion" runat="server" />
    <asp:HiddenField ID="hdnUsuarioDesactivacionID" runat="server" />
</asp:Content>