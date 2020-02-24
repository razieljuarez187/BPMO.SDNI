<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="ConsultarFlotaUI.aspx.cs" Inherits="BPMO.SDNI.Flota.UI.ConsultarFlotaUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloContratoFSL.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .GroupSection
        {
            width: 650px;
            margin: 0px auto;
        }
        .GroupContentCollapsable table
        {
            margin: 20px auto;
            width: 506px;
        }
        .GroupContentCollapsable .btnComando
        {
            margin: 20px auto 0px auto;
            display: inherit;
        }
        .Grid
        {
            width: 90%;
            margin: 25px auto 15px auto;
        }
    </style>
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () { initChild(); });

        function initChild() {
            initPage(); inicializeHorizontalPanels();
        }

        function initPage() {
            $('.CampoFecha').attr('readonly', true);
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
    </script>
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
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">FLOTA - Consultar Flota</asp:Label>
        </div>
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 31px;">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Flota.UI/ConsultarFlotaUI.aspx">
                        CONSULTAR 
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
            </ul>
        </div>
        <!-- Cuerpo -->
        <div id="Formulario" class="GroupSection">
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>
                            ¿QUÉ UNIDAD DESEA CONSULTAR?
                        </td>
                        <td>
                            <img id="img1" class="imgMenu" src="../Contenido/Imagenes/FlechaAbajo.png" alt="Click para Ocultar/Mostrar" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="GroupContentCollapsable">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical">
                            Sucursal
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtSucursal" runat="server" Width="250px" AutoPostBack="true" MaxLength="50" ToolTip="Sucursal" OnTextChanged="txtSucursal_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscarSucursal" CommandName="VerSucursal"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument=''
                                OnClick="ibtnBuscaSucursal_Click" />
                            <asp:HiddenField ID="hdnSucursalID" runat="server" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            # Econ&oacute;mico
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtNumeroEconomico" runat="server" Width="250px" MaxLength="50" ToolTip="N&uacute;mero econ&oacute;mico de la unidad"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            # Serie
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtNumeroSerie" runat="server" Width="250px" MaxLength="80" 
                                ToolTip="N&uacute;mero de serie de la unidad" 
                                ontextchanged="txtNumeroSerie_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="btnBuscarVin" CommandName="VerVin" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                                ToolTip="Consultar n&uacute;mero de serie de la unidad" CommandArgument='' OnClick="btnBuscarVin_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Marca
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtMarca" runat="server" Width="250px" MaxLength="80" AutoPostBack="true" ToolTip="Marca de la unidad" OnTextChanged="txtMarca_TextChanged"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaMarca" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar marca de la unidad" OnClick="ibtnBuscaMarca_Click" />
                            <asp:HiddenField ID="hdnMarcaID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Modelo
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 330px; vertical-align: middle;">
                            <asp:TextBox ID="txtModelo" runat="server" Width="250px" MaxLength="80" AutoPostBack="True" ToolTip="Modelo de la unidad" OnTextChanged="txtModelo_TextChanged"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaModelo" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar modelo de la unidad" OnClick="ibtnBuscaModelo_Click" />
                            <asp:HiddenField ID="hdnModeloID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Modelo Equipo Aliado</td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 330px; vertical-align: middle;">
                            <asp:TextBox ID="txtModeloEquipoAliado" runat="server" Width="250px" 
                                MaxLength="80" AutoPostBack="True" ToolTip="Modelo del Equipo Aliado" 
                                ontextchanged="txtModeloEquipoAliado_TextChanged" ></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscarModeloEQA" runat="server" 
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" 
                                ToolTip="Consultar modelo del Equipo Aliado" 
                                onclick="ibtnBuscarModeloEQA_Click" />
                            <asp:HiddenField ID="hdnModeloEAID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            A&Ntilde;o
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 330px; vertical-align: middle;">
                            <asp:TextBox ID="txtAnio" runat="server" Width="45px" MaxLength="4" CssClass="CampoNumero" ToolTip="A&ntilde;o de la unidad"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revAnio" runat="server" ControlToValidate="txtAnio"
                                ErrorMessage="Formato incorrecto" ValidationExpression="^[123][0-9]{3}$" ValidationGroup="Obligatorios"
                                Display="Dynamic" CssClass="ColorValidator">**</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            # PLACA
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 330px; vertical-align: middle;">
                            <asp:TextBox ID="txtNumeroPlaca" runat="server" Width="115px" MaxLength="20" ToolTip="N&uacute;mero de placa de la unidad"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:Button runat="server" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click" CssClass="btnComando" ToolTip="Consultar Flota" />
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>            
        </div>
        <asp:UpdatePanel ID="UPContenedor" runat="server">
                <ContentTemplate>
                <div id="resultados">
                    <asp:GridView ID="grdElementosFlota" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="10"
                        AllowSorting="false"  EnableSortingAndPagingCallbacks="True" CssClass="Grid" 
                        OnPageIndexChanging="grdElementosFlota_PageIndexChanging" 
                        onrowcommand="grdElementosFlota_RowCommand">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>MODELO</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblModelo" Text='<%# DataBinder.Eval(Container.DataItem,"Unidad.Modelo.Nombre") %>'
                                        Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>MARCA</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMarca" Text='<%# DataBinder.Eval(Container.DataItem,"Unidad.Modelo.Marca.Nombre") %>'
                                        Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                            <HeaderTemplate># ECON&Oacute;MICO</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblNumeroEconocmico" Text='<%# DataBinder.Eval(Container.DataItem,"Unidad.NumeroEconomico") %>' Width="100%"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                            <asp:TemplateField>
                            <HeaderTemplate># SERIE</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblNumeroSerie" Text='<%# DataBinder.Eval(Container.DataItem,"Unidad.NumeroSerie") %>' Width="100%"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>SUCURSAL</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSucursal" Text='<%# DataBinder.Eval(Container.DataItem,"Unidad.Sucursal.Nombre") %>'
                                        Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                        <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="ibtnVer" CommandName="Detalles" ImageUrl="~/Contenido/Imagenes/VER.png"
                                ToolTip="Ver detalles" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"Unidad.UnidadID") %>'
                                ImageAlign="Middle" />
                        </ItemTemplate>
                        <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                        </Columns>
                        <RowStyle CssClass="GridRow" />
                        <HeaderStyle CssClass="GridHeader" />
                        <FooterStyle CssClass="GridFooter" />
                        <PagerStyle CssClass="GridPager" />
                        <SelectedRowStyle CssClass="GridSelectedRow" />
                        <AlternatingRowStyle CssClass="GridAlternatingRow" />
                    </asp:GridView>
                </div>
                </ContentTemplate>
            </asp:UpdatePanel>
    </div>
    <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click"
        Style="display: none;" />
</asp:Content>
