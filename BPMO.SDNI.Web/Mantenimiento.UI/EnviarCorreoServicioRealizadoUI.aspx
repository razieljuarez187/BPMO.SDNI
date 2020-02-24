<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="EnviarCorreoServicioRealizadoUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.EnviarCorreoServicioRealizadoUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloIngresarUnidad.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/MantenimientoResponsive.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/bootstrap.1.8.2.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .GroupSection { max-width: 650px; min-width:100px; margin: 0px auto; }
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
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
			<asp:Label ID="lblEncabezadoLeyenda" runat="server">MANTENIMIENTO - ENVIAR CORREO DE SERVICIO REALIZADO</asp:Label>
		</div>
        <!--Navegación secundaria-->
		<div style="height: 65px;">
			<!-- Menú secundario -->
			<span id="ContenedorMenuSecundario">
            </span>
            <!-- Barra de herramientas -->
			<div id="BarraHerramientas" style="width:100%">
				<div class="WizardSteps">
                </div>
            </div>
            <div class="BarraNavegacionExtra">
            </div>
        </div>
        <!-- Cuerpo -->
        
        <div id="divInformacionGeneral" class="GroupBody" >
            <div id="divInformacionGeneralHeader" class="GroupHeader">
                <span>ENVIAR CORREO SERVICIO REALIZADO</span>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnEnviar" runat="server" Text="Enviar" CssClass="btnWizardGuardar" OnClick="btnFinalizar_Click"/>
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" OnClick="btnCancelar_Click" />
			    </div>
            </div>
            <div id="divInformacionGeneralControles">
                <fieldset>
                    <legend>Informaci&oacute;n del Correo</legend>
                    <table class="trAlinearDerecha table-responsive" style="margin: 0px auto 10px auto; width: auto; border: 1px solid transparent;">
                        <tr>
                            <td class="tdCentradoVertical input-label-responsive">Mensaje de Inicio:</td>
                            <td class="input-space-responsive"></td>
                            <td class="tdCentradoVertical input-group-responsive" colspan="5">
                                <asp:TextBox ID="txtMensajeInicio" runat="server" TextMode="MultiLine" Rows="2" Columns="3" CssClass="input-text-responsive full-width"></asp:TextBox>
                            </td>         
                        </tr>
                        <tr>
                            <td class="tdCentradoVertical input-label-responsive">Unidad:</td>
                            <td class="input-space-responsive"></td>
                            <td class="tdCentradoVertical input-group-responsive" colspan="5">
                                <asp:TextBox ID="txtUnidad" runat="server" ReadOnly="true" Enabled="false" CssClass="input-text-responsive full-width"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdCentradoVertical input-label-responsive">Pr&oacute;ximo Servicio:</td>
                            <td class="input-space-responsive"></td>
                            <td class="tdCentradoVertical input-group-responsive">
                                <asp:TextBox ID="txtProximoServicio" runat="server" ReadOnly="true" Enabled="false" CssClass="input-text-responsive"></asp:TextBox>
                            </td>
                            <td class="input-space-responsive"></td>
                            <td class="tdCentradoVertical input-label-responsive">Fecha Pr&oacute;ximo Servicio:</td>
                            <td class="input-space-responsive"></td>
                            <td class="tdCentradoVertical input-group-responsive">
                                <asp:TextBox ID="txtFechaProximoServicio" runat="server" ReadOnly="true" Enabled="false" CssClass="input-text-responsive"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdCentradoVertical input-label-responsive">Kilometraje:</td>
                            <td class="input-space-responsive"></td>
                            <td class="tdCentradoVertical input-group-responsive">
                                <asp:TextBox ID="txtKilometraje" runat="server" ReadOnly="true" Enabled="false" CssClass="input-text-responsive full-width"></asp:TextBox>
                            </td>
                            <td class="input-space-responsive"></td>
                            <td class="tdCentradoVertical input-label-responsive">Horas</td>
                            <td class="input-space-responsive"></td>
                            <td class="tdCentradoVertical input-group-responsive">
                                <asp:TextBox ID="txtHoras" runat="server" ReadOnly="true" Enabled="false" CssClass="input-text-responsive full-width"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdCentradoVertical input-label-responsive">Mensaje:</td>
                            <td class="input-space-responsive"></td>
                            <td class="tdCentradoVertical input-group-responsive" colspan="5">
                                <asp:TextBox ID="txtMensaje" runat="server" TextMode="MultiLine" Rows="8" Columns="3" CssClass="input-text-responsive full-width"></asp:TextBox>
                            </td>    
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>Informaci&oacute;n de Contacto del Cliente</legend>
                    <table class="trAlinearDerecha table-responsive" style="margin: 0px auto 10px auto; width: auto; border: 1px solid transparent;">
                        <tr>
                            <td class="tdCentradoVertical input-label-responsive">Nombre:</td>
                            <td class="input-space-responsive"></td>
                            <td class="tdCentradoVertical input-group-responsive" colspan="5">
                                <asp:TextBox ID="txtNombreContacto" runat="server" ReadOnly="true" Enabled="false" CssClass="input-text-responsive"></asp:TextBox>
                            </td>         
                        </tr>
                        <tr>
                            <td class="tdCentradoVertical input-label-responsive">Tel&eacute;fono:</td>
                            <td class="input-space-responsive"></td>
                            <td class="tdCentradoVertical input-group-responsive">
                                <asp:TextBox ID="txtTelefono" runat="server" ReadOnly="true" Enabled="false" CssClass="input-text-responsive"></asp:TextBox>
                            </td>
                            <td class="input-space-responsive"></td>
                            <td class="tdCentradoVertical input-label-responsive">Correo:</td>
                            <td class="input-space-responsive"></td>
                            <td class="tdCentradoVertical input-group-responsive">
                                <asp:TextBox ID="txtEmail" runat="server" ReadOnly="true" Enabled="false" CssClass="input-text-responsive"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnLibroActivos" runat="server" />
</asp:Content>
