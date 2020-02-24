<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucInformacionGeneralPSLUI.ascx.cs" 
    Inherits="BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI.ucInformacionGeneralPSLUI" %>

<script type="text/javascript">
    function MostrarDialogo(mostrar, titulo) {
        $("#<%= hdnMostrarDialogoPago.ClientID %>").val("0");
        $("#DialogoCancelarPago").dialog({
            modal: true,
            width: 650,
            autoOpen: false,
            resizable: false,
            title: titulo,
            closeOnEscape: false,
            close: function () { $(this).dialog("destroy"); },
            open: function (event, ui) {
                $(this).parent().find(".ui-dialog-titlebar-close").show();
            }
        });

        $("#DialogoCancelarPago").parent().appendTo("form:first");
        if (mostrar) {
            $("#DialogoCancelarPago").dialog("open");
            $("#<%= hdnMostrarDialogoPago.ClientID %>").val("1");
        }
    }
</script>

<style type = "text/css">
fieldset {
    width: 700px;
    margin-left: 30px;
}    
.centrarDiv {
    width: 700px;
    margin-left: 30px;
}
</style>
<div id="divInformacionGeneralControles">
    <fieldset>
        <legend>Datos de Sucursal</legend>
        <div class="dvIzquierda">
            <table class="trAlinearDerecha">
                <tr>
                    <td class="tdCentradoVertical" style="white-space:nowrap">
                        Sucursal
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtSucursal" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                        <asp:HiddenField ID="hdnSucursalID" runat="server" Visible="False" />
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical" style="white-space:nowrap">
                        Tipo Transacci&oacute;n
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtTipoTransaccion" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div class="dvDerecha">
            <table class="trAlinearDerecha">
                <tr>
                    <td class="tdCentradoVertical" style="white-space:nowrap">
                        Sistema Origen
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical" style="width: 300px;">
                        <asp:TextBox ID="txtSistemaOrigen" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical" style="white-space:nowrap">
                        No. Referencia
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtReferencia" runat="server" MaxLength="200" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </fieldset>
    <fieldset>
        <legend>Informaci&oacute;n de Pago</legend>
        <div class="dvIzquierda">
            <table class="trAlinearDerecha">
                <tr>
                    <td class="tdCentradoVertical" style="white-space:nowrap">
                        C&oacute;digo de Moneda
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtCodigoMoneda" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical" style="white-space:nowrap">
                        Tipo de Cambio
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtTipoCambio" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td  class="tdCentradoVertical" style="white-space:nowrap">
                        Tipo Tasa Cambiaria
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtTipoTazaCambiario" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td  class="tdCentradoVertical" style="white-space:nowrap">
                        Límite de Cr&eacute;dito
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtLimiteCredito" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div class="dvDerecha">
            <table class="trAlinearDerecha">
                <tr>
                    <td class="tdCentradoVertical" style="white-space:nowrap">
                        Forma de Pago
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical" style="width: 300px;">                        
                        <asp:DropDownList ID="ddlFormaPago" runat="server" Width="200px" 
                            Enabled="true" onselectedindexchanged="ddlFormaPago_SelectedIndexChanged">                            
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical" style="white-space:nowrap">
                        D&iacute;as Factura
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtDiasFactura" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical" style="white-space:nowrap">
                        D&iacute;as Cr&eacute;dito
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtDiasCredito" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>

                 <tr>
                    <td class="tdCentradoVertical" style="white-space:nowrap">
                       Crédito Disponible
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtCreditoDisponible" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </fieldset>

    <fieldset>
    <div class="centrarDiv">
        <div class="dvIzquierda">
            <table class="trAlinearDerecha">
                <tr>
                    <td class="tdCentradoVertical" style="white-space:nowrap">
                        Departamento
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtDepartamento" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div class="dvDerecha">
            <table class="trAlinearDerecha">
                <tr>
                    <td class="tdCentradoVertical" style="white-space:nowrap">
                        Bandera Cores
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical" style="width: 300px;">
                        <asp:TextBox ID="txtBanderaCores" runat="server" Width="70px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </fieldset>

    <fieldset>
    <legend>UNIDADES AGREGADAS A RENTA</legend>
    <div class="centrarDiv">
        <asp:UpdatePanel runat="server" ID="updLineasContratos">
            <ContentTemplate>



                <asp:GridView ID="GridLineasUnidad" runat="server" AutoGenerateColumns="false" PageSize="10"
                 AllowPaging="true" CellPadding="4" GridLines="None" CssClass="Grid" Width="100%" AllowSorting="true"
                 OnRowDataBound="GridLineasUnidad_RowDataBound" OnRowCommand="GridLineasUnidad_RowCommand">

                    <Columns>
                        <asp:TemplateField HeaderText="SERIE UNIDAD">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblVIN"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="20%" HorizontalAlign="Justify" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Modelo">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblModelo"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="20%" HorizontalAlign="Justify" />
                        </asp:TemplateField>                        
                        <%--<asp:TemplateField HeaderText="Tipo Tarifa" SortExpression="">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblTipoTarifa"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="10%" HorizontalAlign="Justify" />
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Turno" SortExpression="">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblTurno"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="10%" HorizontalAlign="Justify" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tarifa" SortExpression="">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblTarifa"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                        </asp:TemplateField>                  --%>
                        <asp:TemplateField HeaderText="HORA ADICIONAL" SortExpression="">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblAdicional"></asp:Label>
                                    <asp:ImageButton runat="server" ID="ibtnAdicional" CommandName="ACTUALIZARHORA" ImageUrl="~/Contenido/Imagenes/GUARDAR-ICO.png"
										ToolTip="Hora Adicional" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"PagoContratoPSLID") %>'
										ImageAlign="Middle" />
                            </ItemTemplate>
                            <ItemStyle Width="6%" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DESCRIPCIÓN" SortExpression="">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblDescripcion"></asp:Label>
                                    <asp:ImageButton runat="server" ID="ibtnDescripcion" CommandName="ACTUALIZARDESC" ImageUrl="~/Contenido/Imagenes/GUARDAR-ICO.png"
										ToolTip="Descripción Línea" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"PagoContratoPSLID") %>'
										ImageAlign="Middle" />
                            </ItemTemplate>
                            <ItemStyle Width="6%" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="# CARGOS" SortExpression="">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCargos"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="6%" HorizontalAlign="Center" />
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="MONTO DE CARGOS" SortExpression="">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblMontoCargos"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="20%" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ibtnDetalles" ImageUrl="~/Contenido/Imagenes/VER.png" ToolTip="Ver Detalles"
                                    CommandName="CMDDETALLES" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"PagoContratoPSLID")%>' Width="17px" />
                            </ItemTemplate>
                            <ItemStyle Width="7px" HorizontalAlign="Center" VerticalAlign="Middle" />
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
                <asp:Button ID="btnCancelarPagoPendiente" runat="server" Text="Button" OnClick="btnCancelarPagoPendiente_Click" style="display: none;" />

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </fieldset>

    <div class="centrarDiv">
        <div class="dvIzquierda">
            <table class="trAlinearDerecha" style="margin: 0px auto; width: 530px; display: inherit; border: 1px solid transparent;">

            <tr>
                        <td class="tdCentradoVertical">
                <span>*</span>Moneda
            </td>
                        <td class="separadorCampo">
                &nbsp;
            </td>
                <td class="tdCentradoVertical" colspan="5" style="width: 330px;">               
                    <asp:DropDownList ID="ddlMoneda" Width="200px" runat="server" 
                        AutoPostBack="true" onselectedindexchanged="ddlMoneda_SelectedIndexChanged1" >
                    
                        <asp:ListItem Value="MXN" Text="PESO MEXICANO"></asp:ListItem>
                        <asp:ListItem Value="-1" Text="MONEDA DE CONTRATO"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
            <td>
            </td>
            </tr>

                <tr>
                    <td class="tdCentradoVertical">
                        Observaciones
                    </td>
                    <td class="separadorCampo">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox runat="server" ID="txtObservaciones" MaxLength="250" Width="375px" TextMode="MultiLine" style="resize: none;"
                            Rows="3">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <br />
    <br />
