<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucLineasFacturaUI.ascx.cs" 
    Inherits="BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI.ucLineasFacturaUI" %>

<%--Satisface el caso de uso CU005 – Armar Paquetes Facturacion--%>
<style type = "text/css">
fieldset {
    width: 745px;
    margin-left: 10px;
}    
.overflow 
{
    width:775px; 
    height:310px; 
    overflow: auto; 
    overflow-x: hidden;
}
</style>
<fieldset>
    <table class="trAlinearDerecha" width="100%">
        <tr>
            <td class="tdCentradoVertical" style="white-space:nowrap">
                Líneas
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtLineas" runat="server" Width="30px" Enabled="false"></asp:TextBox>
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical" style="text-align: right; white-space:nowrap">
                Sub-Total
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtSubTotal" runat="server" Width="90px" Enabled="false"></asp:TextBox>
            </td>
            <td class="tdCentradoVertical" style="text-align: right; white-space:nowrap">
                Impuesto
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtImpuesto" runat="server" Width="90px" Enabled="false"></asp:TextBox>
            </td>
            <td class="tdCentradoVertical" style="font-weight:bold; text-align: right; white-space:nowrap">
                Total Factura
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtTotalFactura" runat="server" Width="90px" Enabled="false" style="font-weight:bold;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical" colspan="5" style="text-align: right; white-space:nowrap"><span>*</span>Uso CFDI</td>
            <td class="separadorCampo">&nbsp;</td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtClaveUsoCFDI" runat="server" Width="90px" MaxLength="25"
                            ontextchanged="txtClaveUsoCFDI_TextChanged" AutoPostBack="True"></asp:TextBox>
                <asp:ImageButton ID="ibtnBuscarUsoCFDI" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                        OnClick="ibtnBuscarUsoCFDI_Click" Style="width: 17px" />
            </td>
            <td class="tdCentradoVertical" colspan="6">
                <asp:TextBox ID="txtDescripcionUsoCFDI" runat="server" Width="250px" MaxLength="100" ReadOnly="true" Enabled="false"></asp:TextBox>
            </td>
        </tr>
    </table>
