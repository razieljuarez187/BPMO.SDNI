<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="DetalleConfiguracionSistemaUnidadUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.DetalleConfiguracionSistemaUnidadUI" %>

<%@ Register Src="~/Mantenimiento.UI/ucConfiguracionSistemaUnidad.ascx" TagName="ucConfSistemaUnidad" TagPrefix="ucPagina1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloIngresarUnidad.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/MantenimientoResponsive.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/bootstrap.1.8.2.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .GroupSection { width: 750px; margin: 0 auto;}
        #divInformacionGeneralControles {padding: 1em;}
        #divInformacionGeneralControles table { margin: 20px auto; }
        .td-finder { float: left; white-space: nowrap;}
    </style>
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/bootstrap-1.8.2.js") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/mantenimiento-responsive.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            initScript();
        });

        function initScript() {
            cloneMenu();
            loadMenuPrincipalSelected();
            listenClickMenuResponsive();
            ConfiguracionBarraHerramientas();
        }
        
        </script>
        
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

   <div id="PaginaContenido">
        <!--Barra de Localización-->
        <div id="BarraUbicacion">
			<asp:Label ID="lblEncabezadoLeyenda" runat="server">DETALLE CONFIGURACI&Oacute;N DE SISTEMA DE UNIDAD</asp:Label>
		</div>
        <!--Navegación secundaria-->
		<div style="height: 80px;">
			<!-- Menú secundario -->
			<ul id="MenuSecundario" class="menuCompuesto">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Mantenimiento.UI/ConsultarConfiguracionSistemaUnidadUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="hlRegistarContactoCliente" runat="server" NavigateUrl="~/Mantenimiento.UI/RegistrarConfiguracionSistemaUnidadUI.aspx">
                        REGISTRAR 
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <div id="BarraHerramientas" style="width:100%">
				<asp:Menu runat="server" ID="mConfiguracion" IncludeStyleBlock="False" Orientation="Horizontal"
                    CssClass="MenuPrimario" OnMenuItemClick="mConfiguracion_MenuItemClick">
                    <Items>
                        <asp:MenuItem Text="Editar" Value="EditarConfiguracion"></asp:MenuItem>
						<asp:MenuItem Text="Eliminar" Value="EliminarConfiguracion"></asp:MenuItem>
                    </Items>
                    <LevelSubMenuStyles>
                        <asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" />
                    </LevelSubMenuStyles>
                    <DynamicHoverStyle CssClass="itemSeleccionado" />
                    <DynamicSelectedStyle CssClass="itemSeleccionado"></DynamicSelectedStyle>
                    <StaticSelectedStyle CssClass="itemSeleccionado"></StaticSelectedStyle>
                </asp:Menu>
                <div class="Ayuda" style="float: right">
                    <input id="btnHelp" type="button" class="btnAyuda" onclick="ShowHelp();" />
                </div>
            </div>
            <div class="BarraNavegacionExtra">
				<input id="btnNuevoConsulta" type="button" value="Consulta" onclick="window.location='<%= Page.ResolveUrl("~/Mantenimiento.UI/ConsultarConfiguracionSistemaUnidad.aspx") %>'" />
            </div>
			
        </div>
        <!-- Cuerpo -->
        <div id="divInformacionGeneral" class="GroupBody" >
            <div id="divInformacionGeneralHeader" class="GroupHeader">
                <span>DETALLE CONFIGURACI&Oacute;N DE SISTEMA DE UNIDAD</span>
                <div class="GroupHeaderOpciones Ancho1Opciones">
					<asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btnWizardEditar" OnClick="btnEditar_Click"/>
			    </div>
            </div>
            <div id="divInformacionGeneralControles">
                <ucPagina1:ucConfSistemaUnidad ID="ucConfSistemaUnidad" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>