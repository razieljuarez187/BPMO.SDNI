<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAsignacionEquiposAliadosUI.ascx.cs" Inherits="BPMO.SDNI.Equipos.UI.ucAsignacionEquiposAliadosUI" %>
<div id="dvAsignarEquipoAliado">
    <table class="trAlinearDerecha">
        <tr>
            <td class="tdCentradoVertical"># Serie</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 330px;">
                <asp:TextBox ID="txtEquipoAliado" runat="server" Width="201px" AutoPostBack="true" ontextchanged="txtEquipoAliado_TextChanged"></asp:TextBox>
                <asp:ImageButton ID="ibtnBuscaEquipoAliado" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" onclick="ibtnBuscaEquipoAliado_Click" />
                <asp:HiddenField ID="hdnEquipoAliadoID" runat="server" />
                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btnAgregarMediano" onclick="btnAgregar_Click" />
            </td>
        </tr>
    </table>
    <div class="Contenedor">
        <asp:GridView ID="grvEquiposAliados" runat="server" CssClass="Grid" style="width: 100%;"
            AutoGenerateColumns="False" onrowcommand="grvEquiposAliados_RowCommand" 
            onrowdatabound="grvEquiposAliados_RowDataBound" 
            onselectedindexchanged="grvEquiposAliados_SelectedIndexChanged">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate># Serie</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.NumeroSerie")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                
                  <asp:TemplateField Visible="false" >
                            <ItemTemplate>
                             <asp:Label ID="lbEquipoAliadoID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"EquipoAliadoID") %>'></asp:Label> 
                            </ItemTemplate>
                             <HeaderStyle HorizontalAlign="Center" Width="10%"/>              
                        </asp:TemplateField>
                                
                <asp:TemplateField>
                    <HeaderTemplate>Modelo</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.Modelo.Nombre")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Año</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.Anio")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Fabricante</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.Fabricante")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Tipo</HeaderTemplate>
                    <ItemTemplate>
                        <%# (DataBinder.Eval(Container, "DataItem.TipoEquipoServicio") != null) ? DataBinder.Eval(Container, "DataItem.TipoEquipoServicio.Nombre") : "" %>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Hrs Iniciales</HeaderTemplate>
                    <ItemTemplate>
                        <%# string.Format("{0:#,##0}", DataBinder.Eval(Container, "DataItem.HorasIniciales"))%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>

                  <asp:TemplateField>
                    <HeaderTemplate>Mantenimiento</HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chbxEntraMantenimiento" runat="server" AutoPostBack="True" 
    oncheckedchanged="chbxEntraMantenimiento_CheckedChanged" />
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
    </div>
</div>   


<br />
<asp:Button ID="btnResult" runat="server" Text="Button" onclick="btnResult_Click" style="display: none;" />
<asp:HiddenField ID="hdnUsuarioAutenticado" runat="server" />