</fieldset>
<asp:Button ID="btnResultLineas" runat="server" Text="Button" OnClick="btnResultLineas_Click" Style="display: none;" />
<%--Campos ocultos--%>
<asp:HiddenField runat="server" ID="hdnUsoCFDIId" Value="" />
<div class="overflow">
    <asp:Repeater ID="rptLineas" runat="server" 
        onitemdatabound="rptLineas_ItemDataBound">
        <HeaderTemplate>            
            <fieldset>                
        </HeaderTemplate>
        <ItemTemplate>
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <asp:Label ID="lblNoLinea" runat="server"></asp:Label>
            </div>   
            <table class="trAlinearDerecha">
                <tr>
                    <td style="width:50%">
                        <table class="trAlinearDerecha" width="100%">
                            <tr>
                                <td class="tdCentradoVertical" style="white-space:nowrap">
                                    Tipo de Renglón
                                </td>
                                <td style="width: 2px;" >
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox ID="txtTipoRenglon" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical" style="white-space:nowrap">
                                    Clave de Artículo
                                </td>
                                <td style="width: 2px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox ID="txtClaveArticulo" runat="server" Width="200px" Text='<%# DataBinder.Eval(Container.DataItem, "Articulo.NombreCorto") %>' Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="padding-top: 5px; white-space:nowrap">
                                    Descripción
                                </td>
                                <td style="width: 2px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox ID="txtDescripcion" runat="server" Rows="4" Columns="24" Text='<%# DataBinder.Eval(Container.DataItem, "Articulo.Nombre") %>' TextMode="MultiLine"
                                        Enabled="false" MaxLength="200" Style="float: left; max-width: 200px; min-width: 200px; max-height: 90px;
                                        min-height: 90px;"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width:50%">
                        <table class="trAlinearDerecha" width="100%">
                            <tr>
                                <td class="tdCentradoVertical" style="white-space:nowrap">
                                    D&iacute;as de Cr&eacute;dito
                                </td>
                                <td style="width: 2px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical" style="width: 200px;">
                                    <asp:TextBox ID="txtDiasCredito" runat="server" Width="200px" Text='<%# DataBinder.Eval(Container.DataItem, "DiasCredito") %>' Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical" style="white-space:nowrap">
                                    Departamento
                                </td>
                                <td style="width: 2px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox ID="txtDepartamento" runat="server" Width="200px" Text='<%# DataBinder.Eval(Container.DataItem, "Departamento.Nombre") %>' Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="padding-top: 5px; white-space:nowrap">
                                    Observación
                                </td>
                                <td style="width: 2px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox ID="txtObservaciones" runat="server" Rows="4" Columns="24" Text='<%# DataBinder.Eval(Container.DataItem, "Observaciones") %>' TextMode="MultiLine"
                                        Enabled="false" MaxLength="500" Style="float: left; max-width: 200px; min-width: 200px; max-height: 90px;
                                        min-height: 90px;"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <table class="trAlinearDerecha">
                <tr>
                    <td class="tdCentradoVertical" style="white-space:nowrap; width:150px;">
                        Cantidad
                    </td>
                    <td style="width: 1px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtCantidad" runat="server" Width="60px" Text='<%# DataBinder.Eval(Container.DataItem, "Cantidad") %>' Enabled="false"></asp:TextBox>
                    </td>
                    <td style="width: 2px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical" style="text-align: right; white-space:nowrap; width:150px;">
                        Costo Unitario
                    </td>
                    <td style="width: 1px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtCostoUnitario" runat="server" style="width: 90px;" Text='<%# String.Format("{0:#,##0.00}", DataBinder.Eval(Container.DataItem, "CostoUnitario")) %>' Enabled="false"></asp:TextBox>
                    </td>
                    <td class="tdCentradoVertical" style="text-align: right; white-space:nowrap; width:150px;">
                        Precio Unitario
                    </td>
                    <td style="width: 1px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtPrecioUnitario" runat="server" style="width: 90px;" Text='<%# String.Format("{0:#,##0.00}", DataBinder.Eval(Container.DataItem, "PrecioUnitario")) %>' Enabled="false"></asp:TextBox>
                    </td>
                </tr>
            </table>

            <table class="trAlinearDerecha">
                <tr>
                    <td class="tdCentradoVertical" style="white-space:nowrap; width:150px;">
                        Descuento Unitario
                    </td>
                    <td style="width: 1px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical" style="width: 120px;">
                        <asp:TextBox ID="txtDescuentoUnitaro" runat="server" Width="60px" Text='<%# String.Format("{0:#,##0.00}", DataBinder.Eval(Container.DataItem, "DescuentoUnitario")) %>' Enabled="false"></asp:TextBox>
                    </td>
                    <td style="width: 2px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical" style="text-align: right; white-space:nowrap; width:150px;">
                        Retenci&oacute;n Unitario
                    </td>
                    <td style="width: 1px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtRetencionUnitario" runat="server" style="width: 90px;" Text='<%# String.Format("{0:#,##0.00}", DataBinder.Eval(Container.DataItem, "RetencionUnitaria")) %>' Enabled="false"></asp:TextBox>
                    </td>
                    <td class="tdCentradoVertical" style="text-align: right; white-space:nowrap; width:150px;">
                        Impuesto Unitario
                    </td>
                    <td style="width: 1px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical" >
                        <asp:TextBox ID="txtImpuestoUnitario" runat="server" style="width: 90px;" Text='<%# String.Format("{0:#,##0.00}", DataBinder.Eval(Container.DataItem, "ImpuestoUnitario")) %>' Enabled="false"></asp:TextBox>
                    </td>
                </tr>
            </table>            
                           
            <hr style="width:100%; border-style:solid; border-width:thin" />
       
            <table class="trAlinearDerecha">
                <tr>
                    <td style="width: 85%;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical" style="font-weight:bold; width: 170px; text-align: right; white-space:nowrap">
                        Precio Total
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtPrecioTotal" runat="server" Text='<%# String.Format("{0:#,##0.00}", Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Cantidad")) * Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "PrecioUnitario")) + Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "ImpuestoUnitario"))) %>' Enabled="false" style="font-weight:bold;"></asp:TextBox>
                    </td>
                </tr>
            </table>            
        </ItemTemplate>
        <FooterTemplate>
            </fieldset>             
        </FooterTemplate>
    </asp:Repeater>
</div>
    
    


