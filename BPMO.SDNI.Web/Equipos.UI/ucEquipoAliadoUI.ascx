<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEquipoAliadoUI.ascx.cs"
    Inherits="BPMO.SDNI.Equipos.UI.ucEquipoAliadoUI" %>
<!-- Satisface a la solicitud de Cambio SC0005 -->
<!-- Satisface a la solicitud de cambio SC0035 -->

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
<table class="trAlinearDerecha" style="width: 530px; margin: 0px auto;">
    <tr>
        <td class="tdCentradoVertical">
            <span>*</span>Sucursal
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtSucursal" runat="server" MaxLength="80" Width="270px" AutoPostBack="true"
                OnTextChanged="txtSucursal_TextChanged"></asp:TextBox>
            <asp:ImageButton runat="server" ID="ibtnBuscarSucursal" CommandName="VerSucursal"
                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument=''
                OnClick="btnBuscarSucursal_Click" />
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            <span>*</span># Serie
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtNumeroSerie" runat="server" MaxLength="80" Width="270px" AutoPostBack="true"
                OnTextChanged="txtNumeroSerie_TextChanged"></asp:TextBox>
            <asp:ImageButton ID="ibtnBuscaEquipo" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                OnClick="ibtnBuscaEquipo_Click" />
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
            <span id="spanFabricante" runat="server">*</span>Fabricante
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
            <span id="spanMarca" runat="server">*</span>Marca
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
            <span id="spanModelo" runat="server">*</span>Modelo
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
            <span id="spanPBV" runat="server">*</span>PBV
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtPBV" runat="server" CssClass="CampoNumero" MaxLength="13" Width="80px"></asp:TextBox>
        </td>
    </tr>
    <tr id="rowPBC" runat="server">
        <td class="tdCentradoVertical">
            <span id="spanPBC" runat="server">*</span><asp:Label id="lblPBC" runat="server">PBC</asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtPBC" runat="server" CssClass="CampoNumero" MaxLength="13" Width="80px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            <span>*</span>Tipo Equipo
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtTipoEquipoNombre" runat="server" Width="203px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            <span>*</span>Tipo Equipo Aliado
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 320px;">
            <asp:DropDownList ID="ddlTipoEquipoAliado" runat="server" Width="200px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            <span>&nbsp;</span>Dimensi&oacute;n
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtDimenciones" runat="server" MaxLength="250" Width="200px"></asp:TextBox>
        </td>
    </tr>
    <!-- SCOO -->
    <tr id="rowHorasIniciales" runat="server">
        <td class="tdCentradoVertical">
            <span>*</span>Horas Iniciales
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtHorasIniciales" runat="server" CssClass="InputReset CampoNumeroEntero" MaxLength="9"
                Width="60px"></asp:TextBox>
        </td>
    </tr>
 
    <tr  id="rowKilometrajeInicial" runat="server">
        <td class="tdCentradoVertical">
            <span>*</span>Kilometraje Inicial
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtKilometrajeInicial" runat="server" 
                CssClass="InputReset CampoNumeroEntero" MaxLength="9"
                Width="60px"></asp:TextBox>
        </td>
    </tr>
    <!-- SC001 Final -->		
    <tr id= "trActivo" runat="server" visible="true">
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
<asp:Button ID="btnResult" runat="server" Text="" OnClick="btnResult_Click" Style="display: none;" />
<asp:HiddenField runat="server" ID="hdnEstatusEquipoAliado"></asp:HiddenField>
<asp:HiddenField runat="server" ID="hdnLiderID"></asp:HiddenField>
<asp:HiddenField runat="server" ID="hdnOracleID"></asp:HiddenField>
<asp:HiddenField runat="server" ID="hdnUnidadOperativaID"></asp:HiddenField>
<asp:HiddenField runat="server" ID="hdnSucursalID"></asp:HiddenField>
<asp:HiddenField runat="server" ID="hdnEquipoAliadoID"></asp:HiddenField>
<asp:HiddenField runat="server" ID="hdnEquipoID"></asp:HiddenField>
<asp:HiddenField runat="server" ID="hdnModeloID"></asp:HiddenField>
<asp:HiddenField runat="server" ID="hdnTipoEquipoAliadoID"></asp:HiddenField>
<asp:HiddenField runat="server" ID="hdnLibroActivos" />
<asp:HiddenField ID="hdnTipoMensaje" runat="server" />
<asp:HiddenField ID="hdnMensaje" runat="server" />
<asp:HiddenField ID="hdnEmpresaConPermiso" runat="server" />
<asp:HiddenField ID="hdnMarcaID" runat="server"></asp:HiddenField>
