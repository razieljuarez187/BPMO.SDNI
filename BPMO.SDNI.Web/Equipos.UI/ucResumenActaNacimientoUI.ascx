<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucResumenActaNacimientoUI.ascx.cs" Inherits="BPMO.SDNI.Equipos.UI.ucResumenActaNacimientoUI" %>
<%--Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad--%>
<%--Satisface al CU079 - Consultar Acta de Nacimiento de Unidad--%>
<%--Satisface la solicitud de cambio SC0006--%>

<table class="trAlinearDerecha" style="margin: 0px auto 10px auto; width: 530px; display: inherit; border: 1px solid transparent;">
        <tr>
        <td class="tdCentradoVertical">
            <asp:Label ID="lblVIN" runat="server">VIN</asp:Label>
        </td>
        <td style="width: 20px;">
        &nbsp;
    </td>
        <td class="tdCentradoVertical" style="width: 330px;">
        <asp:TextBox ID="txtVIN" runat="server" Width="275px"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td class="tdCentradoVertical">
		    <asp:Label ID="lblAreaDepartamento" runat="server">&Aacute;rea/Departamento</asp:Label>
		</td>
		<td style="width: 20px;">
			&nbsp;
		</td>
		<td class="tdCentradoVertical" style="width: 330px;">
			<asp:TextBox ID="txtAreaDepartamento" runat="server" Width="275px"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td class="tdCentradoVertical">
			SUCURSAL
		</td>
		<td style="width: 20px;">
			&nbsp;
		</td>
		<td class="tdCentradoVertical" style="width: 330px;">
			<asp:TextBox ID="txtSucursal" runat="server" Width="275px"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td class="tdCentradoVertical">
			ESTATUS
		</td>
		<td style="width: 20px;">
			&nbsp;
		</td>
		<td class="tdCentradoVertical" style="width: 330px;">
			<asp:TextBox ID="txtEstatus" runat="server" Width="275px"></asp:TextBox>
            <asp:HiddenField ID="hdnEstatus" runat="server" />
		</td>
	</tr>
	<tr id="trRegistroFC" runat="server">
		<td class="tdCentradoVertical">
			FECHA REGISTRO
		</td>
		<td style="width: 20px;">
			&nbsp;
		</td>
		<td class="tdCentradoVertical" style="width: 330px;">
			<asp:TextBox ID="txtFechaRegistro" runat="server" Width="275px"></asp:TextBox>
		</td>
	</tr>
	<tr id="trRegistroUC" runat="server">
		<td class="tdCentradoVertical">
			USUARIO REGISTRO
		</td>
		<td style="width: 20px;">
			&nbsp;
		</td>
		<td class="tdCentradoVertical" style="width: 330px;">
			<asp:TextBox ID="txtUsuarioRegistro" runat="server" Width="275px"></asp:TextBox>
            <asp:HiddenField ID="hdnUC" runat="server" />
		</td>
	</tr>
	<tr id="trActualizacionFUA" runat="server">
		<td class="tdCentradoVertical">
			FECHA MODIFICACIÓN
		</td>
		<td style="width: 20px;">
			&nbsp;
		</td>
		<td class="tdCentradoVertical" style="width: 330px;">
			<asp:TextBox ID="txtFechaModificacion" runat="server" Width="275px"></asp:TextBox>
		</td>
	</tr>
	<tr id="trActualziacionUUA" runat="server">
		<td class="tdCentradoVertical">
			USUARIO MODIFICACIÓN
		</td>
		<td style="width: 20px;">
			&nbsp;
		</td>
		<td class="tdCentradoVertical" style="width: 330px;">
			<asp:TextBox ID="txtUsuarioModificacion" runat="server" Width="275px"></asp:TextBox>
            <asp:HiddenField ID="hdnUUA" runat="server" />
		</td>
	</tr>
</table>
<div id="divBotones" runat="server" class="trAlinearDerecha" style="margin: 0px auto; width: 530px; display: inherit; border: 1px solid transparent; text-align: center;">
	<asp:Button ID="btnConfiguracionMantenimientos" runat="server" 
        Text="Configuración Mantenimientos Preventivos" CssClass="btnLibrePositivo" 
        style="margin-bottom: 5px; width: 70%;" 
        onclick="btnConfiguracionMantenimientos_Click" />
	<asp:Button ID="btnRegistrarMantenimientos" runat="server" 
        Text="Registrar Mantenimientos Recomendados" CssClass="btnLibrePositivo" 
        style="margin-bottom: 5px; width: 70%;" 
        onclick="btnRegistrarMantenimientos_Click" />
</div>

<div id="divSiniestros" runat="server" visible="false">
    <fieldset id="fsHistoricoSiniestros" style="width: 95%; margin: 10px auto;">
        <legend>Histórico Siniestros</legend>
        <asp:GridView ID="gvHistoricoSiniestros" runat="server" CssClass="Grid" style="width: 100%;" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField HeaderText="Fecha Siniestro" DataField="FechaSiniestro" />
                <asp:BoundField HeaderText="Fecha Dictamen" DataField="FechaDictamen" />
                <asp:BoundField HeaderText="Observaciones" DataField="Observaciones" />
            </Columns>
            <RowStyle CssClass="GridRow" />
            <HeaderStyle CssClass="GridHeader" />
            <FooterStyle CssClass="GridFooter" />
            <PagerStyle CssClass="GridPager" />
            <SelectedRowStyle CssClass="GridSelectedRow" />
            <AlternatingRowStyle CssClass="GridAlternatingRow" />
        </asp:GridView>
    </fieldset>
</div>

<asp:HiddenField ID="hdnEquipoID" runat="server" />
<asp:HiddenField ID="hdnUnidadID" runat="server" />