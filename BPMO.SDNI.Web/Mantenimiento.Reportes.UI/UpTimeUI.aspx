<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Reportes.Master" AutoEventWireup="true" CodeBehind="UpTimeUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.Reportes.UI.UpTimeUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="childHead" runat="server">
<script type="text/javascript">
    function BtnBuscarUnidad(guid, xml, sender) {
        var width = ObtenerAnchoBuscador(xml);

        $.BuscadorWeb({
            xml: xml,
            guid: guid,
            btnSender: $("#" + sender),
            features: {
                dialogWidth: width,
                dialogHeight: '320px',
                center: 'yes',
                maximize: '0',
                minimize: 'no'
            }
        });
    }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="encabezadoLeyenda" runat="server">
    <asp:Label ID="lblEncabezadoLeyenda" runat="server">REPORTES - Reporte de Up Time</asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navegacionSecundaria" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="encabezadoFiltrosReporte" runat="server">
    ¿Qué reporte de Up Time deseas consultar?
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="filtrosAdicionalesArriba" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="filtrosAdicionalesAbajo" runat="server">
    <%--<tr>
        <td class="tdCentradoVertical">MES FINAL</td>
        <td style="width: 20px;">
                &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 320px;">
            <asp:DropDownList ID="ddlMes" DataValueField="Value" DataTextField="Text" runat="server">
            </asp:DropDownList>
        </td>
    </tr>--%>
    <tr>
        <td class="tdCentradoVertical">
            VIN
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 320px;">
            <asp:TextBox ID="txtNumVin" runat="server" MaxLength="30" Width="275px" AutoPostBack="True"></asp:TextBox>
            <asp:ImageButton runat="server" ID="btnBuscarVin" CommandName="VerVin" ImageUrl="~/Contenido/Imagenes/Detalle.png"
            ToolTip="Consultar VIN" CommandArgument='' OnClick="btnBuscarVin_Click" />
        </td>
    </tr>
    <tr>
        <td><asp:HiddenField ID="hdnLibroActivos" runat="server" /></td>
    </tr>
    
    <asp:Button ID="btnResult2" runat="server" Text="Button" OnClick="btnResult2_Click" Style="display: none;" />
</asp:Content>
