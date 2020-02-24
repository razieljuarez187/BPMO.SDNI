<%@ Page Language="C#" MasterPageFile="~/Contenido/MasterPages/Reportes.Master" AutoEventWireup="true" CodeBehind="ReporteIngresoRentasUI.aspx.cs" Inherits="BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI.ReporteIngresoRentasUI" %>

<asp:Content ID="childHead" ContentPlaceHolderID="childHead" runat="server">
<script>
    function ctmvtxtFechaInicio_ClientValidate(sender, args) {
        args.IsValid = validarRangoFechas('#<%= this.txtFechaInicio.ClientID %>', '#<%= this.txtFechaInicio.ClientID %>');
    }

    function ctmvtxtFechaFin_ClientValidate(sender, args) {
        args.IsValid = validarRangoFechas('#<%= this.txtFechaFin.ClientID %>', '#<%= this.txtFechaFin.ClientID %>');
    }

    function validarRangoFechas(inputId1, inputId2) {
        var txtFechaInicio = $(inputId1).val().trim();
        var txtFechaFin = $(inputId2).val().trim();

        var val1 = $(inputId1).datepicker("getDate");
        var val2 = $(inputId2).datepicker("getDate");

        if (txtFechaInicio != "" && val1 == null) {
            return false;
        }

        if (txtFechaFin != "" && val2 == null) {
            return false;
        }

        if (val1 != null && val2 != null)
            return val1 <= val2;

        return true;
    }
</script>
</asp:Content>
<asp:Content ID="encabezadoLeyenda" ContentPlaceHolderID="encabezadoLeyenda" runat="server">
    <asp:Label ID="lblEncabezadoLeyenda" runat="server">REPORTES - Reporte de ingreso de rentas</asp:Label>
</asp:Content>
<asp:Content ID="navegacionSecundaria" ContentPlaceHolderID="navegacionSecundaria" runat="server">
</asp:Content>
<asp:Content ID="encabezadoFiltrosReporte" ContentPlaceHolderID="encabezadoFiltrosReporte" runat="server">
    Reporte de ingreso de rentas
</asp:Content>
<asp:Content ID="filtrosAdicionalesArriba" ContentPlaceHolderID="filtrosAdicionalesArriba" runat="server">
</asp:Content>
<asp:Content ID="filtrosAdicionalesAbajo" ContentPlaceHolderID="filtrosAdicionalesAbajo" runat="server">
<tr>
    <td class="tdCentradoVertical">
        <asp:Label ID="lblFechaInicioRequired" runat="server" Text="*" Visible="false"/><asp:Label ID="lblFechaInicio" runat="server" AssociatedControlID="" Text="Fecha Inicio"></asp:Label>
    </td>
    <td class="separadorCampo"></td>
    <td class="tdCentradoVertical">
        <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="CampoFecha"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvtxtFechaInicio" runat="server" ValidationGroup="Requeridos" 
            ErrorMessage="La fecha es un dato requerido" Text="*" Visible="false"
            ControlToValidate="txtFechaInicio" Display="Dynamic" SetFocusOnError="True" 
            ToolTip="La fecha es un dato requerido">
        </asp:RequiredFieldValidator>
        <asp:CustomValidator ID="ctmvtxtFechaInicio" runat="server" ValidationGroup="Requeridos" 
            ErrorMessage="Las fechas no son válidas o los rangos no son válidos" Text="*" 
            ControlToValidate="txtFechaInicio" ClientValidationFunction="ctmvtxtFechaInicio_ClientValidate"
            ToolTip="Las fechas no son válidas o los rangos no son válidos" 
            Display="Dynamic" SetFocusOnError="true" >
        </asp:CustomValidator>
        &nbsp;
    </td>
</tr>

<tr>
    <td class="tdCentradoVertical">
        <asp:Label ID="lblFechaFinRequired" runat="server" Text="*" Visible="false"/><asp:Label ID="lblFechaFin" runat="server" AssociatedControlID="txtFechaFin" Text="Fecha Fin"></asp:Label>
    </td>
    <td class="separadorCampo"></td>
    <td class="tdCentradoVertical">
        <asp:TextBox ID="txtFechaFin" runat="server" CssClass="CampoFecha"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvtxtFechaFin" runat="server" ValidationGroup="Requeridos" 
            ErrorMessage="La fecha es un dato requerido" Text="*" Visible="false" 
            ControlToValidate="txtFechaFin" Display="Dynamic" SetFocusOnError="True" 
            ToolTip="La fecha es un dato requerido">
        </asp:RequiredFieldValidator>

        <asp:CustomValidator ID="ctmvtxtFechaFin" runat="server" ValidationGroup="Requeridos" 
            ErrorMessage="Las fechas no son válidas o los rangos no son válidos" Text="*"
            ControlToValidate="txtFechaInicio" ClientValidationFunction="ctmvtxtFechaFin_ClientValidate"
            ToolTip="Las fechas no son válidas o los rangos no son válidos" 
            Display="Dynamic" SetFocusOnError="true" >
        </asp:CustomValidator>

        &nbsp;
    </td>
</tr>
</asp:Content>
<asp:Content ID="ResultadosAbajo" ContentPlaceHolderID="ResultadosAbajo" runat="server">

    <div class="ContenedorMensajes">
        <span class="Requeridos"></span>
        <br />
        <span class="FormatoIncorrecto"></span>
    </div>        
</asp:Content>
