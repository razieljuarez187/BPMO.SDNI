<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarPlantillaUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.UI.RegistrarPlantillaUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #MenuSecundario { float: left; height: 64px; }
        #BarraHerramientas { width: 835px; float: right; }
        .GroupBody { margin: 10px auto; width: 761px; }
        .GroupHeader { width: 100%; }
        #ControlesDatos { min-height: 120px; margin-top: 10px; position: inherit; border: 1px solid transparent; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">C&Aacute;TALOGOS - REGISTRAR PLANTILLA DE CONTRATO</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li>
					<asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Contratos.UI/ConsultarPlantillaUI.aspx">
						CONSULTAR
						<img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
					</asp:HyperLink>
				</li>
				<li class="MenuSecundarioSeleccionado">
					<asp:HyperLink ID="hlRegistroOrden" runat="server" NavigateUrl="~/Contratos.UI/RegistrarPlantillaUI.aspx">
						REGISTRAR
						<img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
					</asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <div id="BarraHerramientas">
                <div class="Ayuda" style="top: 0px;">
                    <input id="btnAyuda" type="button" onclick="ShowHelp();" class="btnAyuda" />
                </div>
            </div>
        </div>
        <div id="DatosCatalogo" class="GroupBody">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <span>CARGAR PLANTILLA DE CONTRATOS</span>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnGuardar" runat="server" Text="GUARDAR" CssClass="btnWizardTerminar" ValidationGroup="uplArchivo" OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" OnClick="btnCancelar_Click" />
                </div>
            </div>
            <div id="ControlesDatos">
                <table class="trAlinearDerecha" style="width: 530px; margin: 0px auto;">
                    <tr>
                        <td class="tdCentradoVertical"><span>*</span>Modulo</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 330px;">
                            <asp:DropDownList runat="server" ID="ddlModuloContratos" ToolTip="Modulos de contratos" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical"><span>*</span>Archivo</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 330px;">
                            <asp:UpdatePanel ID="updPnimagenes" runat="server">
                                <ContentTemplate>
                                    <asp:FileUpload ID="uplArchivo" runat="server" />
                                    <asp:RequiredFieldValidator ID="rfvUplArchivo" runat="server" ErrorMessage="*" CssClass="ColorValidator"
                                                ControlToValidate="uplArchivo" ValidationGroup="FileUploadImagen"></asp:RequiredFieldValidator>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnGuardar" />
                                </Triggers>
                            </asp:UpdatePanel>                        
                        </td>
                    </tr>
                </table>
            </div>
            <div class="ContenedorMensajes">
                <span class="Requeridos RequeridosFSL"></span>
                <br />
                <span class="FormatoIncorrecto FormatoIncorrectoFSL"></span>
            </div>
        </div>
    </div>
</asp:Content>
