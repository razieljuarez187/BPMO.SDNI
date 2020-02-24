<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Reportes.Master"
    AutoEventWireup="true" CodeBehind="AniejamientoFlotaUI.aspx.cs" Inherits="BPMO.SDNI.Flota.Reportes.UI.AniejamientoFlotaUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="childHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="encabezadoLeyenda" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navegacionSecundaria" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="encabezadoFiltrosReporte" runat="server">
    <label>¿Qúe Reporte de Añejamiento de Flota desea consultar?</label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="filtrosAdicionalesArriba" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="filtrosAdicionalesAbajo" runat="server">
    <tr id="trAreaDeparmaneto" runat="server">
        <td class="tdCentradoVertical">
            <span>*</span><asp:Label ID="lblDepartamento" runat="server" AssociatedControlID="ddlDepartamento" Text="Área/Depto"></asp:Label>
        </td>
        <td class="separadorCampo">
        </td>
        <td class="tdCentradoVertical">
            <asp:DropDownList ID="ddlDepartamento" DataValueField="Value" DataTextField="Text"
                runat="server">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvddlDepartamento" runat="server" ValidationGroup="GenerarReporte"
                ErrorMessage="El área/Departamento es un dato requerido" Text="*" Visible="false"
                ControlToValidate="ddlDepartamento" Display="Dynamic" SetFocusOnError="True"
                ToolTip="El área/Departamento es un dato requerido" InitialValue="-1"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            <asp:Label runat="server" ID="lblReporteDetallado" Text="Reporte Detallado"></asp:Label>
        </td>
        <td class="separadorCampo">
        </td>
        <td class="tdCentradoVertical">
            <asp:DropDownList runat="server" ID="ddlReporteDetallado" DataValueField="Value" DataTextField="Text"/>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            <label>Activo Fijo</label>
        </td>
        <td class="separadorCampo">
        </td>
        <td class="tdCentradoVertical">
            <asp:DropDownList runat="server" ID="ddlTipoUnidad" DataValueField="Value" DataTextField="Text"/>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            <label>Etiqueta Reporte</label>
        </td>
        <td class="separadorCampo">
        </td>
        <td class="tdCentradoVertical">
            <asp:TextBox runat="server" ID="txtEtiquetaReporte" MaxLength="100" Width="200px"></asp:TextBox>
        </td>
    </tr>
</asp:Content>
