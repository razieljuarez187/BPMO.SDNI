<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHerramientasCMUI.ascx.cs"
	Inherits="BPMO.SDNI.Contratos.Mantto.CM.UI.ucHerramientasCMUI" %>
<%@ Register TagPrefix="uc" TagName="ucListadoPlantillasUI" Src="~/Contratos.UI/ucListadoPlantillasUI.ascx" %>
<%--Satisface al caso de uso CU029 - Consultar Contratos de mantenimiento --%>
<%-- Satisface al caso de uso CU095 - Imprimir Pagaré Contrato CM --%>
<script type="text/javascript">
     function <%= ClientID %>_Inicializar() {
         ConfiguracionBarraHerramientas();
     }
     function DialogoDetallePlantillas() {
            $("#dvDialogPlantillas").dialog({
                modal: true,
                width: 900,
                height: 400,
                resizable: false,
                buttons: {
                    "Aceptar": function () {
                        $(this).dialog("close");
                    }
                }
            });
            $("#dvDialogPlantillas").parent().appendTo("form:first");
        }
</script>
<div id="BarraHerramientas" style="float: right;">
	<asp:Menu runat="server" ID="mnContratos" IncludeStyleBlock="False" Orientation="Horizontal"
		CssClass="MenuPrimario" OnMenuItemClick="mnContratos_MenuItemClick">
		<Items>
			<asp:MenuItem Text="# Contrato" Value="Contrato" Enabled="False"></asp:MenuItem>
			<asp:MenuItem Text="Editar" Value="Editar" Selectable="false">
				<asp:MenuItem Text="Editar Contrato" Value="EditarContrato"></asp:MenuItem>
				<asp:MenuItem Text="Agregar Documentos" Value="AgregarDocumentos"></asp:MenuItem>
			</asp:MenuItem>
			<asp:MenuItem Text="Impresión" Value="Impresion" Selectable="False">
				<asp:MenuItem Text="Contrato" Value="ImprimirContrato"></asp:MenuItem>
				<asp:MenuItem Text="Manual Operaciones" Value="ImprimirManualOperaciones"></asp:MenuItem>
				<asp:MenuItem Text="Anexo A" Value="ImprimirAnexoA"></asp:MenuItem>
				<asp:MenuItem Text="Anexo B: Mantenimientos Preventivos" Value="ImprimirAnexoB">
				</asp:MenuItem>
				<asp:MenuItem Text="Anexo C: Acta de Nacimiento" Value="ImprimirAnexoC"></asp:MenuItem>
				<asp:MenuItem Text="Pagaré" Value="ImprimirPagare"></asp:MenuItem>
				<asp:MenuItem Text="Contrato Completo" Value="ImprimirTodo"></asp:MenuItem>
			</asp:MenuItem>
			<asp:MenuItem Text="Cerrar" Value="Cerrar" Selectable="false">
				<asp:MenuItem Text="Cerrar Contrato" Value="CerrarContrato"></asp:MenuItem>
				<asp:MenuItem Text="Cancelar Contrato" Value="CancelarContrato"></asp:MenuItem>
			</asp:MenuItem>
			<asp:MenuItem Text="Eliminar Contrato" Value="EliminarContrato"></asp:MenuItem>
			<asp:MenuItem Text="Estatus" Value="Estatus" Enabled="False"></asp:MenuItem>
		</Items>
		<StaticItemTemplate>
			<asp:Label runat="server" ID="lblOpcion" Text='<%# Eval("Text") %>' CssClass='<%# (string) Eval("Value") == "Contrato" || (string) Eval("Value") == "Estatus" ? "Informacion" : string.Empty %>'></asp:Label>
			<asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "Contrato" || (string) Eval("Value") == "Estatus" %>'
				Style="width: 100px" CssClass="textBoxDisabled" Enabled="false"></asp:TextBox>
		</StaticItemTemplate>
		<LevelSubMenuStyles>
			<asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" />
		</LevelSubMenuStyles>
		<DynamicHoverStyle CssClass="itemSeleccionado" />
		<DynamicSelectedStyle CssClass="itemSeleccionado"></DynamicSelectedStyle>
		<StaticSelectedStyle CssClass="itemSeleccionado"></StaticSelectedStyle>
	</asp:Menu>
</div>
<div class="BarraNavegacionExtra">
	<input id="btnNuevoConsulta" type="button" value="Nueva Consulta" onclick="window.location='<%= Page.ResolveUrl("~/Contratos.Mantto.CM.UI/ConsultarContratoCMUI.aspx") %>'" />
	<input id="btnPlantillas" runat="server" type="button" value="Plantillas" onclick="DialogoDetallePlantillas();" />
</div>
<asp:HiddenField ID="hdnSubMenuSeleccionado" runat="server" />
<asp:HiddenField ID="hdnMenuSeleccionado" runat="server" />
<asp:HiddenField runat="server" ID="hdnContratoID" />
<div id="dvDialogPlantillas" style="display: none;" title="PLANTILLAS DEL CONTRATO">
	<uc:ucListadoPlantillasUI runat="server" ID="ucucListadoPlantillasUI" />
</div>
