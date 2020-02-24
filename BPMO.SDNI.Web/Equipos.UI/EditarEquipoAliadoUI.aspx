<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="EditarEquipoAliadoUI.aspx.cs" Inherits="BPMO.SDNI.Equipos.UI.EditarEquipoAliadoUI" %>
<%@ Register Src="~/Equipos.UI/ucEquipoAliadoUI.ascx" TagPrefix="uc" TagName="ucEquipoAliadoUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <script type="text/javascript">
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
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">UNIDADES - EDITAR EQUIPO ALIADO</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li id="ConsultarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultarActaNacimiento" runat="server" NavigateUrl="~/Equipos.UI/ConsultarEquipoAliadoUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo">
                    <asp:HyperLink ID="hlkRegistroActaNacimiento" runat="server" NavigateUrl="~/Equipos.UI/RegistrarEquipoAliadoUI.aspx">
                        REGISTRAR
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
			<div id="BarraHerramientas" style="float: right;">
                <asp:Menu runat="server" ID="mnuEquipoAliado" IncludeStyleBlock="False" Orientation="Horizontal" CssClass="MenuPrimario" >
                    <Items>
                        <asp:MenuItem Text="Equipo Aliado" Value="EquipoAliadoID" Enabled="False" Selectable="false"></asp:MenuItem>
                        <asp:MenuItem Text="Editar" Value="Editar" Selected="true" NavigateUrl="#"></asp:MenuItem>
						<asp:MenuItem Text="Eliminar" Value="EliminarEquipoALiado" Enabled="false"></asp:MenuItem>
                    </Items>
                    <StaticItemTemplate>
                        <asp:Label runat="server" ID="lblOpcion" CssClass='<%# (string) Eval("Value") == "EquipoAliadoID" ? "Informacion" : string.Empty %>' Text='<%# Eval("Text") %>'></asp:Label>
                        <asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "EquipoAliadoID" %>' Style="width: 100px" CssClass="textBoxDisabled" ReadOnly="true"></asp:TextBox>
                    </StaticItemTemplate>
                    <LevelSubMenuStyles><asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" /> </LevelSubMenuStyles>
                    <DynamicHoverStyle CssClass="itemSeleccionado"/>
                    <DynamicSelectedStyle CssClass="itemSeleccionado"></DynamicSelectedStyle>
                    <StaticSelectedStyle CssClass="itemSeleccionado"></StaticSelectedStyle>
                </asp:Menu>
                <div class="Ayuda" style="float: right">
                        <input id="btnAyuda" type="button" class="btnAyuda" onclick="ShowHelp();" />
                </div>                
            </div>
            <div class="BarraNavegacionExtra">
                <input id="btnNuevoConsulta" type="button" value="Nueva Consulta" onclick="window.location='<%= Page.ResolveUrl("~/Equipos.UI/ConsultarEquipoAliadoUI.aspx") %>'" />
            </div>
        </div>
        <div id="DatosCatalogo" class="GroupBody">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <asp:Label ID="lblTituloPaso" runat="server" Text="DATOS GENERALES"></asp:Label>
                <div class="GroupHeaderOpciones Ancho2Opciones">                    
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btnWizardGuardar" ValidationGroup="Obligatorios" onclick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnCancelar_Click"  />
                </div>
            </div>
            <div id="ControlesDatos">
                <uc:ucEquipoAliadoUI runat="server" ID="ucucEquipoAliadoUI" />
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
