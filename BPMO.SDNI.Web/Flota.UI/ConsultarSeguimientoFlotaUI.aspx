<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ConsultarSeguimientoFlotaUI.aspx.cs" Inherits="BPMO.SDNI.Flota.UI.ConsultarSeguimientoFlotaUI" %>
<%--Satisface al CU081 - Consultar Seguimiento Flota--%>
<%--Satisface la solicitud de cambio SC0006--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .GroupSection { width: 650px; margin: 0px auto; }
        .GroupContentCollapsable table { margin: 20px auto; width: 506px; }
        .GroupContentCollapsable .btnComando { margin: 20px auto 0px auto; display: inherit; }
        .Grid { width: 90%; margin: 25px auto 15px auto; }
    </style>

    <script type="text/javascript">
        initChild = function () {
            ConfiguracionBarraHerramientas();

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

            $(".GroupHeaderCollapsable").click(function () {
                $(this).next(".GroupContentCollapsable").slideToggle(500);
                if ($(this).find(".imgMenu").attr("src") == "../Contenido/Imagenes/FlechaArriba.png")
                    $(this).find(".imgMenu").attr("src", "../Contenido/Imagenes/FlechaAbajo.png");
                else
                    $(this).find(".imgMenu").attr("src", "../Contenido/Imagenes/FlechaArriba.png");
                return false;
            });
        };
        $(document).ready(initChild);
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
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">FLOTA - CONSULTAR SEGUIMIENTO DE FLOTA</asp:Label>
        </div>
        <div style="height: 80px;" id="ContenedorMenuSecundario">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li id="ConsultarFlota" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultarFlota" runat="server" NavigateUrl="~/Flota.UI/ConsultarSeguimientoFlotaUI.aspx">
						CONSULTAR UNIDAD
						<img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
                <li id="Movimiento">
                    <asp:HyperLink ID="hlkMovimiento" runat="server" NavigateUrl="~/Flota.UI/CambiarSucursalEquipoAliadoUI.aspx">
						MOVER EQ. ALIADO
						<img id="img2" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
            </ul>
        </div>
        <!-- Cuerpo -->
        <div id="Formulario" class="GroupSection">
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>¿A QU&Eacute; UNIDAD DESEA DARLE SEGUIMIENTO?</td>
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
                            <asp:TextBox ID="txtSucursal" runat="server" MaxLength="30" Width="275px" AutoPostBack="True"
                                OnTextChanged="txtSucursal_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="btnBuscarSucursal" CommandName="VerSucursal"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument=''
                                OnClick="btnBuscarSucursal_Click" />
                            <asp:HiddenField ID="hdnSucursalID" runat="server" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical"># Serie</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtNumVin" runat="server" MaxLength="30" Width="275px" AutoPostBack="True"
                                OnTextChanged="txtNumVin_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="btnBuscarVin" CommandName="VerVin" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                                ToolTip="Consultar VIN" CommandArgument='' OnClick="btnBuscarVin_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical"># Econ&oacute;mico</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtNumeroEconomico" runat="server" MaxLength="50" Width="301px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Marca</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 330px;">
                            <asp:TextBox ID="txtMarca" runat="server" Width="275px" AutoPostBack="True" ToolTip="Marca de la unidad"
                                ontextchanged="txtMarca_TextChanged"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaMarca" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" 
                                ToolTip="Consultar marca de la unidad" onclick="ibtnBuscaMarca_Click" />
                            <asp:HiddenField ID="hdnMarcaID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Modelo</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 330px; vertical-align: middle;">
                            <asp:TextBox ID="txtModelo" runat="server" Width="275px" MaxLength="80" AutoPostBack="True" ToolTip="Modelo de la unidad" 
                                OnTextChanged="txtModelo_TextChanged"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaModelo" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" 
                                ToolTip="Consultar modelo de la unidad" OnClick="ibtnBuscaModelo_Click" />
                            <asp:HiddenField ID="hdnModeloID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Tipo de Unidad</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 330px; vertical-align: middle;">
                            <asp:TextBox ID="txtTipoUnidad" runat="server" Width="275px" AutoPostBack="True" ToolTip="Tipo de unidad"
                                ontextchanged="txtTipoUnidad_TextChanged"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaTipoUnidad" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" 
                                ToolTip="Consultar tipo de unidad" onclick="ibtnBuscaTipoUnidad_Click" />
                            <asp:HiddenField ID="hdnTipoUnidadID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Propietario</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtPropietario" runat="server" MaxLength="360" Width="301px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">&Aacute;rea/Departamento</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:DropDownList ID="ddlArea" runat="server" Width="200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Estatus</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <%--SC0006 - El nuevo estatus es agregado desde el presenter--%>
                            <asp:DropDownList ID="ddlEstatus" runat="server" Width="200px" AppendDataBoundItems="true">
                                <asp:ListItem Value="" Text="Todos" Selected="true"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Fecha Alta</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtFechaAltaInicial" runat="server" MaxLength="30" Width="95px" CssClass="CampoFecha"></asp:TextBox>
                            &nbsp;a&nbsp;
                            <asp:TextBox ID="txtFechaAltaFinal" runat="server" MaxLength="30" Width="95px" CssClass="CampoFecha"></asp:TextBox>
                        </td>                    
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Fecha Baja</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtFechaBajaInicial" runat="server" MaxLength="30" Width="95px" CssClass="CampoFecha"></asp:TextBox>
                            &nbsp;a&nbsp;
                            <asp:TextBox ID="txtFechaBajaFinal" runat="server" MaxLength="30" Width="95px" CssClass="CampoFecha"></asp:TextBox>
                        </td>                    
                    </tr>
                </table>
                <asp:Button runat="server" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click" CssClass="btnComando" ToolTip="Consultar Flota" />
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </div>
        <!-- Resultados -->
        <asp:UpdatePanel ID="UPContenedor" runat="server">
            <ContentTemplate>
                <asp:GridView runat="server" ID="grdResultado" AutoGenerateColumns="false" PageSize="10" AllowPaging="true" AllowSorting="false" 
                    EnableSortingAndPagingCallbacks="true" CssClass="Grid" OnPageIndexChanging="grdResultado_PageIndexChanging"
                    OnRowCommand="grdResultado_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="NombreSucursal" HeaderText="Sucursal">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NumeroSerie" HeaderText="# Serie">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NumeroEconomico" HeaderText="# Econ&oacute;mico">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AreaText" HeaderText="&Aacute;rea / Depto.">
                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FechaAlta" HeaderText="Fecha Alta" DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle HorizontalAlign="Left" Width="90px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FechaBaja" HeaderText="Fecha Baja" DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle HorizontalAlign="Left" Width="90px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="EstatusText" HeaderText="Estatus">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="btnVer" CommandName="Detalles" ImageUrl="~/Contenido/Imagenes/VER.png"
                                        ToolTip="Ver expediente" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"Unidad.UnidadID") %>'
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
