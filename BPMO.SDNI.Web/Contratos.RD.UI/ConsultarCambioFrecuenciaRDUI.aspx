<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ConsultarCambioFrecuenciaRDUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.RD.UI.ConsultarCambioFrecuenciaRDUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .GroupSectionFixCambioSucursal {
            width: 70%;
            margin: 0 auto;
        }
        .GroupContentCollapsable > table {
            width: 100%;
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
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>"
        type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () { initChild(); });

        function initChild() {
            inicializeHorizontalPanels();
        }

        function inicializeHorizontalPanels() {
            $(".GroupHeaderCollapsable").click(function () {
                $(this).next(".GroupContentCollapsable").slideToggle(500);
                if ($(this).find(".imgMenu").attr("src") == "../Contenido/Imagenes/FlechaArriba.png")
                    $(this).find(".imgMenu").attr("src", "../Contenido/Imagenes/FlechaAbajo.png");
                else
                    $(this).find(".imgMenu").attr("src", "../Contenido/Imagenes/FlechaArriba.png");
                return false;
            });
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
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPCIONES - Cambiar Frecuencia de Contratos de RD</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div id="Navegacion" style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 32px;">
                <li id="RegistrarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkRegistro" NavigateUrl="~/Contratos.RD.UI/ConsultarCambioFrecuenciaRDUI.aspx"
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
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>
                            <label>
                                ¿QU&Eacute; CONTRATO DESEA CAMBIAR DE FRECUENCIA DE FACTURACI&Oacute;N?</label>
                        </td>
                        <td>
                            <img id="img1" class="imgMenu" src="../Contenido/Imagenes/FlechaAbajo.png" alt="Click para Ocultar/Mostrar" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="GroupContentCollapsable">
                <table>
                    <tr>
                        <td colspan = "3">&nbsp;</td>
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
                            <asp:TextBox ID="txtSucursal" runat="server" MaxLength="30" Width="70%" AutoPostBack="True"
                                OnTextChanged="txtSucursal_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscarSucursal" CommandName="VerSucursal"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument=''
                                OnClick="ibtnBuscarSucursal_Click" />
                            <asp:HiddenField ID="hdnSucursalID" runat="server" Value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdRightConsultaCambioSucursalContrato">
                            <label>
                                CLIENTE</label>
                        </td>
                        <td class="tdCenterConsultaCambioSucursalContrato">
                            &nbsp;
                        </td>
                        <td class="tdLeftConsultaCambioSucursalContrato">
                            <asp:TextBox ID="txtNombreCuentaCliente" runat="server" MaxLength="100" Width="70%"
                                AutoPostBack="True" OnTextChanged="txtNombreCuentaCliente_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscarCliente" CommandName="VerSucursal"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument=''
                                OnClick="ibtnBuscarCliente_Click" />
                            <asp:HiddenField runat="server" ID="hdnClientID" />
                        </td>
                    </tr>
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
                        <td colspan="3" class="tdButtonConsultaCambioSucursalContrato">
                            <asp:Button runat="server" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click" CssClass="btnComando" />
                        </td>
                    </tr>
                </table>
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="UPContenedor" runat="server">
            <ContentTemplate>
                <asp:GridView runat="server" ID="grdContratos" AutoGenerateColumns="false" PageSize="10"
                    AllowPaging="true" AllowSorting="false" EnableSortingAndPagingCallbacks="true"
                    CssClass="Grid" OnPageIndexChanging="grdContratos_PageIndexChanging" OnRowCommand="grdContratos_RowCommand"
                    OnRowDataBound="grdContratos_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="NumeroContrato" HeaderText="# Contrato" SortExpression="NumeroContrato">
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Sucursal
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblSucursal" Text='<%# DataBinder.Eval(Container.DataItem,"Sucursal.Nombre") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Cliente
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCliente" Text='<%# DataBinder.Eval(Container.DataItem,"Cliente.Nombre") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="28%" />
                            <ItemStyle HorizontalAlign="Left" Width="28%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Fecha Inicio
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblFechaInicio" Text='<%# DataBinder.Eval(Container.DataItem,"FechaContrato") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Fecha Promesa
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblFechaPromesa" Text='<%# DataBinder.Eval(Container.DataItem,"FechaPromesaDevolucion") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Frecuencia
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblFrecuencia" Text='<%# DataBinder.Eval(Container.DataItem,"FrecuenciaFacturacion") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnVer" CommandName="Detalles" ImageUrl="~/Contenido/Imagenes/VER.png"
                                    ToolTip="Ver detalles" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"ContratoID") %>'
                                    ImageAlign="Middle" />
                            </ItemTemplate>
                            <ItemStyle Width="2%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="GridRow" />
                    <HeaderStyle CssClass="GridHeader" />
                    <FooterStyle CssClass="GridFooter" />
                    <PagerStyle CssClass="GridPager" />
                    <SelectedRowStyle CssClass="GridSelectedRow" />
                    <AlternatingRowStyle CssClass="GridAlternatingRow" />
                </asp:GridView>
            </ContentTemplate>            
        </asp:UpdatePanel>
        <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
        <asp:HiddenField runat="server" ID = "hdnTipoContrato" Value="" />
    </div>
</asp:Content>
