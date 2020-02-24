<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCatalogoDocumentosUI.ascx.cs" Inherits="BPMO.SDNI.Comun.UI.ucCatalogoDocumentosUI" %>
<asp:UpdatePanel ID="updPn" runat="server">
    <ContentTemplate>
        <table class="trAlinearDerecha">
            <tr>
                <td align="right" style="padding-top: 5px">Archivo</td>
                <td style="width: 5px;">&nbsp;</td>
                <td>
                    <asp:FileUpload ID="uplArchivo" runat="server" Width="290px" />
                    <asp:RequiredFieldValidator ID="rfvUplArchivo" runat="server" ErrorMessage="*" CssClass="ColorValidator"
                        ControlToValidate="uplArchivo" ValidationGroup="FileUpload"></asp:RequiredFieldValidator>
                </td>
                <td align="right" style="padding-top: 5px" runat="server" id="tdlblObservaciones">Observaciones</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 310px;" runat="server" id="tdtxtObservaciones">
                    <asp:TextBox runat="server" ID="txtObservaciones" TextMode="MultiLine" Width="285px" Height="90px"
                        Style="max-width: 285px; min-width: 285px; max-height: 90px; min-height: 90px;">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvObservaciones" runat="server" ErrorMessage="*"
                        CssClass="ColorValidator" ControlToValidate="txtObservaciones" ValidationGroup="FileUpload"></asp:RequiredFieldValidator>
                </td>
            </tr>

            <tr>
				<td align="right" style="padding-top: 5px">
					&nbsp;</td>
				<td style="width: 5px;">
					&nbsp;</td>
				<td>
					&nbsp;</td>
				<td align="right" style="padding-top: 5px">
					&nbsp;</td>
				<td style="width: 5px;">
					&nbsp;</td>
				<td class="tdCentradoVertical" style="width: 310px;">
					<asp:Label ID="lblValidacionTipoArchivo" runat="server" Text="" Visible="False" CssClass="ColorValidator"></asp:Label>
				</td>
			</tr>

            <tr>
                <td colspan="6">
                    <asp:Button ID="btnAgregarTabla" runat="server" Text="Agregar a Tabla" CssClass="btnAgregarATabla" style="margin-right: 10px;"
                        OnClick="btnAgregarTabla_Click" ValidationGroup="FileUpload" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnAgregarTabla" />
    </Triggers>
</asp:UpdatePanel>
<asp:GridView ID="grdArchivos" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="False" PageSize="5"
    CellPadding="4" GridLines="None" CssClass="Grid" Width="95%" style="margin: 10px auto;" OnPageIndexChanging="grdArchivos_PageIndexChanging"  
    OnRowCommand="grdArchivos_RowCommand" OnRowDataBound="grdArchivos_RowDataBound">
    <Columns>
        <asp:BoundField HeaderText="Nombre" DataField="Nombre" SortExpression="NOMBRE">
        </asp:BoundField>
        <asp:TemplateField>
            <HeaderTemplate>Extensión</HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" ID="lblExtension" Text='<%# DataBinder.Eval(Container.DataItem,"TipoArchivo.Extension") %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle Width="100px" />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:ImageButton runat="server" ID="ibtEliminar" CommandName="eliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png"
                    ToolTip="Eliminar" CommandArgument='<%#Container.DataItemIndex%>' OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');" />
            </ItemTemplate>
            <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:ImageButton runat="server" ID="ibtDescargar" CommandName="descargar" ImageUrl="~/Contenido/Imagenes/DESCARGAR-ICO.png"
                    ToolTip="Descargar" CommandArgument='<%#Container.DataItemIndex%>' Visible='<%# (int?)DataBinder.Eval(Container,"DataItem.Id") != null ? true:false %>' />
            </ItemTemplate>
            <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
        </asp:TemplateField>
    </Columns>
    <HeaderStyle CssClass="GridHeader" />
    <EditRowStyle CssClass="GridAlternatingRow" />
    <PagerStyle CssClass="GridPager" />
    <RowStyle CssClass="GridRow" />
    <FooterStyle CssClass="GridFooter" />
    <SelectedRowStyle CssClass="GridSelectedRow" />
    <AlternatingRowStyle CssClass="GridAlternatingRow" />
    <EmptyDataTemplate>
        <b>No se han agregado archivos.</b>
    </EmptyDataTemplate>
</asp:GridView>
<asp:HiddenField ID="hdnIdentificador" runat="server" />
<asp:HiddenField ID="hdnModoEdicion" runat="server" />
<asp:HiddenField ID="hdnTipoAdjunto" runat="server" />
<asp:HiddenField ID="hdnTieneObservaciones" runat="server" />
