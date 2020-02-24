<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucInformacionGeneralUI.ascx.cs"
    Inherits="BPMO.SDNI.Contratos.FSL.UI.ucInformacionGeneralUI" %>
<%-- 
    Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
    Satisface al caso de uso CU023 - Editar Contrato Full Service Leasing 
    Satisface al caso de uso CU015 - Registrar Contrato Full Service Leasing 
--%>
<script type="text/javascript">
    // Inicializa el control de Información General
    function <%= ClientID %>_Inicializar() {
        var FechaContrato = $('#<%= txtFechaContrato.ClientID %>');
        if (FechaContrato.length > 0) {
            FechaContrato.datepicker({
                yearRange: '-100:+10',
                changeYear: true,
                changeMonth: true,
                showButtonPanel: true,
                dateFormat: "dd/mm/yy",
                buttonImage: '../Contenido/Imagenes/calendar.gif',
                buttonImageOnly: true,
                toolTipText: "Fecha del Contrato",
                showOn: 'button',
                defaultDate: (FechaContrato.val().length == 10) ? FechaContrato.val() : new Date()
            });

            FechaContrato.attr('readonly', true);
        }
    }
</script>
<fieldset>
    <legend>Información General</legend>
    <div class="dvIzquierda">
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical">
                    <span>*</span>Fecha Contrato
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtFechaContrato" runat="server" CssClass="CampoFecha"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">
                    Empresa
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtEmpresa" runat="server" Width="250px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-top: 5px">
                    Domicilio Empresa
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtDireccionEmpresa" runat="server" Rows="5" Columns="30" TextMode="MultiLine"
                                MaxLength="500" Style="float: left; max-width: 250px; min-width: 250px;
                                max-height: 90px; min-height: 90px;"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="dvDerecha">
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical">
                    <span>*</span>Sucursal
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical">
                    <asp:DropDownList ID="ddlSucursales" runat="server" Width="300px">
                        <asp:ListItem Text="Seleccione una opción" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">
                    <span>*</span>Representante
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtRepresentante" runat="server" MaxLength="250" Width="290px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">
                    <span>*</span>Moneda
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical">
                    <asp:DropDownList ID="ddlMonedas" runat="server" Width="205px">
                        <asp:ListItem Text="Seleccione una opción" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
</fieldset>
<asp:HiddenField runat="server" ID="hdnEmpresaID"/>
<asp:HiddenField runat="server" ID="hdnPorcentajePenalizacion" />