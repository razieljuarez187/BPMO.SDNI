<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucInformacionAdicionalUI.ascx.cs" Inherits="BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI.ucInformacionAdicionalUI" %>
<%--Satisface el caso de uso CU005 – Armar Paquetes Facturacion--%>
<asp:Panel ID="pnlContenedor" runat="server" CssClass="Contenedor">
    <table class="trAlinearDerecha" style="margin: 0px auto; width: 530px; display: inherit;
        border: 1px solid transparent;">
        <tr>
            <td class="tdCentradoVertical">
                Etiqueta
            </td>
            <td class="separadorCampo">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                <asp:TextBox runat="server" ID="txtEtiqueta" Width="375px" MaxLength="100" TextMode="MultiLine" style="text-transform:uppercase; resize: none;"
                    Rows="3"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvtxtEtiqueta" runat="server" 
                    ValidationGroup="InformacionAdicional" Text="*" 
                    ToolTip="La etiqueta es requerida" 
                    ErrorMessage="La etiqueta es requerida" ControlToValidate="txtEtiqueta" 
                    Display="Dynamic"></asp:RequiredFieldValidator>
            </td>        
        </tr>
        <tr>
        <td class="tdCentradoVertical">
            Valor
        </td>
        <td class="separadorCampo">
            &nbsp;
        </td>
        <td >
            <asp:TextBox runat="server" ID="txtValor" Width="375px" MaxLength="100" TextMode="MultiLine" style="text-transform:uppercase; resize: none;"
                Rows="3"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvtxtValor" runat="server" 
                ValidationGroup="InformacionAdicional" Text="*" 
                ToolTip="El valor es requerido" 
                ErrorMessage="El valor es requerido" ControlToValidate="txtValor" 
                Display="Dynamic"></asp:RequiredFieldValidator>
        </td>
        
    </tr>
        <tr>
            <td>
            
            </td>
            <td></td>
            <td style="text-align: right;">
                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" ValidationGroup="InformacionAdicional" CssClass="btnAgregarMediano" OnClick="btnAgregar_Click" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="grvDatosAdicionales" runat="server" CssClass="Grid97Percent" AutoGenerateColumns="False"
        OnRowCommand="grvDatosAdicionales_RowCommand" ShowHeaderWhenEmpty="True" 
        EmptyDataText="No hay datos adicionales agregados">
        <EmptyDataTemplate>
            <span>No hay datos adicionales agregados</span>
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    Etiqueta</HeaderTemplate>
                <ItemTemplate>
                    <%# DataBinder.Eval(Container, "DataItem.Etiqueta")%>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Valor</HeaderTemplate>
                <ItemTemplate>
                    <%#  DataBinder.Eval(Container, "DataItem.Valor") %>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>

            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png"
                        ToolTip="Eliminar" CommandName="Eliminar" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' />
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
</asp:Panel>
