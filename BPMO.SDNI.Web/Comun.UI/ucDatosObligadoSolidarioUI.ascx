<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDatosObligadoSolidarioUI.ascx.cs"
	Inherits="BPMO.SDNI.Comun.UI.ucDatosObligadoSolidarioUI" %>
<%@ Register Src="~/Comun.UI/ucDatosActaConstitutivaUI.ascx" TagPrefix="uc1" TagName="ucDatosActaConstitutivaUI" %>

<script language="javascript" type="text/javascript">


	function MostrarOcultarRepresentantesObligados() {

		if ($("#<%= ddlTipoObligado.ClientID%>").val() != "1") {
			$("#<%= updRepresentantes.ClientID%>").css("display", "none");
		}
		else {
			$("#<%= updRepresentantes.ClientID%>").css("display", "block");

		}

		$("#<%= ddlTipoObligado.ClientID%>").on('change', function () {

			if ($("#<%= ddlTipoObligado.ClientID%>").val() == "1") {
				$("#<%= updRepresentantes.ClientID%>").css("display", "block");
				$("#<%=hdnConfirmarEliminar.ClientID%>").val("1");
				$("#<%= hdnTipoObligadoAnterior.ClientID %>").val($("#<%= ddlTipoObligado.ClientID%>").val());
			}
			else {
				if ($("#<%= hdnTipoObligadoAnterior.ClientID %>").val() == "1") {

					var $div = $('<div title="Confirmación"></div>');
					$div.append('Si cambia el tipo de obligado solidario se eliminaran los representantes legales agregados.<br />¿Desea Continuar?');
					$("#dialog:ui-dialog").dialog("destroy");
					window.respuesta = "";
					$($div).dialog({
						dialogClass: "no-close",
						closeOnEscape: true,
						modal: true,
						minWidth: 460,
						minHeight: 250,
						buttons: {
							Aceptar: function () {
								$(this).dialog("close");
								$("#<%=hdnConfirmarEliminar.ClientID%>").val("1");
								
								__doPostBack('', '')
							},
							Cancelar: function () {
								$(this).dialog("close");
								$("#<%= btnAgregarRepresentanteObligado.ClientID%>").css("display", "block");
								$("#<%= updRepresentantes.ClientID%>").css("display", "block");
								$("#<%=hdnConfirmarEliminar.ClientID%>").val("0");
								$("#<%= ddlTipoObligado.ClientID%>").val("1");
								$("#<%= hdnTipoObligadoAnterior.ClientID %>").val($("#<%= ddlTipoObligado.ClientID%>").val());
								__doPostBack('', '')
							}
						}
					});

				} else {
					$("#<%= hdnTipoObligadoAnterior.ClientID %>").val($("#<%= ddlTipoObligado.ClientID%>").val());
					$("#<%=hdnConfirmarEliminar.ClientID%>").val("1");
				}
			}

			$("#<%= hdnTipoObligadoAnterior.ClientID %>").val($("#<%= ddlTipoObligado.ClientID%>").val());
		});
	}

	function validaNumericos(event) {
	    if (event.charCode >= 48 && event.charCode <= 57) {
	        return true;
	    }
	    return false;
	}

</script>



