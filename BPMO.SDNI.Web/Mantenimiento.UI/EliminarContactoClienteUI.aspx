<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="EliminarContactoClienteUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.EliminarContactoClienteUI" %>

<%@ Register Src="~/Mantenimiento.UI/ucContactoCliente.ascx" TagName="ucContactoCliente" TagPrefix="ucPagina1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/MantenimientoResponsive.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/bootstrap.1.8.2.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #ContenedorMenuSecundario,GroupSection { display: inline-block}
        .GroupSection { width: 850px; margin: 0 auto;}
        .GroupBody {max-width: 850px;}
        #divInformacionGeneral {margin: 0 auto;}
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
        }

        function confirmarSetActivo(mensaje) {
            var $div = $('<div title="Confirmación"></div>');
            $div.append('¿Esta seguro que desea ' + mensaje + ' ?');
            $("#dialog:ui-dialog").dialog("destroy");
            $($div).dialog({
                closeOnEscape: true,
                modal: true,
                minWidth: 460,
                close: function () { $(this).dialog("destroy"); },
                buttons: {
                    Aceptar: function () {
                        $(this).dialog("close");
                        __doPostBack("<%= btnEliminar.UniqueID %>", "");

                    },
                    Cancelar: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
        </script>
        
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

   <div id="PaginaContenido">
        <!--Barra de Localización-->
        <div id="BarraUbicacion">
			<asp:Label ID="lblEncabezadoLeyenda" runat="server">MANTENIMIENTO - ELIMINAR CONTACTO CLIENTE</asp:Label>
		</div>
        <!--Navegación secundaria-->
		<div style="height: 80px;">
			<!-- Menú secundario -->
			<ul id="MenuSecundario" class="menuCompuesto">
                <li>
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Mantenimiento.UI/ConsultarContactoClienteUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="hlRegistarContactoCliente" runat="server" NavigateUrl="~/Mantenimiento.UI/RegistrarContactoClienteUI.aspx">
                        REGISTRAR 
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
			<div id="BarraHerramientas" style="width:100%">
				<asp:Menu runat="server" ID="mContactoCliente" IncludeStyleBlock="False" Orientation="Horizontal"
                    CssClass="MenuPrimario" OnMenuItemClick="mContactoCliente_MenuItemClick">
                    <Items>
                        <asp:MenuItem Text="Editar" Value="EditarContactoCliente"></asp:MenuItem>
						<asp:MenuItem Text="Eliminar" Value="EliminarContactoCliente" Selected="true" NavigateUrl="#"></asp:MenuItem>
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
				<input id="btnNuevoConsulta" type="button" value="Nueva Consulta" onclick="window.location='<%= Page.ResolveUrl("~/Mantenimiento.UI/ConsultarContactoClienteUI.aspx") %>'" />
            </div>
        </div>
    <!-- Cuerpo -->
        <div id="divInformacionGeneral" class="GroupBody" >
            <div id="divInformacionGeneralHeader" class="GroupHeader">
                <asp:Label  ID="spanTituloInformacionGeneral" runat="server">ELIMINAR CONTACTO CLIENTE</asp:Label>
                <div class="GroupHeaderOpciones Ancho2Opciones">
					<asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btnWizardGuardar" onclick="btnFinalizar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" OnClick="btnCancelar_Click" />
			    </div>
            </div>
            <div id="divInformacionGeneralControles">
                <ucPagina1:ucContactoCliente ID="ucContactoCliente" runat="server" />
            </div>
        </div>
    </div>
    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" style="display: none;" onclick="btnEliminar_Click" />
</asp:Content>
