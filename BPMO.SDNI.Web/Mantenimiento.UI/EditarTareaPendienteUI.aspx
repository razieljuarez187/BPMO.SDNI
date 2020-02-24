<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="EditarTareaPendienteUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.EditarTareaPendienteUI" %>
<%@ Register Src="~/Mantenimiento.UI/ucTareasPendientesUI.ascx" TagPrefix="uc" TagName="ucTareasPendientesUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../Contenido/Estilos/EstiloTareaPendiente.css" rel="stylesheet" type="text/css" />
<link href="../Contenido/Estilos/MantenimientoResponsive.css" rel="stylesheet" type="text/css" />
<link href="../Contenido/Estilos/bootstrap.1.8.2.css" rel="stylesheet" type="text/css" />
 <style type="text/css">
        #MenuSecundario { float: left; height: 64px; }
        #BarraHerramientas { width: 835px; float: right; }

        #ControlesDatos { min-height: 120px; margin-top: 10px; position: inherit; border: 1px solid transparent; }
        
         .GroupSection { width: 400px; margin: 0 auto;}
        .GroupBody {max-width: 700px;}
        .GroupHeader span,.GroupHeaderCollapsable span { margin-top: 2px;}
        #divInformacionGeneral {margin: 0 auto;}
        #divInformacionGeneralControles {padding: 1em;}
        #divInformacionGeneralControles table { margin: 20px auto; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">DETALLE TAREA PENDIENTE</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
        <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li id="ConsultarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultarTareaPendiente" runat="server" NavigateUrl="~/Mantenimiento.UI/ConsultarTareaPendienteUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo">
                    <asp:HyperLink ID="hlkRegistroTareaPendiente" runat="server" NavigateUrl="~/Mantenimiento.UI/RegistrarTareaPendienteUI.aspx">
                        REGISTRAR
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
              <!-- Barra de herramientas -->
			<div id="BarraHerramientas" style="width:100%">
				<asp:Menu runat="server" ID="mTareaPendiente" IncludeStyleBlock="False" Orientation="Horizontal"
                    CssClass="MenuPrimario" OnMenuItemClick="mTareaPendiente_MenuItemClick">
                    <Items>
                        <asp:MenuItem Text="Tarea Pendiente" Enabled="False" Selectable="false"></asp:MenuItem>
						<asp:MenuItem Text="Editar" Value="EditarTareaPendiente"></asp:MenuItem>
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
        </div>
        <div id="DatosCatalogo" class="GroupBody">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <asp:Label ID="lblTituloPaso" runat="server" Text="DATOS GENERALES"></asp:Label>
                    <div id="divOpciones" class="GroupHeaderOpciones Ancho1Opciones" runat="server">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btnWizardGuardar" ValidationGroup="Obligatorios" onclick="btnGuardar_Click" />                
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnCancelar_Click" Visible="false"  />
                    <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btnWizardEditar" OnClick="btnEditar_Click"/>
                </div>
            </div>
            <div id="divInformacionGeneralControles">
                <uc:ucTareasPendientesUI runat="server" ID="ucTareasPendientesUI" />
            </div>
            <div class="ContenedorMensajes">
                <span class="Requeridos"></span>
            </div>
        </div>        
    </div>
    <asp:HiddenField ID="hdnTipoMensaje" runat="server" />
    <asp:HiddenField ID="hdnMensaje" runat="server" />
</asp:Content>

