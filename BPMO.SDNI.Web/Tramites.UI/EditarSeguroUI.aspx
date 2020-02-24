<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="EditarSeguroUI.aspx.cs" Inherits="BPMO.SDNI.Tramites.UI.EditarSeguroUI" %>
<%@ Register Src="~/Tramites.UI/ucSeguroUI.ascx" TagPrefix="uc" TagName="ucSeguroUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloSeguroUI.css" rel="stylesheet" type="text/css" />
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%=Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#tabs").tabs();
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(onRequestStart);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onRequestEnd);
            ConfiguracionBarraHerramientas();
        });

        function onRequestStart() {
        }

        function onRequestEnd() {
            $("#tabs").tabs();
            ConfiguracionBarraHerramientas();
        } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">UNIDADES - EDITAR SEGURO</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
			<ul id="MenuSecundario">
				<li class="MenuSecundarioSeleccionado">
					<asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Tramites.UI/ConsultarSeguroUI.aspx">
						CONSULTAR
						<img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
					</asp:HyperLink>
				</li>
			</ul>
            <!-- Barra de herramientas -->
            <div id="BarraHerramientas">
                <asp:Menu runat="server" ID="mnuSeguro" IncludeStyleBlock="False" Orientation="Horizontal" CssClass="MenuPrimario">
                    <Items>
                        <asp:MenuItem Text="VIN" Value="DescripcionTramitable" Enabled="False"></asp:MenuItem>
                        <asp:MenuItem Text="# P&oacute;liza" Value="NumeroPoliza" Enabled="False"></asp:MenuItem>
                        <asp:MenuItem Text="Editar" Value="Editar" Selected="true" NavigateUrl="#"></asp:MenuItem>
                    </Items>
                    <StaticItemTemplate>
                        <asp:Label runat="server" ID="lblOpcion" Text='<%# Eval("Text") %>' CssClass='<%# ((string)Eval("Value") == "DescripcionTramitable" || (string)Eval("Value") == "NumeroPoliza")? "Informacion" : string.Empty %>' ReadOnly="True" ></asp:Label>
                        <asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "DescripcionTramitable" || (string) Eval("Value") == "NumeroPoliza" %>' Style="width: 100px" CssClass="textBoxDisabled" ReadOnly="True"></asp:TextBox>
                    </StaticItemTemplate>
                    <LevelSubMenuStyles>
                        <asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" />
                    </LevelSubMenuStyles>
                    <DynamicHoverStyle CssClass="itemSeleccionado" />
                    <DynamicSelectedStyle CssClass="itemSeleccionado"></DynamicSelectedStyle>
                    <StaticSelectedStyle CssClass="itemSeleccionado"></StaticSelectedStyle>
                </asp:Menu>
                <div class="Ayuda">
                    <input id="btnAyuda" type="button" onclick="ShowHelp();" class="btnAyuda" />
                </div>
            </div>
			<div class="BarraNavegacionExtra">
				<input id="btnNuevoConsulta" type="button" value="Nueva Consulta" onclick="window.location='<%= Page.ResolveUrl("~/Tramites.UI/ConsultarSeguroUI.aspx") %>'" />
			</div>
        </div>

        <div id="DatosCatalogo" class="GroupBody">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <span>SEGUROS</span>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ToolTip="Guardar Seguro" CssClass="btnWizardGuardar" ValidationGroup="Obligatorios" onclick="btnGuardar_Click"/>
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" ToolTip="Cancelar" CssClass="btnWizardCancelar" ValidationGroup="Obligatorios" onclick="btnCancelar_Click"/>                    
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