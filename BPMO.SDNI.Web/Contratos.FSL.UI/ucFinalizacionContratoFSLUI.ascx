<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucFinalizacionContratoFSLUI.ascx.cs"
	Inherits="BPMO.SDNI.Contratos.FSL.UI.ucFinalizacionContratoFSLUI" %>
	<%--Satisface al caso de uso CU026 - Registrar Terminacion de Contrato Full Service Leasing--%>
<script type="text/javascript">
    // Inicializa el control de Información General
    function <%= ClientID %>_Inicializar() {
        var FechaCierre = $('#<%= txtFechaCierre.ClientID %>');
        if (FechaCierre.length > 0) {
            FechaCierre.datepicker({
                yearRange: '-100:+10',
                changeYear: true,
                changeMonth: true,
                showButtonPanel: true,
                dateFormat: "dd/mm/yy",
                buttonImage: '../Contenido/Imagenes/calendar.gif',
                buttonImageOnly: true,
                toolTipText: "Fecha de cierre",
                showOn: 'button',
                defaultDate: (FechaCierre.val().length == 10) ? FechaCierre.val() : new Date()
            });

            FechaCierre.attr('readonly', true);
        }
    }
</script>
<fieldset>
<div class="dvIzquierda">
		<table class="trAlinearDerecha">
			<tr>
				<td align="right" style="padding-top: 5px">
					<span><asp:Label runat="server" ID="lblObservaciones" Text=""></asp:Label></span>Observaciones
				</td>
				<td style="width: 5px;">
					&nbsp;
				</td>
				<td class="tdCentradoVertical">
					<asp:TextBox ID="txtObservacionesCierre" runat="server" Rows="5" Columns="30" TextMode="MultiLine"
						MaxLength="500" Style="float: left; max-width: 250px; min-width: 250px; max-height: 90px;
						min-height: 90px;"></asp:TextBox>
				</td>
			</tr>
			
		</table>
	</div>
	<div class="dvDerecha">
		<table class="trAlinearDerecha">
			<tr>
				<td class="tdCentradoVertical">
					<span>*</span>Fecha Cierre
				</td>
				<td style="width: 5px;">
					&nbsp;
				</td>
				<td class="tdCentradoVertical">
					<asp:TextBox ID="txtFechaCierre" runat="server" CssClass="CampoFecha"
						OnTextChanged="txtFechaCierre_TextChanged" AutoPostBack="True" Enabled="False"></asp:TextBox>
				</td>
			</tr>
			
			<tr id="trMotivo" runat="server" Visible="False">
				<td class="tdCentradoVertical">
					<span>*</span>Motivo
				</td>
				<td style="width: 5px;">
					&nbsp;
				</td>
				<td class="tdCentradoVertical">
					<asp:TextBox ID="txtMotivo" runat="server" Width="250px" Style=" max-width: 250px;
						min-width: 250px;" MaxLength="150"></asp:TextBox>
				</td>
			</tr><tr id="trPenalizacion" runat="server" visible="False">
				<td class="tdCentradoVertical">
					<span>*</span>Penalizaci&oacute;n
				</td>
				<td style="width: 5px;">
					&nbsp;
				</td>
				<td class="tdCentradoVertical">
					<asp:TextBox ID="txtPenalizacion" runat="server" CssClass="CampoNumero" Enabled="False"></asp:TextBox>
				</td>
			</tr>
		</table>
	</div>
	
	<asp:HiddenField runat="server" ID="hdnPlazoMeses"/>
	<asp:HiddenField runat="server" ID="hdnMensualidad"/>
	<asp:HiddenField runat="server" ID="hdnModoEdicion"/>
	<asp:HiddenField runat="server" ID="hdnFechaInicioContrato"/>
	<asp:HiddenField runat="server" ID="hdnFechaFinContrato" />
	<asp:HiddenField runat="server" ID="hdnPorcentajePenalizacion"/>

</fieldset>
