<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTramiteVerificacionUI.ascx.cs" Inherits="BPMO.SDNI.Tramites.UI.ucTramiteVerificacionUI" %>
<%@ Register src="../Comun.UI/ucCatalogoDocumentosUI.ascx" tagname="ucCatalogoDocumentosUI" tagprefix="uc1" %>
<script type="text/javascript" >
    function <%= ClientID%>_initPage() {
        if (!$("#<%=txtFechaFinal.ClientID %>").is(':disabled') && !$("#<%=txtFechaInicio.ClientID %>").is(':disabled')) {
            var dates = $("#<%=txtFechaInicio.ClientID %>, #<%=txtFechaFinal.ClientID %>").datepicker({
                        changeMonth: true,
                        changeYear: true,
                        showOn: "both",
                        buttonImage: "../Contenido/Imagenes/calendar.gif",
                        defaultDate: "+1w",
                        buttonImageOnly: true,
                        maxDate: "",
                        onSelect: function (selectedDate) {
                         var option = this.id == "<%=txtFechaInicio.ClientID%>" ? "minDate" : "maxDate",
                          instance = $(this).data("datepicker"),
                          date = $.datepicker.parseDate(
                           instance.settings.dateFormat ||
                           $.datepicker._defaults.dateFormat,
                           selectedDate, instance.settings);
                         dates.not(this).datepicker("option", option, date);
                            }
                        });
        }
        $("#<%=txtFechaFinal.ClientID %>").attr('readonly',true);
        $("#<%=txtFechaInicio.ClientID %>").attr('readonly',true);

    }
</script>
<table class="trAlinearDerecha">
    <tr>
        <td class="tdCentradoVertical"><span>*</span>Folio</td>
        <td class="tdCentradoVertical">
            <asp:TextBox runat = "server" ID="txtFolio" MaxLength="150"></asp:TextBox>
        </td>
        <td class="tdCentradoVertical" align="right"><span>*</span>Fecha Inicial</td>
        <td class="tdCentradoVertical">
            <asp:TextBox runat="server" ID="txtFechaInicio" CssClass="CampoFecha"></asp:TextBox>            
        </td>
        <td class="tdCentradoVertical" align="right"><span>*</span>Fecha Final</td>
        <td class="tdCentradoVertical">
            <asp:TextBox runat="server" ID="txtFechaFinal" CssClass="CampoFecha"></asp:TextBox>            
        </td>
    </tr>
</table>
<uc1:ucCatalogoDocumentosUI ID="ucCatalogoDocumentosVerificacion" runat="server" />
<asp:HiddenField runat="server" ID="hdnTipoVerificacion" />
