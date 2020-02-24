<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ConsultarEquipoAliadoUI.aspx.cs" Inherits="BPMO.SDNI.Equipos.UI.ConsultarEquipoAliadoUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Satisface a la solicitud de Cambio SC0005 -->
    <style type="text/css">
        .GroupSection { width: 650px; margin: 0px auto; }
        .GroupContentCollapsable table { margin: 20px auto; width: 506px; }
        .GroupContentCollapsable .btnComando { margin: 20px auto 0px auto; display: inherit;}        
        .Grid { width: 90%; margin: 25px auto 15px auto; }        
    </style>
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
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">UNIDADES - CONSULTAR EQUIPO ALIADOS</asp:Label>
        </div>
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 64px;">
                <li id="ConsultarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultarActaNacimiento" runat="server" NavigateUrl="~/Equipos.UI/ConsultarEquipoAliadoUI.aspx" ForeColor="White"> 
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo">
                    <asp:HyperLink ID="hlkRegistroActaNacimiento" runat="server" NavigateUrl="~/Equipos.UI/RegistrarEquipoAliadoUI.aspx"> 
                        REGISTRAR
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
        </div>
        <!-- Cuerpo -->
        <div id="Formulario" class="GroupSection">
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>¿QU&Eacute; EQUIPO ALIADO DESEA CONSULTAR?</td>
                        <td>
                            <img id="img1" class="imgMenu" src="../Contenido/Imagenes/FlechaAbajo.png" alt="Click para Ocultar/Mostrar" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="GroupContentCollapsable">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical">Sucursal</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox id="txtSucursal" runat="server" Width="275px" AutoPostBack="true" ontextchanged="txtSucursal_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="btnBuscarSucursal" CommandName="VerSucursal" ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument='' onclick="btnBuscarSucursal_Click"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentreadoVertical"># Serie</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox id="txtNumeroSerie" runat="server" Width="275px"></asp:TextBox> 
                            <asp:ImageButton runat="server" ID="btnBuscarVin" CommandName="VerVin" ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar VIN" CommandArgument='' onclick="btnBuscarVin_Click" />
                        </td>                    
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Marca</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox id="txtMarca" runat="server" Width="275px" AutoPostBack="true" ontextchanged="txtMarca_TextChanged"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaMarca" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" onclick="ibtnBuscaMarca_Click" />
                        </td>                    
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">¿Es Activo Oracle?</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:DropDownList id="ddlActivoOracle" runat="server" Width="200px">
                                <asp:ListItem Selected="True" Value="0">TODOS</asp:ListItem>
                                <asp:ListItem Value="1">SI</asp:ListItem>
                                <asp:ListItem Value="2">NO</asp:ListItem>
                            </asp:DropDownList>
                        </td>                    
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Tipo Equipo Aliado</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical"  style="width: 320px;">
                            <asp:DropDownList runat="server" ID="ddlTipoEquipoAliado" Width="200px"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Estatus</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:DropDownList ID="ddlEstatusEquipo" runat="server" Width="200px"></asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="btnComando" onclick="btnBuscar_Click" />
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>             
        </div>
        <div id="resultados">
            <asp:GridView runat="server" id="grdEquiposAliados" AutoGenerateColumns="False" AllowPaging="True" 
                EnableSortingAndPagingCallbacks="True" CssClass="Grid" onpageindexchanging="grdEquiposAliados_PageIndexChanging" 
                onrowcommand="grdEquiposAliados_RowCommand" 
                onrowdatabound="grdEquiposAliados_RowDataBound">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>Sucursal</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblSucursal" Text='<%# DataBinder.Eval(Container.DataItem,"Sucursal.Nombre") %>' Width="100%"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="# SERIE" DataField="NumeroSerie"></asp:BoundField>
                    <asp:BoundField HeaderText="FABRICANTE" DataField="Fabricante"></asp:BoundField>
                    <asp:TemplateField>
                        <HeaderTemplate>MARCA</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblMarca" Text='<%# DataBinder.Eval(Container.DataItem,"Modelo.Marca.Nombre") %>' Width="100%"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="AÑO MODELO" Visible="False"></asp:BoundField>                    
                    <asp:TemplateField>
                        <HeaderTemplate>TIPO EQUIPO</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblTipoEquipoServicio" Text='<%# DataBinder.Eval(Container.DataItem,"TipoEquipoServicio.Nombre") %>' Width="100%"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>ACTIVO ORACLE</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblActivo" Text='<%# DataBinder.Eval(Container.DataItem,"EsActivo") %>' Width="100%"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" Width="70px" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="ESTATUS" DataField="Estatus">
                        <ItemStyle Width="110px" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="btnVer" CommandName="Detalles" ImageUrl="~/Contenido/Imagenes/VER.png"
                                ToolTip="Ver detalles" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
                                ImageAlign="Middle" />
                        </ItemTemplate>
                        <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="EquipoAliadoID" HeaderText="EquipoAliadoID" Visible="False">
                    </asp:BoundField>
                    <asp:BoundField DataField="EquipoID" HeaderText="EquipoID" Visible="False"></asp:BoundField>
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
    <asp:HiddenField runat="server" ID="hdnSucursalID"></asp:HiddenField>
    <asp:HiddenField runat="server" ID="hdnMarcaID"></asp:HiddenField>
    <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
    <asp:HiddenField ID="hdnLibroActivos" runat="server" />
</asp:Content>
