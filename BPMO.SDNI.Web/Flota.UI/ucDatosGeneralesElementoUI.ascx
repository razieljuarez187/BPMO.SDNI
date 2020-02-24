<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDatosGeneralesElementoUI.ascx.cs" Inherits="BPMO.SDNI.Flota.UI.ucDatosGeneralesElementoUI" %>
<style type="text/css">
    .MensajeInfo {
        color: #fc0404;
        font-size: 11px;     
    }
</style>
<table class="trAlinearDerecha" style="border: 1px solid transparent;">
    <tr>
        <td class="tdCentradoVertical" style="width: 200px"><asp:Label ID="lblNumeroEconomico" runat="server"># Econ&oacute;mico</asp:Label></td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtNumeroEconomico" runat="server" Width="275px"></asp:TextBox>
        </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical"><asp:Label ID="lblNumeroSerie" runat="server"># de Serie</asp:Label></td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtNumeroSerie" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>    
    <tr>
        <td class="tdCentradoVertical"><asp:Label ID="lblPlacasFederales" runat="server">Placas Federales</asp:Label> </td> 
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtPlacasFederales" runat="server" Width="110px"></asp:TextBox>
        </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical">Placas Estatales</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtPlacasEstales" runat="server" Width="110px"></asp:TextBox>
        </td>
    </tr>    
    <tr>
        <td class="tdCentradoVertical">Marca</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtMarca" runat="server" Width="275px"></asp:TextBox>
        </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical">Modelo</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtModelo" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>        
    <tr>
        <td class="tdCentradoVertical">Capacidad Tanque</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtCapacidadTanque" runat="server" Width="175px" ToolTip="Capacidad del Tanque de Combustible KM" CssClass="CampoNumero"></asp:TextBox>
        </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical"><asp:Label ID="lblRendimientoTanque" runat="server"><span>**</span>Rendimiento Tanque</asp:Label></td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtRendimientoTanque" runat="server" Width="175px" ToolTip="Rendimiento del Tanque de Combustible KM" CssClass="CampoNumero"></asp:TextBox>
        </td>
    </tr>  
    <tr>
        <td class="tdCentradoVertical"><asp:Label ID="lblPBC" runat="server">PBC</asp:Label></td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtCapacidadCarga" runat="server" Width="175px" ToolTip="Capacidad de Carga (PBC)" CssClass="CampoNumero"></asp:TextBox>
        </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical">Sucursal</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtSucursal" runat="server" Width="180px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">A&ntilde;o</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtAnio" runat="server" Width="40px"></asp:TextBox>
        </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical"><asp:Label ID="lblClaveProductoServicio" 
                runat="server" Text="Producto Servicio" Visible="False"></asp:Label></td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtClaveProductoServicio" runat="server" Columns="30" 
                MaxLength="15" Visible="False"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">&nbsp;</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">&nbsp;</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical">&nbsp;</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtDescripcionProductoServicio" runat="server" Width="250px" MaxLength="100" ReadOnly="true" Enabled="false" Visible="False"></asp:TextBox>
        </td>
    </tr>
</table>
<div class="ContenedorMensajes">
    <span class="MensajeInfo">[**]Rendimiento del motor KM/Litro</span>
</div>
<asp:HiddenField ID="hdnUnidadID" runat="server" />
<asp:HiddenField ID="hdnEquipoID" runat="server" />
<asp:HiddenField ID="hdnLiderID" runat="server" />
<asp:HiddenField ID="hdnOracleID" runat="server" />

