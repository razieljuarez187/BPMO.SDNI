<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ConsultarTramitesUI.aspx.cs" Inherits="BPMO.SDNI.Tramites.UI.ConsultarTramitesUI" %>
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
    </style>
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%=Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
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
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">UNIDADES - CONSULTAR TR&Aacute;MITES</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li id="ConsultarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultarActaNacimiento" runat="server" NavigateUrl="~/Tramites.UI/ConsultarTramitesUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
            </ul>
        </div>
        <div id="Formulario" class="GroupSection">
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>¿DE QU&Eacute; UNIDAD DESEA CONSULTAR TR&Aacute;MITES?</td>
                        <td>
                            <img id="img1" class="imgMenu" src="../Contenido/Imagenes/FlechaAbajo.png" alt="Click para Ocultar/Mostrar" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="GroupContentCollapsable">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical"># Serie de Unidad</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtNumeroSerie" runat="server" MaxLength="30" Columns="40" OnTextChanged="txtNumeroSerie_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscarUnidad" CommandName="VerUnidad"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Unidades" CommandArgument=''
                                OnClick="ibtnBuscarUnidad_Click" />
                        </td>
                    </tr>
                 </table>
                 <asp:Button runat="server" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click" CssClass="btnComando" />
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
             </div>
          </div> 
          <asp:UpdatePanel ID="UPContenedor" runat="server">
               <ContentTemplate>
                    <asp:GridView runat="server" ID="grdUnidades" AutoGenerateColumns="false" PageSize="10" AllowPaging="true" AllowSorting="false" 
                    EnableSortingAndPagingCallbacks="true" CssClass="Grid" OnPageIndexChanging="grdUnidades_PageIndexChanging"
                    OnRowCommand="grdUnidades_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="NumeroSerie" HeaderText="Número Serie" SortExpression="NumeroSerie">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left"/>
                            </asp:BoundField>
                            <asp:TemplateField>
                                <HeaderTemplate>Modelo</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblModelo" Text='<%# DataBinder.Eval(Container.DataItem,"Modelo.Nombre")%>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>Marca</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMarca" Text='<%# DataBinder.Eval(Container.DataItem,"Modelo.Marca.Nombre")%>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                            
                            <asp:TemplateField>
                                <HeaderTemplate>Tipo de Unidad</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTipoEquipoServicio" Text='<%# DataBinder.Eval(Container.DataItem,"TipoEquipoServicio.Nombre")%>' ></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left"/>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="btnVer" CommandName="Detalles" ImageUrl="~/Contenido/Imagenes/VER.png"
                                        ToolTip="Ver detalles" CommandArgument='<%# ((GridViewRow) Container).DataItemIndex %>'
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
    <asp:HiddenField runat="server" ID="hdnTramitableID" />
    <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
</asp:Content>
