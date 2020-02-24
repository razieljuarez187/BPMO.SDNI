<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="CambiarSucursalEquipoAliadoUI.aspx.cs" Inherits="BPMO.SDNI.Flota.UI.CambiarSucursalEquipoAliadoUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #MenuSecundario { float: left; height: 64px; }
        #BarraHerramientas { width: 835px; float: right; }
        .GroupBody { margin: 10px auto; width: 761px; }
        .GroupHeader { width: 100%; }
        #ControlesDatos
        {
            min-height: 120px;
            margin-top: 10px;
            position: inherit;
            border: 1px solid transparent;
        }
        
        #ControlesDatos fieldset
        {
            width: 95%;
            margin-left: auto;
            margin-right: auto;
            margin-bottom: 15px;
        }
    </style>
    <script type="text/javascript">
        function BtnBuscar(guid, xml) {
            var width = ObtenerAnchoBuscador(xml);

            $.BuscadorWeb({
                xml: xml,
                guid: guid,
                btnSender: $("#<%=btnResult.ClientID %>"),
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
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">FLOTA - REASIGNACI&Oacute;N DE SUCURSAL PARA EQUIPO ALIADO</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li id="ConsultarCatalogo">
                    <asp:HyperLink ID="hlkConsultarFlota" runat="server" NavigateUrl="~/Flota.UI/ConsultarSeguimientoFlotaUI.aspx">
						CONSULTAR UNIDAD
						<img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
                <li id="Movimiento" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkMovimiento" runat="server" NavigateUrl="~/Flota.UI/CambiarSucursalEquipoAliadoUI.aspx">
						MOVER EQ. ALIADO
						<img id="imgCambioSucursalEA" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
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
                <asp:Label ID="lblTituloPaso" runat="server" Text="REASIGNACIÓN DE SUCURSAL PARA EQUIPO ALIADO"></asp:Label>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btnWizardGuardar"
                        OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar"
                        OnClick="btnCancelar_Click" />
                </div>
            </div>
            <div id="ControlesDatos">
                <fieldset>
                    <legend>DETALLE EQUIPO ALIADO</legend>
                    <table class="trAlinearDerecha" style="margin: 0px auto;">
                        <tr>
                            <td class="tdCentradoVertical">
                                <span>*</span># Serie
                            </td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical" style="width: 280px;">
                                <asp:TextBox ID="txtNumeroSerie" runat="server" Width="240px" AutoPostBack="True"
                                    OnTextChanged="txtEquipoAliado_TextChanged"></asp:TextBox>
                                <asp:ImageButton ID="ibtnBuscaEquipoAliado" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                                    OnClick="ibtnBuscaEquipoAliado_Click" Height="17px" ToolTip="Consultar Unidad" />
                            </td>
                            <td class="tdCentradoVertical" style="text-align: right;">
                                Clave Oracle
                            </td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtOracleID" runat="server" Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdCentradoVertical">
                                Marca
                            </td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtMarca" runat="server" Width="250px"></asp:TextBox>
                            </td>
                            <td class="tdCentradoVertical" style="text-align: right;">
                                Horas Iniciales
                            </td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtHorasIniciales" runat="server" CssClass="CampoNumero" Width="60px"
                                    MaxLength="150"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdCentradoVertical">
                                Modelo
                            </td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtModelo" runat="server" Width="250px"></asp:TextBox>
                            </td>
                            <td class="tdCentradoVertical" style="text-align: right;">
                                A&ntilde;o Modelo
                            </td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtAnioModelo" runat="server" Width="80px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdCentradoVertical">
                                PBV
                            </td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtPBV" runat="server" CssClass="CampoNumero" MaxLength="50" Width="80px"></asp:TextBox>
                            </td>
                            <td class="tdCentradoVertical" style="text-align: right;">
                                PBC
                            </td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtPBC" runat="server" CssClass="CampoNumero" Width="80px" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdCentradoVertical">
                                Tipo Equipo
                            </td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtTipoEquipo" runat="server" Width="203px"></asp:TextBox>
                            </td>
                            <td class="tdCentradoVertical" style="text-align: right;">
                                Estatus
                            </td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtEstatusEquipo" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>SUCURSAL ORIGEN</legend>
                    <table class="trAlinearDerecha" style="width: 690px; margin: 0px auto;">
                        <tr>
                            <td class="tdCentradoVertical" style="width: 75px;">
                                Sucursal
                            </td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical" style="width: 275px;">
                                <asp:TextBox ID="txtSucursalActual" runat="server" Width="240px"></asp:TextBox>
                                <asp:HiddenField ID="hdnSucursalActualID" runat="server" />
                            </td>
                            <td class="tdCentradoVertical" style="text-align: right;">
                                Empresa
                            </td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtEmpresaActual" runat="server" Width="240px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table class="trAlinearDerecha" style="width: 690px; margin: 0px auto">
                        <tr>
                            <td class="tdCentradoVertical" style="width: 75px;">
                                Domicilio
                            </td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtDomicilioActual" runat="server" Rows="5" Columns="30" TextMode="MultiLine"
                                    MaxLength="500" Style="float: left; max-width: 590px; min-width: 590px; max-height: 40px;
                                    min-height: 40px;"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>SUCURSAL DESTINO</legend>
                    <table class="trAlinearDerecha" style="width: 690px; margin: 0px auto">
                        <tr>
                            <td class="tdCentradoVertical" style="width: 75px;">
                                <span>*</span>Sucursal
                            </td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtSucursalDestino" runat="server" Width="240px" OnTextChanged="txtSucursalDestino_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>
                                <asp:ImageButton runat="server" ID="ibtnBuscarSucursal" CommandName="VerSucursal"
                                    ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument=''
                                    OnClick="ibtnBuscarSucursal_Click" />
                                <asp:HiddenField ID="hdnSucursalDestinoID" runat="server" />
                            </td>
                            <td class="tdCentradoVertical" style="text-align: right;">
                                Empresa
                            </td>
                            <td style="width: 5px;">&nbsp;</td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtEmpresaDestino" runat="server" Width="240px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table class="trAlinearDerecha" style="width: 690px; margin: 0px auto">
                        <tr>
                            <td class="tdCentradoVertical" style="width: 75px;">
                                Domicilio
                            </td>
                            <td style="width: 5px;">
                                &nbsp;
                            </td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtDomicilioDestino" runat="server" Rows="5" Columns="30" TextMode="MultiLine"
                                    MaxLength="500" Style="float: left; max-width: 590px; min-width: 590px; max-height: 40px;
                                    min-height: 40px;"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div class="ContenedorMensajes">
                <span class="Requeridos"></span>
                <br />
                <span class="FormatoIncorrecto"></span>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnEstatusID" runat="server" />
    <asp:HiddenField ID="hdnEmpresaActualID" runat="server" />
    <asp:HiddenField ID="hdnEmpresaDestinoID" runat="server" />
    <asp:HiddenField ID="hdnEquipoAliadoID" runat="server" />
    <asp:HiddenField ID="hdnEquipoID" runat="server" />
    <asp:HiddenField ID="hdnLiderID" runat="server" />
    <asp:HiddenField ID="hdnModeloID" runat="server" />
    <asp:HiddenField ID="hdnOracleID" runat="server" />
    <asp:HiddenField ID="hdnSucursalID" runat="server" />
    <asp:HiddenField ID="hdnTipoEquipoAliadoID" runat="server" />
    <asp:HiddenField ID="hdnUnidadOperativaID" runat="server" />
    <asp:HiddenField ID="hdnMensaje" runat="server" />
    <asp:HiddenField ID="hdnTipoMensaje" runat="server" />
    <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
</asp:Content>
