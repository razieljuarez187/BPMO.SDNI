<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Reportes.Master" AutoEventWireup="true" CodeBehind="ComparativoMantenimientoUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.Reportes.UI.ComparativoMantenimientoUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="childHead" runat="server">
    <style type="text/css">
        .space {width: 20px; }
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
    <asp:Label ID="lblEncabezadoLeyenda" runat="server">REPORTES - REPORTE COMPARATIVO MANTENIMIENTOS</asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navegacionSecundaria" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="encabezadoFiltrosReporte" runat="server">
    REPORTE COMPARATIVO MANTENIMIENTOS
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="filtrosAdicionalesArriba" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="filtrosAdicionalesAbajo" runat="server">
        <tr>
            <td class="tdCentradoVertical"><span></span>VIN</td>
            <td class="space">&nbsp;</td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtVIN" runat="server" MaxLength="30" CssClass="input-find-responsive" AutoPostBack="True" OnTextChanged="txtVIN_TextChanged"></asp:TextBox>
                <asp:ImageButton runat="server" ID="btnBuscarVIN" CommandName="VerVIN"
                    ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar VIN" OnClick="btnBuscarVIN_Click"/>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical"><span>*</span>FECHA INICIO</td>
            <td class="space">&nbsp;</td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtFechaInicio" runat="server"
                    CausesValidation="True" ValidationGroup="FormatoValido" CssClass="CampoFecha input-date-responsive"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revFechaInicio" runat="server" ControlToValidate="txtFechaInicio"
                    Display="Dynamic" ErrorMessage="Formato inválido" ValidationExpression="(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d"
                    ValidationGroup="Obligatorios" CssClass="ColorValidator">**</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical"><span>*</span>FECHA FIN</td>
            <td class="space" >&nbsp;</td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtFechaFin" runat="server"
                    CausesValidation="True" ValidationGroup="FormatoValido" CssClass="CampoFecha input-date-responsive"></asp:TextBox>
                <asp:RegularExpressionValidator ID="reFechaFin" runat="server" ControlToValidate="txtFechaFin"
                    Display="Dynamic" ErrorMessage="Formato inválido" ValidationExpression="(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d"
                    ValidationGroup="Obligatorios" CssClass="ColorValidator">**</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="hdnLibroActivos" runat="server" />
                <asp:Button ID="btnResult2" runat="server" Text="Button" OnClick="btnResult2_Click" CausesValidation="false" UseSubmitBehavior="false" Style="display: none;" />
            </td>
        </tr>
</asp:Content>
