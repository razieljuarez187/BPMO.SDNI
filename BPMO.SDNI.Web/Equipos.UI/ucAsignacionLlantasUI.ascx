<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAsignacionLlantasUI.ascx.cs" Inherits="BPMO.SDNI.Equipos.UI.ucAsignacionLlantasUI" %>
<%@ Register Src="~/Equipos.UI/ucLlantaUI.ascx" TagName="ucLlantaUI" TagPrefix="ucLL" %>
<div id="dvAsignarLlantas">
    <fieldset>
        <legend>Llantas</legend>
        <ucLL:ucLlantaUI ID="ucLlanta" runat="server" />
        <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btnAgregarMediano" onclick="btnAgregar_Click" />
        <div class="Contenedor">
            <asp:GridView ID="grvLlantas" runat="server" CssClass="Grid"
                AutoGenerateColumns="False" onrowcommand="grvLlantas_RowCommand">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>C&oacute;digo</HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.Codigo")%>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>Marca</HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.Marca")%>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>Modelo</HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.Modelo")%>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>Medida</HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.Medida")%>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>Profundidad</HeaderTemplate>
                        <ItemTemplate>
                            <%# string.Format("{0:#,##0.0000}", DataBinder.Eval(Container, "DataItem.Profundidad") )%>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Right" Width="100px" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>Posici&oacute;n</HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.Posicion").ToString() %>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Right" Width="70px" />
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
    </fieldset>
    <fieldset>
        <legend>Refacción</legend>
        <ucLL:ucLlantaUI ID="ucRefaccion" runat="server" />
    </fieldset>
</div>
<asp:HiddenField ID="hdnUsuarioAutenticado" runat="server" />
