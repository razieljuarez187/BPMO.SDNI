<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAutorizadorUI.ascx.cs" Inherits="BPMO.SDNI.Comun.UI.ucAutorizadorUI" %>
<%-- 
    Satisface al caso de uso CU098 - Configurar Autorizadores para los Contratos
--%>
<!--Funcionalidad Deshabilitar Enter en cajas de texto-->
<script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
<script type="text/javascript">
    function BtnBuscar(guid, xml) {
        var width = ObtenerAnchoBuscador(xml);

        $.BuscadorWeb({
            xml: xml,
            guid: guid,
            btnSender: $("#<%=btnResult.ClientID %>"),
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

<table class="trAlinearDerecha" style="width: 530px; display: inherit; margin: 0px auto; border: 1px solid transparent;">
    <tr>
        <td class="tdCentradoVertical"><span>*</span>SUCURSAL</td>
        <td style="width: 5px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 300px;">
            <asp:TextBox ID="txtSucursal" runat="server" Width="275px" MaxLength="80" AutoPostBack="true" OnTextChanged="txtSucursal_TextChanged"></asp:TextBox>
            <asp:ImageButton runat="server" ID="ibtnBuscarSucursal" CommandName="VerSucursal"
                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument=''
                OnClick="ibtnBuscarSucursal_Click" />
            <asp:HiddenField ID="hdnSucursalID" runat="server" Visible="False" />
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical"><span>*</span>
            TIPO AUTORIZACIÓN
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 320px;">
            <asp:DropDownList ID="ddlTipoAutorizacion" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoAutorización_SelectedIndexChanged" Width="200px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical"><span>*</span>
            EMPLEADO
        </td>
        <td style="width: 5px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 300px;">
            <asp:TextBox ID="txtEmpleado" runat="server" MaxLength="30" Width="275px"
                AutoPostBack="True" OnTextChanged="txtNombreEmpleado_TextChanged"></asp:TextBox>
            <asp:ImageButton runat="server" ID="ibtnBuscarEmpleado" CommandName="VerEmpleado"
                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Empleados" CommandArgument=''
                OnClick="ibtnBuscarEmpleado_Click" />
                <asp:HiddenField ID="hdnEmpleadoID" runat="server" Visible="False" />
        </td>
    </tr>                    
    <tr>
        <td class="tdCentradoVertical"><span>*</span>CORREO ELECTR&Oacute;NICO</td>
        <td style="width: 5px;">&nbsp;</td>
        <td class="tdCentradoVertical">
            <asp:TextBox ID="txtEmail" runat="server" Width="275px"></asp:TextBox>
        </td>
     </tr>
     <tr>
        <td class="tdCentradoVertical"><span>*</span>TEL&Eacute;FONO</td>
        <td style="width: 5px;">&nbsp;</td>
        <td class="tdCentradoVertical">
            <asp:TextBox ID="txtTelefono" runat="server" Width="275px"></asp:TextBox>
        </td>
     </tr>
     <tr>
        <td class="tdCentradoVertical">
            <span>*</span>¿S&Oacute;LO NOTIFICACI&Oacute;N?
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 330px; padding-top: 0px;">
            <asp:RadioButtonList ID="rbNotificacion" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="1">SI</asp:ListItem>
                <asp:ListItem Value="0">NO</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr id="trEstatus" runat="server">
        <td class="tdCentradoVertical">
            Estatus
        </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 320px;">
            <asp:DropDownList ID="ddlEstatus" runat="server" Width="100px">
                <asp:ListItem Value="True" Text="Activo" Selected="true"></asp:ListItem>
                <asp:ListItem Value="False" Text="Inactivo"></asp:ListItem>
            </asp:DropDownList>
        </td>
     </tr>
     <tr id="trUC" runat="server">
        <td class="tdCentradoVertical">
            Usuario de Registro
        </td>
        <td style="width: 5px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 300px;">
            <asp:TextBox ID="txtUsuarioRegistro" Width="275px" runat="server"></asp:TextBox>
            <asp:HiddenField ID="hdnUC" runat="server" />
        </td>
    </tr>
    <tr id="trFC" runat="server">
        <td class="tdCentradoVertical">
            Fecha de Registro
        </td>
        <td style="width: 5px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 300px;">
            <asp:TextBox ID="txtFechaRegistro" Width="275px" runat="server" CssClass="CampoFecha"></asp:TextBox>
        </td>
    </tr>
    <tr id="trUUA" runat="server">
        <td class="tdCentradoVertical">
            Usuario de &Uacute;ltima Modificaci&oacute;n
        </td>
        <td style="width: 5px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 300px;">
            <asp:TextBox ID="txtUsuarioEdicion" Width="275px" runat="server"></asp:TextBox>
            <asp:HiddenField ID="hdnUUA" runat="server" />
        </td>
    </tr>
    <tr id="trFUA" runat="server">
        <td class="tdCentradoVertical">
            Fecha de &Uacute;ltima Modificaci&oacute;n
        </td>
        <td style="width: 5px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 300px;">
            <asp:TextBox ID="txtFechaEdicion" Width="275px" runat="server" CssClass="CampoFecha"></asp:TextBox>
        </td>
    </tr>
</table>
<asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
<asp:HiddenField ID="hdnAutorizadorId" runat="server" />
<asp:HiddenField ID="hdnUnidadOperativaID" runat="server" />
<asp:HiddenField ID="hdnModoRegistro" runat="server" />

