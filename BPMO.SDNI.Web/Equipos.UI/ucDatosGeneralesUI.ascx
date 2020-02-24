<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDatosGeneralesUI.ascx.cs" Inherits="BPMO.SDNI.Equipos.UI.ucDatosGeneralesUI" %>
<%@ Register TagPrefix="uc" TagName="ucCatalogoDocumentosUI" Src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" %>

<link href="../Contenido/Estilos/Tema.JqueryUI/jquery.ui.timepicker.css" rel="Stylesheet" type="text/css"/>
<script src="../Contenido/Scripts/jquery.ui.timepicker.js" type="text/javascript"></script>
<script src="../Contenido/Scripts/jquery.ui.timepicker-es.js" type="text/javascript"></script>
<table class="trAlinearDerecha" style="margin-right:20; margin: 0px auto; width: 530px; display: inherit; border: 1px solid transparent;">
    <tr>
        <td class="tdCentradoVertical"><span>*</span><asp:Label runat="server" ID="lblAreaDepto">Área/Departamento</asp:Label></td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:DropDownList ID="ddlArea" Width="200px" runat="server" AutoPostBack="true" 
                onselectedindexchanged="ddlArea_SelectedIndexChanged">
                <asp:ListItem Value="x" Text="Seleccionar"></asp:ListItem>
            </asp:DropDownList>
             <asp:DropDownList ID="ddlTipoRentaEmpresas" Width="200px" runat="server" AutoPostBack="true" 
               onselectedindexchanged="ddlTipoRentaEmpresas_SelectedIndexChanged">
                <asp:ListItem Value="x" Text="Seleccionar" ></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical"><span>*</span><asp:Label runat="server" ID="lblPropietario">Proveedor</asp:Label></td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtPropietario" runat="server" Width="275px" AutoPostBack="True" ontextchanged="txtPropietario_TextChanged"></asp:TextBox>
            <asp:ImageButton ID="ibtnBuscaPropietario" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" onclick="ibtnBuscaPropietario_Click" />
            <asp:HiddenField ID="hdnPropietarioID" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical"><span>*</span>Cliente</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtCliente" runat="server" Width="275px" AutoPostBack="True" ontextchanged="txtCliente_TextChanged"></asp:TextBox>
            <asp:ImageButton ID="ibtnBuscaCliente" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" onclick="ibtnBuscaCliente_Click" />
            <asp:HiddenField ID="hdnClienteID" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical"><span>*</span>Sucursal</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtSucursal" runat="server" Width="275px" AutoPostBack="True" ontextchanged="txtSucursal_TextChanged"></asp:TextBox>
            <asp:ImageButton ID="ibtnBuscaSucursal" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" onclick="ibtnBuscaSucursal_Click" />
            <asp:HiddenField ID="hdnSucursalID" runat="server" />
        </td>
    </tr>
    <tr id="trFabricante" runat="server" >
        <td class="tdCentradoVertical" ><span>*</span>Fabricante</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtFabricante" runat="server" Width="301px" Enabled="false"></asp:TextBox>
        </td>
    </tr>
    <tr id="trBloqueoUnidad" runat="server">
        <td class="tdCentradoVertical"><span>Unidad Bloqueada</span></td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:CheckBox runat="server" ID="cbBloqueoUnidad"/>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical"><span>*</span>¿Entra unidad bloqueada a mantenimiento?</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:CheckBox ID="chbxEntraMantenimiento" runat="server" AutoPostBack="True" 
                oncheckedchanged="chbxEntraMantenimiento_CheckedChanged" />
        </td>
    </tr>
    <tr id="trNumOrdenCompra" runat="server">
        <td class="tdCentradoVertical" ># Orden de compra del proveedor</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtNumeroOrdenCompra" CssClass="CampoNumeroEntero" runat="server" MaxLength="10" Width="95px" ></asp:TextBox>
        </td>
    </tr>
    <tr id="trMontoArrendamiento" runat="server">
        <td class="tdCentradoVertical" >Monto de Arrendamiento</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;"> 
            <asp:TextBox  ID="txtMontoArrendamiento" runat="server" MaxLength="21" CssClass="txtMontoArrendamiento" Width="190px" onkeypress="return ImporteArrendamiento(event, this);" ></asp:TextBox>
        </td>
    </tr>
    <tr id="trTipoMoneda" runat="server">
        <td class="tdCentradoVertical" ><span>*</span>Tipo Moneda</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:DropDownList ID="ddlMonedas" Width="200px" runat="server" AutoPostBack="false" CssClass="ddlMonedas" >
                <asp:ListItem Value="x" Text="Seleccionar" ></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr id="trFechaInicioArrendamiento" runat="server">
        <td class="tdCentradoVertical"><span>*</span>Fecha Inicio de Arrendamiento</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
       
           <asp:TextBox ID="txtFechaInicioArrendamiento" runat="server" MaxLength="30" Width="95px" ValidationGroup="FormatoValido"  CssClass="CampoFechas"></asp:TextBox>
          
        </td>
    </tr>
    <tr id="trFechaFinArrendamiento" runat="server">
        <td class="tdCentradoVertical"><span>*</span>Fecha Fin Arrendamiento</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtFechaFinArrendamiento" runat="server" MaxLength="30" Width="95px" CssClass="CampoFechas"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:Button ID="btnAgregarFechas" runat="server" Text="Agregar fechas" 
                CssClass="btnAgregarATabla" style="margin-right: 15px;" 
                onclick="btnAgregarFechas_Click"/>
        </td>
    </tr>
    <tr id="trdNuevasFechasInicio" runat="server">
        <td class="tdCentradoVertical"><span>*</span>Nueva Fecha Inicio</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
           <asp:TextBox ID="txtNuevaFechaInicio" runat="server" MaxLength="30" Width="95px" CssClass="CampoFecha"></asp:TextBox>
          
        </td>
    </tr>
    <tr id="trdNuevasFechasFin" runat="server">
        <td class="tdCentradoVertical"><span>*</span>Nueva Fecha Fin</td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtNuevaFechaFinal" runat="server" MaxLength="30" Width="95px" CssClass="CampoFecha"></asp:TextBox>
        </td>
    </tr>
</table>
<p></p>
<div ID="divArchivos" runat="server">                    
    <fieldset id="fsDocumentosAdjuntos">
        <legend>Adjuntar archivo Orden de Compra</legend>
        <uc:ucCatalogoDocumentosUI ID="ucCatalogoDocumentosOC" runat="server" />
    </fieldset>
</div>
<asp:Button ID="btnResult" runat="server" Text="Button" onclick="btnResult_Click" style="display: none;" />
<asp:HiddenField ID="hdnUsuarioAutenticado" runat="server" />
<asp:HiddenField ID="hdnNombreClienteUnidadOperativa" runat="server" />
<asp:HiddenField ID="hdnArrendamientoNuevo" runat="server" />
<asp:HiddenField ID="hdnModo" runat="server" />
