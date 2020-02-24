<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTramiteFiltroAKUI.ascx.cs" Inherits="BPMO.SDNI.Tramites.UI.ucTramiteFiltroAKUI" %>

<script type="text/javascript">
    function <%=ClientID %>_initPage() {

        if (!$("#<%=txtFechaInstalacion.ClientID %>").is(':disabled')) {
            $("#<%=txtFechaInstalacion.ClientID %>").datepicker({
                changeMonth: true,
                changeYear: true,
                showOn: "both",
                buttonImage: "../Contenido/Imagenes/calendar.gif",
                buttonImageOnly: true
            });
        }
        $("#<%=txtFechaInstalacion.ClientID %>").attr('readonly',true);
    }
</script>

<table class="trAlinearDerecha">
    <tr>
        <td class="tdCentradoVertical"><span>*</span># Serie</td>
        <td class="tdCentradoVertical">
            <asp:TextBox runat="server" ID="txtNumeroSerie" MaxLength="50"></asp:TextBox>
        </td >
        <td class="tdCentradoVertical" align="right"><span>*</span>Fecha Instalaci&oacute;n</td>
        <td class="tdCentradoVertical">
            <asp:TextBox runat="server" ID="txtFechaInstalacion" CssClass="CampoFecha"></asp:TextBox>
            
        </td>
    </tr>
</table>