</div>

<asp:HiddenField runat="server" ID="hdnMostrarDialogoPago" Value="0"/>
<asp:HiddenField runat="server" ID="hdnPagoIDGrid" Value="0"/>
<asp:HiddenField runat="server" ID="hdnTipoLinea" Value=""/>

    <div id="DialogoCancelarPago" style="display: none">		    
		    <div class="dvIzquierda">
			    <table class="trAlinearDerecha" style="width:600px">
				    <tr>
					    <td class="tdCentradoVertical">
						    
					    </td>
					    <td style="width: 20px;">
						    &nbsp;
					    </td>
					    <td class="tdCentradoVertical">
						    <asp:TextBox runat="server" ID="txtMotivoCancelacion" MaxLength="50" TextMode="MultiLine" Rows="3" Columns="20"
							    Width="600px"></asp:TextBox>
					    </td>
				    </tr>			
				    </table>
		    </div>
		    <div class="dvDerecha">
				     <asp:button runat="server" ID="btnValidarAutorizacion" Text="Agregar" 
							    onclick="btnValidarAutorizacion_Click" OnClientClick="__blockUIModal();" CssClass="btnWizardGuardar" /> 
                <asp:Button runat="server" ID="btnCancelarCambioTarifa" Text="Cancelar" 
                    CssClass="btnWizardCancelar" onclick="btnDescartarActualizacion_Click" OnClientClick="MostrarDialogo(false,'');"/>
		    </div>
    </div>

