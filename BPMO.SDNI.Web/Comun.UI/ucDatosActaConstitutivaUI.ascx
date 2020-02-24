<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDatosActaConstitutivaUI.ascx.cs"
    Inherits="BPMO.SDNI.Comun.UI.ucDatosActaConstitutivaUI" %>
<div id="divInformacionActaControles">
    <table class="trAlinearDerecha">
        <tr>
            <td class="tdCentradoVertical etiquetaControl">
                <label id="lblNumEscritura" runat="server"><span>*</span># Escritura</label>
            </td>
            <td class="tdCentradoVertical espacio">
                &nbsp;
            </td>
            <td class="tdCentradoVertical control">
                <asp:TextBox ID="txtNumeroEscrituraCliente" runat="server" Width="135px" MaxLength="500"></asp:TextBox>
            </td>
            <td class="tdCentradoVertical espacio">
                &nbsp;
            </td>
            <td class="tdCentradoVertical etiquetaControl">
                <label id="lblFechaEscritura" runat="server"><span>*</span>Fecha Escritura</label>
            </td>
            <td class="tdCentradoVertical espacio">
                &nbsp;
            </td>
            <td class="tdCentradoVertical control">
                <asp:TextBox ID="txtFechaEscrituraCliente" runat="server" CssClass="CampoFecha"></asp:TextBox>
            </td>
        </tr>
        <tr id="trSeccion1" runat="server">
            <td class="tdCentradoVertical etiquetaControl">
                <span>*</span>Nombre del Notario
            </td>
            <td class="tdCentradoVertical espacio">
                &nbsp;
            </td>
            <td class="tdCentradoVertical control">
                <asp:TextBox ID="txtNombreNotarioCliente" runat="server" Width="200px" MaxLength="250"></asp:TextBox>
            </td>
            <td class="tdCentradoVertical espacio">
                &nbsp;
            </td>
            <td class="tdCentradoVertical etiquetaControl">
                <span>*</span># Notar&iacute;a
            </td>
            <td class="tdCentradoVertical espacio">
                &nbsp;
            </td>
            <td class="tdCentradoVertical control">
                <asp:TextBox ID="txtNumeroNotariaCliente" runat="server" Width="135px" MaxLength="500"></asp:TextBox>
            </td>
        </tr>
        <tr id="trSeccion2" runat="server">
            <td class="tdCentradoVertical etiquetaControl">
                <span>*</span>Localidad de la Notar&iacute;a
            </td>
            <td class="tdCentradoVertical espacio">
                &nbsp;
            </td>
            <td class="tdCentradoVertical control">
                <asp:TextBox ID="txtLocalidadNotariaCliente" runat="server" MaxLength="100"></asp:TextBox>
                <div class="indicacion">
                    (Formato: Ciudad,Estado)</div>
            </td>
            <td class="tdCentradoVertical espacio">
                &nbsp;
            </td>
            <td class="tdCentradoVertical etiquetaControl" style="text-align: right;">
                # Folio de Inscripci&oacute;n
            </td>
            <td class="tdCentradoVertical espacio">
                &nbsp;
            </td>
            <td class="tdCentradoVertical control">
                <asp:TextBox ID="txtNumeroFolioCliente" runat="server" Width="135px" MaxLength="500"></asp:TextBox>
            </td>
        </tr>
        <tr id="trSeccion3" runat="server">
            <td class="tdCentradoVertical etiquetaControl" id="tdFechaRPPC">
                Fecha RPPC
            </td>
            <td class="tdCentradoVertical espacio">
                &nbsp;
            </td>
            <td class="tdCentradoVertical control">
                <asp:TextBox ID="txtFechaRPPCCliente" runat="server" CssClass="CampoFecha"></asp:TextBox>
                &nbsp;
            </td>
            <td class="tdCentradoVertical espacio">
                &nbsp;
            </td>
            <td class="tdCentradoVertical etiquetaControl">
                Localidad RPPC
            </td>
            <td class="tdCentradoVertical espacio">
                &nbsp;
            </td>
            <td class="tdCentradoVertical control">
                <asp:TextBox ID="txtLocalidadRPPCCliente" runat="server" MaxLength="100"></asp:TextBox>
                <div class="indicacion">
                    (Formato: Ciudad,Estado)</div>
            </td>
        </tr>		
        <tr id="trSeccion4" runat="server">
            <td>
                Activo
            </td>
            <td class="tdCentradoVertical espacio">
                &nbsp;
            </td>
            <td colspan="3">
                <asp:CheckBox ID="chkActivo" runat="server" />
            </td>
        </tr>	
    </table>	
    <asp:HiddenField ID="hdnActaId" runat="server" />	
</div>
