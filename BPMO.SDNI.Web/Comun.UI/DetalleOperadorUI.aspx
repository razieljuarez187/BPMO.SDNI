<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="DetalleOperadorUI.aspx.cs" Inherits="BPMO.SDNI.Comun.UI.DetalleOperadorUI" %>
<%@ Register Src="~/Comun.UI/ucOperadorUI.ascx" TagName="ucOperadorUI" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #MenuSecundario { float: left; height: 64px; }
        #BarraHerramientas { width: 835px; float: right; }
        .GroupBody { margin: 10px auto; width: 761px; }
        .GroupHeader { width: 100%; }
        #ControlesDatos { min-height: 120px; margin-top: 17px; position: inherit; }
    </style>
    <script language="javascript" type="text/javascript" id="JQuerySection">
        initChild = function () {
            ConfiguracionBarraHerramientas();
            $("span:contains('*')").css({ 'display': 'none' });
        };

        $(document).ready(function () {
            initChild();
        });
    </script>
    <script language="javascript" type="text/javascript" id="JavaScriptFunctions">
        function Dialog() {
            $("#dialog").dialog({
                modal: true,
                width: 1100,
                height: 400,
                resizable: false,
                open: function () {
                    $("#<%= txtMotivoDesactivacion.ClientID %>").val("");
                },
                buttons: {
                    "Aceptar": function () {
                        $(this).dialog("close");
                    }
                }
            });
            $("#dialog").parent().appendTo("form:first");
        }
       
    </script>
    <script type="text/javascript">
        function MostrarBloquear() {
            $.blockUI({ message: $('#divDesactivarOperador'),
                fadeIn: 700,
                fadeOut: 700,
                showOverlay: true,
                css: {
                    border: 'none',
                    padding: '0px',
                    width: '340px',
                    backgroundColor: '#fff',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: 1,
                    color: '#000'
                }
            });

            $('#divDesactivarOperador').parent().appendTo($("form:first"));
        }
        function CambiarBloquear() {
            $.blockUI({ message: 'Espere por favor...' });
        }
        function OcultarBloquear() {
            $.unblockUI();
            $("<%= txtMotivoDesactivacion.ClientID %>").val("");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!--Barra de Localización-->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">CAT&Aacute;LOGOS - CONSULTAR DETALLES DE OPERADOR</asp:Label>
        </div>
        <!--Menú secundario-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultar" runat="server" NavigateUrl="~/Comun.UI/ConsultarOperadorUI.aspx"> CONSULTAR <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /></asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="hlkRegistro" runat="server" NavigateUrl="~/Comun.UI/RegistrarOperadorUI.aspx"> REGISTRAR <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /></asp:HyperLink>
                </li>
            </ul>
            <div id="BarraHerramientas">
                <asp:Menu runat="server" ID="mOperador" IncludeStyleBlock="False" Orientation="Horizontal" CssClass="MenuPrimario" OnMenuItemClick="mOperador_MenuItemClick">
                    <Items>
                        <asp:MenuItem Text="Operador" Value="OperadorID" Enabled="False" Selectable="false">
                        </asp:MenuItem>
                        <asp:MenuItem Text="Editar" Value="Editar"></asp:MenuItem>
                        <asp:MenuItem Text="Estatus" Value="Estatus" Enabled="False" Selectable="false"></asp:MenuItem>
                        <asp:MenuItem Text="Desactivar" Value="Desactivar" NavigateUrl="javascript:MostrarBloquear();">
                        </asp:MenuItem>
                    </Items>
                    <StaticItemTemplate>
                        <asp:Label runat="server" ID="lblOpcion" CssClass='<%# (string) Eval("Value") == "OperadorID" || (string) Eval("Value") == "Estatus" ? "Informacion" : string.Empty %>' Text='<%# Eval("Text") %>'></asp:Label>
                        <asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "OperadorID" || (string) Eval("Value") == "Estatus" %>' Style="width: 100px" CssClass="textBoxDisabled" Enabled="false"></asp:TextBox>
                    </StaticItemTemplate>
                    <LevelSubMenuStyles>
                        <asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" />
                    </LevelSubMenuStyles>
                </asp:Menu>
                <div class="Ayuda" style="float: right">
                    <input id="btnAyuda" type="button" class="btnAyuda" onclick="ShowHelp();" />
                </div>
            </div>
            <div class="BarraNavegacionExtra">
                <input id="btnNuevoConsulta" type="button" value="Nueva Consulta" onclick="window.location='<%= Page.ResolveUrl("~/Comun.UI/ConsultarOperadorUI.aspx") %>'" />
            </div>
        </div>
        
        <div id="DatosCatalogo" class="GroupBody">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <asp:Label ID="lblTituloPaso" runat="server" Text="DATOS GENERALES"></asp:Label>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btnWizardEditar" OnClick="btnEditar_Click" />
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" cssClass="btnWizardRegresar" OnClick="btnRegresar_Click"/>
                </div>
            </div>
            <div id="ControlesDatos">
                <uc:ucOperadorUI ID="ucDatosOperadorUI" runat="server" />
            </div>
            <br />
        </div>
    </div>
    <asp:HiddenField ID="hdnFC" runat="server" />
    <asp:HiddenField ID="hdnUC" runat="server" />
    <asp:HiddenField ID="hdnFUA" runat="server" />
    <asp:HiddenField ID="hdnUUA" runat="server" />
    <asp:HiddenField ID="hdnFechaDesactivacion" runat="server" />
    <asp:HiddenField ID="hdnUsuarioDesactivacionID" runat="server" />
    <asp:HiddenField ID="hdnEstatusNuevo" runat="server" Value="0" />
    <div id="divDesactivarOperador" style="text-transform: uppercase; display: none; padding: 10px;">
        <table style="margin-left: auto; margin-right: auto; text-align: center;">
            <tr>
                <td colspan="2" style="font-weight: bold">
                    <asp:Label ID="lblTituloDesactivar" runat="server" Text="Desactivar Operador"></asp:Label>
                </td>
            </tr>
            <tr><td colspan="2">&nbsp;</td></tr>
            <tr>
                <td colspan="2">
                    <span>*</span><asp:Label ID="lblDescripcionDesactivar" runat="server" Text="Motivo de la desactivaci&oacute;n"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="txtMotivoDesactivacion" runat="server" MaxLength="300" Rows="5"
                        TextMode="MultiLine" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr><td colspan="2">&nbsp;</td></tr>
            <tr>
                <td>
                    <asp:Button ID="btnConfirmarDesactivar" CssClass="btnWizardTerminar" runat="server"
                        Text="Confirmar" OnClick="btnConfirmarDesactivar_Click" OnClientClick="CambiarBloquear();" />
                </td>
                <td>
                    <input type="button" class="btnWizardCancelar" value="Cancelar" onclick="OcultarBloquear();" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
