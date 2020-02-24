<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="CambiarContratoSucursalUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.UI.CambiarContratoSucursalUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        .GroupSectionFixCambioSucursal {
            width: 70%;
            margin: 0 auto;
        }
        .tdRightConsultaCambioSucursalContrato {
            width: 30%;
            text-align: right;
        }
        .tdCenterConsultaCambioSucursalContrato {
            width: 2%;
        }
        .tdLeftConsultaCambioSucursalContrato {
            width: 68%;
        }
        .tdButtonConsultaCambioSucursalContrato  {
            text-align: center;
            margin: 0 auto;
        }
        .Grid
        {
            width: 90%;
            margin: 25px auto 15px auto;
        }
    </style>
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () { initChild(); });

        function initChild() {
        }

        function BtnBuscar(guid, xml, senderId) {
            var width = ObtenerAnchoBuscador(xml);
            var height = "320px";

            if (!senderId)
                senderId = '<%= this.btnResult.ClientID %>';

            var sender = $("#" + senderId);

            $.BuscadorWeb({
                xml: xml,
                guid: guid,
                btnSender: sender,
                features: {
                    dialogWidth: width,
                    dialogHeight: height,
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
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPCIONES - Cambiar Contratos de Sucursal</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div id="Navegacion" style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 32px;">
                <li id="RegistrarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkRegistro" NavigateUrl="~/Contratos.UI/ConsultarContratoSucursalUI.aspx"
                        runat="server" CausesValidation="False">
                        CONSULTAR
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <div id="BarraHerramientas" style="width: 835px; float: right;">
                <div class="Ayuda" style="top: 0px;">
                    <input id="btnAyuda" type="button" onclick="ShowHelp();" class="btnAyuda" />
                </div>
            </div>
        </div>
        <!-- Cuerpo Contenido -->
        <div id="CuerpoPagina" class="GroupSection GroupSectionFixCambioSucursal">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <span>CAMBIAR CONTRATO DE SUCURSAL</span>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnGuardar" runat="server" Text="GUARDAR" 
                        CssClass="btnWizardTerminar" onclick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="CANCELAR" CssClass="btnWizardCancelar" onclick="btnCancelar_Click" />
                </div>
            </div>
            <div class="GroupContentCollapsable">
                <fieldset style="width: 100%; margin 0 auto;">
                    <legend>DATOS DEL CONTRATO</legend>
                    <table style="width: 100%; margin 0 auto;">
                        <tr>
                            <td class="tdRightConsultaCambioSucursalContrato">
                                <label>
                                    CONTRATO</label>
                            </td>
                            <td class="tdCenterConsultaCambioSucursalContrato">
                                &nbsp;
                            </td>
                            <td class="tdLeftConsultaCambioSucursalContrato">
                                <asp:TextBox ID="txtNumeroContrato" runat="server" MaxLength="50" Width="40%"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdnContratoId" Value="" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdRightConsultaCambioSucursalContrato">
                                <label>
                                    SUCURSAL</label>
                            </td>
                            <td class="tdCenterConsultaCambioSucursalContrato">
                                &nbsp;
                            </td>
                            <td class="tdLeftConsultaCambioSucursalContrato">
                                <asp:TextBox ID="txtSucursal" runat="server" MaxLength="70" Width="50%"></asp:TextBox>
                                <asp:HiddenField ID="hdnSucursalID" runat="server" Value="" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset style="width: 100%; margin 0 auto;">
                    <legend>NUEVA SUCURSAL</legend>
                    <table style="width: 100%; margin 0 auto;">
                        <tr>
                            <td class="tdRightConsultaCambioSucursalContrato">
                                <label>
                                    SUCURSAL
                                 </label>
                            </td>
                            <td class="tdCenterConsultaCambioSucursalContrato">
                                &nbsp;
                            </td>
                            <td class="tdLeftConsultaCambioSucursalContrato">
                                <asp:TextBox ID="txtSucursalNombreNueva" runat="server" MaxLength="70" Width="50%" AutoPostBack="True"
                                OnTextChanged="txtSucursalNombreNueva_TextChanged"></asp:TextBox>
                                <asp:ImageButton runat="server" ID="ibtnBuscarSucursal" CommandName="VerSucursal"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument=''
                                OnClick="ibtnBuscarSucursal_Click" />
                                <asp:HiddenField runat="server" ID="hdnSucursalIdNueva" Value="" />
                            </td>
                        </tr>
                        <tr>
                            <td  class="tdRightConsultaCambioSucursalContrato">
                                <label>OBSERVACIONES</label>
                            </td>
                            <td class="tdCenterConsultaCambioSucursalContrato">
                                &nbsp;
                            </td>
                            <td class="tdLeftConsultaCambioSucursalContrato">
                                <asp:TextBox runat="server" ID="txtObservaciones" Width="70%" TextMode="MultiLine" Rows="5" MaxLength="300" 
                                    style="max-width: 300px; min-width: 300px;  max-height: 100px; min-height: 100px;"/>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </div>
        <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
        <asp:HiddenField runat="server" ID = "hdnTipoContrato" Value="" />
    </div>
</asp:Content>
