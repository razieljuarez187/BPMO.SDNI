<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDeducibleSeguroUI.ascx.cs" Inherits="BPMO.SDNI.Tramites.UI.ucDeducibleSeguroUI" %>
<fieldset>        
    <legend>Deducible</legend>
    <table id="deducible" class="trAlinearDerecha">
        <tr>
            <td class="tdCentradoVertical"><span>*</span>CONCEPTO</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtConcepto" runat="server" ToolTip="CONCEPTO" MaxLength="30" Width="275px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical"><span>*</span>PORCENTAJE</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtPorcentaje" runat="server" CssClass="CampoPorcentaje" ToolTip="PORCENTAJE" Width="275px" MaxLength="7"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Button ID="btnAgregar" runat="server" Text="AGREGAR" CssClass="btnAgregarATabla" onclick="btnAgregar_Click" />
            </td>
        </tr>
    </table>
</fieldset>
<div id="deducibles">
    <asp:GridView ID="grdDeducibles" runat="server" AutoGenerateColumns="False"  
        CssClass="Grid" AllowPaging="True" AllowSorting="True" 
        onrowcommand="grdDeducibles_RowCommand" 
        onpageindexchanging="grdDeducibles_PageIndexChanging" align="center">
        <Columns>
            <asp:BoundField HeaderText="DeducibleID" DataField="DeducibleID" Visible="false" />
            <asp:BoundField HeaderText="CONCEPTO" DataField="Concepto" />
            <asp:BoundField HeaderText="PORCENTAJE" DataField="Porcentaje" 
				DataFormatString="{0:n2}" >
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
<asp:HiddenField ID="hdnTramiteID" runat="server" />
<asp:HiddenField ID="hdnDeducibleID" runat="server" />
