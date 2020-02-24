<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ConsultarConfiguracionDescuentoPSLUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.ConsultarConfiguracionDescuentoPSLUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .GroupSection { width: 650px; margin: 0px auto; }
        .GroupContentCollapsable table { margin: 20px auto; width: 506px; }
        .GroupContentCollapsable .btnComando { margin: 20px auto 0px auto; display: inherit; }
        .Grid { width: 90%; margin: 25px auto 15px auto; }
    </style>
    <script type="text/javascript" src="../Contenido/Scripts/ObtenerFormatoImporte.js"></script>
    <script type="text/javascript">
        $(document).ready(function () { initChild(); });

        function initChild() {
            initPage(); inicializeHorizontalPanels();
        }

        function initPage() {

            var dateFormat = "dd/mm/yy",
            from = $(".CampoFechaInicio").datepicker({
                                                        yearRange: '-100:+10',
                                                        changeYear: true,
                                                        changeMonth: true,
                                                        buttonImage: '../Contenido/Imagenes/calendar.gif',
                                                        buttonImageOnly: true,
                                                        toolTipText: "Fecha de inicio de descuento",
                                                        showOn: 'button'
                                                     })

            to = $(".CampoFechaFinal").datepicker({
                                                        yearRange: '-100:+10',
                                                    changeYear: true,
                                                    changeMonth: true,
                                                    buttonImage: '../Contenido/Imagenes/calendar.gif',
                                                    buttonImageOnly: true,
                                                    toolTipText: "Fecha final de descuento",
                                                    showOn: 'button'
                                                   }).on("change", function () {
                                                                                  from.datepicker("option", "maxDate", getDate(this));
                                                                               });

                function getDate(element) {
                        var date;
                    try {
                        date = $.datepicker.parseDate(dateFormat, element.value);
                        } 
                    catch (error) {
                        date = null;
                        }
                    
                        return date;
                }

                

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
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">DESCUENTOS - CONSULTAR DESCUENTOS</asp:Label>
        </div>

     
        <div style="height: 80px;" id="ContenedorMenuSecundario">
            <!-- Menú secundario -->
            
            
            <ul id="MenuSecundario" style="float: left; height: 64px;">
                <li id="ConsultarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultarDescuentosConfiguracion" runat="server" NavigateUrl="~/Contratos.PSL.UI/ConsultarConfiguracionDescuentoPSLUI.aspx" ForeColor="White"> 
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo">
                    <asp:HyperLink ID="hlkConsultarConfiguracionDescuentos" runat="server" NavigateUrl="~/Contratos.PSL.UI/RegistrarConfiguracionDescuentoPSLUI.aspx"> 
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
                        <td>¿QU&Eacute; DESCUENTO DESEA CONSULTAR?</td>
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
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtCliente" runat="server" MaxLength="30" Width="275px" AutoPostBack="True"
                                OnTextChanged="txtCliente_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="btnBuscarClientes" CommandName="VerClientes"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Clientes" CommandArgument=''
                                OnClick="btnBuscarClientes_Click" />
                            <asp:HiddenField ID="hdnClienteID" runat="server" />
                        </td>
                    </tr>


                    <tr>
                        <td class="tdCentradoVertical">Sucursal</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtSucursal" runat="server" MaxLength="30" Width="275px" AutoPostBack="True"
                                OnTextChanged="txtSucursal_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="btnBuscarSucursal" CommandName="VerSucursal"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument=''
                                OnClick="btnBuscarSucursal_Click" />
                            <asp:HiddenField ID="hdnSucursalID" runat="server" Visible="False" />
                        </td>
                    </tr>


                    <tr>
                        <td class="tdCentradoVertical">Fecha Inicial</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox name="FechaInicio" ID="txtFechaInicial" runat="server" MaxLength="30" Width="95px" CssClass="CampoFechaInicio"></asp:TextBox>
                        </td>
                    </tr>

                       <tr>
                        <td class="tdCentradoVertical">Fecha final</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtFechaFinal" runat="server" MaxLength="30" Width="95px" CssClass="CampoFechaFinal"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td class="tdCentradoVertical">Estatus</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:DropDownList ID="ddlEstatus" runat="server" Width="200px">
                                <asp:ListItem Value="0" Text="Todos" Selected="true"></asp:ListItem>
                                <asp:ListItem Value="1" Text="ACTIVO"></asp:ListItem>
                                <asp:ListItem Value="2" Text="INACTIVO"></asp:ListItem>
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
                <asp:GridView runat="server" ID="grvConfiguracionDescuentos" AutoGenerateColumns="false" PageSize="10" AllowPaging="true" AllowSorting="false" 
                    EnableSortingAndPagingCallbacks="true" CssClass="Grid" OnPageIndexChanging="grvConfiguracionDescuentos_PageIndexChanging"
                    OnRowCommand="grvConfiguracionDescuentoso_RowCommand" OnRowDataBound="grvConfiguracionDescuentos_RowDataBound">
                    <Columns>

                        <asp:TemplateField>
                            <HeaderTemplate>Cliente</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCliente" Text='<%# DataBinder.Eval(Container.DataItem,"Cliente.Nombre") %>' Width="100%"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderTemplate>Sucursal</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblSucursal" Text='<%# DataBinder.Eval(Container.DataItem,"Sucursal.Nombre") %>' Width="100%"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderTemplate>Activo</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Checkbox runat="server" ID="ChkActivo" disabled  Width="100%"></asp:Checkbox>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnVer" CommandName="Detalles" ImageUrl="~/Contenido/Imagenes/VER.png"
                                    ToolTip="Ver detalles" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
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
    <asp:HiddenField ID="hdnLibroActivos" runat="server" />

</asp:Content>
