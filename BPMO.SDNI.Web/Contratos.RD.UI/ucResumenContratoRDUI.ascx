<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucResumenContratoRDUI.ascx.cs"
    Inherits="BPMO.SDNI.Contratos.RD.UI.ucResumenContratoRDUI" %>
<%-- 
    Satisface al caso de uso CU003 - Consultar Contratos Renta Diaria
    Satisface a la solicitud de cambio SC0035
--%>
<table class="trAlinearDerecha" style="margin-bottom: 5px; width:100%;">
    <tr>
        <td class="tdCentradoVertical" style="width: 165px;">Sucursal</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 320px;">
            <asp:TextBox ID="txtSucursal" runat="server" Width="250px" Enabled="false"></asp:TextBox>
        </td>
        <td class="tdCentradoVertical" align="right">Empresa</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtEmpresa" runat="server" Width="250px" Enabled="false"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical" style="width: 165px;">Fecha Contrato</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 320px;">
            <asp:TextBox ID="txtFechaContrato" runat="server" CssClass="CampoFecha" 
                Enabled="false" Width="151px"></asp:TextBox>
        </td>
        <td class="tdCentradoVertical" align="right" style="width: 165px;">Fecha Cierre de Contrato</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 320px;">
            <asp:TextBox ID="txtFechaCierreContrato" runat="server" CssClass="CampoFecha" 
                Enabled="false" Width="151px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical" style="width: 165px;">Cliente</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 320px;">
            <asp:TextBox ID="txtCliente" runat="server" Width="250px" Enabled="false"></asp:TextBox>
        </td>
        <td class="tdCentradoVertical" align="right">Direcci&oacute;n del Cliente</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" rowspan="3">
            <asp:TextBox ID="txtDireccion" runat="server" Rows="5" Columns="30" TextMode="MultiLine"
                MaxLength="500" Style="float: left; max-width: 250px; min-width: 250px; max-height: 90px;
                min-height: 90px;" Enabled="false"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical" style="width: 165px;">RFC</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 320px;">
            <asp:TextBox ID="txtRFC" runat="server" Width="250px" Enabled="false"></asp:TextBox>
        </td>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td class="tdCentradoVertical" style="width: 165px;"><label>Cuenta Oracle</label></td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 320px;">
            <asp:TextBox ID="txtNumeroCuentaOracle" runat="server" Width="250px" Enabled="false"></asp:TextBox>
        </td>
        <td colspan="2" style="height: 30px;">&nbsp;</td>
    </tr>
    <tr>
        <td class="tdCentradoVertical" style="width: 165px;"><label>Frecuencia Facturaci&oacute;n</label></td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 320px;">
            <asp:TextBox runat="server" ID="txtFrecuenciaFacturacion" Width="250px" Enabled="false"></asp:TextBox>
        </td>
        <td colspan="2" style="height: 30px;">
            &nbsp;
        </td>
    </tr>
</table>
<asp:HiddenField runat="server" ID="hdnEmpresaID" />
