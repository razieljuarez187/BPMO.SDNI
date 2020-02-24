<%@ Page Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="DetalleConfigurarAlertaUI.aspx.cs" Inherits="BPMO.SDNI.Facturacion.MonitoreoPagos.UI.DetalleConfigurarAlertaUI" %>

<%@ Register src="ucConfigurarAlertaUI.ascx" tagname="ucConfigurarAlertaUI" tagprefix="uc1" %>

<%--Satisface el caso de uso CU009 – Configuración Notificación de facturación--%>

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
	        //ConfiguracionBarraHerramientas();
	        $("span:contains('*')").css({ 'display': 'none' });
	    };
	    $(document).ready(initChild);
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
		<!-- Barra de localización -->
		<div id="BarraUbicacion">
			<asp:Label ID="lblEncabezadoLeyenda" runat="server">CONFIGURACIÓN DE NOTIFICACIÓN DE ALERTA - DETALLE DE EMPLEADO PARA RECIBIR NOTIFICACIÓN</asp:Label>
		</div>
		<!-- Menú secundario -->
		<div style="height: 80px;">
			<ul id="MenuSecundario">
				<li class="MenuSecundarioSeleccionado">
					<asp:HyperLink ID="hlkConsultar" runat="server" NavigateUrl="~/Facturacion.MonitoreoPagos.UI/ConsultarConfigurarAlertaUI.aspx">
						CONSULTAR
						<img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
					</asp:HyperLink>
				</li>
				<li>
					<asp:HyperLink ID="hlkRegistro" runat="server" NavigateUrl="~/Facturacion.MonitoreoPagos.UI/RegistrarConfigurarAlertaUI.aspx">
						REGISTRAR
						<img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
					</asp:HyperLink>
				</li>
			</ul>
			<!-- Barra de herramientas -->
            <div id="BarraHerramientas">				
				<div class="Ayuda" style="float: right">
					<input id="Button1" type="button" class="btnAyuda" onclick="ShowHelp();" />
				</div>
			</div>
			
		</div>

		<div id="DatosCatalogo" class="GroupBody">
			<div id="EncabezadoDatosCatalogo" class="GroupHeader">
				<asp:Label ID="lblTituloPaso" runat="server" Text="DETALLE DE EMPLEADO PARA RECIBIR NOTIFICACIÓN"></asp:Label>
				<div class="GroupHeaderOpciones">
					<asp:Button ID="btnEditar" runat="server" Text="Editar" CausesValidation="false" CssClass="btnWizardEditar" OnClick="btnEditar_Click" />
                    <asp:Button ID="btnRegresar" runat="server" Text="REGRESAR" CausesValidation="false" CssClass="btnWizardRegresar" onclick="btnRegresar_Click" />
				</div>
			</div>
            <div id="ControlesDatos">
			    <uc1:ucConfigurarAlertaUI ID="ucConfigurarAlerta" runat="server" />
            </div>
            <br />
		</div>
	</div>

    <asp:HiddenField ID="hdnTipoMensaje" runat="server" />	
	<asp:HiddenField ID="hdnMensaje" runat="server" />
</asp:Content>
