<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Reportes.Master" AutoEventWireup="true" CodeBehind="MantenimientoRealizadoContraProgramadoUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.Reportes.UI.MantenimientoRealizadoContraProgramadoUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="childHead" runat="server">
    <style type="text/css">
    .Grid { width: 90%; margin: 25px auto 15px auto; }
</style>
<script type="text/javascript">
    function BtnBuscar2(guid, xml) {
        var width = ObtenerAnchoBuscador(xml);

        $.BuscadorWeb({
            xml: xml,
            guid: guid,
            btnSender: $("#<%=btnResult2.ClientID %>"),
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
    <asp:Label ID="lblEncabezadoLeyenda" runat="server">REPORTES - Reporte de Mantenimiento Realizado vs Programado</asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navegacionSecundaria" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="encabezadoFiltrosReporte" runat="server">
    ¿Qué mantenimiento realizado contra programado desea consultar?
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="filtrosAdicionalesArriba" runat="server">

</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="filtrosAdicionalesAbajo" runat="server">
    <tr>
        <td class="tdCentradoVertical"><span>*</span>MES FIN</td>
        <td class="separadorCampo"></td>
        <td class="tdCentradoVertical">
            <asp:DropDownList ID="ddlMesFin" DataValueField="Value" DataTextField="Text" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            VIN
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 320px;">
            <asp:TextBox ID="txtVin" runat="server" MaxLength="30" AutoPostBack="True"></asp:TextBox>
            <asp:ImageButton runat="server" ID="btnBuscarVin" CommandName="VerVin" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                ToolTip="Consultar VIN" CommandArgument='' OnClick="btnBuscarVin_Click" />
        </td>
    </tr>
    <tr>
        <asp:Button ID="btnResult2" runat="server" Text="Button" OnClick="btnResult_Click2" Style="display: none;" />
        <td><asp:HiddenField ID="hdnLibroActivos" runat="server" /></td>
    </tr>

    <tr>
        <asp:UpdatePanel ID="UPContenedor" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gvMantenimientos" runat="server" AutoGenerateColumns="false" PageSize="10" AllowPaging="true" AllowSorting="false"
                EnableSortingAndPagingCallbacks="true" CssClass="Grid">
                    <Columns>

                    </Columns>
                    
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </tr>

</asp:Content>
