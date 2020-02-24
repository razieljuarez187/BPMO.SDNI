<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucListadoPlantillasUI.ascx.cs" Inherits="BPMO.SDNI.Contratos.UI.ucListadoPlantillasUI" %>
<asp:GridView ID="grdArchivos" runat="server" AutoGenerateColumns="False" 
    AllowPaging="True" AllowSorting="False" PageSize="5"
    CellPadding="4" GridLines="None" CssClass="Grid" onrowcommand="grdArchivos_RowCommand" 
    onrowdatabound="grdArchivos_RowDataBound" 
    onpageindexchanging="grdArchivos_PageIndexChanging">
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
</asp:GridView>
<asp:HiddenField ID="hdnIdentificador" runat="server" />
<asp:HiddenField ID="hdnModoEdicion" runat="server" />
<asp:HiddenField ID="hdnArchivoEliminar" runat="server" />