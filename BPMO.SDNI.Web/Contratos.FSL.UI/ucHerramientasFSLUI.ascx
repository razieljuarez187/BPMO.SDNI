<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHerramientasFSLUI.ascx.cs"
    Inherits="BPMO.SDNI.Contratos.FSL.UI.ucHerramientasFSLUI" %>
    <%@ Register TagPrefix="uc" TagName="ucListadoPlantillasUI" Src="~/Contratos.UI/ucListadoPlantillasUI.ascx" %>
<%-- 
    Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
    Satisface al caso de uso CU023 - Editar Contrato Full Service Leasing 
	Satisface al caso de uso CU093 - Imprimir Pagaré Contrato FSL 
 --%>
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
            <asp:MenuItem Text="# Contrato" Value="Contrato" Enabled="False"></asp:MenuItem>
            <asp:MenuItem Text="Editar" Value="EditarContrato" Selectable="false">
                <asp:MenuItem Text="Editar Contrato" Value="EditarContratoFSL"></asp:MenuItem>
                <asp:MenuItem Text="Modificar Unidades" Value="ModificarUnidadesContratoFSL"></asp:MenuItem>
                <asp:MenuItem Text="Agregar Documentos" Value="AgregarDocumentos"></asp:MenuItem>
            </asp:MenuItem>
            <asp:MenuItem Text="Impresión" Value="Impresion" Selectable="False">
                <asp:MenuItem Text="Contrato Maestro" Value="ContratoMaestro"></asp:MenuItem>
                <asp:MenuItem Text="Constancia" Value="ConstanciaBienes"></asp:MenuItem>
                <asp:MenuItem Text="Manual Operaciones" Value="ManualOperaciones">
                </asp:MenuItem>
                <asp:MenuItem Text="Anexo A" Value="AnexoA"></asp:MenuItem>
                <asp:MenuItem Text="Anexo B: Mantenimientos Preventivos" Value="AnexoB">
                </asp:MenuItem>
                <asp:MenuItem Text="Anexo C: Acta de nacimiento" Value="AnexoC"></asp:MenuItem>
                <%--<asp:MenuItem Text="Anexo de Bienes" Value="AnexoBienes"></asp:MenuItem>--%>
                <asp:MenuItem Text="Anexos" Value="Anexos"></asp:MenuItem>
				<asp:MenuItem Text="Pagaré" Value="ImprimirPagare"></asp:MenuItem>
            </asp:MenuItem>
            <asp:MenuItem Text="Cerrar Contrato" Value="CerrarContrato"></asp:MenuItem>
            <asp:MenuItem Text="Eliminar Contrato" Value="EliminarContrato"></asp:MenuItem>
            <asp:MenuItem Text="Estatus" Value="Estatus" Enabled="False"></asp:MenuItem>
            <asp:MenuItem Text="Plantilla Contrato" Value="FormatoContrato" Selectable="False">
                <asp:MenuItem Text="Persona Física" Value="PersonaFisica"></asp:MenuItem>
                <asp:MenuItem Text="Persona Moral" Value="PersonaMoral"></asp:MenuItem>
            </asp:MenuItem>
        </Items>
        <StaticItemTemplate>
            <asp:Label runat="server" ID="lblOpcion" Text='<%# Eval("Text") %>' CssClass='<%# ((string)Eval("Value") == "Contrato" || (string)Eval("Value") == "Estatus")? "Informacion" : string.Empty %>' ReadOnly="True" ></asp:Label>
            <asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "Contrato" || (string) Eval("Value") == "Estatus" %>' Style="width: 100px" CssClass="textBoxDisabled" ReadOnly="True"></asp:TextBox>
        </StaticItemTemplate>
        <LevelSubMenuStyles><asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" /> </LevelSubMenuStyles>
        <DynamicHoverStyle CssClass="itemSeleccionado"/>
        <DynamicSelectedStyle CssClass="itemSeleccionado"></DynamicSelectedStyle>
        <StaticSelectedStyle CssClass="itemSeleccionado"></StaticSelectedStyle>
    </asp:Menu>
    <div class="Ayuda" style="float: right">        
        <input id="Button1" type="button" class="btnAyuda" onclick="ShowHelp();" />
        
    </div>
</div>
<!---------->
<div class="BarraNavegacionExtra">
    <input id="btnNuevoConsulta" type="button" value="Nueva Consulta" onclick="window.location='<%= Page.ResolveUrl("~/Contratos.FSL.UI/ConsultarContratosFSLUI.aspx") %>'" />
    <input id="btnPlantillas" runat="server" type="button" value="Plantillas"  onclick="DialogoDetallePlantillas();" />
</div>
<asp:HiddenField ID="hdnSubMenuSeleccionado" runat="server" />
<asp:HiddenField ID="hdnMenuSeleccionado" runat="server" />
<asp:HiddenField runat="server" ID="hdnContratoID"/>
<div id="dvDialogPlantillas" style="display: none;" title="PLANTILLAS DEL CONTRATO">
    <uc:ucListadoPlantillasUI runat="server" ID="ucucListadoPlantillasUI" />
</div>
