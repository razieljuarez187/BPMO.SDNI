<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="~/Equipos.UI/EditarLlantaUI.aspx.cs" Inherits="BPMO.SDNI.Equipos.UI.EditarLlantaUI" %>

<%@ Register Src="~/Equipos.UI/ucLlantaUI.ascx" TagName="ucLlantaUI" TagPrefix="ucLlanta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
     <script src="<%= 
Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" 
type="text/javascript"></script>
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
<%--Satisface al CU089 - Bitácora de Llantas--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">UNIDADES - EDITAR LLANTA</asp:Label>
        </div>
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Equipos.UI/ConsultarLlantaUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="hlRegistroOrden" runat="server" NavigateUrl="~/Equipos.UI/RegistrarLlantaUI.aspx">
                        REGISTRAR 
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <div id="BarraHerramientas">
                <asp:Menu runat="server" ID="mEditarLlanta" IncludeStyleBlock="False" Orientation="Horizontal"
                    CssClass="MenuPrimario">
                    <Items>
                        <asp:MenuItem Text="Llanta" Value="LlantaID" Enabled="False" Selectable="false">
                        </asp:MenuItem>
                        <asp:MenuItem Text="Editar" Value="EditarLlanta" Selected="true" NavigateUrl="#">
                        </asp:MenuItem>
						<asp:MenuItem Text="Eliminar" Value="EliminarLlanta" Enabled="false"></asp:MenuItem>
                    </Items>
                    <StaticItemTemplate>
                        <asp:Label runat="server" ID="lblOpcion" CssClass='<%# (string) Eval("Value") == "LlantaID" ? "Informacion" : string.Empty %>' Text='<%# Eval("Text") %>'></asp:Label>
                        <asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "LlantaID" %>'
                            Style="width: 100px" CssClass="textBoxDisabled" Enabled="false"></asp:TextBox>
                    </StaticItemTemplate>
                    <LevelSubMenuStyles>
                        <asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" />
                    </LevelSubMenuStyles>
                    <DynamicHoverStyle CssClass="itemSeleccionado" />
                    <DynamicSelectedStyle CssClass="itemSeleccionado"></DynamicSelectedStyle>
                    <StaticSelectedStyle CssClass="itemSeleccionado"></StaticSelectedStyle>
                </asp:Menu>
                <div class="Ayuda" style="float: right">
                    <input id="Button1" type="button" class="btnAyuda" onclick="ShowHelp();" />
                </div>
            </div>
        </div>

        <div id="DatosCatalogo" class="GroupBody">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <asp:Label ID="lblTituloPaso" runat="server" Text="EDITAR LLANTA"></asp:Label>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnEditar" runat="server" Text="Guardar" CssClass="btnWizardGuardar" OnClick="btnEditar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" OnClick="btnCancelar_Click" />
                </div>
            </div>
            <div id="ControlesDatos">
                <ucLlanta:ucLlantaUI ID="ucLlanta" runat="server" />
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
