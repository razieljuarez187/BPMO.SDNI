<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Reportes.Master" AutoEventWireup="true" CodeBehind="RendimientoUnidadUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.Reportes.UI.RendimientoUnidadUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="childHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="encabezadoLeyenda" runat="server">
    <asp:Label ID="lblEncabezadoLeyenda" runat="server">REPORTES - REPORTE DE RENDIMIENTO POR UNIDAD</asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navegacionSecundaria" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="encabezadoFiltrosReporte" runat="server">
    <label>¿QUÉ REPORTE DE RENDIMIENTO POR UNIDAD DESEA CONSULTAR?</label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="filtrosAdicionalesArriba" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="filtrosAdicionalesAbajo" runat="server">
    <tr id="trMesFinal" runat="server">
        <td class="tdCentradoVertical">
            <asp:Label ID="lblMesFinalRequired" runat="server" Text="*" Visible="false"/><asp:Label ID="lblMesFinal" runat="server" AssociatedControlID="ddlMesFinal" Text="Mes Final"></asp:Label>
        </td>
        <td class="separadorCampo"></td>
        <td class="tdCentradoVertical">
            <asp:DropDownList ID="ddlMesFinal" DataValueField="Value" DataTextField="Text" runat="server">
            </asp:DropDownList>               

            <asp:RequiredFieldValidator ID="rfvtxtMesFinal" runat="server" ValidationGroup="GenerarReporte" 
                ErrorMessage="El mes final es un dato requerido" Text="*" Visible="false"
                ControlToValidate="ddlMesFinal" Display="Dynamic" SetFocusOnError="True" 
                ToolTip="El mes final es un dato requerido">
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr id="trReporteGlobal" runat="server">
        <td class="tdCentradoVertical">
            <asp:Label ID="lblReporteGlobalRequired" runat="server" Text="*" Visible="false"/><asp:Label ID="lblReporteGlobal" runat="server" AssociatedControlID="ddlReporteGlobal" Text="Tipo Reporte"></asp:Label>
        </td>
        <td class="separadorCampo"></td>
        <td class="tdCentradoVertical">
            <asp:DropDownList ID="ddlReporteGlobal" DataValueField="Value" DataTextField="Text" runat="server">
            </asp:DropDownList>               

            <asp:RequiredFieldValidator ID="rfvReporteGlobal" runat="server" ValidationGroup="GenerarReporte" 
                ErrorMessage="El Tipo de Reporte es un dato requerido" Text="*" Visible="false"
                ControlToValidate="ddlReporteGlobal" Display="Dynamic" SetFocusOnError="True" 
                ToolTip="El Tipo de Reporte es un dato requerido">
            </asp:RequiredFieldValidator>
        </td>
    </tr>
</asp:Content>
