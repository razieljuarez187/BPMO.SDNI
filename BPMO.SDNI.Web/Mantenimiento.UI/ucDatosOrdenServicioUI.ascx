<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDatosOrdenServicioUI.ascx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.ucDatosOrdenServicioUI" %>
    <table class="trAlinearDerecha form-two-columns-responsive" style="margin: 0px auto 10px auto; width: auto; display: inherit; border: 1px solid transparent;">
        <tr>
            <td class="tdCentradoVertical input-label-responsive">VIN</td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:TextBox ID="txtFVin" ReadOnly="true" Enabled="false" runat="server" CssClass="input-text-responsive"></asp:TextBox>
                <asp:HiddenField ID="hdnIdMantenimiento" runat="server" />
            </td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-label-responsive"><asp:Label ID="lblNumeroEconomico" runat="server">Número Económico</asp:Label> </td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:TextBox ID="txtFNumEco" ReadOnly="true" Enabled="false" runat="server" CssClass="input-text-responsive"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical input-label-responsive">Modelo:</td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:TextBox ID="txtFModelo" ReadOnly="true" Enabled="false" runat="server" CssClass="input-text-responsive"></asp:TextBox>
            </td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-label-responsive">Cliente: </td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:TextBox ID="txtFCliente" ReadOnly="true" Enabled="false" runat="server" CssClass="input-text-responsive"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical input-label-responsive">Kilometraje</td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:TextBox ID="txtFKilometraje" ReadOnly="true" Enabled="false" runat="server" CssClass="CampoNumeroEntero input-text-responsive"></asp:TextBox>
            </td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-label-responsive">Horometro</td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:TextBox ID="txtFHorometro" ReadOnly="true" Enabled="false" runat="server" CssClass="CampoNumeroEntero input-text-responsive"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical input-label-responsive">Combustible total(lts)</td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:TextBox ID="txtFCombusTotal" ReadOnly="true" Enabled="false" runat="server" CssClass="CampoNumeroEntero input-text-responsive"></asp:TextBox>
            </td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-label-responsive">Tipo de Servicio:</td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:TextBox ID="txtFTipoServicio" ReadOnly="true" Enabled="false" runat="server" CssClass="input-text-responsive"></asp:TextBox>
            </td>
        </tr>
        <tr style="display:none;">
            <td class="tdCentradoVertical input-label-responsive">SUCURSAL:</td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:TextBox ID="txtFSucursal" ReadOnly="true" Enabled="false" runat="server" CssClass="input-text-responsive"></asp:TextBox>
            </td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-label-responsive">TALLER:</td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:TextBox ID="txtFTaller" ReadOnly="true" Enabled="false" runat="server" CssClass="input-text-responsive"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical input-label-responsive">Combustible entrada (lts): </td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:TextBox ID="txtFCombusEntra" ReadOnly="true" Enabled="false" runat="server" CssClass="CampoNumeroEntero input-text-responsive"></asp:TextBox>
            </td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-label-responsive">Combustible salida (lts):</td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:TextBox ID="txtFCombusSalida" ReadOnly="true" Enabled="false" runat="server" CssClass="CampoNumeroEntero input-text-responsive"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical input-label-responsive">Tipo Orden de Servicio:</td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:TextBox ID="txtFTipoOrdenServicio" ReadOnly="true" Enabled="false" runat="server" CssClass="input-text-responsive"></asp:TextBox>
            </td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-label-responsive">Controlista:</td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:TextBox ID="txtFControlista" ReadOnly="true" Enabled="false" runat="server" CssClass="input-text-responsive"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical input-label-responsive">Operador:</td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive" colspan="4">
                <asp:TextBox ID="txtFOperador" ReadOnly="true" Enabled="false" runat="server" CssClass="input-text-responsive"></asp:TextBox>
            </td>
            <td class="input-space-responsive">&nbsp;</td>
        </tr>
        
    </table>