<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="ConsultarTableroRDUI.aspx.cs" Inherits="BPMO.SDNI.Flota.UI.ConsultarTableroRDUI" %>

<%-- 
    Satisface al caso de uso CU009 - Consultar Tablero de Seguimiento Unidades Renta Diaria
--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #MenuSecundario
        {
            float: left;
            height: 31px;
        }
        #BarraHerramientas
        {
            width: 835px;
            float: right;
        }
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
            width: 98%;
            margin: 25px auto 15px auto;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () { initChild(); });

        function initChild() {
            initPage(); inicializeHorizontalPanels();
        }

        function initPage() {
            $('.CampoFecha').each(function () {
                if ($(this).attr("disabled") != false && $(this).attr("disabled") != "disabled") {
                    $(this).datepicker({
                        yearRange: '-100:+10',
                        changeYear: true,
                        changeMonth: true,
                        dateFormat: "dd/mm/yy",
                        buttonImage: '../Contenido/Imagenes/calendar.gif',
                        buttonImageOnly: true,
                        toolTipText: "Fecha",
                        showOn: 'button'
                    });

                    $(this).attr('readonly', true);
                }
            });
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
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">FLOTA - TABLERO DE SEGUIMIENTO DE UNIDADES DE RENTA DIARIA</asp:Label>
        </div>
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 32px;">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/flota.UI/ConsultarTableroRDUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <div id="BarraHerramientas">
                <asp:Menu runat="server" ID="mnTableroRD" IncludeStyleBlock="False" Orientation="Horizontal"
                    CssClass="MenuPrimario" OnMenuItemClick="mnTableroRD_MenuItemClick">
                    <Items>
                        <asp:MenuItem Text="Hacer reservaci&oacute;n" Value="HacerReservacion" />
                    </Items>
                    <LevelSubMenuStyles>
                        <asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" />
                    </LevelSubMenuStyles>
                    <DynamicHoverStyle CssClass="itemSeleccionado" />
                    <DynamicSelectedStyle CssClass="itemSeleccionado"></DynamicSelectedStyle>
                    <StaticSelectedStyle CssClass="itemSeleccionado"></StaticSelectedStyle>
                </asp:Menu>
            </div>
        </div>
        <!-- Cuerpo -->
        <div id="Formulario" class="GroupSection">
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>
                            ¿QU&Eacute; UNIDAD DE RENTA DIARIA DESEA CONSULTAR?
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
                            # Econ&oacute;mico
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtNumeroEconomico" runat="server" Width="275px"></asp:TextBox>
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
                            <asp:TextBox ID="txtMarca" runat="server" Width="275px" AutoPostBack="true" OnTextChanged="txtMarca_TextChanged"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaMarca" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                                OnClick="ibtnBuscaMarca_Click" ToolTip="Consultar marcas" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Modelo
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtModelo" runat="server" Width="275px" AutoPostBack="true" OnTextChanged="txtModelo_TextChanged"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaModelo" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                                OnClick="ibtnBuscaModelo_Click" ToolTip="Consultar modelos" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Sucursal
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtSucursal" runat="server" Width="275px" AutoPostBack="true" OnTextChanged="txtSucursal_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscarSucursal" CommandName="VerSucursal"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument=''
                                OnClick="btnBuscarSucursal_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentreadoVertical">
                            Cliente
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtCuentaCliente" runat="server" Width="275px" AutoPostBack="true" OnTextChanged="txtCuentaCliente_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscarCuentaCliente" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                                ToolTip="Consultar Clientes" CommandArgument='' OnClick="ibtnBuscarCuentaCliente_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Estatus Unidad
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:DropDownList ID="ddlEstatusUnidad" runat="server" Width="200px">
                                <asp:ListItem Selected="True" Value="0">TODOS</asp:ListItem>
                                <asp:ListItem Value="1">DISPONIBLE</asp:ListItem>
                                <asp:ListItem Value="2">PEDIDA</asp:ListItem>
                                <asp:ListItem Value="3">RENTADA</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            ¿En Taller?
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:DropDownList ID="ddlEstaEnTaller" runat="server" Width="200px">
                                <asp:ListItem Selected="True" Value="0">TODOS</asp:ListItem>
                                <asp:ListItem Value="1">SI</asp:ListItem>
                                <asp:ListItem Value="2">NO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            ¿En Reserva?
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:DropDownList ID="ddlEstaReservada" runat="server" Width="200px">
                                <asp:ListItem Selected="True" Value="0">TODOS</asp:ListItem>
                                <asp:ListItem Value="1">SI</asp:ListItem>
                                <asp:ListItem Value="2">NO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            # Contrato
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtNumeroContrato" runat="server" Width="190px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Fecha Inicio Contrato
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtFechaInicial" runat="server" MaxLength="30" Width="95px" CssClass="CampoFecha"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Fecha Fin
                            <br />
                            Contrato
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtFechaFinal" runat="server" MaxLength="30" Width="95px" CssClass="CampoFecha"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="btnComando" OnClick="btnBuscar_Click" />
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </div>
        <div id="resultados">
            <asp:GridView runat="server" ID="grdTableroRD" AutoGenerateColumns="False" AllowPaging="True"
                EnableSortingAndPagingCallbacks="True" CssClass="Grid" OnPageIndexChanging="grdTableroRD_PageIndexChanging"
                OnRowCommand="grdTableroRD_RowCommand" OnRowDataBound="grdTableroRD_RowDataBound">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            # Eco.</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblNumeroEconomico" Text='<%# DataBinder.Eval(Container.DataItem,"Unidad.NumeroEconomico") %>'
                                Width="100%"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Modelo</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblModelo" Text='<%# DataBinder.Eval(Container.DataItem,"Unidad.Modelo.Nombre") %>'
                                Width="100%"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" Width="90px" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Sucursal</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblSucursal" Text='<%# DataBinder.Eval(Container.DataItem,"Unidad.Sucursal.Nombre") %>'
                                Width="100%"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Cliente</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblCliente" Text='<%# DataBinder.Eval(Container.DataItem,"Cliente.Nombre") %>'
                                Width="100%"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField>
                         <HeaderTemplate>
                            Estatus</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblEstatus" Text='<%# DataBinder.Eval(Container.DataItem,"EstatusActualText") %>'
                                Width="80px"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            ¿En Taller?</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblEstaEnTaller" Text='<%# DataBinder.Eval(Container.DataItem,"EstaEnTaller") %>'
                                Width="100%"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" Width="60px" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            ¿En Reserva?</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblEstaReservada" Text='<%# DataBinder.Eval(Container.DataItem,"EstaReservada") %>'
                                Width="100%"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" Width="60px" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="# Contrato" DataField="NumeroContrato">
                        <ItemStyle Width="90px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Fecha Contrato" DataField="FechaContrato" DataFormatString="{0:dd/MM/yyyy}">
                        <ItemStyle Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="D&iacute;as Renta Prog." DataField="DiasRentaProgramados">
                        <ItemStyle Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="D&iacute;as Renta Actual" DataField="DiasRentaActuales">
                        <ItemStyle Width="50px" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="btnVerUnidad" CommandName="DetallesUnidad" ImageUrl="~/Contenido/Imagenes/unidad.png"
                                ToolTip="Ver unidad" Enabled="<%# ActivarDetallesUnidad %>" CommandArgument='<%#  DataBinder.Eval(Container.DataItem,"Unidad.UnidadID") %>'
                                ImageAlign="Middle" />
                        </ItemTemplate>
                        <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="btnVerContrato" CommandName="DetallesContrato"
                                ImageUrl="~/Contenido/Imagenes/contrato.png" ToolTip="Ver contrato" Enabled= "<%# ActivarDetallesContrato %>"
                                CommandArgument='<%#  DataBinder.Eval(Container.DataItem,"Unidad.UnidadID") %>'
                                ImageAlign="Middle" />
                        </ItemTemplate>
                        <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="UnidadID" HeaderText="UnidadID" Visible="False"></asp:BoundField>
                </Columns>
                <RowStyle CssClass="GridRow" />
                <HeaderStyle CssClass="GridHeader" />
                <FooterStyle CssClass="GridFooter" />
                <PagerStyle CssClass="GridPager" />
                <SelectedRowStyle CssClass="GridSelectedRow" />
                <AlternatingRowStyle CssClass="GridAlternatingRow" />
            </asp:GridView>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnUnidadOperativaID"></asp:HiddenField>
    <asp:HiddenField runat="server" ID="hdnMarcaID"></asp:HiddenField>
    <asp:HiddenField runat="server" ID="hdnModeloID"></asp:HiddenField>
    <asp:HiddenField runat="server" ID="hdnSucursalID"></asp:HiddenField>
    <asp:HiddenField runat="server" ID="hdnCuentaClienteID"></asp:HiddenField>
    <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click"
        Style="display: none;" />
</asp:Content>
