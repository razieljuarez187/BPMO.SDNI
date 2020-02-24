<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="DetalleReservacionRDUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.RD.UI.DetalleReservacionRDUI" %>
<%@ Register Src="~/Contratos.RD.UI/ucReservacionRDUI.ascx" TagName="ucReservacionUI" TagPrefix="ucReservacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #MenuSecundario { float: left; height: 64px; }
        #BarraHerramientas { width: 835px; float: right; }
        .GroupBody { margin: 10px auto; width: 761px; }
        .GroupHeader { width: 100%; }
        #ControlesDatos { min-height: 120px; position: inherit; border: 1px solid transparent; }
        #ControlesDatos fieldset { display: inherit; width: 96%; margin-top: 10px; margin-bottom: 13px; margin-left: 2%; margin-right: 2%; }
    </style>
    <script type="text/javascript">
        initChild = function () {
            <%= ucReservacion.ClientID %>_Inicializar();
            ConfiguracionBarraHerramientas();
            $("span:contains('*')").css({ 'display': 'none' });
        };
        $(document).ready(initChild);
    </script>
    <script type="text/javascript">
        function confirmarCancelar() {
            var $div = $('<div title="Confirmación"></div>');
            $div.append('¿Desea Cancelar la Reservación?');
            $("#dialog:ui-dialog").dialog("destroy");
            $($div).dialog({
                closeOnEscape: true,
                modal: true,
                minWidth: 460,
                close: function () { $(this).dialog("destroy"); },
                buttons: {
                    Aceptar: function () {
                        $(this).dialog("close");
                        __doPostBack("<%= btnCancelar.UniqueID %>", "");
                    },
                    Cancelar: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - CONSULTAR DETALLES DE RESERVACI&Oacute;N</asp:Label>
        </div>
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 64px;">
                <li id="ConsultarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultar" runat="server" NavigateUrl="~/Contratos.RD.UI/ConsultarReservacionRDUI.aspx">
						CONSULTAR
						<img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo">
                    <asp:HyperLink ID="hlkRegistro" runat="server" NavigateUrl="~/Contratos.RD.UI/RegistrarReservacionRDUI.aspx">
						REGISTRAR
						<img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
            </ul>
			<!-- Barra de herramientas -->
			<div id="BarraHerramientas">
				<asp:Menu runat="server" ID="mReservacion" IncludeStyleBlock="False" Orientation="Horizontal"
					CssClass="MenuPrimario" OnMenuItemClick="mReservacion_MenuItemClick">
					<Items>
						<asp:MenuItem Text="# Reservación" Value="Numero" Enabled="False" Selectable="false">
						</asp:MenuItem>
						<asp:MenuItem Text="Editar" Value="Editar"></asp:MenuItem>
						<asp:MenuItem Text="Cancelar" Value="Cancelar"></asp:MenuItem>
					</Items>
					<StaticItemTemplate>
						<asp:Label runat="server" ID="lblOpcion" CssClass='<%# (string) Eval("Value") == "Numero" || (string) Eval("Value") == "Activo" ? "Informacion" : string.Empty %>' Text='<%# Eval("Text") %>'></asp:Label>
						<asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "Numero" || (string) Eval("Value") == "Activo" %>' Style="width: 100px" CssClass="textBoxDisabled" Enabled="false"></asp:TextBox>
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
				<input id="btnNuevoConsulta" type="button" value="Nueva Consulta" onclick="window.location='<%= Page.ResolveUrl("~/Contratos.RD.UI/ConsultarReservacionRDUI.aspx") %>'" />
			</div>
        </div>
        
        <div id="DatosCatalogo" class="GroupBody">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <asp:Label ID="lblTituloPaso" runat="server" Text="DETALLES DE RESERVACIÓN"></asp:Label>
                <div class="GroupHeaderOpciones Ancho2Opciones">
					<asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btnWizardEditar" OnClick="btnEditar_Click" />
                    <asp:Button ID="btnRegresar" runat="server" Text="REGRESAR" CssClass="btnWizardRegresar" onclick="btnRegresar_Click" />
                </div>
            </div>
            <div id="ControlesDatos">
                <ucReservacion:ucReservacionUI ID="ucReservacion" runat="server" />
            </div>      
        </div>
    </div>
    <asp:Button ID="btnCancelar" runat="server" Text="Button" OnClick="btnCancelar_Click" Style="display: none;" />
</asp:Content>
