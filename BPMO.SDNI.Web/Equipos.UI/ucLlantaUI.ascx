<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucLlantaUI.ascx.cs" Inherits="BPMO.SDNI.Equipos.UI.ucLlantaUI" %>
<%--Satisface al CU089 - Bitacora de Llantas--%>
<%@ Register Src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" TagPrefix="uc" TagName="ucCatalogoDocumentosUI" %>
<table class="trAlinearDerecha" style="width: 530px; display: inherit; margin: 0px auto; border: 1px solid transparent;">
    <tr>
        <td class="tdCentradoVertical">
            <span>*</span>C&oacute;digo
        </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtCodigo" Width="275px" runat="server" AutoPostBack="true" MaxLength="150"
                OnTextChanged="txtCodigo_TextChanged"></asp:TextBox>
            <asp:ImageButton ID="ibtnBuscaLlanta" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                OnClick="ibtnBuscaCodigo_Click" />
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            <span>*</span>Sucursal
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 320px;">
            <asp:TextBox ID="txtSucursal" runat="server" MaxLength="30" Width="275px" AutoPostBack="true" ontextchanged="txtSucursal_TextChanged"></asp:TextBox>
            <asp:ImageButton runat="server" ID="ibtnBuscarSucursal" CommandName="VerSucursal" ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument='' onclick="ibtnBuscarSucursal_Click"/>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            <span>*</span>Marca
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtMarca" Width="301px" runat="server" MaxLength="150"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            <span>*</span>Modelo
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtModelo" Width="301px" runat="server" MaxLength="150"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            <span>*</span>Medida
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtMedida" Width="301px" runat="server" MaxLength="150"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            <span>*</span>Profundidad
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtProfundidad" Width="301px" runat="server" MaxLength="13" CssClass="CampoNumero"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            <span>*</span>¿Est&aacute; Revitalizada?
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px; padding-top: 0px;">
            <asp:RadioButtonList ID="rbLlantaRevitalizada" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="1">SI</asp:ListItem>
                <asp:ListItem Value="0">NO</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr id="trStock" runat="server">
        <td class="tdCentradoVertical">
            <span>*</span>¿Est&aacute; en Stock?
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px; padding-top: 0px;">
            <asp:RadioButtonList ID="rbLlantaStock" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="1">SI</asp:ListItem>
                <asp:ListItem Value="0">NO</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr id="trDescripcionEnllantable" runat="server">
        <td class="tdCentradoVertical">
            # Serie de la Unidad
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtDescripcionEnllantable" Width="301px" runat="server" MaxLength="50"></asp:TextBox>
        </td>
    </tr>
    <tr id="trPosicion" runat="server">
        <td class="tdCentradoVertical">
            Posici&oacute;n
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtPosicion" Width="100px" runat="server" CssClass="CampoNumeroEntero" MaxLength="2"></asp:TextBox>
        </td>
    </tr>
    <tr id="trActiva" runat="server">
        <td class="tdCentradoVertical">
            <span>*</span>¿Est&aacute; Activa?
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px; padding-top: 0px;">
            <asp:RadioButtonList ID="rbLlantaActiva" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="1">SI</asp:ListItem>
                <asp:ListItem Value="0">NO</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr id="trUC" runat="server">
        <td class="tdCentradoVertical">
            Usuario de Creaci&oacute;n
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtUsuarioRegistro" Width="275px" runat="server"></asp:TextBox>
            <asp:HiddenField ID="hdnUC" runat="server" />
        </td>
    </tr>
    <tr id="trFC" runat="server">
        <td class="tdCentradoVertical">
            Fecha de Creaci&oacute;n
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtFechaRegistro" Width="275px" runat="server" CssClass="CampoFecha"></asp:TextBox>
        </td>
    </tr>
    <tr id="trUUA" runat="server">
        <td class="tdCentradoVertical">
            Usuario de &Uacute;ltima Actualizaci&oacute;n
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtUsuarioEdicion" Width="275px" runat="server"></asp:TextBox>
            <asp:HiddenField ID="hdnUUA" runat="server" />
        </td>
    </tr>
    <tr id="trFUA" runat="server">
        <td class="tdCentradoVertical">
            Fecha de &Uacute;ltima Actualizaci&oacute;n
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox ID="txtFechaEdicion" Width="275px" runat="server" CssClass="CampoFecha"></asp:TextBox>
        </td>
    </tr>
</table>
<div id="dvDocumentos" runat="server">
    <fieldset id="fsDocumentosAdjuntos" style="width: 95%; margin: 10px auto;">
        <legend>Documentos de Llantas</legend>
        <uc:ucCatalogoDocumentosUI ID="ucCatalogoDocumentos" runat="server" />
    </fieldset>
</div>
<asp:Button ID="btnResult" runat="server" Text="" OnClick="btnResult_Click" Style="display: none;" />
<asp:HiddenField ID="hdnLlantaId" runat="server" />
<asp:HiddenField ID="hdnTipoEnllantable" runat="server" />
<asp:HiddenField ID="hdnEnllantableId" runat="server" />
<asp:HiddenField ID="hdnEsRefaccion" runat="server" />
<asp:HiddenField ID="hdnSucursalID" runat="server"></asp:HiddenField>
<asp:HiddenField ID="hdnSucursalEnllantableId" runat="server"></asp:HiddenField>
<asp:HiddenField ID="hdnBuscarSoloActivos" runat="server" />
<asp:HiddenField ID="hdnBuscarSoloStock" runat="server" />
