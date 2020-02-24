<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTramitePlacalUI.ascx.cs" Inherits="BPMO.SDNI.Tramites.UI.ucTramitePlacaUI" %>
<script type="text/javascript">
function <%=ClientID %>_initPage() {
    if (!$("#<%=txtFechaEnvio.ClientID %>").is(':disabled') && !$("#<%=txtFechaRecepcion.ClientID %>").is(':disabled')) {
        var dates = $("#<%=txtFechaEnvio.ClientID %>, #<%=txtFechaRecepcion.ClientID %>").datepicker({
                        changeMonth: true,
                        changeYear: true,
                        showOn: "both",
                        buttonImage: "../Contenido/Imagenes/calendar.gif",
                        defaultDate: "+1w",
                        buttonImageOnly: true,
                        maxDate: "",
                        onSelect: function (selectedDate) {
                         var option = this.id == "<%=txtFechaEnvio.ClientID%>" ? "minDate" : "maxDate",
                          instance = $(this).data("datepicker"),
                          date = $.datepicker.parseDate(
                           instance.settings.dateFormat ||
                           $.datepicker._defaults.dateFormat,
                           selectedDate, instance.settings);
                         dates.not(this).datepicker("option", option, date);
                            }
                        });
    }
    $("#<%=txtFechaRecepcion.ClientID %>").attr('readonly', true);
    $("#<%=txtFechaEnvio.ClientID %>").attr('readonly',true);
}

</script>

<div runat="server" ID="divPlacaEstatal">
    <table class="trAlinearDerecha">
        <tr>
            <td class="tdCentradoVertical"><span>*</span>#</td>
            <td class="tdCentradoVertical"><asp:TextBox runat="server" ID="txtNumeroPlacaEstatal" MaxLength="20"></asp:TextBox></td>            
        </tr>
    </table>
</div>
<div runat ="server" ID="divPlacaFederal">
    <table class="trAlinearDerecha">
        <tr>
            <td class="tdCentradoVertical"><span>*</span>#</td>
            <td class="tdCentradoVertical">
                <asp:TextBox runat="server" ID="txtNumeroPlacaFederal" MaxLength="20"></asp:TextBox>
            </td>
            <td class="tdCentradoVertical" align="right"><span>*</span># Gu&iacute;a</td>
            <td>
                <asp:TextBox runat="server" ID="txtNumeroGuia" MaxLength="150"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical"><span>*</span>Fecha Env&iacute;o Documentos</td>
            <td class="tdCentradoVertical">
                <asp:TextBox runat="server" ID="txtFechaEnvio" Columns="20" CssClass="CampoFecha"></asp:TextBox>
                
            </td>
            <td class="tdCentradoVertical" align="right"><span>*</span>Fecha Recepci&oacute;n</td>
            <td class="tdCentradoVertical">
                <asp:TextBox runat="server" ID="txtFechaRecepcion" Columns="20" CssClass="CampoFecha"></asp:TextBox>                
            </td>
        </tr>		
        <tr id="trActivo" runat="server">
            <td class="tdCentradoVertical">Activo</td>
            <td colspan="3">
                <asp:CheckBox ID="chkActivo" runat="server" Checked="true" />                
            </td>            
        </tr>		
    </table>
</div>
<asp:HiddenField runat="server" ID="hdnTipoPlaca" />