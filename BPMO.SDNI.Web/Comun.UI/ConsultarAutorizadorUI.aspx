<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ConsultarAutorizadorUI.aspx.cs" Inherits="BPMO.SDNI.Comun.UI.ConsultarAutorizadorUI" %>
<%-- 
    Satisface al caso de uso CU098 - Configurar Autorizadores para los Contratos
--%>
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
<script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
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
        <!-- Barra de localizacion -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - CONFIGURAR AUTORIZADORES</asp:Label>
        </div>
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 64px;">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Comun.UI/ConsultarAutorizadorUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="hlRegistroOrden" runat="server" NavigateUrl="~/Comun.UI/RegistrarAutorizadorUI.aspx">
                        REGISTRAR
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
                            ¿QUÉ AUTORIZADOR DE CONTRATO QUIERE CONSULTAR?
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
                        <td class="tdCentradoVertical">
                            SUCURSAL
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
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
                            TIPO AUTORIZACIÓN
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:DropDownList ID="ddlTipoAutorizacion" runat="server" Width="200px"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            EMPLEADO
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtEmpleado" runat="server" MaxLength="30" Width="275px"
                                AutoPostBack="True" OnTextChanged="txtNombreEmpleado_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscarEmpleado" CommandName="VerEmpleado"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Empleados" CommandArgument=''
                                OnClick="ibtnBuscarEmpleado_Click" />
                                <asp:HiddenField ID="hdnEmpleadoID" runat="server" Visible="False" />
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
                <asp:GridView runat="server" ID="grdAutorizadores" AutoGenerateColumns="false" PageSize="10"
                    AllowPaging="true" AllowSorting="false" EnableSortingAndPagingCallbacks="true"
                    CssClass="Grid" OnPageIndexChanging="grdAutorizadores_PageIndexChanging" OnRowCommand="grdAutorizadores_RowCommand"
                    OnRowDataBound="grdAutorizadores_RowDataBound">
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
                                <HeaderTemplate>Tipo Autorización
                                    </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTipoAutorizacion" Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                           <%-- <asp:BoundField DataField="TipoAutorizacion" HeaderText="">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="110px" />
                            </asp:BoundField>--%>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Nombre Empleado</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEmpleadoNombre" Text='<%# DataBinder.Eval(Container.DataItem,"Empleado.Nombre") %>'
                                        Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Correo Electrónico</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEmpleadoEmail" Text='<%# DataBinder.Eval(Container.DataItem,"Empleado.Email") %>'
                                        Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Teléfono</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEmpleadoTelefono" Text='<%# DataBinder.Eval(Container.DataItem,"Empleado.RFC") %>'
                                        Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnVer" CommandName="Detalles" ImageUrl="~/Contenido/Imagenes/VER.png"
                                    ToolTip="Ver detalles" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"AutorizadorID") %>'
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
