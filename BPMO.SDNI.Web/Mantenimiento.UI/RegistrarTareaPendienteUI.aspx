<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarTareaPendienteUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.RegistrarTareaPendienteUI" %>
<%@ Register Src="~/Mantenimiento.UI/ucTareasPendientesUI.ascx" TagPrefix="uc" TagName="ucTareasPendientesUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../Contenido/Estilos/EstiloTareaPendiente.css" rel="stylesheet" type="text/css" />
<link href="../Contenido/Estilos/MantenimientoResponsive.css" rel="stylesheet" type="text/css" />
<link href="../Contenido/Estilos/bootstrap.1.8.2.css" rel="stylesheet" type="text/css" />
<style>

        .GroupSection { width: 400px; margin: 0 auto;}
        .GroupBody {max-width: 700px;}
        .GroupHeader span,.GroupHeaderCollapsable span { margin-top: 2px;}

</style>
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
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">REGISTRAR TAREA PENDIENTE</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
        <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li id="ConsultarCatalogo">
                    <asp:HyperLink ID="hlkConsultarTareaPendiente" runat="server" NavigateUrl="~/Mantenimiento.UI/ConsultarTareaPendienteUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkRegistroTareaPendiente" runat="server" NavigateUrl="~/Mantenimiento.UI/RegistrarTareaPendienteUI.aspx">
                        REGISTRAR
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
        </div>
        <div id="DatosCatalogo" class="GroupBody">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <asp:Label ID="lblTituloPaso" runat="server" Text="DATOS GENERALES"></asp:Label>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btnWizardGuardar" ValidationGroup="Obligatorios" onclick="btnGuardar_Click" />                    
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnCancelar_Click"  />
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
