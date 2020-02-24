<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucOperadorUI.ascx.cs" Inherits="BPMO.SDNI.Comun.UI.ucOperadorUI" %>
<script type="text/javascript">
    // Inicializa el control de Información General
    function <%= ClientID %>_Inicializar() {
        var fechaNacimiento = $('#<%= txtFechaNacimiento.ClientID %>');
        if (fechaNacimiento.length > 0) {
            if (fechaNacimiento.attr("disabled") != false && fechaNacimiento.attr("disabled") != "disabled") {
                fechaNacimiento.datepicker({
                    yearRange: '-50:+0',
                    changeYear: true,
                    changeMonth: true,
                    showButtonPanel: true,
                    dateFormat: "dd/mm/yy",
                    buttonImage: '../Contenido/Imagenes/calendar.gif',
                    buttonImageOnly: true,
                    toolTipText: "Fecha del contrato",
                    showOn: 'button',
                    defaultDate: (fechaNacimiento.val().length == 10) ? fechaNacimiento.val() : new Date()
                });

                fechaNacimiento.attr('readonly', true);
            }
        }
        var fechaExpiracion = $('#<%= txtLicenciaFechaExpiracion.ClientID %>');
        if (fechaExpiracion.length > 0) {
            if (fechaExpiracion.attr("disabled") != false && fechaExpiracion.attr("disabled") != "disabled") {
                fechaExpiracion.datepicker({
                    yearRange: '-10:+7',
                    changeYear: true,
                    changeMonth: true,
                    showButtonPanel: true,
                    dateFormat: "dd/mm/yy",
                    buttonImage: '../Contenido/Imagenes/calendar.gif',
                    buttonImageOnly: true,
                    toolTipText: "Fecha del contrato",
                    showOn: 'button',
                    defaultDate: (fechaExpiracion.val().length == 10) ? fechaExpiracion.val() : new Date()
                });

                fechaExpiracion.attr('readonly', true);
            }
        }
    }
</script>
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
<table class="trAlinearDerecha" style="width: 530px; margin: 0px auto;">
    <tr>
        <td class="tdCentradoVertical"><span>*</span>Cliente </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:HiddenField runat="server" ID="hdnOperadorID" />
            <asp:HiddenField runat="server" ID="hdnCuentaClienteID" />
            <asp:HiddenField runat="server" ID="hdnEstatus" Value="1" />
            <asp:TextBox ID="txtCuentaClienteNombre" runat="server" Width="275px" MaxLength="150" AutoPostBack="True"
                OnTextChanged="txtCuentaClienteNombre_TextChanged"></asp:TextBox>
            <asp:ImageButton ID="ibtnBuscarCliente" runat="server" ImageUrl="../Contenido/Imagenes/Detalle.png"
                ToolTip="Consultar Cliente" OnClick="ibtnBuscarCliente_Click" />
        </td>
    </tr>    
    <tr>
        <td class="tdCentradoVertical"><span>*</span>Nombre </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox runat="server" ID="txtNombre" MaxLength="150" Width="300px"></asp:TextBox>
        </td>
    </tr>  
    <tr>
        <td class="tdCentradoVertical"><span>*</span>Fecha Nacimiento </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox runat="server" ID="txtFechaNacimiento" CssClass="CampoFecha" MaxLength="10" Width="90px" AutoPostBack="True" 
                OnTextChanged="txtOperadorFechaNacimiento_TextChanged"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical"><span>*</span>Años Experiencia </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox runat="server" ID="txtAniosExperiencia" MaxLength="2" Width="60px" CssClass="CampoNumeroEntero"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical"><span>*</span>Calle </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox runat="server" ID="txtCalle" MaxLength="150" Width="300px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical"><span>*</span>Ciudad </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox runat="server" ID="txtCiudad" MaxLength="150" Width="200px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical"><span>*</span>Estado </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox runat="server" ID="txtEstado" MaxLength="150" Width="200px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical"><span>*</span>C&oacute;digo Postal </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical" style="width: 330px;">
            <asp:TextBox runat="server" ID="txtCodigoPostal" MaxLength="150" Width="60px"></asp:TextBox>
        </td>
    </tr>
</table>
<fieldset style="width: 96%; margin-right: auto; margin-left: auto;">
    <legend>Datos de la Licencia</legend>
    <table class="trAlinearDerecha" style="width: 530px; margin: 0px auto;">
        <tr>
            <td class="tdCentradoVertical"><span>*</span># </td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 330px;">
                <asp:TextBox runat="server" ID="txtLicenciaNumero" MaxLength="20" Width="150px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical"><span>*</span>Tipo </td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 330px;">
                <asp:DropDownList runat="server" ID="ddlTipoLicencia" ToolTip="Tipo de licencia del operador." Width="90px">
                    <asp:ListItem Value="0">ESTATAL</asp:ListItem>
                    <asp:ListItem Selected="True" Value="1">FEDERAL</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical"><span>*</span>Fecha Expiraci&oacute;n</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 330px;">
                <asp:TextBox runat="server" ID="txtLicenciaFechaExpiracion" CssClass="CampoFecha"
                    MaxLength="10" Width="90px" AutoPostBack="True" OnTextChanged="txtLicenciaFechaExpiracion_TextChanged"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical"><span>*</span>Estado Expedici&oacute;n</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 330px;">
                <asp:TextBox runat="server" ID="txtLicenciaEstadoExpedicion" MaxLength="150" Width="200px"></asp:TextBox>
            </td>
        </tr>
    </table>
</fieldset>
<asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />