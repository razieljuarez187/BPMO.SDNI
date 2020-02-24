<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDatosRentaUI.ascx.cs"
    Inherits="BPMO.SDNI.Contratos.FSL.UI.ucDatosRentaUI" %>
<%@ Import Namespace="BPMO.SDNI.Contratos.FSL.BO" %>
<%-- 
    Satisface al Caso de uso CU022 - Consultar Contrato Full Service Leasing
    Satisface al Caso de uso CU023 - Editar Contratos Full Service Leasing
--%>
<script type="text/javascript">
    function <%= ClientID %>_Inicializar() {
        var ibtnEditarMeses = $('#<%= ibtnEditarMeses.ClientID %>');
        
        if (ibtnEditarMeses.length > 0) {
            ibtnEditarMeses.click(<%= ClientID %>_ValidaCambioPlazo);
        }

        var txtMeses = $('#<%= txtMeses.ClientID %>');
        if (txtMeses.length > 0) {
            if (txtMeses.val().length > 0)
                txtMeses.attr('readonly', true).addClass('textBoxDisabled');
        }
        
        $('#<%= txtMeses.ClientID %>').keyup(function() {
            var meses = $(this).val();
            var txtPlazo = $('#<%= txtPlazo.ClientID %>');
            if (meses != '') {
                if (!isNaN(meses)) {
                    var plazo = meses / 12;
                    var plazoFxd = Math.round(plazo);

                    if (plazoFxd < plazo) plazo = Math.round(plazoFxd + 0.5);
                    if (meses <= 12) plazo = 1;
                    if (meses == 0) plazo = 0;

                    txtPlazo.val(plazo.toFixed(0));
                } else 
                    txtPlazo.val('');
                
            } else 
                txtPlazo.val('');
        });
    }

    function <%= ClientID %>_Buscar(guid, xml) {
        var width = ObtenerAnchoBuscador(xml);

        $.BuscadorWeb({
            xml: xml,
            guid: guid,
            btnSender: $("#<%=btnResult.ClientID %>"),
            features: {
                dialogWidth: width,
                dialogHeight: '280px'
            }
        });
    }
    
    // Solicita la confirmacion del usuario para modificar el plazo del contrato
    function <%= ClientID %>_ValidaCambioPlazo() {
            var $div = $('<div title="Confirmación"></div>');
            $div.append('Si edita el plazo del contrato se eliminaran las unidades agregadas.<br />¿Desea Continuar?');
            $("#dialog:ui-dialog").dialog("destroy");
            $($div).dialog({
                autoOpen: true,
                closeOnEscape: true,
                modal: true,
                minWidth: 460,
                minHeight: 250,
                buttons: {
                    Aceptar: function () {
                        $(this).dialog("close");
                        $("#<%=txtMeses.ClientID %>").attr('readonly', false).removeClass('textBoxDisabled');
                    },
                    Cancelar: function () {
                        $(this).dialog("close");
                        $("#<%=txtMeses.ClientID %>").attr('readonly', true).addClass('textBoxDisabled');
                    }
                }
            });
        return false;
    }
</script>
<div id="divDatosRenta" class="GroupBody">
    <div id="divDatosRentaHeader" class="GroupHeader">
        <span>DATOS DE RENTA</span>
    </div>
    <div id="divDatosRentaControles">
        <fieldset>
            <legend>Información de la Renta</legend>
            <table class="trAlinearDerecha" style="margin: 0 auto;">
                <tr>
                    <td class="tdCentradoVertical">
                        <span>*</span>Plazo
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtMeses" runat="server" Width="60px" CssClass="CampoNumeroEntero"
                            OnTextChanged="txtMeses_TextChanged" AutoPostBack="true" MaxLength="3"></asp:TextBox>
                        <asp:ImageButton ID="ibtnEditarMeses" ImageUrl="~/Contenido/Imagenes/EDITAR-ICO.png"
                            runat="server" />
                    </td>
                    <td class="tdCentradoVertical leyendaAyuda">
                        (Meses)
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtPlazo" runat="server" Enabled="false" Width="60px" CssClass="CampoNumeroEntero"></asp:TextBox>
                    </td>
                    <td class="tdCentradoVertical leyendaAyuda">
                        (Años)
                    </td>
                    <td class="tdCentradoVertical" style="width: 200px;" align="right">
                        <span>*</span>Ubicación Taller Servicio
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td style="width: 325px;">
                        <asp:TextBox ID="txtUbicacionTaller" runat="server" MaxLength="300" Columns="50"
                            Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical" style="text-align: left;" colspan="6">
                        <span>*</span>¿Incluye Seguro?&nbsp;
                        <asp:DropDownList ID="ddlIncluyeSeguro" runat="server" Width="190px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlIncluyeSeguro_OnSelectedIndexChanged">
                            <asp:ListItem Text="Seleccione una opción" Value="-1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="tdCentradoVertical" align="right">
                        <span>Porcentaje Adicional</span>
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td>
                       <asp:TextBox ID="txtPorcentajeSeguro" runat="server" Width="50px" CssClass="CampoNumeroEntero" MaxLength="3"/>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical" style="text-align: left;" colspan="6">
                        <span>*</span>¿Incluye Llantas?&nbsp;
                        <asp:DropDownList ID="ddlIncluyeLlantas" runat="server" Width="190px">
                            <asp:ListItem Text="Seleccione una opción" Value="-1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="tdCentradoVertical" align="right">
                        <span>Frecuencia Cobro</span>
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlFrecuenciaCobroSeguro" runat="server" Width="190px">
                            <asp:ListItem Text="Seleccione una opción" Value="-1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical" style="text-align: left;" colspan="6">
                        <span>*</span>¿Incluye Lavado?&nbsp;
                        <asp:DropDownList ID="ddlIncluyeLavado" runat="server" Width="190px">
                            <asp:ListItem Text="Seleccione una opción" Value="-1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="tdCentradoVertical" align="right">
                        <span>*</span>¿Incluye Pintura?                        
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlIncluyePintura" runat="server" Width="190px">
                            <asp:ListItem Text="Seleccione una opción" Value="-1"></asp:ListItem>
                        </asp:DropDownList>                        
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset class="SoloBordeSuperior">
            <legend>
                <asp:Label runat="server" ID="lblTitulo" Text="Agregar Unidades a Renta"></asp:Label></legend>
            <table class="trAlinearDerecha">
                <tr>
                    <td class="tdCentradoVertical" style="width: 30px;">
                        <asp:Label runat="server" ID="lblVIN" Text="VIN"></asp:Label>
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical" style="width: 330px;">
                        <asp:TextBox ID="txtNumeroSerie" runat="server" Width="275px" OnTextChanged="txtNumeroSerie_TextChanged" AutoPostBack="True"></asp:TextBox>
                        <asp:ImageButton ID="ibtnBuscarUnidad" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                            OnClick="ibtnBuscarUnidad_Click" Style="width: 17px" />
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updAgregarUnidad" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnAgregarUnidad" CssClass="btnAgregarATabla" runat="server" Text="Agregar Unidad"
                                    Enabled="False" OnClick="btnAgregarUnidad_Click" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtNumeroSerie" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ibtnBuscarUnidad" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</div>
<div class="GroupBody LineasContrato">
    <asp:UpdatePanel runat="server" ID="updLineasContratos" ChildrenAsTriggers="True">
        <ContentTemplate>
            <asp:GridView ID="grdLineasContrato" runat="server" AutoGenerateColumns="False" PageSize="10"
                AllowPaging="True" CellPadding="4" GridLines="None" CssClass="Grid" Width="100%"
                AllowSorting="True" OnRowDataBound="grdLineasContrato_RowDataBound" OnPageIndexChanging="grdLineasContrato_PageIndexChanging"
                OnRowCommand="grdLineasContrato_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="VIN">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblVIN"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="10%" HorizontalAlign="Justify" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Modelo">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblModelo"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="10%" HorizontalAlign="Justify" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Km Estimado Anual">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblKmEstimadoAnual"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="10%" HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Déposito Garantía" SortExpression="">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblDepositoGarantia"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="10%" HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comisión Apertura" SortExpression="">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblComisionApertura"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="10%" HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cargo Fijo x Mes" SortExpression="">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblCargoFijoMes"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="10%" HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Opción Compra">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="cbOpcionComprea" Checked='<%#((LineaContratoFSLBO)Container.DataItem).ConOpcionCompra == true %>' Enabled="False" Width="17px" />
                        </ItemTemplate>
                    <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cargo KM" SortExpression="">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="ibtnCargoKM" ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Ver Cargo por KM">
                            </asp:ImageButton>
                        </ItemTemplate>
                        <ItemStyle Width="8%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cargo HR" SortExpression="">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="ibtnCargoHR" ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Ver Cargo por Hora">
                            </asp:ImageButton>
                        </ItemTemplate>
                        <ItemStyle Width="8%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cargos EA" SortExpression="">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="ibtnCargoEA" ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Ver Cargo por Equipo Aliado">
                            </asp:ImageButton>
                        </ItemTemplate>
                        <ItemStyle Width="8%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ACTIVA" SortExpression="">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblActiva"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="6%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png" ToolTip="Eliminar"
                                CommandName="CMDELIMINAR" CommandArgument='<%#Container.DataItemIndex%>' Width="17px" />
                        </ItemTemplate>
                        <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="ibtnDetalles" ImageUrl="~/Contenido/Imagenes/VER.png" ToolTip="Ver Detalles"
                                CommandName="CMDDETALLES" CommandArgument='<%#Container.DataItemIndex%>' Width="17px" />
                        </ItemTemplate>
                        <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="GridHeader" HorizontalAlign="Center" />
                <EditRowStyle CssClass="GridAlternatingRow" />
                <PagerStyle CssClass="GridPager" />
                <RowStyle CssClass="GridRow" />
                <FooterStyle CssClass="GridFooter" />
                <SelectedRowStyle CssClass="GridSelectedRow" />
                <AlternatingRowStyle CssClass="GridAlternatingRow" />
                <EmptyDataTemplate>
                    <b>No se han asignado Unidades al Contrato.</b>
                </EmptyDataTemplate>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<asp:HiddenField runat="server" ID="hdnUnidadID" />
<asp:HiddenField runat="server" ID="hdnEquipoID" />
<div id="<%= ClientID %>_divCargos" style="display: none">
</div>
<div id="<%= ClientID %>_divConfirmacion" style="display: none">
</div>
<asp:Button runat="server" ID="btnResult" Style="display: none" OnClick="btnResult_Click" />
