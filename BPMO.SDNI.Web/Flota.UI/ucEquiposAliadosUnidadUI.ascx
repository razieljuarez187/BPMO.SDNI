<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEquiposAliadosUnidadUI.ascx.cs" Inherits="BPMO.SDNI.Flota.UI.ucEquiposAliadosUnidadUI" %>
<style type="text/css">        
        .Grid
        {
            width: 90%;
            margin: 25px auto 15px auto;
        }
    </style>    
    <div id="divAliados">
        <asp:GridView ID="grdEquiposAliados" runat="server" autoGenerateColumns="False" 
            AllowPaging="True" PageSize="10"
                        AllowSorting="false"  EnableSortingAndPagingCallbacks="True" 
            CssClass="Grid" >
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate># Serie</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblNumeroSerie" Text='<%# DataBinder.Eval(Container.DataItem,"NumeroSerie") %>'
                            Width="100%"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>A&ntilde;o</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblAnio" Text='<%# DataBinder.Eval(Container.DataItem,"Anio") %>'
                            Width="100%"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Dimensiones</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDimenciones" Text='<%# DataBinder.Eval(Container.DataItem,"Dimension") %>'
                            Width="100%"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>PBV</HeaderTemplate>
                    <ItemTemplate>
                    <%--SC0020 --%>
                        <asp:Label runat="server" ID="lblPBV" Text='<%# String.Format("{0: 0.00##}",DataBinder.Eval(Container.DataItem,"PBV") )%>'
                            Width="100%"></asp:Label>
                    <%--SC0020 --%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="PBC">
                    <%--<HeaderTemplate>PBC</HeaderTemplate>--%>
                    <ItemTemplate>
                    <%--SC0020 --%>
                        <asp:Label runat="server" ID="lblPBC" Text='<%# String.Format("{0: 0.00##}",DataBinder.Eval(Container.DataItem,"PBC") )%>'
                            Width="100%"></asp:Label>
                    <%--SC0020 --%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Modelo</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPBC" Text='<%# DataBinder.Eval(Container.DataItem,"Modelo.Nombre") %>'
                            Width="100%"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
            </Columns>
            <RowStyle CssClass="GridRow" />
            <HeaderStyle CssClass="GridHeader" />
            <FooterStyle CssClass="GridFooter" />
            <PagerStyle CssClass="GridPager" />
            <SelectedRowStyle CssClass="GridSelectedRow" />
            <AlternatingRowStyle CssClass="GridAlternatingRow" />
            <EmptyDataTemplate>
			<b>No se han asignado Equipos Aliados a la Unidad.</b>
		</EmptyDataTemplate>
        </asp:GridView>
    </div>
<asp:HiddenField ID="hdnUnidadID" runat="server" />
<asp:HiddenField ID="hdnEquipoID" runat="server" />
<asp:HiddenField ID="hdnLiderID" runat="server" />
<asp:HiddenField ID="hdnOracleID" runat="server" />
<asp:HiddenField ID="hdnNumeroEconomico" runat="server" />
<asp:HiddenField ID="hdnNumeroSerie" runat="server" />