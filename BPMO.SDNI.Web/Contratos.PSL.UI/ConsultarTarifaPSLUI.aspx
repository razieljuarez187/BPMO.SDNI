<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ConsultarTarifaPSLUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.ConsultarTarifaPSLUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
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
        <!-- Barra de Localización-->
        <div id="BarraUbicacion">
            <asp:Label runat="server" ID="lblEncabezadoLeyenda">CAT&Aacute;LOGOS - CONSULTAR LISTA DE PRECIOS</asp:Label>
        </div>
        <!-- Menú Secundario-->
        <div style="height: 80px">
            <ul id="MenuSecundario" style="float: left;height: 64px">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink runat="server" ID="hlConsultar" NavigateUrl="~/Contratos.PSL.UI/ConsultarTarifaPSLUI.aspx">
                        CONSULTAR 
                        <img id="imgConsultaCatalogo" src="<%=Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="seleccion"/>
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink runat="server" ID="hlRegistrar" NavigateUrl="~/Contratos.PSL.UI/RegistrarTarifaPSLUI.aspx">
                        REGISTRAR 
                        <img id="imgRegistroCatalogo" src="<%=Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="seleccion"/>
                    </asp:HyperLink>
                </li>
            </ul>
        </div>
        <!--Cuerpo de la página-->
        <div id="Formulario" class="GroupSection">
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>
                            ¿QUÉ LISTA DE PRECIOS DESEA CONSULTAR?
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
                            <asp:Label runat="server" ID="lblSucursal">SUCURSAL</asp:Label>
                        </td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtSucursal" runat="server" MaxLength="30" Width="275px" AutoPostBack="True"
                                OnTextChanged="txtSucursal_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscarSucursal" CommandName="VerSucursal"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument=''
                                OnClick="ibtnBuscarSucursal_Click" />
                            <asp:HiddenField ID="hdnSucursalID" runat="server" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            <asp:Label runat="server" ID="lblModelo">Modelo</asp:Label>
                        </td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtNombreModelo" runat="server" MaxLength="50" Width="275px" AutoPostBack="True" OnTextChanged="txtNombreModelo_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscarModelo" CommandName="VerModelo"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Modelos" CommandArgument=''
                                OnClick="ibtnBuscarModelo_Click" />
                            <asp:HiddenField runat="server" ID="hdnModeloID"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                           <asp:Label runat="server" ID="lblMoneda"> MONEDA</asp:Label>
                        </td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:DropDownList runat="server" ID="ddlMoneda" Width="200px">
                                <asp:ListItem Text="Todos" Value="-1"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                           <asp:Label runat="server" ID="lblTipoTarifa"> TIPO TARIFA</asp:Label>
                        </td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:DropDownList ID="ddlTipoTarifa" runat="server" Width="200px">
                                <asp:ListItem Value="-1" Text="Todos" Selected="true"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                            <td class="tdCentradoVertical">
                                Per&iacute;odo
                            </td>
                            <td style="width: 20px;">
                                &nbsp;
                            </td>
                            <td class="tdCentradoVertical">
                                <asp:DropDownList runat="server" ID="ddlPeriodoTarifa" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                             <td class="tdCentradoVertical" style="text-align: right;">
                                Turno
                            </td>
                            <td style="width: 20px;">
                                &nbsp;
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlTarifaTurno" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Estatus
                        </td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:DropDownList ID="ddlEstatus" runat="server" Width="100px">
                                <asp:ListItem Value="true" Text="Activo" Selected="true"></asp:ListItem>
                                <asp:ListItem Value="false" Text="Inactivo"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <asp:Button runat="server" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click"
                    CssClass="btnComando" />
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="UPContenedor" runat="server">
            <ContentTemplate>
                <asp:GridView runat="server" ID="grdTarifas" AutoGenerateColumns="false" PageSize="10"
                    AllowPaging="true" AllowSorting="false" EnableSortingAndPagingCallbacks="true"
                    CssClass="Grid" OnPageIndexChanging="grdTarifas_PageIndexChanging" OnRowCommand="grdTarifas_RowCommand"
                    OnRowDataBound="grdTarifas_RowDataBound">
                    <Columns>
                        
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
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Modelo</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblModelo" Text='<%# DataBinder.Eval(Container.DataItem,"Modelo.Nombre") %>' Width="100%"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Moneda</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblMoneda" Text='<%# DataBinder.Eval(Container.DataItem,"Divisa.MonedaDestino.Nombre") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>Tipo Tarifa</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblTipoTarifa" Text='<%# DataBinder.Eval(Container.DataItem,"TipoTarifaID") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Tarifa" HeaderText="Tarifa" DataFormatString="{0:#,##0.00##}">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left"/>
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnVer" CommandName="Detalle" ImageUrl="~/Contenido/Imagenes/VER.png"
                                    ToolTip="Ver detalles" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"TarifaPSLID") %>'
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
    <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;"/>
</asp:Content>
