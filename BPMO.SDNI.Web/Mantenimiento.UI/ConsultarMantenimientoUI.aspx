<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" 
    AutoEventWireup="true" CodeBehind="ConsultarMantenimientoUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.ConsultarMantenimientoUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloIngresarUnidad.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/MantenimientoResponsive.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/bootstrap.1.8.2.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .GroupSection { max-width: 650px; min-width:100px; margin: 0px auto; }
        .GroupContentCollapsable table { margin: 20px auto; width: 506px; }
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
            inicializeHorizontalPanels();
            cloneMenu();
            loadMenuPrincipalSelected();
            listenClickMenuResponsive();
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

    <div>
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label runat="server">MANTENIMIENTO - CONSULTAR ORDEN DE SERVICIO IDEALEASE</asp:Label>
        </div>
        <div id="ContenedorMenuSecundario" style="height: 80px;">
        </div>
        <!-- Cuerpo -->
        <div id="Div1" class="GroupSection">
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>CONSULTAR ORDEN DE SERVICIO IDEALEASE</td>
                        <td>
                            <img id="img1" class="imgMenu" src="../Contenido/Imagenes/FlechaAbajo.png" alt="Click para Ocultar/Mostrar" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="GroupContentCollapsable">
                <table class="trAlinearDerecha table-responsive">
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>VIN</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtVIN" runat="server" MaxLength="30" CssClass="input-find-responsive" AutoPostBack="true" OnTextChanged="txtVIN_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="btnBuscarVin" CommandName="VerVin" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                                ToolTip="Consultar VIN" CommandArgument='' OnClick="btnBuscarVin_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>NÚMERO ECONÓMICO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtNumeroEconomico" runat="server" CssClass="input-text-responsive"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>MODELO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtModelo" runat="server" CssClass="input-find-responsive" AutoPostBack="true" OnTextChanged="txtModelo_TextChanged"></asp:TextBox>
                            <asp:HiddenField ID="hdnModeloID" runat="server"/>
                            <asp:ImageButton runat="server" ID="btnBuscarModelo" CommandName="Modelo" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                                ToolTip="Consultar Unidad" CommandArgument='' OnClick="btnBuscarModelo_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>NÚMERO ORDEN DE SERVICIO</td>
                        <td class="input-space-responsive" >&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtNumeroOrdenServicio" runat="server" CssClass="CampoNumeroEntero input-text-responsive"></asp:TextBox>
 
                        </td>
                    </tr>
                </table>
                <asp:Button runat="server" ID="btnBuscarOrdenes" Text="Buscar" OnClick="OnClickBuscarUnidades" CssClass="btnComando" />
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                </div>
            </div>
        </div>
    </div>
    <br />
    <asp:UpdatePanel ID="UPContenedor" runat="server">
        <ContentTemplate>
            <div style="margin-left:20px; margin-right:20px; width:95% !important; overflow: auto;">
                <asp:GridView ID="gvOrdenesServicio" runat="server" AutoGenerateColumns="False" CssClass="Grid" 
                    AllowPaging="true" PageSize="10" EnableViewState="false" OnRowCommand="grvOrdenesServicio_RowCommand"
                    OnPageIndexChanging="grvOrdenesServicio_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="229px" HeaderText="VIN/# SERIE">
                            <ItemTemplate>
                                    <asp:Label ID="lbNumeroSerie" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"NumeroSerie") %>' Width="229px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="229px" HeaderText="# ECON&Oacute;MICO">
                            <ItemTemplate>
                                    <asp:Label ID="lbNumeroEconomico" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"NumeroEconomico") %>' Width="229px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="229px" HeaderText="MODELO">
                            <ItemTemplate>
                                    <asp:Label ID="lbModelo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Modelo") %>' Width="229px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="229px" HeaderText="ORDEN SERVICIO">
                            <ItemTemplate>
                                    <asp:Label ID="lbEstatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"OrdenServicio") %>' Width="229px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <ItemStyle CssClass="lbGridRightAlign" />
                        </asp:TemplateField>
					    <asp:TemplateField ItemStyle-Width="50px" HeaderText="VER">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnEditOS" CommandName="Editar" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' ImageUrl="~/Contenido/Imagenes/VER.png" ToolTip="" ImageAlign="Middle" />
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
            <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
            <asp:HiddenField ID="hdnLibroActivos" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