<div id="dvDatosObligadoSolidario">
	<table class="trAlinearDerecha">
		<tr>
			<td class="tdCentradoVertical etiquetaControl">
				<span>*</span>Tipo de Obligado Solidario
			</td>
			<td class="espacio">
			</td>
			<td class="tdCentradoVertical control">
				<asp:DropDownList runat="server" ID="ddlTipoObligado" AutoPostBack="True" />
			</td>
			<td class="espacio">
				&nbsp;
			</td>
			<td class="tdCentradoVertical etiquetaControl">
				<span>*</span>Nombre
			</td>
			<td class="espacio">
				&nbsp;
			</td>
			<td class="tdCentradoVertical control">
				<asp:TextBox ID="txtNombre" runat="server" Width="180px" MaxLength="250"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td class="tdCentradoVertical etiquetaControl">
				<span>*</span>Tel&eacute;fono
			</td>
			<td class="espacio">
				&nbsp;
			</td>

			<td class="tdCentradoVertical control">
				<asp:TextBox Class="validanumericos" ID="txtTelefono" runat="server" Columns="14" MaxLength="10" onkeypress="return validaNumericos(event)" ></asp:TextBox>
			</td>
			<td class="espacio">
				&nbsp;
			</td>
			<td class="tdCentradoVertical etiquetaControl">
				<span>*</span>Direcci&oacute;n
			</td>
			<td class="espacio">
				&nbsp;
			</td>
			<td class="tdCentradoVertical control">
				<asp:TextBox ID="txtDireccion" runat="server" Width="180px" MaxLength="250"></asp:TextBox>
			</td>
		</tr>
        <tr id="trRfc" runat="server">
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
	<asp:UpdatePanel runat="server" ID="updRepresentantes">
		<ContentTemplate>
			<uc1:ucDatosActaConstitutivaUI runat="server" ID="ucDatosActaConstitutivaUI" />
			<br />
			<div class="dvOpciones">
				<asp:Button ID="btnAgregarRepresentanteObligado" runat="server" CssClass="btnAgregarATabla"
					Text="Rep. Legal" Enabled="true" OnClick="btnAgregarRepresentanteObligado_Click" />
			</div>
			<br />
			<div id="divRepresentantesObligados" class="GroupBody" style="width: 860px;">
				<div id="divRepresentantesObligadosHeader" class="GroupHeader">
					<span>Representantes Legales</span>
				</div>
				<div id="divRepresentantesObligadosControles">
					<asp:GridView ID="grdRepresentantesLegales" runat="server" AutoGenerateColumns="false"
						CellPadding="4" GridLines="None" CssClass="Grid" PageSize="5" AllowPaging="True"
						AllowSorting="True" OnRowCommand="grdRepresentantesLegales_RowCommand" OnPageIndexChanging="grdRepresentantesLegales_PageIndexChanging"
						Width="95%">
						<Columns>
							<asp:BoundField HeaderText="Nombre" DataField="Nombre"></asp:BoundField>
							<asp:TemplateField HeaderText="Dirección" ItemStyle-HorizontalAlign="Justify">
								<ItemTemplate>
									<asp:Label runat="server" ID="lblDireccion" Text='<%# ((BPMO.SDNI.Comun.BO.RepresentanteLegalBO)Container.DataItem).DireccionPersona.Calle %>'></asp:Label>
								</ItemTemplate>
								<ItemStyle Width="300px" />
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Folio RPPC" ItemStyle-HorizontalAlign="Justify">
								<ItemTemplate>
									<asp:Label runat="server" ID="lblFolioRPPC" Text='<%# ((BPMO.SDNI.Comun.BO.RepresentanteLegalBO)Container.DataItem).ActaConstitutiva.NumeroRPPC %>'></asp:Label>
								</ItemTemplate>
								<ItemStyle Width="150px" />
							</asp:TemplateField>
							<asp:TemplateField>
								<ItemTemplate>
									<asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png" ToolTip="Eliminar"
										CommandName="CMDELIMINAR" CommandArgument='<%#Container.DataItemIndex%>' />
								</ItemTemplate>
								<ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
							</asp:TemplateField>
						</Columns>
						<HeaderStyle CssClass="GridHeader" />
						<EditRowStyle CssClass="GridAlternatingRow" />
						<PagerStyle CssClass="GridPager" />
						<RowStyle CssClass="GridRow" />
						<FooterStyle CssClass="GridFooter" />
						<SelectedRowStyle CssClass="GridSelectedRow" />
						<AlternatingRowStyle CssClass="GridAlternatingRow" />
					</asp:GridView>
				</div>
			</div>

		</ContentTemplate>
	</asp:UpdatePanel>

	<asp:UpdatePanel runat="server" ID="updControles">
	<ContentTemplate>
			<asp:HiddenField ID="hdnObligadoID" runat="server" />
			<asp:HiddenField runat="server" ID="hdnTipoObligadoAnterior" Value="" />
			<asp:HiddenField runat="server" ID="hdnEliminarRepresentantes" Value="No" />
			<asp:HiddenField runat="server" ID="hdnConfirmarEliminar" Value="0" />
	</ContentTemplate></asp:UpdatePanel>
</div>
