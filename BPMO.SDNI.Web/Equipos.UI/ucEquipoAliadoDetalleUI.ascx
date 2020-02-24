<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEquipoAliadoDetalleUI.ascx.cs"
    Inherits="BPMO.SDNI.Equipos.UI.ucEquipoAliadoDetalleUI" %>

<style type="text/css"> 
    .InputReset,
        select
        {
	        height: 25px;
	        width: 82%;
	        font-family : Century Gothic, Arial, Verdana, Serif; 
	
	        border-color:#d7d7d7; 
	        border-style:solid; 
	        border-width:1px;
	        border-radius:5px; -moz-border-radius:5px; -webkit-border-radius:5px; -khtml-border-radius:5px;
        }
        .InputReset
        {
	        padding-right:5px; padding-left:5px;
        }
        .InputReset,
        select[disabled]
        {
	        border-color:#d2d2d2; 
	        border-style:solid; 
	        border-width:1px;
        }
</style>
<!-- Satisface a la solicitud de Cambio SC0005 -->
<table class="trAlinearDerecha" style="width: 530px; margin: 0px auto;">
    <tr>
        <td class="tdCentradoVertical">
            Empresa
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtEmpresa" runat="server" Width="270px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            Sucursal
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtSucursal" runat="server" Width="270px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            # Serie
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtNumeroSerie" runat="server" Width="270px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            ¿Es Activo?
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtActivoOracle" runat="server" Width="30px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            Clave Oracle
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtOracleID" runat="server" Width="150px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            Fabricante
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtFabricante" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            Marca
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtMarca" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            Modelo
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtModelo" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            A&ntilde;o Modelo
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtAnioModelo" runat="server" Width="80px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            PBV
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtPBV" runat="server" CssClass="CampoNumero" MaxLength="50" Width="80px"></asp:TextBox>
        </td>
    </tr>
    <tr id="rowPBC" runat="server">
        <td class="tdCentradoVertical">
            <asp:Label id="lblPBC" runat="server">PBC</asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtPBC" runat="server" CssClass="CampoNumero" Width="80px" MaxLength="50"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            Tipo Equipo
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtTipoEquipo" runat="server" Width="203px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            Tipo Equipo Aliado
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtTipoEquipoAliado" runat="server" Width="203px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            Dimensi&oacute;n
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtDimenciones" runat="server" Width="200px" MaxLength="250"></asp:TextBox>
        </td>
    </tr>
    <tr id="rowHorasIniciales" runat="server">
        <td class="tdCentradoVertical">
            Horas Iniciales
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtHorasIniciales" runat="server" 
                CssClass="CampoNumero InputReset" Width="60px"
                MaxLength="150" ></asp:TextBox>
        </td>
    </tr>
     <tr id="rowKilometrajeInicial" runat="server">
        <td class="tdCentradoVertical">
            Kilometros Iniciales
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtKilometrosIniciales" runat="server" 
                CssClass="CampoNumero InputReset" Width="60px"
                MaxLength="150"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            Estatus
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtEstatusEquipo" runat="server" Width="100px"></asp:TextBox>
        </td>
    </tr>	
    <tr>
        <td class="tdCentradoVertical">
            <span>&nbsp;</span>Activo
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:CheckBox ID="chkActivo" runat="server" />
        </td>
    </tr>	
</table>
<asp:HiddenField runat="server" ID="hdnEstatusID"></asp:HiddenField>
<asp:HiddenField runat="server" ID="hdnLiderID"></asp:HiddenField>
<asp:HiddenField runat="server" ID="hdnOracleID"></asp:HiddenField>
<asp:HiddenField runat="server" ID="hdnUnidadOperativaID"></asp:HiddenField>
<asp:HiddenField runat="server" ID="hdnSucursalID"></asp:HiddenField>
<asp:HiddenField runat="server" ID="hdnEquipoAliadoID"></asp:HiddenField>
<asp:HiddenField runat="server" ID="hdnEquipoID"></asp:HiddenField>
<asp:HiddenField runat="server" ID="hdnModeloID"></asp:HiddenField>
<asp:HiddenField runat="server" ID="hdnTipoEquipoAliadoID"></asp:HiddenField>
<asp:HiddenField ID="hdnTipoMensaje" runat="server" />
<asp:HiddenField ID="hdnMensaje" runat="server" />
