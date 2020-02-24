<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucConfigurarAlertaUI.ascx.cs" Inherits="BPMO.SDNI.Facturacion.MonitoreoPagos.UI.ucConfigurarAlertaUI" %>

<%--Satisface el caso de uso CU009 – Configuración Notificación de facturación--%>
<%--Satisface a la solicitud de cambios SC0008--%>

<table class="trAlinearDerecha" style="width: 530px; display: inherit; margin: 0px auto; border: 1px solid transparent;">
    <tr>
        <td class="tdCentradoVertical"><span>*</span>
            SUCURSAL
        </td>
        <td style="width: 20px;">&nbsp;</td>
        <td style="white-space:nowrap">
            <asp:TextBox ID="txtSucursal" runat="server" Width="275px" MaxLength="80" ValidationGroup="ActualizarRegistro" AutoPostBack="true" OnTextChanged="txtSucursal_TextChanged"></asp:TextBox>
            <asp:ImageButton runat="server" ID="ibtnBuscarSucursal" CommandName="VerSucursal"
                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument=''
                OnClick="ibtnBuscarSucursal_Click" CausesValidation="False" />            
            <asp:HiddenField ID="hdnSucursalID" runat="server" Visible="False" />
            <asp:HiddenField ID="hdnClaveSucursal" runat="server" Visible="False" />            
        </td>
    </tr>
    <tr>
         <td class="tdCentradoVertical"><span>*</span>
            EMPLEADO
        </td>
        <td style="width: 20px;">&nbsp;</td>
        <td style="white-space:nowrap">
            <asp:TextBox ID="txtEmpleado" runat="server" MaxLength="30" Width="275px" ValidationGroup="ActualizarRegistro"
                AutoPostBack="True" OnTextChanged="txtNombreEmpleado_TextChanged"></asp:TextBox>
            <asp:ImageButton runat="server" ID="ibtnBuscarEmpleado" CommandName="VerEmpleado"
                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Empleados" CommandArgument=''
                OnClick="ibtnBuscarEmpleado_Click" CausesValidation="False" />            
            <asp:HiddenField ID="hdnEmpleadoID" runat="server" Visible="False" />
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical"><span>*</span>CORREO ELECTR&Oacute;NICO</td>
        <td style="width: 20px;">&nbsp;</td>
        <td>
            <asp:TextBox ID="txtCorreoElectronico" ValidationGroup="ActualizarRegistro" runat="server" Width="275px"></asp:TextBox>            
            <%--<asp:RegularExpressionValidator ID="revtxtCorreoElectronico" runat="server"  ValidationGroup="ActualizarRegistro"                  
                ErrorMessage="El valor no corresponde a una dirección de correo electrónico válido" 
                Display="Dynamic" ControlToValidate="txtCorreoElectronico" 
                SetFocusOnError="True" 
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                ToolTip="El valor no corresponde a una dirección de correo electrónico válido">*</asp:RegularExpressionValidator>--%>
        </td>
    </tr>

    <td class="tdCentradoVertical"><span>*</span>
        ESTATUS
    </td>
        <td style="width: 20px;">&nbsp;</td>
        <td>
            <%--Representación de un valor booleano (true o false) como una lista desplegable--%>
            <asp:DropDownList ID="ddlEstatus" runat="server" ValidationGroup="ActualizarRegistro">
                <asp:ListItem Value="True" Selected="true">ACTIVO</asp:ListItem>
                <asp:ListItem Value="False">INACTIVO</asp:ListItem>
            </asp:DropDownList>
        </td>

    <tr>
        <td class="tdCentradoVertical"><span>*</span>
            NÚMERO DE DÍAS
        </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="style1">
            <asp:TextBox ID="txtNumeroDeDias" runat="server" Width="40px" 
                ValidationGroup="ActualizarRegistro" MaxLength="5"></asp:TextBox>       

            <asp:RangeValidator ID="rngvtxtNumeroDeDias" runat="server" MinimumValue="1" MaximumValue="32767"
                 ErrorMessage="El valor debe de ser un número mayor a cero o no corresponde a un número válido" 
                 SetFocusOnError="True" ControlToValidate="txtNumeroDeDias" ValidationGroup="ActualizarRegistro"
                 Display="Dynamic" ToolTip="El valor debe de ser un número mayor a cero o no corresponde a un número válido" 
                Type="Integer">**
            </asp:RangeValidator>            
        </td>
    </tr>
</table>
<asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" CausesValidation="False" UseSubmitBehavior="false" />

<asp:HiddenField runat="server" ID="hdnUnidadOperativaID" />
<asp:HiddenField ID="hdnConfiguracionAlertaID" runat="server" />
<asp:HiddenField ID="hdnUC" runat="server" />
<asp:HiddenField ID="hdFC" runat="server" />

<%--SC0008 - Adición de campo para perfil asociado a la configuración--%>
<asp:HiddenField ID="hdfPerfilID" runat="server" />