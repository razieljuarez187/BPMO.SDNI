<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="ConsultarListadoVerificacionUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.RD.UI.ConsultarListadoVerificacionUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloContratoFSL.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .GroupSection
        {
            width: 650px;
            margin: 0px auto;
        }
        .GroupContentCollapsable table
        {
            margin: 20px auto;
            width: 506px;
        }
        .GroupContentCollapsable .btnComando
        {
            margin: 20px auto 0px auto;
            display: inherit;
        }
        .Grid
        {
            width: 90%;
            margin: 25px auto 15px auto;
        }
    </style>
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>"
        type="text/javascript"></script>
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
		<!-- Barra de localización -->
		<div id="BarraUbicacion">
			<asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - CONSULTAR CHECK LIST UNIDAD</asp:Label>
		</div>

        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 31px;">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Contratos.RD.UI/ConsultarListadoVerificacionUI.aspx"> CONSULTAR <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /></asp:HyperLink>
                </li>
            </ul>
        </div>
        <!-- Cuerpo -->
        <div id="Formulario" class="GroupSection">
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>
                            ¿QU&Eacute; CHECK LIST DESEA REALIZAR?
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
                            Sucursal
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtSucursal" runat="server" Width="250px" AutoPostBack="true" MaxLength="50"
                                ToolTip="Sucursal" OnTextChanged="txtSucursal_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscarSucursal" CommandName="VerSucursal"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar sucursales" CommandArgument=''
                                OnClick="ibtnBuscaSucursal_Click" />
                            <asp:HiddenField ID="hdnSucursalID" runat="server" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            # Econ&oacute;mico
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtNumeroEconomico" runat="server" Width="250px" MaxLength="50"
                                ToolTip="N&uacute;mero econ&oacute;mico de la unidad"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            # DE Serie
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtNumeroSerie" runat="server" Width="250px" MaxLength="80" ToolTip="N&uacute;mero de serie de la unidad" AutoPostBack="true"
                                OnTextChanged="txtNumeroSerie_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="btnBuscarVin" CommandName="VerVin" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                                ToolTip="Consultar n&uacute;mero de serie de la unidad" CommandArgument='' OnClick="btnBuscarVin_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            # DE PLACA
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 330px; vertical-align: middle;">
                            <asp:TextBox ID="txtNumeroPlaca" runat="server" Width="115px" MaxLength="30" ToolTip="N&uacute;mero de placa de la unidad"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Modelo
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 330px; vertical-align: middle;">
                            <asp:TextBox ID="txtModelo" runat="server" Width="250px" MaxLength="80" AutoPostBack="True"
                                ToolTip="Modelo de la unidad" OnTextChanged="txtModelo_TextChanged"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaModelo" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                                ToolTip="Consultar modelo de la unidad" OnClick="ibtnBuscaModelo_Click" />
                            <asp:HiddenField ID="hdnModeloID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            N&Uacute;MERO DE CONTRATO
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtNumeroContrato" runat="server" Width="250px" MaxLength="20" ToolTip="Número de Contrato"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Cliente
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 330px; vertical-align: middle;">
                            <asp:HiddenField runat="server" ID="hdnClienteID" />
                            <asp:HiddenField runat="server" ID="hdnCuentaClienteID" />
                            <asp:TextBox ID="txtNombreCliente" runat="server" Width="250px" MaxLength="150" AutoPostBack="True" ToolTip="Nombre del Cliente"
                                OnTextChanged="txtNombreCliente_TextChanged"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscarCliente" runat="server" ImageUrl="../Contenido/Imagenes/Detalle.png"
                                ToolTip="Consultar cliente" OnClick="ibtnBuscarCliente_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            TIPO
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 330px; vertical-align: middle;">
                            <asp:DropDownList runat="server" ID="ddlTipolistado" ToolTip="Tipo de Check List" />
                        </td>
                    </tr>
                </table>
                <asp:Button runat="server" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click"
                    CssClass="btnComando" ToolTip="Consultar Check List" />
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </div>
            <asp:UpdatePanel ID="UPContenedor" runat="server">
            <ContentTemplate>
                <div id="resultados">
                    <asp:HiddenField runat="server" ID="hfUrlWindow"/>
                    <asp:GridView ID="grdListadosVerificacion" runat="server" AutoGenerateColumns="False"
                        AllowPaging="True" PageSize="10" AllowSorting="false" EnableSortingAndPagingCallbacks="True"
                        CssClass="Grid" 
                        onpageindexchanging="grdListadosVerificacion_PageIndexChanging" 
                        onrowcommand="grdListadosVerificacion_RowCommand" 
                        onrowdatabound="grdListadosVerificacion_RowDataBound" >
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    TIPO CHECK LIST</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTipoCheckList" Text='<%# DataBinder.Eval(Container.DataItem,"TipoListadoText") %>'
                                        Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="85px" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    # ECON&Oacute;MICO</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblNumeroEconomico" Text='<%# DataBinder.Eval(Container.DataItem,"Unidad.NumeroEconomico") %>'
                                        Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate># SERIE</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblNumeroSerie" Text='<%# DataBinder.Eval(Container.DataItem,"Unidad.NumeroSerie") %>'
                                        Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    MODELO</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblModelo" Text='<%# DataBinder.Eval(Container.DataItem,"Unidad.Modelo.Nombre") %>'
                                        Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    PLACA FEDERAL</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPlacaFederal" Text='' Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    PLACA ESTATAL</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPlacaEstatal" Text='' Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    # CONTRATO</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblNumeroContrato" Text='<%# DataBinder.Eval(Container.DataItem,"NumeroContrato") %>'
                                        Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    CLIENTE</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMarca" Text='<%# DataBinder.Eval(Container.DataItem,"CuentaCliente.Nombre") %>'
                                        Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
							<asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="ibtnVer" CommandName="Detalles" ImageUrl="~/Contenido/Imagenes/unidad.png"
                                        ToolTip="Ver detalle Unidad" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"Unidad.UnidadID") %>'
                                        ImageAlign="Middle" />
                                </ItemTemplate>
                                <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField> 
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="ibtnRegistrarCheck" CommandName="Registrar" ImageUrl="~/Contenido/Imagenes/GUARDAR-ICO.png"
										ToolTip="Realizar Check List" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"ContratoID") %>'
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
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click"
        Style="display: none;" />
</asp:Content>
