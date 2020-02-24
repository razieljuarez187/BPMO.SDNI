<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEndosoSeguroUI.ascx.cs" Inherits="BPMO.SDNI.Tramites.UI.ucEndosoSeguroUI" %>
<script type="text/javascript">
    $(document).ready(function () {
        $("#tabs").tabs();
    });        
</script>
<fieldset>
    <legend>Endoso</legend>
    <table id="endoso" class="trAlinearDerecha">
        <tr>
            <td class="tdCentradoVertical"><span>*</span>MOTIVO</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtMotivo" runat="server" ToolTip="Motivo del endoso" MaxLength="30" Width="275px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical"><span>*</span>IMPORTE</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtImporte" runat="server" CssClass="CampoNumero" ToolTip="Importe" Width="275px" MaxLength="13"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Button ID="btnAgregar" runat="server" Text="AGREGAR" CssClass="btnAgregarATabla" onclick="btnAgregar_Click" />
            </td>
        </tr>
    </table>
</fieldset>
<div id="endosos">
    <asp:GridView ID="grdEndosos" runat="server" AutoGenerateColumns="False" 
        CssClass="Grid" AllowPaging="True" AllowSorting="True" 
        onpageindexchanging="grdEndosos_PageIndexChanging" 
        onrowcommand="grdEndosos_RowCommand" align="center">
        <Columns>
            <asp:BoundField HeaderText="EndosoID" DataField="EndosoID" Visible="false" />
            <asp:BoundField HeaderText="Motivo" DataField="Motivo" />
            <asp:BoundField HeaderText="Importe" DataField="Importe" 
				DataFormatString="{0:#,##0.0000}" >
                <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png" ToolTip="Eliminar"
                        CommandName="Eliminar" CommandArgument='<%#Container.DataItemIndex%>' />
                </ItemTemplate>
                <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
        </Columns>
        <RowStyle CssClass="GridRow" />
        <HeaderStyle CssClass="GridHeader" />
        <FooterStyle CssClass="GridFooter" />
        <PagerStyle CssClass="GridPager" />
        <SelectedRowStyle CssClass="GridSelectedRow" />
        <AlternatingRowStyle CssClass="GridAlternatingRow" />
    </asp:GridView>
</div>
<table id="tbSumatoriasEndosos" runat="server" class="trAlinearDerecha Totales">
    <tr>
        <td class="tdCentradoVertical">
            <asp:label id="lblSumaendosos" runat="server" text="SUMA ENDOSOS"></asp:label>
        </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical">
            <asp:TextBox ID="txtSumaEndosos" runat="server" CssClass="CampoMoneda" ToolTip="Suma de Endosos" Enabled="False"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tdCentradoVertical">
            <asp:label id="lblPrimaAnualTotal" runat="server" text="PRIMA ANUAL TOTAL"></asp:label>
        </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="tdCentradoVertical">
            <asp:TextBox ID="txtPrimaAnualTotal" runat="server" CssClass="CampoMoneda" ToolTip="Prima Anual Total" Enabled="False"></asp:TextBox>
        </td>
    </tr>
</table>
<asp:HiddenField ID="hdnTramiteID" runat="server" />
<asp:HiddenField ID="hdnEndosoID" runat="server" />
<asp:HiddenField ID="hdnPrimaAnual" runat="server" />