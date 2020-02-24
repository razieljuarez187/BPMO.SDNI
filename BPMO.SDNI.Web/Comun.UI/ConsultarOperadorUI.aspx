<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ConsultarOperadorUI.aspx.cs" Inherits="BPMO.SDNI.Comun.UI.ConsultarOperadorUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!--Funcionalidad Deshabilitar Enter en cajas de texto-->
<script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <style type="text/css">
        .GroupSection { width: 650px; margin: 0px auto; }
        .GroupContentCollapsable table { margin: 20px auto; width: 506px; }
        .GroupContentCollapsable .btnComando { margin: 20px auto 0px auto; display: inherit; }
        .Grid { width: 90%; margin: 25px auto 15px auto; }
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
        <!--Barra de Localización-->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">CAT&Aacute;LOGOS - CONSULTAR OPERADORES</asp:Label>
        </div>
        <!--Menú secundario-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultar" runat="server" NavigateUrl="~/Comun.UI/ConsultarOperadorUI.aspx"> CONSULTAR <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /></asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="hlkRegistrar" runat="server" NavigateUrl="~/Comun.UI/RegistrarOperadorUI.aspx"> REGISTRAR <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/></asp:HyperLink>
                </li>
            </ul>
        </div>

        <!--Filtros de la consulta-->
        <div id="Formulario" class="GroupSection">
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>¿QUÉ OPERADOR DESEA CONSULTAR?</td>
                        <td>
                            <img id="img1" class="imgMenu" src="../Contenido/Imagenes/FlechaAbajo.png" alt="Click para Ocultar/Mostrar" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="GroupContentCollapsable">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical">Cliente</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;"><asp:HiddenField ID="hdnCuentaClienteID" runat="server" />
                            <asp:TextBox ID="txtCuentaClienteNombre" runat="server" MaxLength="250" Width="275px"
                                AutoPostBack="True" OnTextChanged="txtNombreCuentaCliente_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscarCliente" CommandName="VerSucursal"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Clientes" CommandArgument=''
                                OnClick="ibtnBuscarCliente_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Nombre</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtNombre" runat="server" Width="275px" MaxLength="150"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">N&uacute;mero licencia</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtLicenciaNumero" runat="server" MaxLength="30" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Estatus</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:DropDownList runat="server" ID="ddlEstatus" Width="150px">
                                <asp:ListItem Text="TODOS" Value=""></asp:ListItem>
                                <asp:ListItem Text="ACTIVO" Value="ACTIVO"></asp:ListItem>
                                <asp:ListItem Text="INACTIVO" Value="INACTIVO"></asp:ListItem>
                            </asp:DropDownList>
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
                <asp:GridView runat="server" ID="grdOperadores" AutoGenerateColumns="False" AllowPaging="True" 
                    EnableSortingAndPagingCallbacks="True" CssClass="Grid" OnPageIndexChanging="grdOperadores_PageIndexChanging"
                    OnRowCommand="grdOperadores_RowCommand" onrowdatabound="grdOperadores_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>Cliente</HeaderTemplate>
                            <ItemTemplate><%# DataBinder.Eval(Container.DataItem,"Cliente.Nombre") %></ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="AñosExperiencia" HeaderText="Años Exp." />
                        <asp:TemplateField>
                            <HeaderTemplate>Tipo licencia</HeaderTemplate>
                            <ItemTemplate><%# DataBinder.Eval(Container.DataItem, "Licencia.Tipo")%></ItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate># Licencia</HeaderTemplate>
                            <ItemTemplate><%# DataBinder.Eval(Container.DataItem, "Licencia.Numero")%></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>Fecha expiración</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblFechaExpiracion" Text='<%# DataBinder.Eval(Container.DataItem,"Licencia.FechaExpiracion") %>' Width="100%"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="90px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>Estado Expedición</HeaderTemplate>
                            <ItemTemplate><%# DataBinder.Eval(Container.DataItem, "Licencia.Estado")%></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>Estatus</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblEstatus" Text='<%# DataBinder.Eval(Container.DataItem,"Estatus").ToString().ToUpper().Replace("TRUE","ACTIVO").Replace("FALSE","INACTIVO") %>' Width="100%"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px"/>
                            <ItemStyle HorizontalAlign="Left" Width="80px"/>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnVer" CommandName="Detalles" ImageUrl="~/Contenido/Imagenes/VER.png"
                                    ToolTip="Ver detalles" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"OperadorID") %>'
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
    <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
</asp:Content>
