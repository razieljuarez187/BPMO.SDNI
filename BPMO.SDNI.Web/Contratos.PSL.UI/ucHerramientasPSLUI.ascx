<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHerramientasPSLUI.ascx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.ucHerramientasPSLUI" %>

<%@ Register TagPrefix="uc" TagName="ucListadoPlantillasUI" Src="~/Contratos.UI/ucListadoPlantillasUI.ascx" %>

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
    <asp:Menu runat="server" ID="mnContratos" IncludeStyleBlock="False"
        Orientation="Horizontal" CssClass="MenuPrimario" 
        onmenuitemclick="mnContratos_MenuItemClick">
        <Items>
            <asp:MenuItem Text="# Contrato" Value="Contrato" Enabled="False" Selectable="False"></asp:MenuItem>
            <asp:MenuItem Text="Editar" Value="EditarContrato" Selectable="false">
                <asp:MenuItem Text="Editar Contrato" Value="EditarContratoRO"></asp:MenuItem>
                <asp:MenuItem Text="Agregar Documentos" Value="AgregarDocumentos"></asp:MenuItem>
                <asp:MenuItem Text="Renovar Contrato" Value="RenovarContrato"></asp:MenuItem>
                <asp:MenuItem Text="Generar Solicitud Pago" Value="GenerarSolicitudPago"></asp:MenuItem>
                <asp:MenuItem Text="Intercambio de unidad" Value="IntercambioUnidadContrato"></asp:MenuItem>
            </asp:MenuItem>
            <asp:MenuItem Text="Impresión" Value="Impresion" Selectable="False">
                <asp:MenuItem Text="Contrato" Value="ImpContratoRO"></asp:MenuItem>
                <asp:MenuItem Text="Pagaré" Value="ImpPagareRO"></asp:MenuItem>
            </asp:MenuItem>
            <asp:MenuItem Text="Cerrar Contrato" Value="CerrarContrato"></asp:MenuItem>
            <asp:MenuItem Text="Eliminar Contrato" Value="EliminarContrato" Enabled="false"></asp:MenuItem>
            <asp:MenuItem Text="Plantilla Check List" Value="PlantillaCheckList"></asp:MenuItem>
            <asp:MenuItem Text="Estatus" Value="Estatus" Enabled="False"></asp:MenuItem>
        </Items>
        <StaticItemTemplate>
            <asp:Label runat="server" ID="lblOpcion" Text='<%# Eval("Text") %>' CssClass='<%# (string) Eval("Value") == "Contrato" || (string) Eval("Value") == "Estatus" ? "Informacion" : string.Empty %>' ></asp:Label>
            <asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "Contrato" || (string) Eval("Value") == "Estatus" %>' Style="width:100px;font-size:0.9em;" CssClass="textBoxDisabled" Enabled="false"></asp:TextBox>
        </StaticItemTemplate>
        <LevelSubMenuStyles><asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" /> </LevelSubMenuStyles>
        <DynamicHoverStyle CssClass="itemSeleccionado"/>
        <DynamicSelectedStyle CssClass="itemSeleccionado"></DynamicSelectedStyle>
        <StaticSelectedStyle CssClass="itemSeleccionado"></StaticSelectedStyle>
    </asp:Menu>
</div>
<div class="BarraNavegacionExtra"> 
    <input id="btnPlantillas" runat="server" type="button" value="Plantillas" onclick="DialogoDetallePlantillas();" />
</div>
<asp:HiddenField ID="hdnEstatusContrato" runat="server" />
<asp:HiddenField ID="hdnSubMenuSeleccionado" runat="server" />
<asp:HiddenField ID="hdnMenuSeleccionado" runat="server" />
<asp:HiddenField runat="server" ID="hdnContratoID"/>
<div id="dvDialogPlantillas" style="display: none;" title="PLANTILLAS DEL CONTRATO">
    <uc:ucListadoPlantillasUI runat="server" ID="ucucListadoPlantillasUI" />
</div>
