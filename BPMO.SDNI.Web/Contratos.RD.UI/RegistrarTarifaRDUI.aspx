<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="RegistrarTarifaRDUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.RD.UI.RegistrarTarifaRDUI" %>

<%@ Register Src="~/Contratos.RD.UI/ucTarifaRDUI.ascx" TagPrefix="uc" TagName="ucTarifaRDUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloCatTarifas.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" id="JQuerySection">
        $(document).ready(function () {
            initChild();
            ConfiguracionBarraHerramientas();
        });
    </script>
    <script language="javascript" type="text/javascript" id="JavaScriptFunctions">
        initChild = function () {
            ConfiguracionBarraHerramientas();
            
            //Formato de Fechas
            if (!$(".CampoFecha").is(':disabled')) {
                $('.CampoFecha').datepicker({
                    minDate: -0,
                    yearRange: '-100:+10',
                    changeYear: true,
                    changeMonth: true,
                    dateFormat: "dd/mm/yy",
                    buttonImage: '../Contenido/Imagenes/calendar.gif',
                    buttonImageOnly: true,
                    toolTipText: "Fecha de compra",
                    showOn: 'button'
                });
            }

            $('.CampoFecha').attr('readonly', true);

            ConfiguracionBarraHerramientas();
        };

        function BtnBuscar(guid, xml) {
            var width = ObtenerAnchoBuscador(xml);

            $.BuscadorWeb({
                xml: xml,
                guid: guid,
                btnSender: $("#<%=btnResult.ClientID %>"),
                features: {
                    dialogWidth: width,
                    dialogHeight: '320px'
                }
            });
        }

        function mostrarLeyenda() {
            $("#dvLeyendaSucursales").dialog({
                autoOpen: true,
                modal: true,
                resizable: false,
                close: function () { $(this).dialog("destroy"); }
            });
            $("#dvLeyendaSucursales").parent().appendTo("form:first");
        }
