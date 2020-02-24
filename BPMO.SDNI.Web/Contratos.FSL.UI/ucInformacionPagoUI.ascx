<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucInformacionPagoUI.ascx.cs"
    Inherits="BPMO.SDNI.Contratos.FSL.UI.ucInformacionPagoUI" %>
    <%-- 
    Satisface al Caso de Uso CU022 - Consultar Contratos Full Service Leasing
    Satisface al Caso de Uso CU023 - Editar Contrato Full Service Leasing
    Satisface al Caso de Uso CU015 - Registrar Contrato Full Service Leasing
    --%>
    <script type="text/javascript">
        //Inicializa el Control de Informacion de Pago
        function <%= ClientID %>_Inicializar() {
            var FechaInicio = $('#<%= txtFechaInicio.ClientID %>');
            
            if (FechaInicio.length > 0) {
                FechaInicio.datepicker({
                    yearRange: '-10:+10',
                    changeYear: true,
                    changeMonth: true,
                    showButtonPanel: true,
                    dateFormat: "dd/mm/yy",
                    buttonImage: '../Contenido/Imagenes/calendar.gif',
                    buttonImageOnly: true,
                    toolTipText: "Fecha de Inicio del Contrato",
                    showOn: 'button',
                    defaultDate: (FechaInicio.val().length == 10) ? FechaInicio.val() : new Date()
                });

                FechaInicio.attr('readonly', true);
            }
        }
    </script>
<fieldset id="fsInformacionPago">
    <legend>Información de Pago</legend>
    <table class="trAlinearDerecha">
        <tr>
            <td class="tdCentradoVertical">
                TOTAL A PAGAR
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtTotal" runat="server" Columns="35" ReadOnly="True" CssClass="CampoMoneda"
                    Width="224px"></asp:TextBox>
            </td>
            <td class="tdCentradoVertical">
                Mensualidad
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtMensualidad" runat="server" CssClass="CampoMoneda" ReadOnly="True"
                    Columns="35"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical">
                <span>*</span>FECHA INICIO CONTRATO
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtFechaInicio" runat="server" Columns="30" OnTextChanged="txtFechaInicio_TextChanged" CssClass="CampoFecha"
                    AutoPostBack="True"></asp:TextBox>                
            </td>
            <td class="tdCentradoVertical">
                FECHA FIN DE CONTRATO
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtFechaFin" runat="server" Columns="30" CssClass="CampoFecha"></asp:TextBox>
            </td>
        </tr>
        <tr style="display:none;">
            <td class="tdCentradoVertical">
                CUENTA BANCARIA
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                <asp:DropDownList ID="ddlCuentaBancaria" runat="server">
                    <asp:ListItem Text="Seleccione una opcion" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="tdCentradoVertical">
                BANCO
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtBanco" runat="server" Columns="35"></asp:TextBox>
            </td>
        </tr>
        <tr style="display:none;">
            <td class="tdCentradoVertical">
                LUGAR
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtLugar" runat="server" Columns="35"></asp:TextBox>
            </td>
            <td class="tdCentradoVertical">
                BENEFICIARIO
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtBeneficiario" runat="server" Columns="35"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical">
                <span>*</span>Días para Pagar</td>
            <td style="width: 5px;">
                &nbsp;</td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtDiasPago" runat="server" CssClass="CampoNumeroEntero"></asp:TextBox></td>
            <td class="tdCentradoVertical">
                &nbsp;</td>
            <td style="width: 5px;">
                &nbsp;</td>
            <td class="tdCentradoVertical">
                &nbsp;</td>
        </tr>
    </table>
</fieldset>
