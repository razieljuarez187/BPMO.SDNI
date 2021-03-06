﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Contenido/MasterPages/Site.Master" CodeBehind="ConsultarContratoROUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.ConsultarContratoROUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .GroupSection
        {
            width: 650px;
            margin: 0 auto;
        }
        .GroupContentCollapsable table
        {
            margin: 20px auto;
            width: 506px;
        }
        .GroupContentCollapsable .btnComando
        {
            margin: 20px auto 0 auto;
            display: inherit;
        }
        .Grid
        {
            width: 90%;
            margin: 25px auto 15px auto;
        }
        
        #BarraHerramientas { width: 832px !important;}
    </style>
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () { initChild(); });

        function initChild() {
            initPage();
            inicializeHorizontalPanels();
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
        <!-- Barra de localizacion -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - Consultar RENTA ORDINARIA</asp:Label>
        </div>
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 64px;">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Contratos.PSL.UI/ConsultarContratoROUI.aspx">
                        CONSULTAR R.O.
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="hlRegistroOrden" runat="server" NavigateUrl="~/Contratos.PSL.UI/RegistrarContratoROUI.aspx">
                        REGISTRAR RENTA R.O.
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
        </div>

        <!-- Cuerpo -->
        <div id = "Formulario" class = "GroupSection">
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>
                            ¿QUÉ CONTRATO DE RENTA QUIERE CONSULTAR?
                        </td>
                        <td>
                            <img id="img1" class="imgMenu" src="../Contenido/Imagenes/FlechaAbajo.png" alt="Click para Ocultar/Mostrar" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class = "GroupContentCollapsable">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class = "tdCentradoVertical"># CONTRATO</td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtNumeroContrato" runat="server" MaxLength="50" Width="275px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                         <td class="tdCentradoVertical">* FECHA INICIO DE CONTRATO</td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtFechaInicioContrato" runat="server" MaxLength="30" Width="95px"
                                CssClass="CampoFecha"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                         <td class="tdCentradoVertical">* FECHA FIN DE CONTRATO</td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtFechaFinContrato" runat="server" MaxLength="30" Width="95px"
                                CssClass="CampoFecha"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">CLIENTE</td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtNombreCuentaCliente" runat="server" MaxLength="100" Width="275px"
                                AutoPostBack="True" OnTextChanged="txtNombreCuentaCliente_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscarCliente" CommandName="VerCliente"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Clientes" CommandArgument=''
                                OnClick="ibtnBuscarCliente_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">SUCURSAL</td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:DropDownList ID="ddlSucursales" runat="server" Width="275px">
                                <asp:ListItem Text="" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">ESTATUS</td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:DropDownList ID="ddlEstatus" runat="server" Width="200px">
                                <asp:ListItem Value="-1" Text="Todos" Selected="true"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class = "tdCentradoVertical">SERIE UNIDAD</td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox id="txtNumeroSerie" runat="server" Width="275px" 
                            AutoPostBack="true" OnTextChanged="txtNumeroSerie_TextChanged"></asp:TextBox> 
                            <asp:ImageButton runat="server" ID="btnBuscarVin" CommandName="VerVin" ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar VIN" CommandArgument='' onclick="btnBuscarVin_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class = "tdCentradoVertical">ECODE</td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtNumeroEconomico" runat="server" MaxLength="50" Width="275px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:Button runat="server" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click" ValidationGroup="Requeridos" CssClass="btnComando" />
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
                                <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                <ItemStyle HorizontalAlign="Left" Width="120px" />
                            </asp:BoundField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Fecha de Contrato</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblFechaContrato" Text='<%# DataBinder.Eval(Container.DataItem,"FechaContrato") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                <ItemStyle HorizontalAlign="Right" Width="90px" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Fecha Cierre de Contrato</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblFechaCierreContrato" Text='<%# DataBinder.Eval(Container.DataItem, "FechaFin") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                <ItemStyle HorizontalAlign="Right" Width="90px" />
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
                                    Sucursal</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSucursal" Text='<%# DataBinder.Eval(Container.DataItem,"Sucursal.Nombre") %>'
                                        Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="EstatusText" HeaderText="Estatus">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="110px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NumeroSerie" HeaderText="Serie Unidad" SortExpression="NumeroSerie">
                                <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                <ItemStyle HorizontalAlign="Left" Width="120px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NumeroEconomico" HeaderText="eCODE" SortExpression="NumeroEconomico">
                                <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                <ItemStyle HorizontalAlign="Left" Width="120px" />
                            </asp:BoundField>
                            <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnVer" CommandName="Detalles" ImageUrl="~/Contenido/Imagenes/VER.png"
                                    ToolTip="Ver detalles" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"ContratoID") %>'
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%--Controles ocultos--%>
    <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
    <asp:HiddenField runat="server" ID="hdnClientID" />
    <asp:HiddenField runat="server" ID="hdnCuentaClienteID" />    
    <asp:HiddenField runat="server" ID="hdnUnidadID" />
</asp:Content>
