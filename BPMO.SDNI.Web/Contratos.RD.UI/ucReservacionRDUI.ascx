<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucReservacionRDUI.ascx.cs" Inherits="BPMO.SDNI.Contratos.RD.UI.ucReservacionRDUI" %>
<link href="../Contenido/Estilos/Tema.JqueryUI/jquery.ui.timepicker.css" rel="stylesheet" type="text/css" />
<script src="../Contenido/Scripts/jquery.ui.timepicker.js" type="text/javascript"></script>
<script src="../Contenido/Scripts/jquery.ui.timepicker-es.js" type="text/javascript"></script>
<style type="text/css">
    .ui-timepicker-buttonpane { display: none; }
</style>
<script type="text/javascript">
    // Inicializa el control de Información General
    function <%= ClientID %>_Inicializar() {
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
        
        /*Configuración de los campos de hora*/
        $('.CampoHora').each(function () {
            if ($(this).length > 0) {
                $(this).timepicker({
                    showPeriod: true,
                    showLeadingZero: true,
                    showCloseButton: true
                });
            }
            $(this).attr('readonly', true);
            $('.ui-timepicker-close').click();
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
<fieldset>
    <legend>Información General</legend>
    <table class="trAlinearDerecha" style="width: 530px; margin: 0px auto;">
        <asp:Panel ID="pnlNumero" runat="server">
            <tr>
                <td class="tdCentradoVertical" style="width: 150px;"><span>*</span># Reservaci&oacute;n</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" colspan="4" style="width:388px;">
                    <asp:TextBox ID="txtNumero" runat="server" MaxLength="30" Width="275px" Enabled="false"></asp:TextBox>
                </td>
            </tr>
        </asp:Panel>
        <tr>
            <td class="tdCentradoVertical" style="width: 150px;"><span>*</span>Sucursal</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" colspan="4">
                <asp:TextBox ID="txtSucursal" runat="server" Width="250px" AutoPostBack="true" MaxLength="100" ToolTip="Sucursal" OnTextChanged="txtSucursal_TextChanged"></asp:TextBox>
                <asp:ImageButton runat="server" ID="ibtnBuscarSucursal" CommandName="VerSucursal" ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar sucursales" CommandArgument='' OnClick="ibtnBuscaSucursal_Click" />
                <asp:HiddenField ID="hdnSucursalID" runat="server" Visible="False" />
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical" style="width: 150px;"><span>*</span>Cliente</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" colspan="4">
                <asp:TextBox ID="txtNombreCuentaCliente" runat="server" MaxLength="150" Width="310px"
                    AutoPostBack="True" OnTextChanged="txtNombreCuentaCliente_TextChanged"></asp:TextBox>
                <asp:ImageButton runat="server" ID="ibtnBuscarCliente" CommandName="VerCliente"
                    ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Clientes" CommandArgument=''
                    OnClick="ibtnBuscarCliente_Click" />
                <asp:HiddenField runat="server" ID="hdnCuentaClienteID"/>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical" style="width: 150px;"><span>*</span>Fecha Inicial</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 125px;">
                <asp:TextBox ID="txtFechaInicio" runat="server" MaxLength="11" CssClass="CampoFecha" Width="90px"
                    AutoPostBack="True" OnTextChanged="txtFecha_TextChanged"></asp:TextBox>
            </td>
            <td class="tdCentradoVertical" style="width: 110px;" align="right"><span>*</span>Hora Inicial</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtHoraInicio" runat="server" MaxLength="7" CssClass="CampoHora" Width="90px"
                     AutoPostBack="True" OnTextChanged="txtFecha_TextChanged"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical" style="width: 150px;"><span>*</span>Fecha Final</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 125px;">
                <asp:TextBox ID="txtFechaFinal" runat="server" MaxLength="11" CssClass="CampoFecha" Width="90px"
                     AutoPostBack="True" OnTextChanged="txtFecha_TextChanged"></asp:TextBox>
            </td>
            <td class="tdCentradoVertical" style="width: 110px;" align="right"><span>*</span>Hora Final</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtHoraFinal" runat="server" MaxLength="7" CssClass="CampoHora" Width="90px"
                     AutoPostBack="True" OnTextChanged="txtFecha_TextChanged"></asp:TextBox>
            </td>
        </tr>
        <asp:Panel ID="pnlUsuarioReservo" runat="server">
            <tr>
                <td class="tdCentradoVertical" style="width: 150px;"><span>*</span>Usuario Reserv&oacute;</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" colspan="4">
                    <asp:TextBox ID="txtUsuarioReservoNombre" runat="server" MaxLength="150" Width="275px" Enabled="false"></asp:TextBox>
                    <asp:HiddenField runat="server" ID="hdnUsuarioReservoID"/>
                </td>
            </tr>
        </asp:Panel>
        <tr>
            <td align="right" style="width: 150px; padding-top: 5px;"><span>*</span>Observaciones</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" colspan="4">
                <asp:TextBox runat="server" ID="txtObservaciones" TextMode="MultiLine" Width="310px" Height="90px"
                    Style="max-width: 310px; min-width: 310px; max-height: 90px; min-height: 90px;">
                </asp:TextBox>
            </td>
        </tr>
        <asp:Panel ID="pnlActivo" runat="server">
            <tr>
                <td class="tdCentradoVertical" style="width: 150px;"><span>*</span>Estatus</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" colspan="4">
                    <asp:TextBox ID="txtActivo" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                    <asp:HiddenField runat="server" ID="hdnActivo"/>
                </td>
            </tr>
        </asp:Panel>
    </table>
</fieldset>
<fieldset>
    <legend>Información de la Unidad</legend>
    <table class="trAlinearDerecha" style="margin: 0px auto;">
        <tr>
            <td class="tdCentradoVertical" style="width: 150px;"><span>*</span>Modelo</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" colspan="6">
                <asp:TextBox ID="txtNombreModelo" runat="server" MaxLength="100" Width="275px" AutoPostBack="True" 
                    OnTextChanged="txtNombreModelo_TextChanged"></asp:TextBox>
                <asp:ImageButton runat="server" ID="ibtnBuscarModelo" CommandName="VerModelo"
                    ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Modelos" CommandArgument=''
                    OnClick="ibtnBuscarModelo_Click" />
                <asp:HiddenField runat="server" ID="hdnModeloID"/>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical" style="width: 150px;"># Econ&oacute;mico</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" colspan="6">
                <asp:TextBox ID="txtNumeroEconomico" runat="server" MaxLength="50" Width="275px" AutoPostBack="True" 
                    OnTextChanged="txtNumeroEconomico_TextChanged"></asp:TextBox>
                <asp:ImageButton runat="server" ID="ibtnBuscarUnidad" CommandName="VerUnidad"
                    ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Unidades" CommandArgument=''
                    OnClick="ibtnBuscarUnidad_Click" />
                <asp:HiddenField runat="server" ID="hdnUnidadID"/>
            </td>
        </tr>
        <asp:Panel ID="pnlDetalleUnidad" runat="server">
            <tr>
                <td class="tdCentradoVertical" style="width: 150px;">VIN</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" colspan="4">
                    <asp:TextBox ID="txtUnidadSerie" runat="server" Width="275px" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical" style="width: 150px;">Marca</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" colspan="2">
                    <asp:TextBox ID="txtUnidadMarcaNombre" runat="server" Width="275px" Enabled="false"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" colspan="2" align="right">Año</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtUnidadAnio" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical" style="width: 150px;">Placa Estatal</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtUnidadPlacaEstatal" runat="server" Width="115px" Enabled="false">xxxxxxxxxx</asp:TextBox>
                </td>
                <td class="tdCentradoVertical" align="right" style="width: 175px;">Placa Federal</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" colspan="3">
                    <asp:TextBox ID="txtUnidadPlacaFederal" runat="server" Width="115px" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical" style="width: 150px;" title="PBC">Capacidad Carga</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" colspan="6">
                    <asp:TextBox ID="txtUnidadCapacidadCarga" runat="server" Width="115px" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical" style="width: 150px;">Capacidad Tanque</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtUnidadCapacidadTanque" runat="server" Width="115px" Enabled="false"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" align="right" style="width: 175px;">Rendimiento Tanque</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" colspan="3">
                    <asp:TextBox ID="txtUnidadRendimientoTanque" runat="server" Width="115px" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical" style="width: 150px;">Estatus Operación</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtUnidadEstatusOperacion" runat="server" Width="115px" Enabled="false"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" align="right" style="width: 175px;">Estatus Mantenimiento</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" colspan="3">
                    <asp:TextBox ID="txtUnidadEstatusMantenimiento" runat="server" Width="115px" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical" style="width: 150px;">Fecha Planeada Liberación</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" colspan="6">
                    <asp:TextBox ID="txtUnidadFechaPlaneadaLiberacion" runat="server" Enabled="false" Width="200px"></asp:TextBox>
                </td>
            </tr>
        </asp:Panel>
    </table>
    <asp:Panel ID="pnlDetalleUnidadEquiposAliados" runat="server">
        <asp:GridView ID="grvUnidadEquiposAliados" runat="server" AutoGenerateColumns="false" CssClass="Grid"
            Style="width: 90%; margin: 10px auto;"
            AllowPaging="false" AllowSorting="false" EnableSortingAndPagingCallbacks="false">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate># Serie</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSerie" Text='<%# DataBinder.Eval(Container.DataItem,"NumeroSerie") %>' Width="100%">
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Año</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblAnio" Text='<%# DataBinder.Eval(Container.DataItem,"Anio") %>' Width="100%">
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Dimensiones</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblAnio" Text='<%# DataBinder.Eval(Container.DataItem,"Dimension") %>' Width="100%">
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>PBV</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPBV" Text='<%# DataBinder.Eval(Container.DataItem,"PBV") %>' Width="100%">
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>PBC</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPBC" Text='<%# DataBinder.Eval(Container.DataItem,"PBC") %>' Width="100%">
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Modelo</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblModeloNombre" Text='<%# DataBinder.Eval(Container.DataItem,"Modelo.Nombre") %>' Width="100%">
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
            </Columns>
            <RowStyle CssClass="GridRow" />
            <HeaderStyle CssClass="GridHeader" />
            <FooterStyle CssClass="GridFooter" />
            <PagerStyle CssClass="GridPager" />
            <SelectedRowStyle CssClass="GridSelectedRow" />
            <AlternatingRowStyle CssClass="GridAlternatingRow" />
        </asp:GridView>
    </asp:Panel>
</fieldset>
<asp:HiddenField runat="server" ID="hdnReservacionID"/>
<asp:HiddenField runat="server" ID="hdnUC"/>
<asp:HiddenField runat="server" ID="hdnFC"/>
<asp:HiddenField runat="server" ID="hdnUUA"/>
<asp:HiddenField runat="server" ID="hdnFUA"/>
<asp:HiddenField runat="server" ID="hdnUOID"/>
<asp:HiddenField runat="server" ID="hdnTipoID"/>
<asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
