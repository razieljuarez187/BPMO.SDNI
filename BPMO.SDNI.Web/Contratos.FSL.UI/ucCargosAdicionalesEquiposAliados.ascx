<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCargosAdicionalesEquiposAliados.ascx.cs"
    Inherits="BPMO.SDNI.Contratos.FSL.UI.ucCargosAdicionalesEquiposAliados" %>
    <%-- Satisface al caso de uso CU015 - Registrar Contrato Full Service Leasing --%>
    <%-- Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing --%>

<script type="text/javascript">
    // Marca el check de la tarifa si se capturó
    function ValidarCapturaTarifa() {
        if (valorRetornado !== undefined && valorRetornado !== null && valorRetornado.length > 0)
            $('#' + valorRetornado).attr('checked', true);
    }
</script>

<div>
    <asp:ListView ID="lvwEquiposAliados" runat="server" OnItemDataBound="lvwEquiposAliados_ItemDataBound">
        <LayoutTemplate>
            <br />
            <fieldset style="border: 0;">
                <legend>Tarifas de Equipos Aliados</legend>
            </fieldset>
            <table class="Grid" width="100%" style="border-collapse: collapse;" cellpadding="4" cellspacing="0">
                <tr class="GridHeader">
                    <th style="width: 10%;" align="center" class="tdCentradoVertical">
                        <asp:Label runat="server" ID="lblNumeroSerie" Text="# VIN" />
                    </th>
                    <th style="width: 10%;" align="center" class="tdCentradoVertical">
                        <asp:Label runat="server" ID="lblModelo" Text="Modelo" />
                    </th>
                    <td style="width: 10%;">
                    </td>
                    <th style="width: 10%;">
                    </th>
                </tr>
                <asp:Panel ID="itemPlaceholder" runat="server">
                </asp:Panel>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td align="center">
                    <asp:Label runat="server" ID="lblVINDato" />
                </td>
                <td class="tdCentradoVertical">
                    <asp:Label runat="server" ID="lblModeloDato" />
                </td>
                <td class="tdCentradoVertical">
                    <asp:CheckBox ID="cbCapturado" runat="server" Text="Tarifas Capturadas" Enabled="False" />
                </td>
                <td class="tdCentradoVertical">
                    <asp:Button ID="btnCapturarTarifas" runat="server" Text="Tarifas"
                        CssClass="btnWizardEditar" />
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
    <asp:HiddenField ID="hdnPlazo" runat="server" />
    <asp:HiddenField ID="hdnTipoCotizacion" runat="server" />
    <asp:HiddenField ID="hdnUnidadID" runat="server" />
    <asp:HiddenField ID="hdnUnidadOperativaID" runat="server" />
    <asp:HiddenField runat="server" ID="hdnModoConsultar"/>
    <asp:HiddenField runat="server" ID="hdnTituloBotones"/>
    <asp:HiddenField ID="hdnCheckAliado" runat="server" />
    <asp:HiddenField ID="hdnShowDialogModal" runat="server" value="0"/>
</div>
