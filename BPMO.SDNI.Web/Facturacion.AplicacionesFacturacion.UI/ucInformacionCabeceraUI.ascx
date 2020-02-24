<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucInformacionCabeceraUI.ascx.cs" 
    Inherits="BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI.ucInformacionCabeceraUI" %>
<%--Satisface el caso de uso CU005 – Armar Paquetes Facturacion--%>
<%--Satisface a la solicitud de cambio SC0016--%>

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
                            Enabled="false" onselectedindexchanged="ddlFormaPago_SelectedIndexChanged">                            
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
    <br />
    <br />
</div>