function btnLeyendaSucursales_onclick() {

}

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!--Barra de Localización-->
        <div id="BarraUbicacion">
            <asp:Label runat="server" ID="lblEncabezadoLeyenda">CAT&Aacute;LOGOS - REGISTRAR TARIFA</asp:Label>
        </div>
        <!--Menú secundario-->
        <div style="height: 65px;">
            <ul id="MenuSecundario">
                <li>
                    <asp:HyperLink runat="server" ID="hlConsultar" NavigateUrl="~/Contratos.RD.UI/ConsultarTarifasRDUI.aspx">
                        CONSULTAR 
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink runat="server" ID="hlRegistrar" NavigateUrl="RegistrarTarifaRDUI.aspx">
                        REGISTRAR 
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de Herramientas -->
            <div id="BarraHerramientas">
                <asp:Menu runat="server" ID="mTarifa" IncludeStyleBlock="False" Orientation="Horizontal"
                    CssClass="MenuPrimario">
                    <Items>
                        <asp:MenuItem Text="Precio Combustible" Value="PrecioCombustible"
                            Selectable="false"></asp:MenuItem>
                    </Items>
                    <StaticItemTemplate>
                        <asp:Label runat="server" ID="lblOpcion" CssClass='<%# (string) Eval("Value") == "PrecioCombustible" ? "Informacion" : string.Empty %>'
                            Text='<%# Eval("Text") %>'></asp:Label>
                        <asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "PrecioCombustible" %>'
                            Style="width: 150px" CssClass="textBoxDisabled" ReadOnly="true"></asp:TextBox>
                    </StaticItemTemplate>
                    <LevelSubMenuStyles>
                        <asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" />
                    </LevelSubMenuStyles>
                </asp:Menu>
                <div class="Ayuda" style="float: right">
                    <input id="btnAyuda" type="button" onclick="ShowHelp();" class="btnAyuda" />
                </div>
            </div>
        </div>
        <div id="divInformacionGeneral" class="GroupBody">
            <div id="divInformacionHeader" class="GroupHeader">
                <span>TARIFA</span>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button runat="server" ID="btnGuardar" Text="Guardar" OnClick="btnGuardar_Click"
                        CssClass="btnWizardGuardar" />
                    <asp:Button runat="server" ID="btnCancelar" Text="Cancelar" OnClick="btnCancelar_Click"
                        CssClass="btnWizardCancelar" />
                </div>
            </div>
            <fieldset id="fsInformacionGeneral">
                <legend>Datos Generales de la tarifa</legend>
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical" style="width: 150px;">
                            <span>*</span>Sucursal
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox runat="server" ID="txtSucursal" AutoPostBack="True" OnTextChanged="txtSucursal_TextChanged"
                                Width="250px"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscarSucursal" ImageUrl="../Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" OnClick="ibtnBuscarSucursal_Click" />
                            <input type="button" runat="server" id="btnLeyendaSucursales"
                                title="La sucursal seleccionada es la MATRIZ de la unidad operativa y, por lo tanto, las tarifas aplicarán para todas las demás sucursales en las que tenga permiso" 
                                class="ui-state-error ui-corner-all ui-icon ui-icon-notice btnCalendar" style="width: 19px; height: 19px; cursor: hand;"
                                onclick="mostrarLeyenda();"/>    
                        </td>
                        <td class="tdCentradoVertical" style="text-align: right;">
                            <span>*</span>Moneda
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 280px;">
                            <asp:DropDownList runat="server" ID="ddlMoneda" Width="200px">
                                <asp:ListItem Text="Seleccione una opción" Value="-1"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical" style="width: 150px;">
                            <span>*</span>Modelo
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox runat="server" ID="txtModelo" AutoPostBack="True" OnTextChanged="txtModelo_TextChanged"
                                Width="250px"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscarModelo" ImageUrl="../Contenido/Imagenes/Detalle.png" ToolTip="Consultar modelo de la unidad"
                                OnClick="ibtnBuscarModelo_Click" />
                        </td>
                        <td class="tdCentradoVertical" style="text-align: right;">
                            <span>*</span>Tipo
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlTipoTarifa" Width="200px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlTipoTarifa_SelectedIndexChanged">
                                <asp:ListItem Text="Seleccione una opción" Value="-1"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            <span>*</span>Descripci&oacute;n
                        </td>
                        <td style="width: 20px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical" colspan="4">
                            <asp:TextBox runat="server" ID="txtDescripcion" MaxLength="150" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <asp:UpdatePanel runat="server" ID="udpClientes">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="pnlDatosCliente">
                                <tr>
                                    <td class="tdCentradoVertical" style="width: 150px;">
                                        <span>*</span>Cliente
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical" style="width: 320px;">
                                        <asp:TextBox runat="server" ID="txtCliente" Width="250px" AutoPostBack="True" OnTextChanged="txtCliente_TextChanged"></asp:TextBox>
                                        <asp:ImageButton runat="server" ID="ibtnBuscarCliente" ImageUrl="../Contenido/Imagenes/Detalle.png"
                                            OnClick="ibtnBuscarCliente_Click" />
                                    </td>
                                    <asp:Panel runat="server" ID="pnlVigencia" Visible="False">
                                        <td class="tdCentradoVertical" style="text-align: right;">
                                            <span>*</span>Vigencia
                                        </td>
                                        <td style="width: 20px;">&nbsp;</td>
                                        <td class="tdCentradoVertical">
                                            <asp:TextBox runat="server" ID="txtVigencia" CssClass="CampoFecha"></asp:TextBox>
                                        </td>
                                    </asp:Panel>
                                </tr>
                                <asp:Panel runat="server" ID="pnlObservaciones" Visible="false">
                                    <tr>
                                        <td style="padding-top: 5px">
                                            <span>*</span>Observaciones
                                        </td>
                                        <td style="width: 20px;">&nbsp;</td>
                                        <td class="tdCentradoVertical" colspan="4">
                                            <asp:TextBox runat="server" ID="txtObservaciones" Width="250px" TextMode="MultiLine" 
                                                style="max-width: 250px; min-width: 250px; max-height: 90px; min-height: 90px;"></asp:TextBox>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTipoTarifa" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </table>
                <div style="text-align: center;">
                    <asp:Button ID="btnCapturarTarifas" runat="server" CssClass="btnComando" Text="Configurar"
                        OnClick="btnCapturarTarifas_Click" />
                </div>
            </fieldset>
            <asp:Panel runat="server" ID="pnlCapturaTarifas">
                <fieldset id="fsTarifas">
                    <legend>Configuracion de tarifas</legend>
                    <uc:ucTarifaRDUI runat="server" ID="ucTarifaRD" />
                </fieldset>
                <asp:Panel runat="server" ID="pnlSeccionSucursal">
                    <fieldset id="fsSucursalesNoAplica">
                        <legend>Sucursales en las que no aplicará la tarifa</legend>
                        <div class="Nota">
                            Nota: Esta sección tiene COMO FINALIDAD quitar las sucursales a las cuales no
                            quiere aplicar la tarifa que se esta configurando.
                        </div>
                        <div class="dvOpciones" style="display: none;">
                            <asp:CheckBox ID="chkAplicarSucursales" runat="server" Text="No aplicar a otras sucursales"
                                AutoPostBack="True" OnCheckedChanged="chkAplicarSucursales_CheckedChanged" />
                        </div>
                        <asp:Panel runat="server" ID="pnlAplicarSucursales">
                            <span>&nbsp;&nbsp;&nbsp;</span> <span>*</span><span>Sucursal</span> <span>&nbsp;&nbsp;</span>
                            <asp:TextBox runat="server" ID="txtSucursalNoAplica" AutoPostBack="True" OnTextChanged="txtSucursalNoAplica_TextChanged"
                                Width="200px"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscarSucursalNoAplica" ViewStateMode="Disabled" ToolTip="Consultar Sucursales"
                                ImageUrl="../Contenido/Imagenes/Detalle.png" OnClick="ibtnBuscarSucursalNoAplica_Click" />
                            <br />
                            <div class="dvOpciones">
                                <asp:Button ID="btnAgregar" runat="server" CssClass="btnAgregarATabla" Text="Agregar"
                                    OnClick="btnAgregar_Click" />
                            </div>
                            <asp:GridView runat="server" ID="grdSucursales" AutoGenerateColumns="False" PageSize="10"
                                AllowPaging="True" AllowSorting="False" EnableSortingAndPagingCallbacks="True"
                                CssClass="Grid" OnPageIndexChanging="grdSucursales_PageIndexChanging" OnRowCommand="grdSucursales_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="Nombre" HeaderText="Sucursal" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="btnEliminar" CommandName="ELIMINAR" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png"
                                                ToolTip="Eliminar" CommandArgument='<%# ((GridViewRow) Container).DataItemIndex %>'
                                                ImageAlign="Middle" />
                                        </ItemTemplate>
                                        <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <EditRowStyle CssClass="GridAlternatingRow" />
                                <PagerStyle CssClass="GridPager" />
                                <RowStyle CssClass="GridRow" />
                                <FooterStyle CssClass="GridFooter" />
                                <SelectedRowStyle CssClass="GridSelectedRow" />
                                <AlternatingRowStyle CssClass="GridAlternatingRow" />
                            </asp:GridView>
                        </asp:Panel>
                    </fieldset>
                </asp:Panel>
            </asp:Panel>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnClienteID" Value="" />
    <asp:HiddenField runat="server" ID="hdnModeloID" Value="" />
    <asp:HiddenField runat="server" ID="hdnSucursalID" Value="" />
    <asp:HiddenField runat="server" ID="hdnSucursalNoAplicaID" Value="" />
    <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
    <div id="dvLeyendaSucursales" title="Sucursal Matriz Seleccionada" style="display: none;">Aplicará a las siguientes sucursales:
        <asp:Label ID="lblLeyendaSucursales" runat="server" Text="" style="font-weight: bold;"></asp:Label> menos las excepciones que especifique
    </div>
</asp:Content>
