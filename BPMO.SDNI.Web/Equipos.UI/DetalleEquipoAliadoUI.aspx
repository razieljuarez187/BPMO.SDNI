<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="DetalleEquipoAliadoUI.aspx.cs" Inherits="BPMO.SDNI.Equipos.UI.DetalleEquipoAliadoUI" %>
<%@ Register Src="~/Equipos.UI/ucEquipoAliadoDetalleUI.ascx" TagPrefix="uc" TagName="ucEquipoAliadoDetalleUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #MenuSecundario { float: left; height: 64px; }
        #BarraHerramientas { width: 835px; float: right; }
        .GroupBody { margin: 10px auto; width: 761px; }
        .GroupHeader { width: 100%; }
        #ControlesDatos { min-height: 120px; margin-top: 17px; position: inherit; }
    </style>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            initChild();
        });
        function initChild() {
            $("span:contains('*')").css({ 'display': 'none' });
            ConfiguracionBarraHerramientas();
        }        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">UNIDADES - CONSULTAR DETALLES DE EQUIPO ALIADO</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li id="ConsultarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultarActaNacimiento" runat="server" NavigateUrl="~/Equipos.UI/ConsultarEquipoAliadoUI.aspx"> CONSULTAR <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /></asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo">
                    <asp:HyperLink ID="hlkRegistroActaNacimiento" runat="server" NavigateUrl="~/Equipos.UI/RegistrarEquipoAliadoUI.aspx"> REGISTRAR <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/></asp:HyperLink>
                </li>
            </ul>  
            <!-- Barra de herramientas -->
			<div id="BarraHerramientas">
                <asp:Menu runat="server" ID="mnuBajaEquipoAliado" IncludeStyleBlock="False" Orientation="Horizontal" CssClass="MenuPrimario" onmenuitemclick="mnuBajaEquipoAliado_MenuItemClick">
					<Items>
						<asp:MenuItem Text="Equipo  Aliado" Value="EquipoAliadoID" Enabled="False" Selectable="false">
						</asp:MenuItem>
                        <asp:MenuItem Text="Editar" Value="Editar"></asp:MenuItem>
						<asp:MenuItem Text="Eliminar" Value="EliminarEquipoALiado"></asp:MenuItem>
					</Items>
					<StaticItemTemplate>
						<asp:Label runat="server" ID="lblOpcion" CssClass='<%# (string) Eval("Value") == "EquipoAliadoID" ? "Información" : string.Empty %>' Text='<%# Eval("Text") %>'></asp:Label>
						<asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "EquipoAliadoID" %>' Style="width: 100px" CssClass="textBoxDisabled" Enabled="false"></asp:TextBox>
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
                <input id="btnNuevoConsulta" type="button" value="Nueva Consulta" onclick="window.location='<%= Page.ResolveUrl("~/Equipos.UI/ConsultarEquipoAliadoUI.aspx") %>'" />
            </div>        
        </div>
        <div id="DatosCatalogo" class="GroupBody">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <asp:Label ID="lblTituloPaso" runat="server" Text="DATOS GENERALES"></asp:Label>
                <div class="GroupHeaderOpciones Ancho1Opciones">
                    <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btnWizardEditar" onclick="btnEditar_Click" />
                </div>
            </div>
            <div id="ControlesDatos">
                <uc:ucEquipoAliadoDetalleUI runat="server" ID="ucucEquipoAliadoUI" />
            </div>
            <br />
        </div>        
    </div>
    <asp:HiddenField ID="hdnTipoMensaje" runat="server" />
    <asp:HiddenField ID="hdnMensaje" runat="server" />
</asp:Content>
