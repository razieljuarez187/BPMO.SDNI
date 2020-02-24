<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucConfiguracionSistemaUnidad.ascx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.ucConfiguracionSistemaUnidad" %>

<table class="trAlinearDerecha table-responsive">
    <tr>
        <td class="tdCentradoVertical input-label-responsive"><span>*</span>CLAVE</td>
        <td class="input-space-responsive">&nbsp;</td>
        <td class="tdCentradoVertical input-group-responsive">
            <asp:TextBox ID="txtClave" runat="server" MaxLength="3" CssClass="input-find-responsive"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical input-label-responsive"><span>*</span>NOMBRE DEL SISTEMA DE UNIDAD</td>
        <td class="input-space-responsive">&nbsp;</td>
        <td class="tdCentradoVertical input-group-responsive">
            <asp:TextBox ID="txtNombre" runat="server" MaxLength="30" CssClass="input-find-responsive"></asp:TextBox>
        </td>
    </tr>
    <tr style="display:none;">
        <td>
            <asp:HiddenField ID="hdnLibroActivos" runat="server" />
        </td>
    </tr>
</table>
<div class="ContenedorMensajes">
    <span class="Requeridos"></span>
</div>