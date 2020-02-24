<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ConsultarContactoClienteUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.ConsultarContactoClienteUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloIngresarUnidad.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/MantenimientoResponsive.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/bootstrap.1.8.2.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .GroupSection { max-width: 650px; min-width:100px; margin: 0px auto; }
        .GroupContentCollapsable table.table-responsive { margin: 20px auto; width: 506px; }
        .GroupContentCollapsable .btnComando { margin: 20px auto 0px auto; }
        .Grid { border: none;}
        .ChildGrid { margin: 0px; padding: 0px; border: none;}
        .btnRegistrarIngreso { background: #DE0814 !important; border: none !important; height: 30px; line-height:30px; color: White; font-weight: bold; text-decoration: none; text-transform: uppercase; width: 185px; cursor: pointer; }
        .Grid th, .ChildGrid th { font-size : 12px !important; text-align: center; }
        .Grid td, .ChildGrid td { font-size : 11px !important; text-align: left; vertical-align: middle; }
        .td-finder { float: left; white-space: nowrap;}
        .ChildGrid td { border: solid 1px #cccccc; border-top: none; }
        
    </style>
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/bootstrap-1.8.2.js") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/mantenimiento-responsive.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            initScript();
        });

        function initScript() {
            cloneMenu();
            loadMenuPrincipalSelected();
            listenClickMenuResponsive();
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


<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

   <div id="PaginaContenido">
        <!--Barra de Localización-->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">MANTENIMIENTO - CONSULTAR CONTACTO CLIENTE</asp:Label>
        </div>
        <!--Menú secundario-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Mantenimiento.UI/ConsultarContactoClienteUI.aspx"> CONSULTAR <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /></asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="hlRegistroContacto" runat="server" NavigateUrl="~/Mantenimiento.UI/RegistrarContactoClienteUI.aspx"> REGISTRAR <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/></asp:HyperLink>
                </li>
            </ul>
        </div>

        <!--Filtros de la consulta-->
        <div id="Formulario" class="GroupSection">
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>¿QU&Eacute; CONTACTO CLIENTE DESEA CONSULTAR?</td>
                        <td>
                            <img id="img1" class="imgMenu" src="../Contenido/Imagenes/FlechaAbajo.png" alt="Click para Ocultar/Mostrar" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="GroupContentCollapsable">
                <table class="trAlinearDerecha table-responsive">
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>SUCURSAL</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtSucursal" runat="server" MaxLength="30" CssClass="input-find-responsive" AutoPostBack="true" OnTextChanged="txtSucursal_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="btnBuscarSucursal" ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="CONSULTAR SUCURSALES" OnClick="btnBuscarSucursal_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>CLIENTE</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtNombreCliente" runat="server" CssClass="input-find-responsive" AutoPostBack="true" OnTextChanged="txtNombreCliente_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="btnBuscarCliente" CommandName="VerCliente"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="CONSULTAR CLIENTES" OnClick="btnBuscarCliente_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>ESTATUS</td>
                        <td class="input-space-responsive" >&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:DropDownList ID="ddlActivo" runat="server" CssClass="input-dropdown-responsive">
                                <asp:ListItem Value="" Text="Todos" Selected="true"></asp:ListItem>
                                <asp:ListItem Value="True" Text="ACTIVO"></asp:ListItem>
                                <asp:ListItem Value="False" Text="INACTIVO"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <asp:Button runat="server" ID="btnBuscarOrdenes" Text="Buscar" OnClick="BuscarContactosCliente_Click" CssClass="btnComando" />
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                </div>
            </div>
        </div>
    <br />
        <asp:UpdatePanel ID="UPContenedor" runat="server">
            <ContentTemplate>
                <div style="margin-left:20px; margin-right:20px; width:95% !important; overflow: auto;">
                    <asp:GridView ID="gvContactoCliente" runat="server" AutoGenerateColumns="False" CssClass="Grid" 
                        AllowPaging="true" PageSize="10" EnableViewState="false" OnRowCommand="gvContactoCliente_RowCommand"
                        OnPageIndexChanging="gvContactoCliente_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="229px" HeaderText="SUCURSAL">
                                <ItemTemplate>
                                        <asp:Label ID="lbNombreContacto" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Sucursal.Nombre") %>' Width="343px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"/>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="229px" HeaderText="CLIENTE">
                                <ItemTemplate>
                                        <asp:Label ID="lbCliente" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"CuentaClienteIdealease.Nombre") %>' Width="344px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"/>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="229px" HeaderText="ESTATUS">
                                <ItemTemplate>
                                        <asp:Label ID="lbEstatus" runat="server" Width="229px"><%# DataBinder.Eval(Container, "DataItem.Activo").ToString().Replace("True", "ACTIVO").Replace("False", "INACTIVO") %></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"/>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
					        <asp:TemplateField ItemStyle-Width="50px" HeaderText="VER">
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="btnVer" CommandName="Ver" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' ImageUrl="~/Contenido/Imagenes/VER.png" ToolTip="" ImageAlign="Middle" />
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
        <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
        <asp:HiddenField ID="hdnLibroActivos" runat="server" />
    </div>
</asp:Content>

