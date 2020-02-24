<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTramiteTenenciaUI.ascx.cs" Inherits="BPMO.SDNI.Tramites.UI.ucTramiteTenenciaUI" %>
<%@ Register src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" tagname="ucCatalogoDocumentosUI" tagprefix="uc1" %>

<script type="text/javascript">
    function <%= ClientID %>_initPage() {
        if (!$("#<%=txtFechaPago.ClientID %>").is(':disabled')) {
            $("#<%=txtFechaPago.ClientID %>").datepicker({
                changeMonth: true,
                changeYear: true,
                showOn: "both",
                buttonImage: "../Contenido/Imagenes/calendar.gif",
                buttonImageOnly: true
            });
        }
        $("#<%=txtFechaPago.ClientID %>").attr('readonly', true);
    }
    </script>  
<table class="trAlinearDerecha">
    <tr>
        <td class="tdCentradoVertical"><span>*</span>Importe</td>
        <td class="tdCentradoVertical"><asp:TextBox runat ="server" ID="txtImporte" CssClass="CampoMoneda" MaxLength="13"></asp:TextBox></td>
        <td class="tdCentradoVertical" align="right"><span>*</span>Fecha de Pago</td>
        <td class="tdCentradoVertical">
        <asp:TextBox ID="txtFechaPago" runat="server" MaxLength="30" Width="95px" CssClass="CampoFecha"></asp:TextBox>
        </td>
        <td class="tdCentradoVertical" align="right"><span>*</span>Folio</td>
        <td class="tdCentradoVertical"><asp:TextBox runat="server" ID="txtFolio" MaxLength="150"></asp:TextBox></td>
    </tr>
</table>
<uc1:ucCatalogoDocumentosUI ID="ucCatalogoDocumentosTenencia" runat="server" />

