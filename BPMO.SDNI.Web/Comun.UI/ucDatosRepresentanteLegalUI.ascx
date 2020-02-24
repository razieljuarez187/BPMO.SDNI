<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDatosRepresentanteLegalUI.ascx.cs"
	Inherits="BPMO.SDNI.Comun.UI.ucDatosRepresentanteLegalUI" %>
<%@ Register Src="~/Comun.UI/ucDatosActaConstitutivaUI.ascx" TagPrefix="uc" TagName="ucDatosActaConstitutivaUI" %>

<script language="javascript" type="text/javascript">
    function validaNumericos(event) {
        if (event.charCode >= 48 && event.charCode <= 57) {
            return true;
        }
        return false;
    }

</script>


<div id="dvDatosRepresentanteLegal">
	<table>
		<tr>
			<td class="tdCentradoVertical etiquetaControl" >
				<span class="requeridos">*</span>Nombre Completo
			</td>
			<td class="tdCentradoVertical espacio">
				&nbsp;
			</td>
			<td class="tdCentradoVertical control">
				<asp:TextBox ID="txtNombreRepresentante" runat="server" Width="200px" MaxLength="250"></asp:TextBox>
			</td>
			<td class="tdCentradoVertical espacio">
				&nbsp;
			</td>
			<td class="tdCentradoVertical etiquetaControl">
				<span class="requeridos">*</span>Direcci&oacute;n
			</td>
			<td class="tdCentradoVertical espacio">
				&nbsp;
			</td>
			<td class="tdCentradoVertical control">
				<asp:TextBox ID="txtDireccionRepresentante" runat="server" MaxLength="250"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td class="tdCentradoVertical etiquetaControl">
				<span class="requeridos">*</span>Teléfono
			</td>
			<td class="tdCentradoVertical espacio">
			</td>
			<td class="tdCentradoVertical control">
				<asp:TextBox ID="txtTelefonoRepresentante" runat="server" Columns="10" MaxLength="10" onkeypress="return validaNumericos(event)"></asp:TextBox>
			</td>
			<td class="tdCentradoVertical espacio" >
				&nbsp;
			</td>
			<td class="tdCentradoVertical etiquetaControl" >
				<span class="requeridos" runat="server" ID="asteriscoDepositario">*</span><asp:Label runat="server" ID="lblDepositario" Text="Depositario"></asp:Label>
			</td>
			<td class="tdCentradoVertical espacio">
			</td>
			<td class="tdCentradoVertical control">
				<asp:DropDownList ID="ddlDepositario" runat="server">
					<asp:ListItem Text="Seleccione una opción" Value="0"></asp:ListItem>
					<asp:ListItem Text="SI" Value="1"></asp:ListItem>
					<asp:ListItem Text="NO" Value="2"></asp:ListItem>
				</asp:DropDownList>
			</td>
		</tr>
	    <tr id="trRfc" runat="server" Visible="False">
	        <td class="tdCentradoVertical etiquetaControl">
	            <span>*</span>RFC
	        </td>
	        <td class="espacio">
	            &nbsp;
	        </td>
	        <td class="tdCentradoVertical control">
	            <asp:TextBox ID="txtRFC" runat="server" Width="180px" MaxLength="15"></asp:TextBox>
	        </td>
	    </tr>
	</table>
	<uc:ucDatosActaConstitutivaUI runat="server" ID="datosActaConstitutivaUI" />
</div>
<asp:HiddenField ID="hdnRepresentanteID" runat="server" />