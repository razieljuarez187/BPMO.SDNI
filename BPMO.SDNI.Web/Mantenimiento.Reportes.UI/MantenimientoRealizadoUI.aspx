<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Reportes.Master" AutoEventWireup="true" CodeBehind="MantenimientoRealizadoUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.Reportes.UI.MantenimientoRealizadoUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="childHead" runat="server">
   <style type="text/css">
       
        .input-find-responsive { min-width: 100px !important;}
        .input-dropdown-responsive { min-width: 156px !important;}
       
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
 <asp:Label ID="lblEncabezadoLeyenda" runat="server">REPORTES - Reporte de Mantenimiento Realizado</asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navegacionSecundaria" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="encabezadoFiltrosReporte" runat="server">
   ¿Qué mantenimiento realizado desea consultar?
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="filtrosAdicionalesArriba" runat="server">
<tr>
                        <td class="tdCentradoVertical input-label-responsive">SUCURSAL</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtSucursal" runat="server" MaxLength="30" CssClass="input-text-responsive"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="btnBuscarSucursal" CommandName="VerSucursal"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument='' OnClick="btnBuscarSucursal_Click" />
                            <asp:HiddenField ID="hdnSucursalID" runat="server" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>TALLER</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:DropDownList ID="ddTalleres" runat="server" 
    CssClass="input-dropdown-responsive" Enabled="False"></asp:DropDownList>
                        </td>
                    </tr>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="filtrosAdicionalesAbajo" runat="server">
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
                <asp:HiddenField ID="hdnUnidadID" runat="server" />
        </td>
    </tr>
    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>FECHA INICIO</td>
                        <td class="input-space-responsive" >&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtFechaInicio" runat="server"
                                CausesValidation="True" ValidationGroup="FormatoValido" CssClass="CampoFecha input-date-responsive"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revFechaInicio" runat="server" ControlToValidate="txtFechaInicio"
                                Display="Dynamic" ErrorMessage="Formato inválido" ValidationExpression="(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d"
                                ValidationGroup="Obligatorios" CssClass="ColorValidator">**</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>FECHA FIN</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtFechaFin" runat="server"
                                CausesValidation="True" ValidationGroup="FormatoValido" CssClass="CampoFecha input-date-responsive"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revFechaFin" runat="server" ControlToValidate="txtFechaFin"
                                Display="Dynamic" ErrorMessage="Formato inválido" ValidationExpression="(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d"
                                ValidationGroup="Obligatorios" CssClass="ColorValidator">**</asp:RegularExpressionValidator>
                        </td>
                    </tr>
    <tr>
        <asp:Button ID="btnResult2" runat="server" Text="Button" OnClick="btnResult2_Click" Style="display: none;" />
        <td><asp:HiddenField ID="hdnLibroActivos" runat="server" /></td>
    </tr>

</asp:Content>
